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
                if (value) {
                    Text += "*";
                } else {
                    Text = Text[0..^1]; // drop dirty
                }
            }
        }

        public LoadOrderWindow() {
            try {
                Instance = this;
                ConfigWrapper.Suspend();
                ProgressWindow.Instance?.SetProgress(10, "Initializing UI ...");
                InitializeComponent();
                LoadSize();

                ProgressWindow.Instance?.SetProgress(20, "Loading Mods ...");
                InitializeModTab();
                ProgressWindow.Instance?.SetProgress(50, "Loading Assets ...");
                InitializeAssetTab();
                ProgressWindow.Instance?.SetProgress(80, "Assets Loaded.");

                Dirty = ConfigWrapper.Dirty;
                ConfigWrapper.Resume();
                ConfigWrapper.SaveConfig();

                menuStrip.tsmiAutoSave.Checked = ConfigWrapper.AutoSave;
                menuStrip.tsmiAutoSave.CheckedChanged += TsmiAutoSave_CheckedChanged;
                menuStrip.tsmiAutoSave.Click += TsmiAutoSave_Click;

                menuStrip.tsmiResetSettings.Click += TsmiResetSettings_Click;
                menuStrip.tsmiReload.Click += ReloadAll_Click;
                menuStrip.tsmiSave.Click += Save_Click;
                menuStrip.tsmiExport.Click += Export_Click;
                menuStrip.tsmiImport.Click += Import_Click;

                ProgressWindow.Instance?.SetProgress(90, "Loading UI ...");
            } catch (Exception ex) {
                ex.Log();
            }
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);
            ProgressWindow.Instance?.SetProgress(90, "UI Loaded");
        }
        protected override void OnVisibleChanged(EventArgs e) {
            base.OnVisibleChanged(e);
            if (Visible) ProgressWindow.Instance?.Close();
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
                    "There are changes that are not saved to to game and will not take effect. " +
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
        private void TsmiResetSettings_Click(object sender, EventArgs e) {
            if (DialogResult.Yes ==
                MessageBox.Show(
                    text: "Are you sure you want to reset all settings?",
                    caption: "Reset settings?",
                    buttons: MessageBoxButtons.YesNo,
                    icon: MessageBoxIcon.Warning
                )) {
                ConfigWrapper.ResetAllConfig();
                ReloadAll();
                launchControl.LoadSettings();
                ConfigWrapper.instance.SaveConfig();
            }
        }

        private void TsmiAutoSave_CheckedChanged(object sender, EventArgs e) {
            ConfigWrapper.AutoSave = menuStrip.tsmiAutoSave.Checked;
        }

        private void TsmiAutoSave_Click(object sender, EventArgs e) =>
            menuStrip.tsmiFile.ShowDropDown(); // prevent hiding menu when clicking auto-save

        private void Export_Click(object sender, EventArgs e) {
            SaveFileDialog diaglog = new SaveFileDialog();
            diaglog.Filter = "xml files (*.xml)|*.xml";
            diaglog.InitialDirectory = LoadOrderProfile.DIR;
            if (diaglog.ShowDialog() == DialogResult.OK) {
                LoadOrderProfile profile = new LoadOrderProfile();
                dataGridMods.ModList.SaveToProfile(profile);
                PackageManager.instance.SaveToProfile(profile);
                profile.Serialize(diaglog.FileName);
            }
        }

        private void Import_Click(object sender, EventArgs e) {
            using (OpenFileDialog ofd = new OpenFileDialog()) {
                ofd.Filter = "xml files (*.xml)|*.xml";
                ofd.InitialDirectory = LoadOrderProfile.DIR;
                if (ofd.ShowDialog() == DialogResult.OK) {
                    var profile = LoadOrderProfile.Deserialize(ofd.FileName);
                    using (var opd = new OpenProfileDialog(profile)) {
                        opd.ShowDialog();
                        bool mods = opd.ItemType != OpenProfileDialog.ItemTypeT.AssetsOnly;
                        bool assets = opd.ItemType != OpenProfileDialog.ItemTypeT.ModsOnly;
                        if (opd.DialogResult == OpenProfileDialog.RESULT_REPLACE)
                            ApplyProfile(profile, mods: mods, assets: assets, replace: true);
                        else if (opd.DialogResult == OpenProfileDialog.RESULT_APPEND)
                            ApplyProfile(profile, mods: mods, assets: assets, replace: false);
                    }
                }
            }
        }

        public void ApplyProfile(LoadOrderProfile profile, bool mods, bool assets, bool replace) {
            Log.Called("mods:" + mods, "assets:" + assets, "replace:" + replace);
            if (mods) {
                dataGridMods.ModList.LoadFromProfile(profile, replace);
                dataGridMods.RefreshModList(true);
            }
            if (assets) {
                PackageManager.instance.LoadFromProfile(profile, replace);
                PopulateAssets();
            }
        }

        private void Save_Click(object sender, EventArgs e) {
            ConfigWrapper.SaveConfig();
        }

        private void ReloadAll_Click(object sender, EventArgs e) {
            ReloadAll();
        }

        public void ReloadAll() {
            dataGridMods.LoadMods(ModPredicate);
            LoadAsssets();
        }

        #endregion

        #region Mod tab
        void InitializeModTab() {
            ComboBoxIncluded.SetItems<IncludedFilter>();
            ComboBoxIncluded.SelectedIndex = 0;
            ComboBoxEnabled.SetItems<EnabledFilter>();
            ComboBoxEnabled.SelectedIndex = 0;
            ComboBoxWS.SetItems<WSFilter>();
            ComboBoxWS.SelectedIndex = 0;

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


            var buttons = ModActionPanel.Controls.OfType<Button>();
            var maxwidth = buttons.Max(b => b.Width);
            foreach (var b in buttons)
                b.MinimumSize = new Size(maxwidth, 0);

            dataGridMods.LoadMods(ModPredicate);

            dataGridMods.CellMouseClick += DataGridMods_CellMouseClick;
        }

        private void RefreshModList(object sender, EventArgs e) => dataGridMods.RefreshModList();

        public bool ModPredicate(PluginManager.PluginInfo p) {
            {
                var filter = ComboBoxIncluded.GetSelectedItem<IncludedFilter>();
                if (filter != IncludedFilter.None) {
                    bool b = filter == IncludedFilter.Included;
                    if (p.IsIncludedPending != b)
                        return false;
                }
            }
            {
                var filter = ComboBoxEnabled.GetSelectedItem<EnabledFilter>();
                if (filter != EnabledFilter.None) {
                    bool b = filter == EnabledFilter.Enabled;
                    if (p.IsEnabledPending != b)
                        return false;
                }
            }
            {
                var filter = ComboBoxWS.GetSelectedItem<WSFilter>();
                if (filter != WSFilter.None) {
                    bool b = filter == WSFilter.Workshop;
                    if (p.IsWorkshop != b)
                        return false;
                }
            }
            {
                var words = TextFilterMods.Text?.Split(" ");
                if (words != null && words.Length > 0) {
                    if (!ContainsWords(p.SearchText, words))
                        return false;
                }
            }

            return true;
        }

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
        void InitializeAssetTab() {
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

            LoadAsssets();

            dataGridAssets.CellMouseClick += DataGridAssets_CellMouseClick;
        }

        public void LoadAsssets() {

            PackageManager.instance.LoadPackages();
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
        private void ApplyAssetFilter(object sender, EventArgs e) {
            ApplyAssetFilter();
            dataGridAssets.AllowUserToResizeColumns = true;
        }

#if ASYNC_FILTER // modify this line to switch to async
        private void ApplyAssetFilter() {
            // TODO: this fails when modifying text box too fast
            dataGridAssets.Rows.Clear(); // work around : removing rows is slow.
            Task.Run(ApplyAssetFilterTask);
        }

        object changeRowsLockObject_ = new object();
        async void ApplyAssetFilterTask() {
            Log.Debug("ApplyAssetFilterTask");
            FilterAssets();
            lock (changeRowsLockObject_) {
                dataGridAssets.RowCount = dataGridAssets.AssetList.Filtered.Count;
                dataGridAssets.Refresh();
            }
        }
#else
        private void ApplyAssetFilter() {
            Log.Debug("ApplyAssetFilterTask");
            FilterAssets();
            dataGridAssets.Refresh();
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
    }
}
