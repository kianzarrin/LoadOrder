
namespace LoadOrderTool.UI {
    partial class DLCControl {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not DLCify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.tableLayoutPanelDLCFilters = new System.Windows.Forms.TableLayoutPanel();
            this.ComboBoxDLCType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TextFilterDLCs = new System.Windows.Forms.TextBox();
            this.DLCActionPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.IncludeAlDLCs = new System.Windows.Forms.Button();
            this.ExcludeAllDLCs = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dgDLCs = new System.Windows.Forms.DataGridView();
            this.CInclude = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.CName = new System.Windows.Forms.DataGridViewLinkColumn();
            this.CDLCType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanelDLCFilters.SuspendLayout();
            this.DLCActionPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDLCs)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanelDLCFilters
            // 
            this.tableLayoutPanelDLCFilters.AutoSize = true;
            this.tableLayoutPanelDLCFilters.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanelDLCFilters.ColumnCount = 5;
            this.tableLayoutPanelDLCFilters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelDLCFilters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelDLCFilters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelDLCFilters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelDLCFilters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelDLCFilters.Controls.Add(this.ComboBoxDLCType, 0, 0);
            this.tableLayoutPanelDLCFilters.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanelDLCFilters.Controls.Add(this.TextFilterDLCs, 0, 0);
            this.tableLayoutPanelDLCFilters.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelDLCFilters.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.AddColumns;
            this.tableLayoutPanelDLCFilters.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelDLCFilters.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tableLayoutPanelDLCFilters.Name = "tableLayoutPanelDLCFilters";
            this.tableLayoutPanelDLCFilters.RowCount = 1;
            this.tableLayoutPanelDLCFilters.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDLCFilters.Size = new System.Drawing.Size(571, 43);
            this.tableLayoutPanelDLCFilters.TabIndex = 10;
            // 
            // ComboBoxDLCType
            // 
            this.ComboBoxDLCType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxDLCType.FormattingEnabled = true;
            this.ComboBoxDLCType.Location = new System.Drawing.Point(345, 5);
            this.ComboBoxDLCType.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ComboBoxDLCType.Name = "ComboBoxDLCType";
            this.ComboBoxDLCType.Size = new System.Drawing.Size(213, 33);
            this.ComboBoxDLCType.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(283, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 25);
            this.label1.TabIndex = 3;
            this.label1.Text = "Filter:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TextFilterDLCs
            // 
            this.TextFilterDLCs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextFilterDLCs.Location = new System.Drawing.Point(4, 5);
            this.TextFilterDLCs.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.TextFilterDLCs.Name = "TextFilterDLCs";
            this.TextFilterDLCs.Size = new System.Drawing.Size(271, 31);
            this.TextFilterDLCs.TabIndex = 4;
            // 
            // DLCActionPanel
            // 
            this.DLCActionPanel.AutoSize = true;
            this.DLCActionPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.DLCActionPanel.Controls.Add(this.IncludeAlDLCs);
            this.DLCActionPanel.Controls.Add(this.ExcludeAllDLCs);
            this.DLCActionPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.DLCActionPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.DLCActionPanel.Location = new System.Drawing.Point(459, 43);
            this.DLCActionPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.DLCActionPanel.Name = "DLCActionPanel";
            this.DLCActionPanel.Size = new System.Drawing.Size(112, 97);
            this.DLCActionPanel.TabIndex = 15;
            this.DLCActionPanel.WrapContents = false;
            // 
            // IncludeAlDLCs
            // 
            this.IncludeAlDLCs.AutoSize = true;
            this.IncludeAlDLCs.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.IncludeAlDLCs.Location = new System.Drawing.Point(3, 3);
            this.IncludeAlDLCs.Name = "IncludeAlDLCs";
            this.IncludeAlDLCs.Size = new System.Drawing.Size(104, 35);
            this.IncludeAlDLCs.TabIndex = 7;
            this.IncludeAlDLCs.Text = "Include All";
            this.IncludeAlDLCs.UseVisualStyleBackColor = true;
            // 
            // ExcludeAllDLCs
            // 
            this.ExcludeAllDLCs.AutoSize = true;
            this.ExcludeAllDLCs.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ExcludeAllDLCs.Location = new System.Drawing.Point(3, 44);
            this.ExcludeAllDLCs.Name = "ExcludeAllDLCs";
            this.ExcludeAllDLCs.Size = new System.Drawing.Size(106, 35);
            this.ExcludeAllDLCs.TabIndex = 3;
            this.ExcludeAllDLCs.Text = "Exclude All";
            this.ExcludeAllDLCs.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dgDLCs);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 43);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(459, 97);
            this.panel1.TabIndex = 16;
            // 
            // dgDLCs
            // 
            this.dgDLCs.AllowUserToAddRows = false;
            this.dgDLCs.AllowUserToDeleteRows = false;
            this.dgDLCs.AllowUserToResizeColumns = false;
            this.dgDLCs.AllowUserToResizeRows = false;
            this.dgDLCs.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgDLCs.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgDLCs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgDLCs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CInclude,
            this.CName,
            this.CDLCType});
            this.dgDLCs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgDLCs.Location = new System.Drawing.Point(0, 0);
            this.dgDLCs.Name = "dgDLCs";
            this.dgDLCs.RowHeadersVisible = false;
            this.dgDLCs.RowHeadersWidth = 62;
            this.dgDLCs.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgDLCs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgDLCs.Size = new System.Drawing.Size(459, 97);
            this.dgDLCs.TabIndex = 15;
            this.dgDLCs.VirtualMode = true;
            // 
            // CInclude
            // 
            this.CInclude.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.CInclude.HeaderText = "Include";
            this.CInclude.MinimumWidth = 8;
            this.CInclude.Name = "CInclude";
            this.CInclude.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.CInclude.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.CInclude.Width = 105;
            // 
            // CName
            // 
            this.CName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.CName.HeaderText = "Name";
            this.CName.MinimumWidth = 8;
            this.CName.Name = "CName";
            this.CName.ReadOnly = true;
            this.CName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.CName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.CName.VisitedLinkColor = System.Drawing.Color.Blue;
            this.CName.Width = 95;
            // 
            // CDLCType
            // 
            this.CDLCType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.CDLCType.HeaderText = "Type";
            this.CDLCType.MinimumWidth = 8;
            this.CDLCType.Name = "CDLCType";
            this.CDLCType.ReadOnly = true;
            this.CDLCType.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.CDLCType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.CDLCType.Width = 85;
            // 
            // DLCControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.DLCActionPanel);
            this.Controls.Add(this.tableLayoutPanelDLCFilters);
            this.Name = "DLCControl";
            this.Size = new System.Drawing.Size(571, 140);
            this.tableLayoutPanelDLCFilters.ResumeLayout(false);
            this.tableLayoutPanelDLCFilters.PerformLayout();
            this.DLCActionPanel.ResumeLayout(false);
            this.DLCActionPanel.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgDLCs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelDLCFilters;
        private System.Windows.Forms.ComboBox ComboBoxDLCType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TextFilterDLCs;
        private System.Windows.Forms.FlowLayoutPanel DLCActionPanel;
        private System.Windows.Forms.Button IncludeAlDLCs;
        private System.Windows.Forms.Button ExcludeAllDLCs;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dgDLCs;
        private System.Windows.Forms.DataGridViewCheckBoxColumn CInclude;
        private System.Windows.Forms.DataGridViewLinkColumn CName;
        private System.Windows.Forms.DataGridViewTextBoxColumn CDLCType;
    }
}
