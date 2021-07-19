namespace LoadOrderTool.UI {
    using CO;
    using LoadOrderTool.Data;
    using System;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;
    public partial class DLCControl : UserControl {
        public LoadOrderToolSettings settings_ => LoadOrderToolSettings.Instace;

        static ConfigWrapper ConfigWrapper => ConfigWrapper.instance;

        public DLCList DLCList;

        int prevSortCol_ = -1;
        bool sortAssending_ = false;
        bool firstTime_ = true;

        public DLCControl() {
            InitializeComponent();
            foreach (DataGridViewColumn col in dgDLCs.Columns) {
                col.SortMode = DataGridViewColumnSortMode.Programmatic;
                col.Width += 1; // workaround : show Glyph
            }

            ComboBoxDLCType.SetItems<DLCType>();
            ComboBoxDLCType.SelectedIndex = 0;
            ComboBoxDLCType.SelectedIndexChanged += ApplyDLCFilter;
            TextFilterDLCs.TextChanged += ApplyDLCFilter;

            IncludeAlDLCs.Click += IncludeAllDLCs_Click;
            ExcludeAllDLCs.Click += ExcludeAllDLCs_Click;

            var buttons = DLCActionPanel.Controls.OfType<Button>();
            var maxwidth = buttons.Max(b => b.Width);
            foreach (var b in buttons)
                b.MinimumSize = new Size(maxwidth, 0);

            LoadDLCs();

            dgDLCs.CellValueNeeded += DgDLCs_CellValueNeeded;
            dgDLCs.CellValuePushed += DgDLCs_CellValuePushed;
            dgDLCs.CurrentCellDirtyStateChanged += DgDLCs_CurrentCellDirtyStateChanged;
            dgDLCs.ColumnHeaderMouseClick += DgDLCs_ColumnHeaderMouseClick;
            dgDLCs.DataError += DgDLCs_DataError;
            dgDLCs.VisibleChanged += DgDLCs_VisibleChanged;
        }

        public void LoadDLCs() {
            DLCManager.instance.Load();
            PopulateDLCs();
        }

        public void PopulateDLCs() {
            Log.Info("Populating DLCs");
            dgDLCs.SuspendLayout();
            try {
                dgDLCs.Rows.Clear();
                DLCList = new DLCList(DLCManager.instance.DLCs, FilterDLCs);
            } catch (Exception ex) {
                Log.Exception(ex);
            } finally {
                dgDLCs.ResumeLayout();
                DgDLCs_Refresh();
            }
        }

        public void IncludeExcludeFilteredDLCs(bool value) {
            try {
                Log.Debug("IncludeExcludeVisibleDLCs() Called.");
                ConfigWrapper.Suspend();
                foreach (var DLC in DLCList.Filtered) {
                    DLC.IsIncluded = value;
                }
                DgDLCs_Refresh();
            } catch (Exception ex) {
                Log.Exception(ex);
            } finally {
                ConfigWrapper.Resume();
                Log.Debug("IncludeExcludeVisibleDLCs() Finished!");
            }
        }

        private void IncludeAllDLCs_Click(object sender, EventArgs e) {
            IncludeExcludeFilteredDLCs(true);
        }

        private void ExcludeAllDLCs_Click(object sender, EventArgs e) {
            IncludeExcludeFilteredDLCs(false);
        }

        #region filter
        private void ApplyDLCFilter(object sender, EventArgs e) {
            ApplyDLCFilter();
            dgDLCs.AllowUserToResizeColumns = true;
        }

        private void ApplyDLCFilter() {
            Log.Debug("ApplyDLCFilterTask");
            FilterDLCs();
            DgDLCs_Refresh();
        }
        public void FilterDLCs() => DLCList?.FilterItems();

        public void FilterDLCs(DLCList DLCList) {
            var dlcTypes = ComboBoxDLCType.GetSelectedItem<DLCType>();
            var words = TextFilterDLCs.Text?.Split(" ");
            DLCList.FilterItems(item => DLCPredicateFast(item, dlcTypes, words));
        }

        bool DLCPredicateFast(
            DLCManager.DLCInfo a,
            DLCType dlcType,
            string[] words) {
            if (dlcType != DLCType.None) {
                if (a.DLCType != dlcType)
                    return false;
            }
            if (words != null && words.Length > 0) {
                if (!LoadOrderWindow.ContainsWords(a.Text, words)) {
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region DataGrid
        //read
        public void DgDLCs_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e) {
            try {
                if (DLCList == null) return;
                if (e.RowIndex >= DLCList.Filtered.Count) return;
                var DLC = DLCList.Filtered[e.RowIndex];
                if (e.ColumnIndex == CInclude.Index) {
                    e.Value = DLC.IsIncluded;
                } else if (e.ColumnIndex == CName.Index) {
                    e.Value = DLC.Text ?? "";
                } else if (e.ColumnIndex == CDLCType.Index) {
                    e.Value = DLC.DLCType;
                }
            } catch (Exception ex) {
                Log.Exception(ex);
            }
        }

        //write
        public void DgDLCs_CellValuePushed(object sender, DataGridViewCellValueEventArgs e) {
            if (DLCList == null) return;
            var DLCInfo = DLCList.Filtered[e.RowIndex];
            if (e.ColumnIndex == CInclude.Index) {
                DLCInfo.IsIncluded = (bool)e.Value;
                //Log.Debug($"{DLCInfo} | IsIncluded changes to {cell.Value}");
            } else {
                Log.Error("unexpected column changed: " + dgDLCs.Columns[e.ColumnIndex]?.Name);
            }
        }


        // instant update
        public void DgDLCs_CurrentCellDirtyStateChanged(object sender, EventArgs e) {
            if (dgDLCs.CurrentCell is DataGridViewCheckBoxCell) {
                dgDLCs.EndEdit();
            }
        }

        // sort
        public void DgDLCs_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e) {
            try {
                if (DLCList == null) return;
                if (e.ColumnIndex == prevSortCol_) {
                    sortAssending_ = !sortAssending_;
                } else {
                    sortAssending_ = true;
                    foreach (DataGridViewColumn col in dgDLCs.Columns)
                        col.HeaderCell.SortGlyphDirection = SortOrder.None;
                }
                var sortOrder = sortAssending_ ? SortOrder.Ascending : SortOrder.Descending;
                dgDLCs.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = sortOrder;

                prevSortCol_ = e.ColumnIndex;


                if (e.ColumnIndex == CInclude.Index) {
                    DLCList.SortItemsBy(item => item.IsIncluded, sortAssending_);
                } else if (e.ColumnIndex == CName.Index) {
                    DLCList.SortItemsBy(item => item.Text, sortAssending_);
                } else if (e.ColumnIndex == CDLCType.Index) {
                    DLCList.SortItemsBy(item => item.DLCType, sortAssending_);
                }

                dgDLCs.Rows.Clear();
                DLCList.FilterItems();
                DgDLCs_Refresh();
            } catch (Exception ex) {
                Log.Exception(ex);
            }
        }

        public void DgDLCs_DataError(object sender, DataGridViewDataErrorEventArgs e) {
            Log.Exception(e.Exception);
            e.Cancel = true;
        }

        public void DgDLCs_VisibleChanged(object sender, EventArgs e) {
            if (Visible && firstTime_) {
                dgDLCs.AutoResizeColumns();
                dgDLCs.AutoResizeFirstColumn();
                firstTime_ = false;
            }
        }

        public void DgDLCs_Refresh() {
            if (DLCList != null) {
                if (dgDLCs.RowCount > DLCList.Filtered.Count)
                    dgDLCs.Rows.Clear(); // work around : removing rows is slow.
                dgDLCs.RowCount = DLCList.Filtered.Count;
            }
            dgDLCs.Refresh();
        }
        #endregion

    }
}
