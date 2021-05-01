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
        int prevAssetSortCol_ = -1;
        bool assetSortAssending_ = false;

        public AssetList AssetList;



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

            dataGridAssets.DataError += DataGridAssets_DataError;


            dataGridAssets.VirtualMode = true;
            foreach(DataGridViewColumn col in dataGridAssets.Columns) {
                col.SortMode = DataGridViewColumnSortMode.Programmatic;
                col.Width += 1; // workaround : show hyphon
            }

            dataGridAssets.CellValueNeeded += DataGridAssets_CellValueNeeded; ;
            dataGridAssets.CellToolTipTextNeeded += DataGridAssets_CellToolTipTextNeeded;
            dataGridAssets.CellContentClick += DataGridAssets_CellContentClick;
            dataGridAssets.CellValuePushed += dataGridAssets_CellValuePushed;
            dataGridAssets.CurrentCellDirtyStateChanged += dataGridAssets_CurrentCellDirtyStateChanged;

            dataGridAssets.ColumnHeaderMouseClick += DataGridAssets_ColumnHeaderMouseClick;
            dataGridAssets.VisibleChanged += DataGridAssets_VisibleChanged;
        }

        bool firstTime_ = true;
        private void DataGridAssets_VisibleChanged(object sender, EventArgs e) {
            if (dataGridAssets.Visible && firstTime_) {
                dataGridAssets.AutoResizeColumns();
                firstTime_ = false;
            }
        }

        // override error report.
        private void DataGridAssets_DataError(object sender, DataGridViewDataErrorEventArgs e) {
            Log.Exception(e.Exception);
            e.Cancel = true;
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

                AssetList = new AssetList(PackageManager.instance.GetAssets());
                FilterAssets();
                dataGridAssets.RowCount = AssetList.Filtered.Count;

                ComboBoxAssetTags.Items.Clear();
                ComboBoxAssetTags.Items.Add(NO_TAGS);
                ComboBoxAssetTags.Items.AddRange(PackageManager.instance.GetAllTags());
                ComboBoxAssetTags.SelectedIndex = 0;

                
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
                dataGridAssets.RowCount = AssetList.Filtered.Count;
                dataGridAssets.Refresh();
            }
        }
#else
        private void ApplyAssetFilter() {
            Log.Debug("ApplyAssetFilterTask");
            FilterAssets();
            if(dataGridAssets.RowCount > AssetList.Filtered.Count)
                dataGridAssets.Rows.Clear(); // work around : removing rows is slow.
            dataGridAssets.RowCount = AssetList.Filtered.Count;
            dataGridAssets.Refresh();
        }
