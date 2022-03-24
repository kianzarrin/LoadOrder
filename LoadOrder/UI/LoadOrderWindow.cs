#define ASYNC_FILTER

using CO;
using CO.Packaging;
using CO.Plugins;
using LoadOrderTool.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LoadOrderTool.Data;
using LoadOrderTool.UI;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using CO.PlatformServices;
using System.IO;

namespace LoadOrderTool.UI {
    public partial class LoadOrderWindow : Form {
        enum EnabledFilter {
            [Text("<Enabled/Disabled>")]
            None = 0,
            Enabled,
            Disabled,
        }
        enum IncludedFilter {
            [Text("<Included/Excluded>")]
            None = 0,
            Included,
            Excluded,
        }
        enum WSFilter {
            [Text("<Workshop/Local>")]
            None = 0,
            Workshop,
            Local,
        }

        const string NO_TAGS = "<Tags>";

        public static LoadOrderWindow Instance;
        public LoadOrderToolSettings settings_ => LoadOrderToolSettings.Instace;

        static ConfigWrapper ConfigWrapper => ConfigWrapper.instance;

        bool dirty_;
        public bool Dirty {
            get => dirty_; //fast access.
            set {
                if (dirty_ == value) return;
                dirty_ = value;
                Action act;
                if (value) {
                    act = new Action(delegate () {
                        Text = "Load Order Tool*";
                    });
                } else {
                    act = new Action(delegate () {
                        Text = "Load Order Tool";
                    }); // drop dirty *
                }
                ExecuteThreadSafe(act);
            }
        }

        public void ExecuteThreadSafe(Action act) {
            if (InvokeRequired) {
                Invoke(act);
            } else {
                act.Invoke();
            }
        }

        public LoadOrderWindow() {
            try {
                Instance = this;
                ConfigWrapper.Suspend();
                InitializeComponent();
                LoadSize();
            } catch (Exception ex) {
                ex.Log();
            }
        }

        public async Task LoadAsync() {
            try {
                menuStrip.tsmiAutoSave.Checked = ConfigWrapper.AutoSave;

                await Task.Run(ContentUtil.EnsureSubscribedItems);
                var modTask = InitializeModTab();
                var assetTask = InitializeAssetTab();
                await Task.WhenAll(modTask, assetTask);
                TabContainer.Selected += (_, __) => UpdateStatus();

                UpdateStatus();

                menuStrip.tsmiAutoSave.CheckedChanged += TsmiAutoSave_CheckedChanged;
                menuStrip.tsmiAutoSave.Click += TsmiAutoSave_Click;

                menuStrip.tsmiReloadUGC.Click += ReloadAll_Click;
                menuStrip.tsmiResetCache.Click += TsmiResetCache_Click;
                menuStrip.tsmiReloadSettings.Click += TsmiReloadSettings_Click;
                menuStrip.tsmiUpdateSteamCache.Click += TsmiUpdateSteamCache_Click;
                menuStrip.tsmiResetAllSettings.Click += TsmiResetSettings_Click;

                menuStrip.tsmiSave.Click += Save_Click;
                menuStrip.tsmiExport.Click += Export_Click;
                menuStrip.tsmiImport.Click += Import_Click;

                Dirty = ConfigWrapper.Dirty;
                ConfigWrapper.Resume();
                ConfigWrapper.SaveConfig();

                await CacheWSDetails();
            } catch (Exception ex) {
                ex.Log();
            }
        }

        protected async override void OnLoad(EventArgs e) {
            base.OnLoad(e);
            await LoadAsync();
        }

        protected override void OnResizeEnd(EventArgs e) {
            base.OnResizeEnd(e);
            SaveSize();
        }

        public void LoadSize() {
            if (settings_.FormWidth > 0)
                Width = settings_.FormWidth;
            if (settings_.FormHeight > 0)
                Height = settings_.FormHeight;
        }

        void SaveSize() {
            settings_.FormWidth = Width;
            settings_.FormHeight = Height;

            settings_.Serialize();
        }

