namespace LoadOrderTool.UI {
    using System.Drawing;
    using System.Windows.Forms;
    using System.ComponentModel;
    using System.Windows.Forms.Design;
    using System;
    using LoadOrderTool.Util;
    using CO.PlatformServices;
    using LoadOrderTool.Data;
    using CO.Packaging;

    [Designer(typeof(ControlDesigner))]
    public class AssetDataGrid : DataGridView {
        int prevSortCol_ = -1;
        bool sortAssending_ = false;
        bool firstTime_ = true;

        public AssetList AssetList;

        private DataGridViewCheckBoxColumn cIncluded;
        private DataGridViewLinkColumn cAssetID;
        private DataGridViewTextBoxColumn cName;
        private DataGridViewTextBoxColumn cAuthor;
        private DataGridViewTextBoxColumn cDate;
        private DataGridViewTextBoxColumn cTags;

        public AssetDataGrid() {
            cIncluded = new DataGridViewCheckBoxColumn();
            cAssetID = new DataGridViewLinkColumn();
            cName = new DataGridViewTextBoxColumn();
            cAuthor = new DataGridViewTextBoxColumn();
            cDate = new DataGridViewTextBoxColumn();
            cTags = new DataGridViewTextBoxColumn();

            AllowUserToAddRows = false;
            AllowUserToDeleteRows = false;
            AllowUserToOrderColumns = true;
            AllowUserToResizeRows = false;
            MultiSelect = false;
            RowHeadersVisible = false;
            AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.Beige };
            AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            SelectionMode = DataGridViewSelectionMode.CellSelect;

            Columns.AddRange(new DataGridViewColumn[] {
            cIncluded,
            cAssetID,
            cName,
            cAuthor,
            cDate,
            cTags});

            // 
            // cIncluded
            // 
            this.cIncluded.HeaderText = "Included";
            this.cIncluded.Name = "cIncluded";
            this.cIncluded.Width = 63;
            // 
            // cAssetID
            // 
            this.cAssetID.HeaderText = "ID";
            this.cAssetID.Name = "cAssetID";
            this.cAssetID.ReadOnly = true;
            this.cAssetID.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.cAssetID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.cAssetID.TrackVisitedState = false;
            this.cAssetID.Width = 45;
            // 
            // cName
            // 
            this.cName.HeaderText = "Name";
            this.cName.Name = "cName";
            this.cName.ReadOnly = true;
            this.cName.Width = 68;
            // 
            // cAuthor
            // 
            this.cAuthor.HeaderText = "Author";
            this.cAuthor.Name = "cAuthor";
            this.cAuthor.ReadOnly = true;
            this.cAuthor.Width = 72;
            // 
            // cDate
            // 
            this.cDate.HeaderText = "Date";
            this.cDate.Name = "cDate";
            this.cDate.ReadOnly = true;
            this.cDate.Width = 60;
            // 
            // cTags
            // 
            this.cTags.HeaderText = "Tags";
            this.cTags.Name = "cTags";
            this.cTags.ReadOnly = true;
            this.cTags.Width = 60;

            VirtualMode = true;
            foreach (DataGridViewColumn col in Columns) {
                col.SortMode = DataGridViewColumnSortMode.Programmatic;
                col.Width += 1; // workaround : show hyphon
            }
        }

        //read
        protected override void OnCellValueNeeded(DataGridViewCellValueEventArgs e) {
            base.OnCellValueNeeded(e);
            try {
                if (AssetList == null) return;
                if (e.RowIndex >= AssetList.Filtered.Count) return;
                var asset = AssetList.Filtered[e.RowIndex];
                if (e.ColumnIndex == cIncluded.Index) {
                    e.Value = asset.IsIncludedPending;
                } else if (e.ColumnIndex == cAssetID.Index) {
                    string id = asset.PublishedFileId.AsUInt64.ToString();
                    if (id == "0" || asset.PublishedFileId == PublishedFileId.invalid)
                        id = "Local";
                    e.Value = id;
                } else if (e.ColumnIndex == cName.Index) {
                    e.Value = asset.DisplayText ?? "";
                } else if (e.ColumnIndex == cAuthor.Index) {
                    e.Value = asset.ConfigAssetInfo.Author ?? "";
                } else if (e.ColumnIndex == cDate.Index) {
                    e.Value = asset.StrDate;
                } else if (e.ColumnIndex == cTags.Index) {
                    e.Value = asset.StrTags;
                }
            } catch (Exception ex) {
                Log.Exception(ex);
            }
        }

