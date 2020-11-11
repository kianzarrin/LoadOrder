namespace LoadOrder
{
    using System.Windows.Forms;
    partial class LoadOrder
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.LoadIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ModEnabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewMods = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMods)).BeginInit();
            this.SuspendLayout();
            // 
            // LoadIndex
            // 
            this.LoadIndex.HeaderText = "Index";
            this.LoadIndex.Name = "LoadIndex";
            this.LoadIndex.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.LoadIndex.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.LoadIndex.Width = 45;
            // 
            // ModEnabled
            // 
            this.ModEnabled.DataPropertyName = "(none)";
            this.ModEnabled.HeaderText = "Enabled";
            this.ModEnabled.Name = "ModEnabled";
            this.ModEnabled.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ModEnabled.Width = 55;
            // 
            // Description
            // 
            this.Description.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Description.HeaderText = "Description";
            this.Description.Name = "Description";
            this.Description.ReadOnly = true;
            this.Description.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Description.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewMods
            // 
            this.dataGridViewMods.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.Beige;
            this.dataGridViewMods.AllowUserToResizeColumns = false;
            this.dataGridViewMods.AllowUserToResizeRows = false;
            this.dataGridViewMods.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dataGridViewMods.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMods.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.LoadIndex,
            this.ModEnabled,
            this.Description});
            this.dataGridViewMods.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewMods.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewMods.MultiSelect = false;
            this.dataGridViewMods.Name = "dataGridViewMods";
            this.dataGridViewMods.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridViewMods.Size = new System.Drawing.Size(557, 542);
            this.dataGridViewMods.TabIndex = 0;
            this.dataGridViewMods.Text = "Mods";
            this.dataGridViewMods.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dataGridViewMods_EditingControlShowing);
            // 
            // LoadOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(557, 542);
            this.Controls.Add(this.dataGridViewMods);
            this.Name = "LoadOrder";
            this.Text = "LoadOrder";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMods)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.DataGridView dataGridViewMods;
        private DataGridViewTextBoxColumn LoadIndex;
        private DataGridViewCheckBoxColumn ModEnabled;
        private DataGridViewTextBoxColumn Description;
    }
}