        private void LoadOrderWindow_FormClosing(object sender, FormClosingEventArgs e) {
            if (!ConfigWrapper.AutoSave && ConfigWrapper.Dirty) {
                var result = MessageBox.Show(
                    caption: "Unsaved changes",
                    text:
                    "There are changes that are not saved to game and will not take effect. " +
                    "Save changes to game?",
                    buttons: MessageBoxButtons.YesNoCancel);
                switch (result) {
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        return;
                    case DialogResult.Yes:
                        ConfigWrapper.SaveConfig();
                        break;
                    case DialogResult.No:
                        break;
                    default:
                        Log.Exception(new Exception("FormClosing: Unknown choice"));
                        break;
                }
            }

            ConfigWrapper.Terminate();
            GameSettings.instance.Terminate();
        }

        protected override void OnSizeChanged(EventArgs e) {
            base.OnSizeChanged(e);
            if (!Visible) return; // skip during initialization.
            LoadOrderToolSettings.Instace.FormWidth = Width;
            LoadOrderToolSettings.Instace.FormHeight = Height;
        }



        #region MenuBar
        private async void TsmiResetSettings_Click(object sender, EventArgs e) => await TsmiResetSettings();
        private async Task TsmiResetSettings() {
            if(DialogResult.Yes ==
                MessageBox.Show(
                    text: "Are you sure you want to reset all settings?",
                    caption: "Reset settings?",
                    buttons: MessageBoxButtons.YesNo,
                    icon: MessageBoxIcon.Warning
                )) {
                ConfigWrapper.ResetAllConfig();
                launchControl.LoadSettings();
                await ReloadAll();
                ConfigWrapper.instance.SaveConfig();
            }
        }
        private void TsmiAutoSave_CheckedChanged(object sender, EventArgs e) {
            ConfigWrapper.AutoSave = menuStrip.tsmiAutoSave.Checked;
        }

        private void TsmiAutoSave_Click(object sender, EventArgs e) =>
            menuStrip.tsmiFile.ShowDropDown(); // prevent hiding menu when clicking auto-save

        public void UpdateLastProfileName(string fullPath) {
            LoadOrderToolSettings.Instace.LastProfileName = Path.GetFileNameWithoutExtension(fullPath);
            LoadOrderToolSettings.Instace.Serialize();
            UpdateStatus();
        }

        private void Export_Click(object sender, EventArgs e) {
            SaveFileDialog diaglog = new SaveFileDialog();
            diaglog.Filter = "xml files (*.xml)|*.xml";
            diaglog.InitialDirectory = LoadOrderProfile.DIR;
            if (diaglog.ShowDialog() == DialogResult.OK) {
                LoadOrderProfile profile = new LoadOrderProfile();
                ManagerList.instance.SaveToProfile(profile);
                profile.Serialize(diaglog.FileName);
                UpdateLastProfileName(diaglog.FileName);
            }
        }

        private void Import_Click(object sender, EventArgs e) {
            using (OpenFileDialog ofd = new OpenFileDialog()) {
                ofd.Filter = "xml files (*.xml)|*.xml";
                ofd.InitialDirectory = LoadOrderProfile.DIR;
                if (ofd.ShowDialog() == DialogResult.OK) {
                    var profile = LoadOrderProfile.Deserialize(ofd.FileName);
                    using (var opd = new OpenProfileDialog(profile)) {
                        if (DialogResult.Cancel == opd.ShowDialog())
                            return;
                        bool replace = opd.DialogResult == OpenProfileDialog.RESULT_REPLACE;
                        ApplyProfile(profile, types: opd.ItemTypes, replace: replace);
                        if(replace) UpdateLastProfileName(ofd.FileName);
                        else UpdateStatus();
                    }
                }
            }
        }

        public void ApplyProfile(LoadOrderProfile profile, OpenProfileDialog.ItemTypeT types, bool replace) {
            Log.Called("types:" + types, "replace:" + replace);
            if ((types & OpenProfileDialog.ItemTypeT.Mods) != 0) {
                dataGridMods.ModList.LoadFromProfile(profile, replace);
                dataGridMods.RefreshModList(true);
            }
            if ((types & OpenProfileDialog.ItemTypeT.Assets) != 0) {
                PackageManager.instance.LoadFromProfile(profile, replace);
                PopulateAssets();
            }
            if ((types & OpenProfileDialog.ItemTypeT.DLCs) != 0) {
                DLCManager.instance.LoadFromProfile(profile, replace);
                DLCControl.PopulateDLCs();
            }
            if ((types & OpenProfileDialog.ItemTypeT.SkipFile) != 0) {
                LSMManager.instance.LoadFromProfile(profile, replace);
                LSMControl.Populate();
            }
        }

