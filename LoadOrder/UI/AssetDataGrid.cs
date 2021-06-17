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
        private DataGridViewTextBoxColumn cDateUpdated;
        private DataGridViewTextBoxColumn cDateSubscribed;
        private DataGridViewTextBoxColumn cTags;

        public AssetDataGrid() {
            cIncluded = new DataGridViewCheckBoxColumn();
            cAssetID = new DataGridViewLinkColumn();
            cName = new DataGridViewTextBoxColumn();
            cAuthor = new DataGridViewTextBoxColumn();
            cDateUpdated = new DataGridViewTextBoxColumn();
            cDateSubscribed = new DataGridViewTextBoxColumn();
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
            cDateUpdated,
            cDateSubscribed,
            cTags});

            // 
            // cIncluded
            // 
            cIncluded.HeaderText = "Included";
            cIncluded.Name = "cIncluded";
            cIncluded.Width = 63;
            // 
            // cAssetID
            // 
            cAssetID.HeaderText = "ID";
            cAssetID.Name = "cAssetID";
            cAssetID.ReadOnly = true;
            cAssetID.Resizable = DataGridViewTriState.True;
            cAssetID.SortMode = DataGridViewColumnSortMode.Automatic;
            cAssetID.TrackVisitedState = false;
            cAssetID.Width = 45;
            // 
            // cName
            // 
            cName.HeaderText = "Name";
            cName.Name = "cName";
            cName.ReadOnly = true;
            cName.Width = 68;
            // 
            // cAuthor
            // 
            cAuthor.HeaderText = "Author";
            cAuthor.Name = "cAuthor";
            cAuthor.ReadOnly = true;
            cAuthor.Width = 72;
            // 
            // cDateUpdated
            // 
            cDateUpdated.HeaderText = "Updated";
            cDateUpdated.ToolTipText = "date of last update on workshop";
            cDateUpdated.Name = "cDateUpdated";
            cDateUpdated.ReadOnly = true;
            cDateUpdated.Width = 60;
            // 
            // cDateSubscribed
            // 
            cDateSubscribed.HeaderText = "Subscribed";
            cDateSubscribed.ToolTipText = "subscription date";
            cDateSubscribed.Name = "cDateSubscribed";
            cDateSubscribed.ReadOnly = true;
            cDateSubscribed.Width = 60;
            // 
            // cTags
            // 
            cTags.HeaderText = "Tags";
            cTags.Name = "cTags";
            cTags.ReadOnly = true;
            cTags.Width = 60;

            VirtualMode = true;
            foreach (DataGridViewColumn col in Columns) {
                col.SortMode = DataGridViewColumnSortMode.Programmatic;
                col.Width += 1; // workaround : show Glyph
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
                } else if (e.ColumnIndex == cDateUpdated.Index) {
                    e.Value = asset.StrDateUpdated;
                } else if (e.ColumnIndex == cDateSubscribed.Index) {
                    e.Value = asset.StrDateSubscribed;
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
                } else if (e.ColumnIndex == cDateUpdated.Index) {
                    AssetList.SortItemsBy(item => item.DateUpdated, sortAssending_);
                } else if (e.ColumnIndex == cDateSubscribed.Index) {
                    AssetList.SortItemsBy(item => item.DateSubscribed, sortAssending_);
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
                this.AutoResizeFirstColumn();
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
