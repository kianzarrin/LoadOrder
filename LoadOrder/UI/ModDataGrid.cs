namespace LoadOrderTool.UI {
    using System.Drawing;
    using System.Windows.Forms;
    using System.ComponentModel;
    using System.Windows.Forms.Design;
    using System;
    using LoadOrderTool.Util;
    using CO;
    using CO.Packaging;
    using CO.Plugins;
    using CO.PlatformServices;
    using System.Collections.Generic;
    using System.Linq;
    using System.Diagnostics;
    using LoadOrderTool.Data;
    using System.Threading.Tasks;
    using LoadOrderTool.UI;

    [Designer(typeof(ControlDesigner))]
    public class ModDataGrid : DataGridView {
        public ModList ModList;

        public DataGridViewTextBoxColumn CLoadIndex;
        public DataGridViewCheckBoxColumn CIsIncluded;
        public DataGridViewCheckBoxColumn CEnabled;
        public DataGridViewLinkColumn CModID;
        public DataGridViewTextBoxColumn CDescription;

        public ModDataGrid() {
            CLoadIndex = new DataGridViewTextBoxColumn();
            CIsIncluded = new DataGridViewCheckBoxColumn();
            CEnabled = new DataGridViewCheckBoxColumn();
            CModID = new DataGridViewLinkColumn();
            CDescription = new DataGridViewTextBoxColumn();

            AllowUserToAddRows = false;
            AllowUserToDeleteRows = false;
            AllowUserToResizeRows = false;
            AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.Beige };
            AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            MultiSelect = false;
            RowHeadersVisible = false;

            Columns.AddRange(new DataGridViewColumn[] {
            CLoadIndex,
            CIsIncluded,
            CEnabled,
            CModID,
            CDescription});

            // 
            // CLoadIndex
            // 
            CLoadIndex.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            CLoadIndex.HeaderText = "Order";
            CLoadIndex.Name = "CLoadIndex";
            CLoadIndex.Resizable = DataGridViewTriState.True;
            CLoadIndex.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // CIsIncluded
            // 
            CIsIncluded.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            CIsIncluded.HeaderText = "Include";
            CIsIncluded.Name = "CIsIncluded";
            CIsIncluded.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // CEnabled
            // 
            CEnabled.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            CEnabled.HeaderText = "Enabled";
            CEnabled.Name = "CEnabled";
            CEnabled.Width = 61;
            CEnabled.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // CModID
            // 
            CModID.HeaderText = "ID";
            CModID.Name = "CModID";
            CModID.ReadOnly = true;
            CModID.Resizable = DataGridViewTriState.True;
            CModID.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            CModID.TrackVisitedState = false;
            CModID.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // CDescription
            // 
            CDescription.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            CDescription.HeaderText = "Description";
            CDescription.Name = "CDescription";
            CDescription.ReadOnly = true;
            CDescription.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        // override data error
        protected override void OnDataError(bool displayErrorDialogIfNoHandler, DataGridViewDataErrorEventArgs e) {
            base.OnDataError(displayErrorDialogIfNoHandler, e);
            Log.Exception(e.Exception);
            e.Cancel = true;
        }

        // numeric textbox 
        protected override void OnEditingControlShowing(DataGridViewEditingControlShowingEventArgs e) {
            base.OnEditingControlShowing(e);
            if (CurrentCell.ColumnIndex == 0 && e.Control is TextBox tb) // Desired Column
            {
                tb.KeyPress -= UIUtil.U32TextBox_KeyPress;
                tb.Leave -= UIUtil.U32TextBox_Submit;
                tb.KeyPress += UIUtil.U32TextBox_KeyPress;
                tb.Leave += UIUtil.U32TextBox_Submit;
            }
        }

        // write
        protected override void OnCellValueChanged(DataGridViewCellEventArgs e) {
            base.OnCellContentClick(e);
            try {
                if (ModList == null)return;
                var plugin = ModList.Filtered[e.RowIndex];
                var cell = Rows[e.RowIndex].Cells[e.ColumnIndex];
                var col = cell.OwningColumn;

                if (col == CLoadIndex) {
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
                if (e.RowIndex >= ModList.Filtered.Count || e.RowIndex >= Rows.Count)
                    return;
                var cell = Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (e.ColumnIndex == CDescription.Index && e.Value != null) {
                    cell.ToolTipText = ModList.Filtered[e.RowIndex].ModInfo.Description;
                } else if (e.ColumnIndex == CModID.Index) {
                    cell.ToolTipText = ContentUtil.GetItemURL((string)cell.Value);
                }
            } catch (Exception ex) {
                Log.Exception(ex);
            }
        }

        // click link/button
        protected override void OnCellContentClick(DataGridViewCellEventArgs e) {
            try {
                if (ModList == null) return;
                if (e.RowIndex < 0 || e.RowIndex >= ModList.Filtered.Count) return;
                var cell = Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (e.ColumnIndex == CModID.Index) {
                    string url = ContentUtil.GetItemURL((string)cell.Value);
                    if (url != null)
                        ContentUtil.OpenURL(url);
                }
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
            if (sort)
                ModList.DefaultSort();
            ModList.FilterIn();
            PopulateMods();
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
                string id = mod.PublishedFileId.AsUInt64.ToString();
                if (id == "0" || mod.PublishedFileId == PublishedFileId.invalid)
                    id = "";
                rows.Add(mod.LoadOrder, mod.IsIncludedPending, mod.IsEnabledPending, id, mod.DisplayText);
                //Log.Debug("row added: " + mod.ToString());
            }
            ResumeLayout();
        }
    }
}