        private void Save_Click(object sender, EventArgs e) {
            ConfigWrapper.SaveConfig();
        }

        private async void ReloadAll_Click(object sender, EventArgs e) {
            await ReloadAll();
        }

        private async void TsmiUpdateSteamCache_Click(object sender, EventArgs e) {
            ConfigWrapper.ReloadCSCache();
            await CacheWSDetails();
        }

        private void TsmiReloadSettings_Click(object sender, EventArgs e) {
            if(Dirty) {
                if(ConfigWrapper.AutoSave) { 
                    ConfigWrapper.SaveConfig();
                } else {
                    var res = MessageBox.Show(
                        text: "All unsaved changes will be lost. Do you wish to proceed to replace them by settings from the game?",
                        caption: "Discard changes?",
                        buttons: MessageBoxButtons.OKCancel);
                    if(res == DialogResult.Cancel) {
                        return;
                    }
                }
            }

            ConfigWrapper.Paused = true;
            ConfigWrapper.ReloadAllConfig();

            ManagerList.instance.ReloadConfig();
            launchControl.LoadSettings();

            ConfigWrapper.Dirty = false;
            ConfigWrapper.Paused = false;
            RefreshAll();
        }

        private async void TsmiResetCache_Click(object sender, EventArgs e) => await ResetCache();
        private async Task ResetCache() {
            try {
                ConfigWrapper.ResetCSCache();
                ConfigWrapper.SteamCache = new SteamCache();
                await CacheWSDetails();
            } catch(Exception ex) { ex.Log(); }
        }
        #endregion

        #region Mod tab
        async Task InitializeModTab() {
            ComboBoxIncluded.SetItems<IncludedFilter>();
            ComboBoxIncluded.SelectedIndex = 0;
            ComboBoxEnabled.SetItems<EnabledFilter>();
            ComboBoxEnabled.SelectedIndex = 0;
            ComboBoxWS.SetItems<WSFilter>();
            ComboBoxWS.SelectedIndex = 0;

            var buttons = ModActionPanel.Controls.OfType<Button>();
            var maxwidth = buttons.Max(b => b.Width);
            foreach (var b in buttons)
                b.MinimumSize = new Size(maxwidth, 0);

            await dataGridMods.LoadModsAsync(ModPredicate);

            ComboBoxIncluded.SelectedIndexChanged += RefreshModList;
            ComboBoxEnabled.SelectedIndexChanged += RefreshModList;
            ComboBoxWS.SelectedIndexChanged += RefreshModList;
            TextFilterMods.TextChanged += RefreshModList;

            TabContainer.SelectedIndexChanged += TabContainer_SelectedIndexChanged;
            menuStrip.tsmiHarmonyOrder.Click += SortByHarmony_Click;
            menuStrip.tsmiReverseOrder.Click += ReverseOrder_Click;
            menuStrip.tsmiRandomOrder.Click += RandomizeOrder_Click;
            menuStrip.tsmiResetOrder.Click += ResetOrder_Click;
            IncludeAllMods.Click += IncludeAllMods_Click;
            ExcludeAllMods.Click += ExcludeAllMods_Click;
            EnableAllMods.Click += EnableAllMods_Click;
            DisableAllMods.Click += DisableAllMods_Click;

            dataGridMods.CellMouseClick += DataGridMods_CellMouseClick;

            UpdateStatus();
        }

        private void RefreshModList(object sender, EventArgs e) {
            RefreshModPredicate();
            dataGridMods.RefreshModList(); // filter and populate
        }

        #region predicate
        IncludedFilter modIncludedFilter_;
        EnabledFilter modEnabledFilter_;
        WSFilter modWSFilter_;
        string [] modTextFilter_;

        public void RefreshModPredicate() {
            modIncludedFilter_ = ComboBoxIncluded.GetSelectedItem<IncludedFilter>();
            modEnabledFilter_ = ComboBoxEnabled.GetSelectedItem<EnabledFilter>();
            modWSFilter_ = ComboBoxWS.GetSelectedItem<WSFilter>();
            modTextFilter_ = TextFilterMods.Text?.Split(" ");
        }