        // tooltip
        protected override void OnCellToolTipTextNeeded(DataGridViewCellToolTipTextNeededEventArgs e) {
            base.OnCellToolTipTextNeeded(e);
            try {
                if (AssetList == null) return;
                if (e.RowIndex < 0 || e.RowIndex >= AssetList.Filtered.Count) return;
                var asset = AssetList.Filtered[e.RowIndex];
                if (e.ColumnIndex == cAssetID.Index) {
                    var id = asset.PublishedFileId;
                    string url = ContentUtil.GetItemURL(asset.PublishedFileId);
                    e.ToolTipText = url ?? asset.AssetPath;
                }
            } catch (Exception ex) {
                Log.Exception(ex, $"rowIndex={e.RowIndex}");
            }
        }

        // click link/button
        protected override void OnCellContentClick(DataGridViewCellEventArgs e) {
            base.OnCellContentClick(e);
            try {
                if (AssetList == null) return;
                if (e.RowIndex < 0 || e.RowIndex >= AssetList.Filtered.Count) return;
                var asset = AssetList.Filtered[e.RowIndex];

                if (e.ColumnIndex == cAssetID.Index) {
                    var id = asset.PublishedFileId;
                    string url = ContentUtil.GetItemURL(asset.PublishedFileId);
                    if (url != null)
                        ContentUtil.OpenURL(url);
                    else
                        ContentUtil.OpenPath(asset.AssetPath);
                }
            } catch (Exception ex) {
                Log.Exception(ex);
            }
        }

        //write
        protected override void OnCellValuePushed(DataGridViewCellValueEventArgs e) {
            base.OnCellValuePushed(e);
            if (AssetList == null) return;
            var assetInfo = AssetList.Filtered[e.RowIndex];
            if (e.ColumnIndex == cIncluded.Index) {
                assetInfo.IsIncludedPending = (bool)e.Value;
                //Log.Debug($"{assetInfo} | IsIncludedPending changes to {cell.Value}");
            } else {
                Log.Error("unexpected column changed: " + Columns[e.ColumnIndex]?.Name);
            }
        }


        // instant update
        protected override void OnCurrentCellDirtyStateChanged(EventArgs e) {
            base.OnCurrentCellDirtyStateChanged(e);
            if (CurrentCell is DataGridViewCheckBoxCell) {
                EndEdit();
            }
        }

        // sort
        protected override void OnColumnHeaderMouseClick(DataGridViewCellMouseEventArgs e) {
            base.OnColumnHeaderMouseClick(e);
            try {
                if (AssetList == null) return;
                if (e.ColumnIndex == prevSortCol_) {
                    sortAssending_ = !sortAssending_;
                } else {
                    sortAssending_ = true;
                    foreach (DataGridViewColumn col in Columns)
                        col.HeaderCell.SortGlyphDirection = SortOrder.None;
                }
                var sortOrder = sortAssending_ ? SortOrder.Ascending : SortOrder.Descending;
                Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = sortOrder;
                prevSortCol_ = e.ColumnIndex;


                if (e.ColumnIndex == cIncluded.Index) {
                    AssetList.SortItemsBy(item => item.IsIncludedPending, sortAssending_);
                } else if (e.ColumnIndex == cAssetID.Index) {
                    AssetList.SortItemsBy(item => item.PublishedFileId.AsUInt64, sortAssending_);
                } else if (e.ColumnIndex == cName.Index) {
                    AssetList.SortItemsBy(item => item.DisplayText, sortAssending_);
                } else if (e.ColumnIndex == cAuthor.Index) {
                    // "[unknown" is sorted before "[unknown]". This puts empty before unknown author.
                    AssetList.SortItemsBy(item => item.ConfigAssetInfo.Author ?? "[unknown", sortAssending_);
                } else if (e.ColumnIndex == cDate.Index) {
                    AssetList.SortItemsBy(item => item.Date, sortAssending_);
                } else if (e.ColumnIndex == cTags.Index) {
                    AssetList.SortItemsBy(item => item.StrTags, sortAssending_);
                }

                Rows.Clear();
                AssetList.FilterItems();
                Refresh();
            } catch (Exception ex) {
                Log.Exception(ex);
            }
        }

        protected override void OnDataError(bool displayErrorDialogIfNoHandler, DataGridViewDataErrorEventArgs e) {
            base.OnDataError(displayErrorDialogIfNoHandler, e);
            Log.Exception(e.Exception);
            e.Cancel = true;
        }

        protected override void OnVisibleChanged(EventArgs e) {
            base.OnVisibleChanged(e);
            if (Visible && firstTime_) {
                AutoResizeColumns();
                firstTime_ = false;
            }
        }

        public override void Refresh() {
            if (AssetList != null) {
                if (RowCount > AssetList.Filtered.Count)
                    Rows.Clear(); // work around : removing rows is slow.
                RowCount = AssetList.Filtered.Count;
            }
            base.Refresh();
        }
    }
}
