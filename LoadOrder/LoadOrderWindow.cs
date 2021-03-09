using CO;
using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace LoadOrderTool {
    public partial class LoadOrderWindow : Form {
        public static LoadOrderWindow Instance;

        ModList ModList;

        public LoadOrderWindow()
        {
            InitializeComponent();
            this.dataGridViewMods.AlternatingRowsDefaultCellStyle.BackColor = Color.Beige;
            Instance = this;
            ModList = ModList.GetAllMods();
            ModList.SortBy(ModList.HarmonyComparison);
            Populate();
        }

        private void dataGridViewMods_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dataGridViewMods.CurrentCell.ColumnIndex == 0 && e.Control is TextBox tb) // Desired Column
            {
                tb.KeyPress -= U32TextBox_KeyPress;
                tb.Leave -= U32TextBox_Submit;
                tb.KeyPress += U32TextBox_KeyPress;
                tb.Leave += U32TextBox_Submit;
            }
        }

        private void U32TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) {
                e.Handled = true;
            }
        }

        private void U32TextBox_Submit(object? sender, EventArgs e)
        {
            if (sender is TextBox tb) {
                if (tb.Text == "")
                    tb.Text = "0";
                else
                    tb.Text = UInt32.Parse(tb.Text).ToString();
            }
        }

        private void dataGridViewMods_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            Log.Debug("dataGridViewMods_CellValueChanged() called");
            var plugin = ModList[e.RowIndex];
            var cell = dataGridViewMods.Rows[e.RowIndex].Cells[e.ColumnIndex];
            var col = cell.OwningColumn;

            if (col == LoadIndex) {
                int newVal = Int32.Parse(cell.Value as string);
                int oldVal = e.RowIndex;
                ModList.MoveItem(oldVal, newVal);
                Populate();
            } else if (col == ModEnabled) {
                plugin.isEnabled = (bool)cell.Value;
            } else if (col == IsIncluded) {
                plugin.IsIncluded = (bool)cell.Value;
            } else {
                return;
            }
        }

        public void Populate()
        {
            SuspendLayout();
            var rows = this.dataGridViewMods.Rows;
            rows.Clear();
            Log.Info("Populating");
            foreach (var p in ModList) {
                string savedKey = p.savedEnabledKey_;
                Log.Debug($"plugin info: dllName={p.dllName} harmonyVersion={ ModList.GetHarmonyOrder(p)} " +
                     $"savedKey={savedKey} modPath={p.ModPath}");
            }
            foreach (var mod in ModList) {
                rows.Add(mod.LoadOrder, mod.IsIncluded, mod.isEnabled, mod.DisplayText);
                Log.Debug("row added: " + mod.ToString());
            }
            ResumeLayout();
        }

        private void dataGridViewMods_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if(dataGridViewMods.CurrentCell is DataGridViewCheckBoxCell) {
                dataGridViewMods.EndEdit();
            }
        }

        private void SortByHarmony_Click(object sender, EventArgs e)
        {
            foreach (var p in ModList)
                p.LoadOrder = LoadOrderShared.LoadOrderConfig.DefaultLoadOrder;
            ModList.SortBy(ModList.HarmonyComparison);
            Populate();
        }

        private void EnableAll_Click(object sender, EventArgs e)
        {
            foreach (var p in ModList)
                p.isEnabled = true;
            Populate();

        }

        private void DisableAll_Click(object sender, EventArgs e)
        {
            foreach (var p in ModList)
                p.isEnabled = false;
            Populate();
        }

        private void IncludeAll_Click(object sender, EventArgs e)
        {
            foreach (var p in ModList)
                p.IsIncluded = true;
            Populate();
        }

        private void ExcludeAll_Click(object sender, EventArgs e)
        {
            foreach (var p in ModList)
                p.IsIncluded = false;
            Populate();
        }

        private void ReverseOrder_Click(object sender, EventArgs e)
        {
            ModList.ReverseOrder();
            Populate();
        }

        private void RandomizeOrder_Click(object sender, EventArgs e)
        {
            ModList.RandomizeOrder();
            Populate();
        }

        private void LoadOrder_FormClosing(object sender, FormClosingEventArgs e)
        {
            GameSettings.SaveAll();
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
                }
            }
        }
    }
}