        public bool ModPredicate(PluginManager.PluginInfo p) {
            {
                if (modIncludedFilter_ != IncludedFilter.None) {
                    bool b = modIncludedFilter_ == IncludedFilter.Included;
                    if (p.IsIncludedPending != b)
                        return false;
                }
            }
            {
                if (modEnabledFilter_ != EnabledFilter.None) {
                    bool b = modEnabledFilter_ == EnabledFilter.Enabled;
                    if (p.IsEnabledPending != b)
                        return false;
                }
            }
            {
                if (modWSFilter_ != WSFilter.None) {
                    bool b = modWSFilter_ == WSFilter.Workshop;
                    if (p.IsWorkshop != b)
                        return false;
                }
            }
            {
                if (modTextFilter_ != null && modTextFilter_.Length > 0) {
                    if (!ContainsWords(p.SearchText, modTextFilter_))
                        return false;
                }
            }

            return true;
        }
        #endregion

        private void TabContainer_SelectedIndexChanged(object sender, EventArgs e) =>
            menuStrip.tsmiOrder.Visible = dataGridMods.Visible;

        private void ResetOrder_Click(object sender, EventArgs e) {
            foreach (var mod in dataGridMods.ModList)
                mod.ResetLoadOrder();
            dataGridMods.RefreshModList(sort: true);
        }

        private void SortByHarmony_Click(object sender, EventArgs e) {
            dataGridMods.ModList.ResetLoadOrders();
            dataGridMods.ModList.SortBy(ModList.HarmonyComparison);
            dataGridMods.RefreshModList();
        }

        private void EnableAllMods_Click(object sender, EventArgs e) {
            foreach (var p in dataGridMods.ModList.Filtered)
                p.IsEnabledPending = true;
            dataGridMods.PopulateMods();
        }

        private void DisableAllMods_Click(object sender, EventArgs e) {
            foreach (var p in dataGridMods.ModList.Filtered)
                p.IsEnabledPending = false;
            dataGridMods.PopulateMods();
        }

        private void IncludeAllMods_Click(object sender, EventArgs e) {
            foreach (var p in dataGridMods.ModList.Filtered)
                p.IsIncludedPending = true;
            dataGridMods.PopulateMods();
        }

        private void ExcludeAllMods_Click(object sender, EventArgs e) {
            foreach (var p in dataGridMods.ModList.Filtered)
                p.IsIncludedPending = false;
            dataGridMods.PopulateMods();
        }

        private void ReverseOrder_Click(object sender, EventArgs e) {
            dataGridMods.ModList.ReverseOrder();
            dataGridMods.RefreshModList();
        }

        private void RandomizeOrder_Click(object sender, EventArgs e) {
            dataGridMods.ModList.RandomizeOrder();
            dataGridMods.RefreshModList();
        }

        private void DataGridMods_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e) {
            if (e.Button != MouseButtons.Right)
                return;
            int i = e.RowIndex;
            var mods = dataGridMods.ModList.Filtered;
            if (i >= 0 && i < mods.Count) {
                var mod = mods[i];
                string path = mod.dllPath ?? mod.ModPath;
                new ContextMenuItems(path, mod.PublishedFileId).Show(MousePosition);
            }
        }
        #endregion

        #region AssetTab
        async Task InitializeAssetTab() {
            ComboBoxAssetIncluded.SetItems<IncludedFilter>();
            ComboBoxAssetIncluded.SelectedIndex = 0;
            ComboBoxAssetWS.SetItems<WSFilter>();
            ComboBoxAssetWS.SelectedIndex = 0;

            ComboBoxAssetIncluded.SelectedIndexChanged += ApplyAssetFilter;
            ComboBoxAssetWS.SelectedIndexChanged += ApplyAssetFilter;
            ComboBoxAssetTags.SelectedIndexChanged += ApplyAssetFilter;
            TextFilterAsset.TextChanged += ApplyAssetFilter;

            this.IncludeAllAssets.Click += IncludeAllAssets_Click;
            this.ExcludeAllAssets.Click += ExcludeAllAssets_Click;
            
            var buttons = AssetsActionPanel.Controls.OfType<Button>();
            var maxwidth = buttons.Max(b => b.Width);
            foreach (var b in buttons)
                b.MinimumSize = new Size(maxwidth, 0);

            await LoadAssetsAsync();

            dataGridAssets.CellMouseClick += DataGridAssets_CellMouseClick;

            UpdateStatus();
        }

