using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LoadOrder
{
    public partial class LoadOrder : Form
    {
        public static LoadOrder Instance;
        public LoadOrder()
        {
            InitializeComponent();
            this.dataGridViewMods.AlternatingRowsDefaultCellStyle.BackColor = Color.Beige;
            Instance = this;
            Populate(ModList.GetAllMods());
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
            if(sender is TextBox tb)
            {
                if (tb.Text == "")
                    tb.Text = "0";
                else
                    tb.Text = UInt32.Parse(tb.Text).ToString();
            }
        }

        public void Populate(ModList modList)
        {
            SuspendLayout();
            var rows = this.dataGridViewMods.Rows;
            rows.Clear();
            Log.Info("Populating");
            foreach (var mod in modList)
            {
                rows.Add(mod.LoadOrder, mod.isEnabled, mod.DisplayText);
                Log.Info("row added: " + mod.ToString());
            }
            ResumeLayout();
        }
    }
}
