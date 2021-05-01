using CO;
using CO.Packaging;
using CO.Plugins;
using CO.PlatformServices;
using LoadOrderTool.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using LoadOrderTool.Data;
using System.Threading.Tasks;
using LoadOrderTool.UI;

namespace LoadOrderTool {
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

        static ConfigWrapper ConfigWrapper => ConfigWrapper.instance;

        public LoadOrderWindow() {
            Instance = this;
            ConfigWrapper.Suspend();
            InitializeComponent();

            InitializeModTab();
            InitializeAssetTab();

            AutoSave.Checked = ConfigWrapper.AutoSave;
            Dirty = ConfigWrapper.Dirty;
            ConfigWrapper.Resume();
            ConfigWrapper.SaveConfig();
        }

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

            SaveProfile.Click += SaveProfile_Click;
            LoadProfile.Click += LoadProfile_Click;
            Save.Click += Save_Click;
            AutoSave.CheckedChanged += AutoSave_CheckedChanged;
            ReloadAll.Click += ReloadAll_Click;

            SortByHarmony.Click += SortByHarmony_Click;
            ReverseOrder.Click += ReverseOrder_Click;
            RandomizeOrder.Click += RandomizeOrder_Click;
            ResetOrder.Click += ResetOrder_Click;
            IncludeAllMods.Click += IncludeAllMods_Click;
            ExcludeAllMods.Click += ExcludeAllMods_Click;
            EnableAllMods.Click += EnableAllMods_Click;
            DisableAllMods.Click += DisableAllMods_Click;

            var buttons = ModActionPanel.Controls.OfType<Button>();
            var maxwidth = buttons.Max(b => b.Width);
            foreach (var b in buttons)
                b.MinimumSize = new Size(maxwidth, 0);

            dataGridMods.LoadMods(ModPredicate);
        }

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

        private void LoadOrderWindow_FormClosing(object sender, FormClosingEventArgs e) {
            var configWrapper = ConfigWrapper;
            if (!configWrapper.AutoSave && configWrapper.Dirty) {
                var result = MessageBox.Show(
                    caption: "Unsaved changes",
                    text: 
                    "There are changes that are not saved to to game and will not take effect. " +
                    "Save changes to game?",
                    buttons:MessageBoxButtons.YesNoCancel);
                switch (result) {
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        return;
                    case DialogResult.Yes:
                        configWrapper.SaveConfig();
                        break;
                    case DialogResult.No:
                        break;
                    default:
                        Log.Exception(new Exception("FormClosing: Unknown choice"));
                        break;
                }
                if (!e.Cancel) {
                    ConfigWrapper.Terminate();
                    GameSettings.instance.Terminate();
                }
            }
        }

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


        private void SaveProfile_Click(object sender, EventArgs e) {
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

        private void LoadProfile_Click(object sender, EventArgs e) {
            using (OpenFileDialog diaglog = new OpenFileDialog()) {
                diaglog.Filter = "xml files (*.xml)|*.xml";
                diaglog.InitialDirectory = LoadOrderProfile.DIR;
                if (diaglog.ShowDialog() == DialogResult.OK) {
                    var profile = LoadOrderProfile.Deserialize(diaglog.FileName);
                    dataGridMods.ModList.LoadFromProfile(profile);
                    PackageManager.instance.LoadFromProfile(profile);
                    dataGridMods.RefreshModList(true);
                    PopulateAssets();
                }
            }
        }

        private void Save_Click(object sender, EventArgs e) {
            ConfigWrapper.SaveConfig();
        }

        private void AutoSave_CheckedChanged(object sender, EventArgs e) {
            ConfigWrapper.AutoSave = AutoSave.Checked;
        }

        private void ReloadAll_Click(object sender, EventArgs e) {
            dataGridMods.LoadMods(ModPredicate);
            LoadAsssets();
        }

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

                dataGridAssets.AssetList = new AssetList(PackageManager.instance.GetAssets(), FilterAssets);
            } catch (Exception ex) {
                Log.Exception(ex);
            } finally {
                dataGridAssets.ResumeLayout();
                dataGridAssets.Refresh();
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


#endregion
    }
}