        public async Task LoadAssetsAsync() {
            try {
                AssetDataGrid.SetProgress(0);
                await LoadAssets();
                AssetDataGrid.SetProgress(-1);
            } catch (Exception ex) { ex.Log(); }
        }

        public async Task LoadAssets() {
            await PackageManager.instance.LoadPackages();
            AssetDataGrid.SetProgress(100);
            PopulateAssets();
        }

        public void PopulateAssets() {
            Log.Info("Populating assets");
            dataGridAssets.SuspendLayout();
            try {
                dataGridAssets.Rows.Clear();

                ComboBoxAssetTags.Items.Clear();
                ComboBoxAssetTags.Items.Add(NO_TAGS);
                ComboBoxAssetTags.Items.AddRange(PackageManager.instance.GetAllTags());
                ComboBoxAssetTags.SelectedIndex = 0;
                ComboBoxAssetTags.AutoSize();
                ComboBoxAssetTags.TextChanged += ComboBoxAssetTags_TextChanged;

                dataGridAssets.AssetList = new AssetList(PackageManager.instance.GetAssets(), FilterAssets);
            } catch (Exception ex) {
                Log.Exception(ex);
            } finally {
                dataGridAssets.ResumeLayout();
                dataGridAssets.Refresh();
            }
        }

        private void ComboBoxAssetTags_TextChanged(object sender, EventArgs e) {
            if (ComboBoxAssetTags.SelectedIndex != 0 && ComboBoxAssetTags.Text == "") {
                ComboBoxAssetTags.SelectedIndex = 0;
                Invoke(new Action(ComboBoxAssetTags.SelectAll));
            }
        }

        #region Filter
#if ASYNC_FILTER // modify this line to switch to async
        private async void ApplyAssetFilter(object sender, EventArgs e) {
            await ApplyAssetFilter();
        }

        private async Task ApplyAssetFilter() {
            // TODO: this fails when modifying text box too fast
            await ApplyAssetFilterTask();
            // dataGridAssets.AllowUserToResizeColumns = true;
        }

        CancellationTokenSource filterTokenSource_;
        async Task ApplyAssetFilterTask() {
            if(dataGridAssets?.AssetList?.Filtered == null)
                return;
            filterTokenSource_?.Cancel();
            try {
                filterTokenSource_ = new CancellationTokenSource(); 
                await Task.Delay(150, cancellationToken: filterTokenSource_.Token);
                
                FilterAssets();
                dataGridAssets.SetRowCountFast(dataGridAssets.AssetList.Filtered.Count);
                dataGridAssets.Refresh();
            } catch(OperationCanceledException) {
                // return silently
            } catch( Exception ex) {
                ex.Log();
            } finally {
                filterTokenSource_?.Dispose();
                filterTokenSource_ = null;
            }
        }
#else
        private void ApplyAssetFilter(object sender, EventArgs e) {
            ApplyAssetFilter();
        }

        private void ApplyAssetFilter() {
            Log.Debug("ApplyAssetFilterTask");
            FilterAssets();
            dataGridAssets.Refresh();
            dataGridAssets.AllowUserToResizeColumns = true;
        }
#endif
        public void FilterAssets() => dataGridAssets.AssetList?.FilterItems();

        public void FilterAssets(AssetList assetList) {
            var includedFilter = ComboBoxAssetIncluded.GetSelectedItem<IncludedFilter>();
            var wsFilter = ComboBoxAssetWS.GetSelectedItem<WSFilter>();
            var tagFilter = ComboBoxAssetTags.SelectedItem as string;
            if (tagFilter == NO_TAGS) tagFilter = null;
            var words = TextFilterAsset.Text?.Split(" ");
            assetList.FilterItems(item => AssetPredicateFast(item, includedFilter, wsFilter, tagFilter, words));
            UpdateStatus();
        }

