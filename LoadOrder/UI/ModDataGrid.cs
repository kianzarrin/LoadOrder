namespace LoadOrderTool.UI {
    using CO.PlatformServices;
    using CO.Plugins;
    using LoadOrderTool.Util;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    [Designer(typeof(ControlDesigner))]
    public class ModDataGrid : DataGridView {
        public ModList ModList;
        int prevSortCol_ = -1;
        bool sortAssending_ = false;

        public DataGridViewTextBoxColumn COrder;
        public DataGridViewCheckBoxColumn CIsIncluded;
        public DataGridViewCheckBoxColumn CEnabled;
        public DataGridViewLinkColumn CModID;
        public DataGridViewTextBoxColumn CAuthor;
        public DataGridViewTextBoxColumn CDateUpdated;
        public DataGridViewTextBoxColumn CDateSubscribed;
        public DataGridViewTextBoxColumn CDescription;


        public ModDataGrid() {
            COrder = new DataGridViewTextBoxColumn();
            CIsIncluded = new DataGridViewCheckBoxColumn();
            CEnabled = new DataGridViewCheckBoxColumn();
            CModID = new DataGridViewLinkColumn();
            CDescription = new DataGridViewTextBoxColumn();
            CAuthor = new DataGridViewTextBoxColumn();
            CDateUpdated = new DataGridViewTextBoxColumn();
            CDateSubscribed = new DataGridViewTextBoxColumn();

            Columns.AddRange(new DataGridViewColumn[] {
            COrder,
            CIsIncluded,
            CEnabled,
            CModID,
            CAuthor,
            CDateUpdated,
            CDateSubscribed,
            CDescription});

            AllowUserToAddRows = false;
            AllowUserToDeleteRows = false;
            AllowUserToResizeRows = false;
            AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.Beige };
            AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            MultiSelect = false;
            RowHeadersVisible = false;
            AllowUserToResizeColumns = false;

            // 
            // COrder
            // 
            COrder.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            COrder.HeaderText = "Order";
            COrder.Name = "COrder";
            COrder.Resizable = DataGridViewTriState.True;
            // 
            // CIsIncluded
            // 
            CIsIncluded.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            CIsIncluded.HeaderText = "Include";
            CIsIncluded.Name = "CIsIncluded";
            // 
            // CEnabled
            // 
            CEnabled.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            CEnabled.HeaderText = "Enabled";
            CEnabled.Name = "CEnabled";
            CEnabled.Width = 61;
            // 
            // CModID
            // 
            CModID.HeaderText = "ID";
            CModID.Name = "CModID";
            CModID.ReadOnly = true;
            CModID.Resizable = DataGridViewTriState.True;
            CModID.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            CModID.TrackVisitedState = false;
            // 
            // CAuthor
            // 
            CAuthor.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            CAuthor.HeaderText = "Author";
            CAuthor.Name = "CAuthor";
            CAuthor.ReadOnly = true;
            // 
            // CDateUpdated
            // 
            CDateUpdated.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            CDateUpdated.HeaderText = "Updated";
            CDateUpdated.ToolTipText = "date of last update on workshop";
            CDateUpdated.Name = "Updated";
            CDateUpdated.ReadOnly = true;
            // 
            // CDateSubscribed
            // 
            CDateSubscribed.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            CDateSubscribed.HeaderText = "Subscribed";
            CDateSubscribed.ToolTipText = "subscription date";
            CDateSubscribed.Name = "CDateSubscribed";
            CDateSubscribed.ReadOnly = true;
            CDateSubscribed.Visible = false; // TODO: add this when ready
            // 
            // CDescription
            // 
            CDescription.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            CDescription.HeaderText = "Description";
            CDescription.Name = "CDescription";
            CDescription.ReadOnly = true;

            DataError += ModDataGrid_DataError;
            foreach (DataGridViewColumn col in Columns)
                col.SortMode = DataGridViewColumnSortMode.Programmatic;
        }

        private void ModDataGrid_DataError(object sender, DataGridViewDataErrorEventArgs e) {
            Log.Exception(e.Exception, $"at row:{e.RowIndex} col:{Columns[e.ColumnIndex]}");
            e.Cancel = true;
        }

        protected override void OnVisibleChanged(EventArgs e) {
            base.OnVisibleChanged(e);
            this.AutoResizeFirstColumn();
        }

        // numeric textbox 
        protected override void OnEditingControlShowing(DataGridViewEditingControlShowingEventArgs e) {
            base.OnEditingControlShowing(e);
            if (CurrentCell.ColumnIndex == COrder.Index && e.Control is TextBox tb) // Desired Column
            {
                tb.KeyPress -= UIUtil.U32TextBox_KeyPress;
                tb.Leave -= UIUtil.U32TextBox_Submit;
                tb.KeyPress += UIUtil.U32TextBox_KeyPress;
                tb.Leave += UIUtil.U32TextBox_Submit;
            }
        }

        // write
        protected override void OnCellValueChanged(DataGridViewCellEventArgs e) {
            base.OnCellValueChanged(e);
            try {
                if (ModList == null) return;
                var plugin = ModList.Filtered[e.RowIndex];
                var cell = Rows[e.RowIndex].Cells[e.ColumnIndex];
                var col = cell.OwningColumn;

                if (col == COrder) {
                    int newVal = Int32.Parse(cell.Value as string);
                    var p = ModList.Filtered[e.RowIndex];
                    ModList.MoveItem(p, newVal);
                    RefreshModList();
                } else if (col == CEnabled) {
                    plugin.IsEnabledPending = (bool)cell.Value;
                } else if (col == CIsIncluded) {
                    plugin.IsIncludedPending = (bool)cell.Value;
                } else {
                    return;
                }
            } catch (Exception ex) {
                Log.Exception(ex);
            }
        }


        // instant update
        protected override void OnCurrentCellDirtyStateChanged(EventArgs e) {
            base.OnCurrentCellDirtyStateChanged(e);
            if (CurrentCell is DataGridViewCheckBoxCell) {
                EndEdit();
            }
        }

        // tooltip
        protected override void OnCellFormatting(DataGridViewCellFormattingEventArgs e) {
            base.OnCellFormatting(e);
            try {
                if (ModList == null) return;
                //Log.Info($"e.ColumnIndex={e.ColumnIndex} Description.Index={Description.Index}");
                if (e.RowIndex < 0) return;
                if (e.RowIndex >= ModList.Filtered.Count || e.RowIndex >= Rows.Count) return;
                var cell = Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (e.ColumnIndex == CDescription.Index && e.Value != null) {
                    cell.ToolTipText = ModList.Filtered[e.RowIndex].ModInfo.Description;
                } else if (e.ColumnIndex == CModID.Index) {
                    cell.ToolTipText = ContentUtil.GetItemURL((string)cell.Value) ?? ModList.Filtered[e.RowIndex].ModPath;
                } else {
                    cell.ToolTipText = null;
                }

            } catch (Exception ex) {
                Log.Exception(ex, $"e.ColumnIndex={e.ColumnIndex} e.RowIndex={e.RowIndex}");
            }
        }

        // click link/button
        protected override void OnCellContentClick(DataGridViewCellEventArgs e) {
            base.OnCellContentClick(e);
            try {
                Log.Info("OnCellContentClick");
                if (ModList == null) return;
                if (e.RowIndex < 0 || e.RowIndex >= ModList.Filtered.Count) return;
                var cell = Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (e.ColumnIndex == CModID.Index) {
                    var mod = ModList.Filtered[e.RowIndex];
                    if (mod.IsWorkshop) {
                        string url = ContentUtil.GetItemURL(mod.PublishedFileId);
                        if (url != null)
                            ContentUtil.OpenURL(url);
                    } else {
                        ContentUtil.OpenPath(ModList.Filtered[e.RowIndex].ModPath);
                    }
                }
            } catch (Exception ex) {
                Log.Exception(ex);
            }
        }

        // sort
        protected override void OnColumnHeaderMouseClick(DataGridViewCellMouseEventArgs e) {
            base.OnColumnHeaderMouseClick(e);
            try {
                if (ModList == null) return;
                if (e.ColumnIndex == prevSortCol_) {
                    sortAssending_ = e.ColumnIndex == COrder.Index ? true :!sortAssending_;
                } else {
                    sortAssending_ = true;
                    foreach (DataGridViewColumn col in Columns)
                        col.HeaderCell.SortGlyphDirection = SortOrder.None;
                }
                var sortOrder = sortAssending_ ? SortOrder.Ascending : SortOrder.Descending;
                Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = sortOrder;
                prevSortCol_ = e.ColumnIndex;

                if (e.ColumnIndex == COrder.Index) {
                    ModList.DefaultSort();
                } else if (e.ColumnIndex == CIsIncluded.Index) {
                    ModList.SortItemsBy(item => item.IsIncludedPending, sortAssending_);
                } else if (e.ColumnIndex == CEnabled.Index) {
                    ModList.SortItemsBy(item => item.IsEnabledPending, sortAssending_);
                } else if (e.ColumnIndex == CModID.Index) {
                    ModList.SortItemsBy(item => item.PublishedFileId.AsUInt64, sortAssending_);
                } else if (e.ColumnIndex == CDescription.Index) {
                    ModList.SortItemsBy(item => item.DisplayText, sortAssending_);
                } else if (e.ColumnIndex == CAuthor.Index) {
                    // "[unknown" is sorted before "[unknown]". This puts empty before unknown author.
                    ModList.SortItemsBy(item => item.ModInfo.Author ?? "[unknown", sortAssending_);
                } else if (e.ColumnIndex == this.CDateUpdated.Index) {
                    ModList.SortItemsBy(item => item.DateUpdated, sortAssending_);
                }

                RefreshModList(sort: false);
            } catch (Exception ex) {
                Log.Exception(ex);
            }
        }

        public void LoadMods(Func<PluginManager.PluginInfo, bool> predicateCallback) {
            PluginManager.instance.LoadPlugins();
            ModList = ModList.GetAllMods(predicateCallback);
            RefreshModList(true);
        }

        public void RefreshModList(bool sort = false) {
            if (sort) {
                ModList.DefaultSort();
                COrder.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
            }
            ModList.FilterIn();
            PopulateMods();
        }

        public DataGridViewRow GetRow(PluginManager.PluginInfo pluginInfo) {
            int rowIndex = ModList.Filtered.IndexOf(pluginInfo);
            return Rows[rowIndex];
        }

        public void PopulateMods() {
            SuspendLayout();
            var rows = Rows;
            rows.Clear();
            Log.Info("Populating");
            foreach (var p in ModList.Filtered) {
                string savedKey = p.savedEnabledKey_;
                //Log.Debug($"plugin info: dllName={p.dllName} harmonyVersion={ ModList.GetHarmonyOrder(p)} " +
                //     $"savedKey={savedKey} modPath={p.ModPath}");
            }
            foreach (var mod in ModList.Filtered) {
                try {
                    string id = mod.PublishedFileId.AsUInt64.ToString();
                    if (id == "0" || mod.PublishedFileId == PublishedFileId.invalid)
                        id = "Local";
                    rows.Add(
                        mod.LoadOrder,
                        mod.IsIncludedPending,
                        mod.IsEnabledPending,
                        id,
                        mod.ModInfo.Author ?? "",
                        mod.ModInfo.DateUpdated ?? "",
                        mod.ModInfo.DateSubscribed ?? "",
                        mod.DisplayText ?? "");
                } catch (Exception ex) {
                    Log.Exception(new Exception(
                        $"failed to add mod to row: " +
                        $"LoadOrder={mod.LoadOrder} " +
                        $"IsIncludedPending={mod.IsIncludedPending} " +
                        $"IsEnabledPending={mod.IsEnabledPending} " +
                        $"id={mod.PublishedFileId} " +
                        $"Author={mod.ModInfo.Author} " +
                        $"DateUpdated={mod.ModInfo.DateUpdated} " +
                        $"DateSubscribed={mod.ModInfo.DateSubscribed} " +
                        $"DisplayText={mod.DisplayText}",
                        innerException: ex
                        ));
                }
            }
            ResumeLayout();
            this.AutoResizeFirstColumn();
        }
    }
}
