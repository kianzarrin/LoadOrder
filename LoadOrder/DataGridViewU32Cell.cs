using System.Windows.Forms;

namespace LoadOrder
{
    public class DataGridViewU32Cell : DataGridViewTextBoxCell
    {
        protected override void OnKeyPress(KeyPressEventArgs e, int rowIndex)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
            base.OnKeyPress(e, rowIndex);
        }
    }

    public class DataGridViewU32Column : DataGridViewColumn
    {
        public DataGridViewU32Column() : 
            base(new DataGridViewU32Cell()) { } 
    }
}
