using System;
using System.Drawing;
using System.Windows.Forms;

namespace LoadOrderTool
{
    public partial class LoadOrder : Form
    {
        public static LoadOrder Instance;

        ModList ModList;

        public LoadOrder()
        {
            InitializeComponent();
            this.dataGridViewMods.AlternatingRowsDefaultCellStyle.BackColor = Color.Beige;
            Instance = this;
            ModList = ModList.GetAllMods();
            ModList.DefaultSort();
            Populate();
        }

        private void dataGridViewMods_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= U32TextBox_KeyPress;
            if (dataGridViewMods.CurrentCell.ColumnIndex == 0 && e.Control is TextBox tb) // Desired Column
            {
                tb.KeyPress += U32TextBox_KeyPress;
                tb.Leave += U32TextBox_Submit;
            }
        }

        private void U32TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void U32TextBox_Submit(object? sender, EventArgs e)
        {
            if (sender is TextBox tb)
            {
                if (tb.Text == "")
                    tb.Text = "0";
                else
                    tb.Text = UInt32.Parse(tb.Text).ToString();
            }
        }

        private void dataGridViewMods_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            var plugin = ModList[e.RowIndex];
            var cell = dataGridViewMods.Rows[e.RowIndex].Cells[e.ColumnIndex];
            switch (e.ColumnIndex)
            {
                case 0:
                    int newVal = Int32.Parse(cell.Value as string);
                    int oldVal = e.RowIndex;
                    ModList.MoveItem(oldVal, newVal);
                    Populate();
                    break;
                case 1:
                    bool isEnabled = (bool)cell.Value;
                    plugin.isEnabled = isEnabled;
                    break;
                default:
                    return;
            }
        }

        public void Populate()
        {
            SuspendLayout();
            var rows = this.dataGridViewMods.Rows;
            rows.Clear();
            Log.Info("Populating");
            foreach (var p in ModList)
            {
                string savedKey = p.savedEnabledKey_;
                Log.Debug($"plugin info: savedKey={savedKey} cachedName={p.name} modPath={p.Path}");
            }
            foreach (var mod in ModList)
            {
                rows.Add(mod.LoadOrder, mod.isEnabled, mod.DisplayText);
                Log.Info("row added: " + mod.ToString());
            }
            ResumeLayout();
        }
    }
}
