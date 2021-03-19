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

namespace LoadOrderTool {
    public partial class LoadOrderWindow : Form {
        enum EnabledFilter {
            None = 0,
            Enabled,
            Disabled,
        }
        enum IncludedFilter {
            None = 0,
            Included,
            Excluded,
        }
        enum WSFilter {
            None = 0,
            Workshop,
            Local,
        }

        public static LoadOrderWindow Instance;

        ModList ModList;

        public LoadOrderWindow() {
            Instance = this;
            InitializeComponent();
            dataGridViewMods.AlternatingRowsDefaultCellStyle.BackColor = Color.Beige;

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

            this.SaveProfile.Click += SaveProfile_Click;
            this.LoadProfile.Click += LoadProfile_Click;
            this.Save.Click += Save_Click;
            this.AutoSave.CheckedChanged += AutoSave_CheckedChanged;
            this.ReloadAll.Click += ReloadAll_Click;

            this.SortByHarmony.Click += SortByHarmony_Click;
            this.ReverseOrder.Click += ReverseOrder_Click;
            this.RandomizeOrder.Click += RandomizeOrder_Click;
            this.ResetOrder.Click += ResetOrder_Click;
            this.IncludeAllMods.Click += IncludeAllMods_Click;
            this.ExcludeAllMods.Click += ExcludeAllMods_Click;
            this.EnableAllMods.Click += EnableAllMods_Click;
            this.DisableAllMods.Click += DisableAllMods_Click;

            var buttons = ModsButtons.Controls.OfType<Button>();
            var maxwidth = buttons.Max(b => b.Width);
            foreach (var b in buttons)
                b.MinimumSize = new Size(maxwidth, 0);

            dataGridViewMods.CellFormatting += dataGridViewMods_CellFormatting;
            dataGridViewMods.CellValueChanged += dataGridViewMods_CellValueChanged;
            dataGridViewMods.CurrentCellDirtyStateChanged += dataGridViewMods_CurrentCellDirtyStateChanged;
            dataGridViewMods.EditingControlShowing += dataGridViewMods_EditingControlShowing;

            LoadMods();
            InitializeAssetTab();
        }

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
                var words = TextFilterMods.Text?.Split("");
                if (words != null && words.Length > 0) {
                    bool match = words.Any(word => p.DisplayText.Contains(word, StringComparison.OrdinalIgnoreCase));
                    if (!match)
                        return false;
                }
            }