        bool AssetPredicateFast(
            PackageManager.AssetInfo a,
            IncludedFilter includedFilter,
            WSFilter wSFilter,
            string tagFilter,
            string[] words) {
            if (includedFilter != IncludedFilter.None) {
                bool b = includedFilter == IncludedFilter.Included;
                if (a.IsIncludedPending != b)
                    return false;
            }
            if (wSFilter != WSFilter.None) {
                bool b = wSFilter == WSFilter.Workshop;
                if (a.IsWorkshop != b)
                    return false;
            }
            if (tagFilter != null && !a.GetTags().Contains(tagFilter)) {
                return false;
            }
            if (words != null && words.Length > 0) {
                if (!ContainsWords(a.SearchText, words)){
                    return false;
                }
            }
            return true;
        }

        // checks if the text contains all of the following words
        public static bool ContainsWords(string text, IEnumerable<string> words) {
            if (words is null || !words.Any())
                return true;
            if (string.IsNullOrWhiteSpace(text))
                return false;
            foreach (var word in words) {
                if (!text.Contains(word, StringComparison.OrdinalIgnoreCase))
                    return false;
            }
            return true;
        }
        #endregion

        public void IncludeExcludeFilteredAssets(bool value) {
            try {
                Log.Debug("IncludeExcludeVisibleAssets() Called.");
                ConfigWrapper.Suspend();
                foreach (var asset in dataGridAssets.AssetList.Filtered) {
                    asset.IsIncludedPending = value;
                }
                dataGridAssets.Refresh();
            }catch (Exception ex) {
                Log.Exception(ex);
            } finally {
                ConfigWrapper.Resume();
                Log.Debug("IncludeExcludeVisibleAssets() Finished!");
            }
        }

        private void IncludeAllAssets_Click(object sender, EventArgs e) {
            IncludeExcludeFilteredAssets(true);
        }

        private void ExcludeAllAssets_Click(object sender, EventArgs e) {
            IncludeExcludeFilteredAssets(false);
        }

