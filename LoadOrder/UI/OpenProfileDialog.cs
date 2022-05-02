namespace LoadOrderTool.UI {
    using CO.Packaging;
    using CO.Plugins;
    using LoadOrderTool.Util;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Windows.Forms;
    using LoadOrderTool.Data;

    public partial class OpenProfileDialog : Form {
        [Flags]
        public enum ItemTypeT {
            Mods = 1,
            Assets = 2,
            DLCs = 4,
            [Text("Skip file")] SkipFile = 8,
        }

        public const DialogResult RESULT_APPEND = DialogResult.Yes;
        public const DialogResult RESULT_REPLACE = DialogResult.OK;

        LoadOrderProfile Profile;

        List<IProfileItem> MissingItems;

        public ItemTypeT ItemTypes {
            get {
                ItemTypeT ret = 0;
                foreach (ItemTypeT item in cbItemType.GetSelectedItems<ItemTypeT>()) {
                    ret |= item;
                }
                return ret;
            }
        }

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
            foreach (var item in cbItemType.CheckBoxItems)
                item.Checked = true;

            Populate();

            btnCancel.Click += BtnCancel_Click;
            btnReplace.Click += BtnReplace_Click;
            btnAppend.Click += BtnAppend_Click;
            btnReload.Click += BtnReload_Click;

            btnAppend.SetTooltip("current mods/assets + profile mods/assets");
            btnReplace.SetTooltip("profile mods/assets only");
            SubscribeAll.SetTooltip("subscribe to all the missing workshop items from the table above");
            cbItemType.AutoSize();
        }

        private async void BtnReload_Click(object sender, EventArgs e) {
            await LoadOrderWindow.Instance.ReloadAll();
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
            var missingMods = Profile.Mods.Where(m => m.IsIncluded && !PluginExists(m.GetIncludedPath()));
            var missingAssets = Profile.Assets.Where(m => m.IsIncluded && !AssetExists(m.GetIncludedPath()));
            MissingItems = missingMods.Concat<IProfileItem>(missingAssets).ToList();
            dataGridView1.Rows.Clear();
            dataGridView1.RowCount = MissingItems.Count;
            dataGridView1.Refresh();
            bool anyMissingItems = MissingItems.Any();
            dataGridView1.Visible = anyMissingItems;
            if (!anyMissingItems) {
                lblMissingItems.Text = "No Missing Items.";
            }
            SubscribeAll.Enabled = GetMissingIDs().Length > 0;
        }

        protected void DataGridView1_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e) {
            try {
                if (MissingItems == null) return;
                if (e.RowIndex >= MissingItems.Count) return;
                var item = MissingItems[e.RowIndex];
                if (e.ColumnIndex == cID.Index) {
                    if (item.TryGetID(out var id))
                        e.Value = id.ToString();
                    else
                        e.Value = "Local";
                } else if (e.ColumnIndex == cName.Index) {
                    e.Value = item.GetDisplayText();
                } else if (e.ColumnIndex == cType.Index) {
                    e.Value = item.GetCategoryName();
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
                    if (item.TryGetID(out var id)) {
                        string url = ContentUtil.GetItemURL(id.ToString());
                        e.ToolTipText = url;
                    } else {
                        e.ToolTipText = item.GetIncludedPath() ?? "";
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
                    if (item.TryGetID(out var id)) {
                        string url = ContentUtil.GetItemURL(id.ToString());
                        ContentUtil.OpenURL(url);
                    } else {
                        ContentUtil.OpenPath(item.GetIncludedPath());
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

        public ulong[] GetMissingIDs() {
            List<ulong> ids = new List<ulong>();
            foreach (var item in MissingItems) {
                if (item.TryGetID(out ulong id))
                    ids.Add(id);
            }
            return ids.ToArray();
        }

        private void SubscribeAll_Click(object sender, EventArgs e) {
            ContentUtil.Subscribe(GetMissingIDs());
        }
    }
}