            return true;
        }

        public void LoadMods() {
            PluginManager.instance.LoadPlugins();
            ModList = ModList.GetAllMods();
            RefreshModList(true);
        }

        public void RefreshModList(bool sort = false) {
            if (sort)
                ModList.DefaultSort();
            ModList.FilterIn(ModPredicate);
            PopulateMods();
        }

        public void PopulateMods() {
            SuspendLayout();
            var rows = this.dataGridViewMods.Rows;
            rows.Clear();
            Log.Info("Populating");
            foreach (var p in ModList.Filtered) {
                string savedKey = p.savedEnabledKey_;
                Log.Debug($"plugin info: dllName={p.dllName} harmonyVersion={ ModList.GetHarmonyOrder(p)} " +
                     $"savedKey={savedKey} modPath={p.ModPath}");
            }
            foreach (var mod in ModList.Filtered) {
                rows.Add(mod.LoadOrder, mod.IsIncludedPending, mod.IsEnabledPending, mod.DisplayText);
                Log.Debug("row added: " + mod.ToString());
            }
            ResumeLayout();
        }

        private void U32TextBox_KeyPress(object sender, KeyPressEventArgs e) {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) {
                e.Handled = true;
            }
        }

        private void U32TextBox_Submit(object? sender, EventArgs e) {
            if (sender is TextBox tb) {
                if (tb.Text == "")
                    tb.Text = "0";
                else
                    tb.Text = UInt32.Parse(tb.Text).ToString();
            }
        }



        private void RefreshModList(object sender, EventArgs e) => RefreshModList();

        private void LoadOrderWindow_FormClosing(object sender, FormClosingEventArgs e) {
            var configWrapper = PackageManager.instance.ConfigWrapper;
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
                        break;
                    case DialogResult.Yes:
                        configWrapper.SaveConfig();
                        break;
                    case DialogResult.No:
                        break;
                    default:
                        Log.Exception(new Exception("FormClosing: Unknown choice"));
                        break;
                }
            }
        }

        private void dataGridViewMods_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e) {
            if (dataGridViewMods.CurrentCell.ColumnIndex == 0 && e.Control is TextBox tb) // Desired Column
            {
                tb.KeyPress -= U32TextBox_KeyPress;
                tb.Leave -= U32TextBox_Submit;
                tb.KeyPress += U32TextBox_KeyPress;
                tb.Leave += U32TextBox_Submit;
            }
        }

        private void dataGridViewMods_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
            Log.Debug("dataGridViewMods_CellValueChanged() called");
            var plugin = ModList.Filtered[e.RowIndex];
            var cell = dataGridViewMods.Rows[e.RowIndex].Cells[e.ColumnIndex];
            var col = cell.OwningColumn;

            if (col == LoadIndex) {
                int newVal = Int32.Parse(cell.Value as string);
                ModList.MoveItem(ModList.Filtered[e.RowIndex], newVal);
                RefreshModList();
            } else if (col == ModEnabled) {
                plugin.IsEnabledPending = (bool)cell.Value;
            } else if (col == IsIncluded) {
                plugin.IsIncludedPending = (bool)cell.Value;
            } else {
                return;
            }
        }

        private void dataGridViewMods_CurrentCellDirtyStateChanged(object sender, EventArgs e) {
            if (dataGridViewMods.CurrentCell is DataGridViewCheckBoxCell) {
                dataGridViewMods.EndEdit();
            }
        }

        private void SortByHarmony_Click(object sender, EventArgs e) {
            ModList.ResetLoadOrders();
            ModList.SortBy(ModList.HarmonyComparison);
            RefreshModList();
        }

        private void EnableAllMods_Click(object sender, EventArgs e) {
            foreach (var p in ModList.Filtered)
                p.IsEnabledPending = true;
            PopulateMods();
        }

        private void DisableAllMods_Click(object sender, EventArgs e) {
            foreach (var p in ModList.Filtered)
                p.IsEnabledPending = false;
            PopulateMods();
        }

        private void IncludeAllMods_Click(object sender, EventArgs e) {
            foreach (var p in ModList.Filtered)
                p.IsIncludedPending = true;
            PopulateMods();
        }

        private void ExcludeAllMods_Click(object sender, EventArgs e) {
            foreach (var p in ModList.Filtered)
                p.IsIncludedPending = false;
            PopulateMods();
        }

        private void ReverseOrder_Click(object sender, EventArgs e) {
            ModList.ReverseOrder();
            RefreshModList();
        }

        private void RandomizeOrder_Click(object sender, EventArgs e) {
            ModList.RandomizeOrder();
            RefreshModList();
        }


        private void SaveProfile_Click(object sender, EventArgs e) {
            SaveFileDialog diaglog = new SaveFileDialog();
            diaglog.Filter = "xml files (*.xml)|*.xml";
            diaglog.InitialDirectory = LoadOrderProfile.DIR;
            if (diaglog.ShowDialog() == DialogResult.OK) {
                ModList.SaveProfile(diaglog.FileName);
            }
        }

        private void LoadProfile_Click(object sender, EventArgs e) {
            using (OpenFileDialog diaglog = new OpenFileDialog()) {
                diaglog.Filter = "xml files (*.xml)|*.xml";
                diaglog.InitialDirectory = LoadOrderProfile.DIR;
                if (diaglog.ShowDialog() == DialogResult.OK) {
                    ModList.LoadProfile(diaglog.FileName);
                    ModList.DefaultSort();
                    RefreshModList(true);
                }
            }
        }

        private void Save_Click(object sender, EventArgs e) {
            PluginManager.instance.ConfigWrapper.SaveConfig();
        }

        private void AutoSave_CheckedChanged(object sender, EventArgs e) {
            PluginManager.instance.ConfigWrapper.AutoSave = AutoSave.Checked;
        }

        private void ReloadAll_Click(object sender, EventArgs e) {
            LoadMods();
            LoadAsssets();
        }

        private void dataGridViewMods_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) {
            //Log.Info($"e.ColumnIndex={e.ColumnIndex} Description.Index={Description.Index}");
            if (e.ColumnIndex == Description.Index && e.Value != null) {
                var cell = dataGridViewMods.Rows[e.RowIndex].Cells[e.ColumnIndex];
                cell.ToolTipText = ModList.Filtered[e.RowIndex].ModInfo.Description;
            }
        }

        #region AssetTab
        class ObjectCell : DataGridViewCell {
            object value_;
            protected override object GetValue(int rowIndex) => value_;
            protected override bool SetValue(int rowIndex, object value) {
                value_ = value;
                return true;
            }
            public override bool Visible => false;

            public override object Clone() => new ObjectCell { value_ = value_ };
        }


        DataGridViewColumn cAsset;

        void InitializeAssetTab() {
            cAsset = new DataGridViewColumn(new ObjectCell()) {
                Name = "AssetColumn",
                HeaderText = "AssetColumn",
                Visible = false,
                ReadOnly = true,
            };
            dataGridAssets.Columns.Add(cAsset);

            ComboBoxAssetIncluded.SetItems<IncludedFilter>();
            ComboBoxAssetIncluded.SelectedIndex = 0;
            ComboBoxAssetWS.SetItems<WSFilter>();
            ComboBoxAssetWS.SelectedIndex = 0;

            ComboBoxAssetIncluded.SelectedIndexChanged += FilterAssetRows;
            ComboBoxAssetWS.SelectedIndexChanged += FilterAssetRows;
            ComboBoxAssetTags.SelectedIndexChanged += FilterAssetRows;
            TextFilterAsset.TextChanged += FilterAssetRows;

            this.IncludeAllAssets.Click += IncludeAllAssets_Click;
            this.ExcludeAllAssets.Click += ExcludeAllAssets_Click;

            var buttons = AssetsActionsPanel.Controls.OfType<Button>();
            var maxwidth = buttons.Max(b => b.Width);
            foreach (var b in buttons)
                b.MinimumSize = new Size(maxwidth, 0);

            LoadAsssets();

            dataGridAssets.CellValueChanged += dataGridAssets_CellValueChanged;
            dataGridAssets.CurrentCellDirtyStateChanged += dataGridAssets_CurrentCellDirtyStateChanged;
        }

        private void FilterAssetRows(object sender, EventArgs e) => FilterAssetRows();

        public bool AssetPredicate(PackageManager.AssetInfo a) {
            {
                var filter = ComboBoxAssetIncluded.GetSelectedItem<IncludedFilter>();
                if (filter != IncludedFilter.None) {
                    bool b = filter == IncludedFilter.Included;
                    if (a.IsIncludedPending != b)
                        return false;
                }
            }
            {
                var filter = ComboBoxAssetWS.GetSelectedItem<WSFilter>();
                if (filter != WSFilter.None) {
                    bool b = filter == WSFilter.Workshop;
                    if (a.IsWorkshop != b)
                        return false;
                }
            }
            {
                var filter = ComboBoxAssetTags.SelectedItem as string;
                if(filter != "None" && !a.GetTags().Contains(filter)) {
                    return false;
                }
            }
            {
                var words = TextFilterAsset.Text?.Split(" ");
                if (ContainsWords(a.DisplayText, words))
                    return true;
                if (ContainsWords(a.ConfigAssetInfo.Author, words))
                    return true;
            }
            return true;
        }

        public static bool ContainsWords(string text, IEnumerable<string> words) {
            if (string.IsNullOrWhiteSpace(text) || words == null || !words.Any())
                return false;
            foreach (var word in words) {
                if (text.Contains(word, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        public void LoadAsssets() {
            PackageManager.instance.LoadPackages();
            PopulateAssets();
            FilterAssetRows();
        }

        public void PopulateAssets() {
            Log.Info("Populating assets");
            dataGridAssets.SuspendLayout();
            try {
                dataGridAssets.Rows.Clear();

                foreach (var asset in PackageManager.instance.GetAssets()) {
                    string id = asset.publishedFileID.AsUInt64.ToString();
                    if (id == "0" || asset.publishedFileID == PublishedFileId.invalid)
                        id = "";
                    var tags = asset.ConfigAssetInfo.Tags;
                    var strTags = tags != null ? string.Join(", ", tags) : "";
                    int row = dataGridAssets.Rows.Add(
                        asset.IsIncludedPending,
                        id,
                        asset.AssetName,
                        asset.ConfigAssetInfo.Author,
                        asset.ConfigAssetInfo.Date,
                        strTags,
                        asset);
                    dataGridAssets.Rows[row].Cells[cName.Index].ToolTipText = 
                        asset.ConfigAssetInfo.description;
                }

                ComboBoxAssetTags.Items.Clear();
                ComboBoxAssetTags.Items.Add("None");
                ComboBoxAssetTags.Items.AddRange(PackageManager.instance.GetAllTags());
                ComboBoxAssetTags.SelectedIndex = 0;
            } catch (Exception ex) {
                Log.Exception(ex);
            } finally {
                dataGridAssets.ResumeLayout();
            }
        }

        public void FilterAssetRows() {
            foreach (DataGridViewRow row in dataGridAssets.Rows) {
                var asset = row.Cells[cAsset.Index].Value as PackageManager.AssetInfo;
                row.Visible = AssetPredicate(asset);
            }
        }

        public void IncludeExcludeVisibleAssets(bool value) {
            foreach (DataGridViewRow row in dataGridAssets.Rows) {
                if (!row.Visible) continue;
                var asset = row.Cells[cAsset.Index].Value as PackageManager.AssetInfo;
                row.Cells[cIncluded.Index].Value = asset.IsIncludedPending = value;
            }
        }

        private void IncludeAllAssets_Click(object sender, EventArgs e) {
            IncludeExcludeVisibleAssets(true);
        }

        private void ExcludeAllAssets_Click(object sender, EventArgs e) {
            IncludeExcludeVisibleAssets(false);
        }

        private void dataGridAssets_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
            Log.Debug("dataGridAssets_CellValueChanged() called");
            var row = dataGridAssets.Rows[e.RowIndex];
            var assetInfo = row.Cells[cAsset.Index].Value as PackageManager.AssetInfo;
            var cell = row.Cells[e.ColumnIndex];
            var col = cell.OwningColumn;
            if (col == cIncluded) {
                assetInfo.IsIncludedPending = (bool)cell.Value;
                Log.Debug($"{assetInfo} | IsIncludedPending changes to {cell.Value}");
            } else {
                Log.Error("unexpected column changed: " + col.Name);
            }
        }

        /// <summary>
        /// makes clicking checkbox to immidiately takes effect without the need for user to press enter.
        /// </summary>
        private void dataGridAssets_CurrentCellDirtyStateChanged(object sender, EventArgs e) {
            if (dataGridAssets.CurrentCell is DataGridViewCheckBoxCell) {
                dataGridAssets.EndEdit();
            }
        }
        #endregion

        private void ResetOrder_Click(object sender, EventArgs e) {
            foreach (var mod in ModList)
                mod.ResetLoadOrder();
            RefreshModList(sort:true);
        }
    }
}