        private void DataGridAssets_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e) {
            if (e.Button != MouseButtons.Right)
                return;
            int i = e.RowIndex;
            var assets = dataGridAssets.AssetList.Filtered;
            if (i >= 0 && i < assets.Count) {
                var asset = assets[i];
                string path = asset.AssetPath;
                new ContextMenuItems(path, asset.PublishedFileId).Show(MousePosition);
            }
        }
        #endregion

        public void UpdateStatus() {
            try {
                ModCountLabel.Visible = TabContainer.SelectedTab == ModsTab;
                if(dataGridMods.ModList is ModList modList) {
                    ModCountLabel.Text = $"Mods " +
                        $"enabled:{modList.Count(mod => mod.IsEnabledPending)} " +
                        $"included:{modList.Count(mod => mod.IsIncludedPending)} " +
                        $"total:{modList.Count}";
                }

                AssetCountLabel.Visible = TabContainer.SelectedTab == AssetsTab;
                if(dataGridAssets.AssetList is AssetList asseList) {
                    AssetCountLabel.Text = $"Assets " +
                        $"included:{asseList.Original.Count(asset => asset.IsIncludedPending)} " +
                        $"total:{asseList.Original.Count}";
                }

                string lastProfileName = LoadOrderToolSettings.Instace?.LastProfileName;
                LastProfileLabel.Visible = !lastProfileName.IsNullorEmpty();
                LastProfileLabel.Text = $"Last profile was '{lastProfileName}'";

                DLCNoticeLabel.Visible = TabContainer.SelectedTab == DLCTab;
                UpdateBrokenDownloadsStatus();
            } catch(Exception ex) { ex.Log(); }
        }

        bool IsExcludedMod(PublishedFileId id) {
            var mod = dataGridMods?.ModList?.FirstOrDefault(item => item.PublishedFileId == id);
            if (mod != null)
                return !mod.IsIncludedPending;
            return false;
        }

        bool IsExcluded(PublishedFileId id) {
            var mod = dataGridMods?.ModList?.FirstOrDefault(item => item.PublishedFileId == id);
            if (mod != null)
                return !mod.IsIncludedPending;

            var assets = dataGridAssets?.AssetList?.Original;
            if (assets != null)
                return assets.Any(item => item.PublishedFileId == id && item.IsIncludedPending);
            else
                return false;
        }

        string UpdateBrokenDownloadsStatus() {
            bool red = false, orange = false;
            string reason = "the following are missing:\n";

            var existingIds = ContentUtil.GetSubscribedItems();
            foreach (var item in ConfigWrapper.instance.SteamCache.Items.Where(
                item => item.Status.IsBroken() &&
                existingIds.Contains(item.PublishedFileId) &&
                !IsExcludedMod(item.PublishedFileId)
                )) {
                red = true;
                string type = item.Tags != null && item.Tags.Contains("Mod") ? "MOD" : "asset";
                reason += $"{item.PublishedFileId} [{type}] {item.Name} : {item.Status}\n";
            }

            foreach (var item in ContentUtil.GetMissingDirItems()) {
                orange = true;
                reason += $"{item} : not downloaded\n";
            }

            DownloadWarningLabel.Visible = red || orange;
            if (red) {
                DownloadWarningLabel.Text = "There are broken downloads!";
                DownloadWarningLabel.ForeColor = Color.Red;
                return DownloadWarningLabel.ToolTipText = reason;
            } else if (orange) {
                DownloadWarningLabel.Text = "There maybe broken downloads!";
                DownloadWarningLabel.ForeColor = Color.Orange;
                return DownloadWarningLabel.ToolTipText = reason;
            }
            return "";
        }

        


        public void RefreshAll() {
            dataGridMods.RefreshModList();
            dataGridAssets.Refresh();
            UpdateStatus();
        }

        public async Task ReloadAll() {
            try {
                await Task.Run(ContentUtil.EnsureSubscribedItems);
                ConfigWrapper.Paused = true;
                ConfigWrapper.ReloadCSCache();
                var modTask = dataGridMods.LoadModsAsync(ModPredicate);
                var AssetTask = LoadAssets();
                await Task.WhenAll(modTask, AssetTask);
                ConfigWrapper.Paused = false;
                await CacheWSDetails();
            } catch(Exception ex) { ex.Log(); }
        }

        #region steam cache
        void SetCacheProgress(float percent) {
            if(percent > 100) Log.Error($"percent={percent} nAuthors={nAuthors_} iAuthor={iAuthor_}");
            ModDataGrid.SetProgress(percent, UIUtil.WIN32Color.Warning);
            AssetDataGrid.SetProgress(percent, UIUtil.WIN32Color.Warning);
        }

        public async Task CacheWSDetails(bool save = true) {
            try {
                SetCacheProgress(5);
                var ids = (await Task.Run(ContentUtil.GetSubscribedItems)).ToArray();
                SetCacheProgress(10);

                try {
                    int i = 0;
                    List<Task> tasks = new List<Task>(32);
                    await foreach(var data in SteamUtil.LoadDataAsyncInChunks(ids)) {
                        Assertion.NotNull(data, "Internet connection problem?");
                        var task = Task.Run(() => DTO2Cache(data));
                        tasks.Add(task);
                        i += data.Length;
                        SetCacheProgress(10 + (30 * i) / ids.Length);
                    }
                    await Task.WhenAll(tasks.ToArray());
                } catch(Exception ex) { ex.Log(); }
                SetCacheProgress(45);
                RefreshAll();
                SetCacheProgress(50);

                bool authorsUpdated = true;
                try {
                    authorsUpdated = await Task.Run(CacheAuthors);
                } catch(Exception ex) {
                    ex.Log();
                }
                SetCacheProgress(100);
                if(authorsUpdated) RefreshAuthors();
                if(save) ConfigWrapper.SteamCache.Serialize();
                SetCacheProgress(-1);
                Log.Info(UpdateBrokenDownloadsStatus());
            } catch(Exception ex) { ex.Log(); }
        }

        public static void DTO2Cache(SteamUtil.PublishedFileDTO[] dtos) {
            Assertion.NotNull(dtos);
            var wsItems = ManagerList.GetWSItems();
            foreach(var dto in dtos) {
                var cacheItem = ConfigWrapper.SteamCache.GetOrCreateItem(new PublishedFileId(dto.PublishedFileID));
                cacheItem.Read(dto);

                var wsitems = wsItems.Where(item => dto.PublishedFileID == item.PublishedFileId.AsUInt64);
                foreach(var item in wsitems)
                    item.ResetCache();
                
            }
        }

        int nAuthors_;
        int iAuthor_;
        DateTime lastRefreshUpdate;

        public bool CacheAuthors() {
            try {
                Log.Called();
                static bool predicate(IWSItem item) {
                    Assertion.NotNull(item, "item");
                    Assertion.NotNull(item.SteamCache, $"item: {item.GetType().Name} {item.IncludedPath} {item.PublishedFileId}"); // ws items must have steam cache.
                    return item.SteamCache.Author.IsNullorEmpty() && item.SteamCache.AuthorID != 0;
                }
                var missingAuthors = ManagerList.GetWSItems()
                    .Where(predicate)
                    .Select(item => item.SteamCache.AuthorID)
                    .Distinct()
                    .ToArray();
                Log.Info("Getting author names : " + missingAuthors.ToSTR());
                if (!missingAuthors.Any())
                    return false;

                iAuthor_ = 0;
                nAuthors_ = missingAuthors.Length;
                using (var httpWrapper = new SteamUtil.HttpWrapper()) {
                    Task proc(ulong _authorId) {
                        return Task.Factory.StartNew(
                            () => GetAndApplyPersonaName(httpWrapper, _authorId));
                    }
                    var tasks = missingAuthors.Select(proc);

                    SetCacheProgress(55);
                    Task.WaitAll(tasks.ToArray());
                }
                Log.Succeeded();
                return true;
            }catch (Exception ex) {
                ex.Log();
                return true;
            }
        }

        public async Task GetAndApplyPersonaNameAsync(SteamUtil.HttpWrapper httpWrapper, ulong authorId) {
            string authorName = null;
            for(int numErrors = 0; authorName.IsNullorEmpty();) {
                try {
                    authorName = await SteamUtil.GetPersonaNameAsync(httpWrapper, authorId);
                    Assertion.Assert(!authorName.IsNullorEmpty());
                } catch(Exception ex) {
                    if(authorId == 76561197978975775) {
                        authorName = "Feindbild"; // hard code stubborn profile!
                    } else if (authorId == 76561198025140529) {
                        authorName = "76561198025140529"; // hard code removed profile!
                    } else {
                        numErrors++;
                        if(numErrors > 3) throw;
                        Log.Error(ex.ToString() + $" retry number {numErrors} ...");
                        await Task.Delay(100); // delay 100ms to make sure request goes to the end of the queue.
                    }
                }
            }

            Log.Debug($"Author received: {authorId} -> {authorName}");
            AddAuthor(authorId, authorName);
            SetCacheProgress(60 + (40 * iAuthor_) / nAuthors_);
            iAuthor_++;
            Log.Succeeded();
        }

        public void GetAndApplyPersonaName(SteamUtil.HttpWrapper httpWrapper, ulong authorId) {
            string authorName = null;
            for(int numErrors = 0; authorName.IsNullorEmpty();) {
                try {
                    authorName = SteamUtil.GetPersonaName(httpWrapper, authorId);
                    Assertion.Assert(!authorName.IsNullorEmpty());
                } catch(Exception ex) {
                    numErrors++;
                    if(numErrors > 3) throw;
                    Log.Error(ex.ToString() + $" retry number {numErrors} ...");
                    Thread.Sleep(100); // delay 100ms to make sure request goes to the end of the queue.
                }
            }

            Log.Debug($"Author received: {authorId} -> {authorName}");
            AddAuthor(authorId, authorName);
            SetCacheProgress(60 + (40 * iAuthor_) / nAuthors_);
            iAuthor_++;
            Log.Succeeded();
        }


        public void AddAuthor(ulong id, string name) {
            ConfigWrapper.SteamCache.AddPerson(id, name);
        }

        public void RefreshAuthors() {
            try {
                lastRefreshUpdate = DateTime.Now;

                foreach(var item in ManagerList.GetItems())
                    item.SteamCache?.UpdateAuthor();

                RefreshAll();
            } catch(Exception ex) { ex.Log(); }
        }
        #endregion steam cache
    }
}
