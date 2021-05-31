namespace LoadOrderTool.UI {
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Windows.Forms;
    using CO.Packaging;
    using CO.Plugins;
    using LoadOrderTool.Util;
    using System.IO;

    public partial class OpenProfileDialog : Form {
        public enum ItemTypeT {
            [Text("All items")] AllItems,
            [Text("Mods only")] ModsOnly,
            [Text("Assets only")] AssetsOnly,
        }

        public const DialogResult RESULT_APPEND = DialogResult.Yes;
        public const DialogResult RESULT_REPLACE = DialogResult.OK;

        LoadOrderProfile Profile;

        List<object> MissingItems;

        public ItemTypeT ItemType => cbItemType.GetSelectedItem<ItemTypeT>();

        public OpenProfileDialog(LoadOrderProfile profile) {
            Profile = profile;
            InitializeComponent();
            dataGridView1.VirtualMode = true;
            dataGridView1.CellValueNeeded += DataGridView1_CellValueNeeded;
            dataGridView1.CellToolTipTextNeeded += DataGridView1_CellToolTipTextNeeded;
            dataGridView1.CellContentClick += DataGridView1_CellContentClick;
            dataGridView1.VisibleChanged += DataGridView1_VisibleChanged;
            dataGridView1.DataError += DataGridView1_DataError;
            cbItemType.SetItems<ItemTypeT>();
            cbItemType.SelectedIndex = 0;

            Populate();

            btnCancel.Click += BtnCancel_Click;
            btnReplace.Click += BtnReplace_Click;
            btnAppend.Click += BtnAppend_Click;
            btnReload.Click += BtnReload_Click;
        }

        private void BtnReload_Click(object sender, EventArgs e) {
            LoadOrderWindow.Instance.ReloadAll();
            Populate();
        }

        private void BtnAppend_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void BtnReplace_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        static bool PluginExists(string includedPath) =>
            PluginManager.instance.GetPlugin(includedPath) != null;

        static bool AssetExists(string includedPath) =>
            PackageManager.instance.GetAsset(includedPath) != null;

        private void Populate() {
            var missingMods = Profile.Mods.Where(m => m.IsIncluded && !PluginExists(m.IncludedPathFinal));
            var missingAssets = Profile.Assets.Where(m => m.IsIncluded && !AssetExists(m.IncludedPath));
            MissingItems = missingMods.Concat<object>(missingAssets).ToList();
            dataGridView1.Rows.Clear();
            dataGridView1.RowCount = MissingItems.Count;
            dataGridView1.Refresh();
        }

        static string GetItemPath(object item)
        {
            if (item is LoadOrderProfile.Mod mod)
                return mod.IncludedPathFinal;
            else if (item is LoadOrderProfile.Asset asset)
                return asset.IncludedPath;
            else
                throw new Exception("unknow item : " + item);
        }

        static bool TryGetItemId(object item, out ulong id) {
            string path = GetItemPath(item);
            return ContentUtil.TryGetID(path, out id);
        }

        protected void DataGridView1_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e) {
            try {
                if (MissingItems == null) return;
                if (e.RowIndex >= MissingItems.Count) return;
                var item = MissingItems[e.RowIndex];
                if (e.ColumnIndex == cID.Index) {
                    if (TryGetItemId(item, out var id))
                        e.Value = id.ToString();
                    else
                        e.Value = "Local";
                } else if (e.ColumnIndex == cName.Index) {
                    if (item is LoadOrderProfile.Mod mod)
                        e.Value = mod.DisplayText ?? "";
                    else if (item is LoadOrderProfile.Asset asset)
                        e.Value = asset.DisplayText ?? "";
                } else if (e.ColumnIndex == cType.Index) {
                    if (item is LoadOrderProfile.Mod mod)
                        e.Value = "Mod";
                    else if (item is LoadOrderProfile.Asset asset)
                        e.Value = "Asset";
                }
            } catch (Exception ex) {
                Log.Exception(ex);
            }
        }

        // tooltip
        protected void DataGridView1_CellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e) {
            try {
                if (MissingItems == null) return;
                if (e.RowIndex < 0 || e.RowIndex >= MissingItems.Count) return;
                var item = MissingItems[e.RowIndex];
                if (e.ColumnIndex == cID.Index) {
                    if (TryGetItemId(item, out var id)) {
                        string url = ContentUtil.GetItemURL(id.ToString());
                        e.ToolTipText = url;
                    } else {
                        e.ToolTipText = GetItemPath(item) ?? "";
                    }
                }
            } catch (Exception ex) {
                Log.Exception(ex, $"rowIndex={e.RowIndex}");
            }
        }

        // click link/button
        protected void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) {
            try {
                if (MissingItems == null) return;
                if (e.RowIndex < 0 || e.RowIndex >= MissingItems.Count) return;
                var item = MissingItems[e.RowIndex];

                if (e.ColumnIndex == cID.Index) {
                    if (TryGetItemId(item, out var id)) {
                        string url = ContentUtil.GetItemURL(id.ToString());
                        ContentUtil.OpenURL(url);
                    } else {
                        ContentUtil.OpenPath(GetItemPath(item));
                    }
                }
            } catch (Exception ex) {
                Log.Exception(ex);
            }
        }


        protected void DataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e) {
            Log.Exception(e.Exception);
            e.Cancel = true;
        }

        bool firstTime_;
        protected void DataGridView1_VisibleChanged(object sender, EventArgs e) {
            if (Visible && firstTime_) {
                dataGridView1.AutoResizeColumns();
                firstTime_ = false;
            }
        }
    }
}