#endif
        public void FilterAssets() {
            var includedFilter = ComboBoxAssetIncluded.GetSelectedItem<IncludedFilter>();
            var wsFilter = ComboBoxAssetWS.GetSelectedItem<WSFilter>();
            var tagFilter = ComboBoxAssetTags.SelectedItem as string;
            if (tagFilter == NO_TAGS) tagFilter = null;
            var words = TextFilterAsset.Text?.Split(" ");
            AssetList.FilterItems(item => AssetPredicateFast(item, includedFilter, wsFilter, tagFilter, words));
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
                foreach (var asset in AssetList.Filtered) {
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

        // sort
        private void DataGridAssets_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e) {
            try {
                if (e.ColumnIndex == prevAssetSortCol_) {
                    assetSortAssending_ = !assetSortAssending_;
                } else {
                    assetSortAssending_ = true;
                    foreach (DataGridViewColumn col in dataGridAssets.Columns)
                        col.HeaderCell.SortGlyphDirection = SortOrder.None;
                }
                var sortOrder = assetSortAssending_ ? SortOrder.Ascending : SortOrder.Descending;
                dataGridAssets.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = sortOrder;
                prevAssetSortCol_ = e.ColumnIndex;


                if (e.ColumnIndex == cIncluded.Index) {
                    AssetList.SortItemsBy(item => item.IsIncludedPending, assetSortAssending_);
                } else if (e.ColumnIndex == cAssetID.Index) {
                    AssetList.SortItemsBy(item => item.publishedFileID.AsUInt64, assetSortAssending_);
                } else if (e.ColumnIndex == cName.Index) {
                    AssetList.SortItemsBy(item => item.DisplayText, assetSortAssending_);
                } else if (e.ColumnIndex == cAuthor.Index) {
                    // "[unknown" is sorted before "[unknown]". This puts empty before unknown author.
                    AssetList.SortItemsBy(item => item.ConfigAssetInfo.Author ?? "[unknown", assetSortAssending_);
                } else if (e.ColumnIndex == cDate.Index) {
                    AssetList.SortItemsBy(item => item.ConfigAssetInfo.Date, assetSortAssending_);
                } else if (e.ColumnIndex == cTags.Index) {
                    AssetList.SortItemsBy(item => item.StrTags, assetSortAssending_);
                }

                ApplyAssetFilter();
            } catch (Exception ex) {
                Log.Exception(ex);
            }
        }

        //read
        private void DataGridAssets_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e) {
            try {
                if (e.RowIndex >= AssetList.Filtered.Count) return;
                var asset = AssetList.Filtered[e.RowIndex];
                if (e.ColumnIndex == cIncluded.Index) {
                    e.Value = asset.IsIncludedPending;
                } else if (e.ColumnIndex == cAssetID.Index) {
                    string id = asset.publishedFileID.AsUInt64.ToString();
                    if (id == "0" || asset.publishedFileID == PublishedFileId.invalid)
                        id = "";
                    e.Value = id;
                } else if (e.ColumnIndex == cName.Index) {
                    e.Value = asset.DisplayText ?? "";
                } else if (e.ColumnIndex == cAuthor.Index) {
                    e.Value = asset.ConfigAssetInfo.Author ?? "";
                } else if (e.ColumnIndex == cDate.Index) {
                    e.Value = asset.ConfigAssetInfo.Date ?? "";
                } else if (e.ColumnIndex == cTags.Index) {
                    e.Value = asset.StrTags;
                }
            } catch (Exception ex) {
                Log.Exception(ex);
            }
        }

        // tooltip
        private void DataGridAssets_CellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e) {
            try {
                if (e.RowIndex < 0 || e.RowIndex >= AssetList.Filtered.Count) return;
                var asset = AssetList.Filtered[e.RowIndex];
                if (e.ColumnIndex == cAssetID.Index) {
                    var id = asset.publishedFileID;
                    string url = ContentUtil.GetItemURL(asset.publishedFileID);
                    e.ToolTipText = url ?? "local";
                }
            } catch (Exception ex) {
                Log.Exception(ex, $"rowIndex={e.RowIndex}");
            }
        }

        // click
        private void DataGridAssets_CellContentClick(object sender, DataGridViewCellEventArgs e) {
            try {
                if (e.RowIndex < 0 || e.RowIndex >= AssetList.Filtered.Count) return;
                var asset = AssetList.Filtered[e.RowIndex];

                if (e.ColumnIndex == cAssetID.Index) {
                    var id = asset.publishedFileID;
                    string url = ContentUtil.GetItemURL(asset.publishedFileID);
                    if (url != null)
                        ContentUtil.OpenURL(url);
                }   
            } catch (Exception ex) {
                Log.Exception(ex);
            }
        }

        //write
        private void dataGridAssets_CellValuePushed(object sender, DataGridViewCellValueEventArgs e) {
            var assetInfo = AssetList.Filtered[e.RowIndex];

            if (e.ColumnIndex == cIncluded.Index) {
                assetInfo.IsIncludedPending = (bool)e.Value;
                //Log.Debug($"{assetInfo} | IsIncludedPending changes to {cell.Value}");
            } else {
                Log.Error("unexpected column changed: " + dataGridAssets.Columns[e.ColumnIndex]?.Name);
            }
        }

        // instant modify.
        private void dataGridAssets_CurrentCellDirtyStateChanged(object sender, EventArgs e) {
            if (dataGridAssets.CurrentCell is DataGridViewCheckBoxCell) {
                dataGridAssets.EndEdit();
            }
        }
#endregion
    }
}
