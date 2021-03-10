namespace LoadOrderTool
{
    using System.Windows.Forms;
    partial class LoadOrderWindow
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
        /// the contents of this method with the cdatraode editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.LoadIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsIncluded = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ModEnabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewMods = new System.Windows.Forms.DataGridView();
            this.Save = new System.Windows.Forms.Button();
            this.AutoSave = new System.Windows.Forms.CheckBox();
            this.TabContainer = new System.Windows.Forms.TabControl();
            this.ModsTab = new System.Windows.Forms.TabPage();
            this.AssetsTab = new System.Windows.Forms.TabPage();
            this.SortByHarmony = new System.Windows.Forms.Button();
            this.RandomizeOrder = new System.Windows.Forms.Button();
            this.ReverseOrder = new System.Windows.Forms.Button();
            this.EnableAll = new System.Windows.Forms.Button();
            this.DisableAll = new System.Windows.Forms.Button();
            this.IncludeAll = new System.Windows.Forms.Button();
            this.ExcludeAll = new System.Windows.Forms.Button();
            this.LoadProfile = new System.Windows.Forms.Button();
            this.SaveProfile = new System.Windows.Forms.Button();
            this.ReloadMods = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMods)).BeginInit();
            this.TabContainer.SuspendLayout();
            this.ModsTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // LoadIndex
            // 
            this.LoadIndex.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.LoadIndex.HeaderText = "Index";
            this.LoadIndex.Name = "LoadIndex";
            this.LoadIndex.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.LoadIndex.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.LoadIndex.Width = 45;
            // 
            // IsIncluded
            // 
            this.IsIncluded.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.IsIncluded.HeaderText = "Include";
            this.IsIncluded.Name = "IsIncluded";
            this.IsIncluded.Width = 55;
            // 
            // ModEnabled
            // 
            this.ModEnabled.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ModEnabled.HeaderText = "Enabled";
            this.ModEnabled.Name = "ModEnabled";
            this.ModEnabled.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ModEnabled.Width = 61;
            // 
            // Description
            // 
            this.Description.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Description.HeaderText = "Description";
            this.Description.Name = "Description";
            this.Description.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Description.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewMods
            // 
            this.dataGridViewMods.AllowUserToAddRows = false;
            this.dataGridViewMods.AllowUserToDeleteRows = false;
            this.dataGridViewMods.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Beige;
            this.dataGridViewMods.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewMods.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dataGridViewMods.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMods.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.LoadIndex,
            this.IsIncluded,
            this.ModEnabled,
            this.Description});
            this.dataGridViewMods.Dock = System.Windows.Forms.DockStyle.Left;
            this.dataGridViewMods.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewMods.MultiSelect = false;
            this.dataGridViewMods.Name = "dataGridViewMods";
            this.dataGridViewMods.RowHeadersVisible = false;
            this.dataGridViewMods.Size = new System.Drawing.Size(507, 715);
            this.dataGridViewMods.TabIndex = 0;
            this.dataGridViewMods.Text = "Mods";
            this.dataGridViewMods.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridViewMods_CellFormatting);
            this.dataGridViewMods.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewMods_CellValueChanged);
            this.dataGridViewMods.CurrentCellDirtyStateChanged += new System.EventHandler(this.dataGridViewMods_CurrentCellDirtyStateChanged);
            this.dataGridViewMods.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dataGridViewMods_EditingControlShowing);
            // 
            // Save
            // 
            this.Save.AutoSize = true;
            this.Save.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Save.Location = new System.Drawing.Point(515, 669);
            this.Save.Margin = new System.Windows.Forms.Padding(2);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(101, 27);
            this.Save.TabIndex = 2;
            this.Save.Text = "Save To Game";
            this.Save.UseVisualStyleBackColor = true;
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // AutoSave
            // 
            this.AutoSave.AutoSize = true;
            this.AutoSave.Location = new System.Drawing.Point(621, 673);
            this.AutoSave.Name = "AutoSave";
            this.AutoSave.Size = new System.Drawing.Size(85, 21);
            this.AutoSave.TabIndex = 3;
            this.AutoSave.Text = "Auto-save";
            this.AutoSave.UseVisualStyleBackColor = true;
            this.AutoSave.CheckedChanged += new System.EventHandler(this.AutoSave_CheckedChanged);
            // 
            // TabContainer
            // 
            this.TabContainer.Controls.Add(this.ModsTab);
            this.TabContainer.Controls.Add(this.AssetsTab);
            this.TabContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabContainer.Location = new System.Drawing.Point(0, 0);
            this.TabContainer.Name = "TabContainer";
            this.TabContainer.SelectedIndex = 0;
            this.TabContainer.Size = new System.Drawing.Size(731, 751);
            this.TabContainer.TabIndex = 5;
            // 
            // ModsTab
            // 
            this.ModsTab.Controls.Add(this.dataGridViewMods);
            this.ModsTab.Controls.Add(this.AutoSave);
            this.ModsTab.Controls.Add(this.ReloadMods);
            this.ModsTab.Controls.Add(this.Save);
            this.ModsTab.Controls.Add(this.SortByHarmony);
            this.ModsTab.Controls.Add(this.EnableAll);
            this.ModsTab.Controls.Add(this.LoadProfile);
            this.ModsTab.Controls.Add(this.DisableAll);
            this.ModsTab.Controls.Add(this.SaveProfile);
            this.ModsTab.Controls.Add(this.IncludeAll);
            this.ModsTab.Controls.Add(this.RandomizeOrder);
            this.ModsTab.Controls.Add(this.ExcludeAll);
            this.ModsTab.Controls.Add(this.ReverseOrder);
            this.ModsTab.Location = new System.Drawing.Point(4, 26);
            this.ModsTab.Name = "ModsTab";
            this.ModsTab.Padding = new System.Windows.Forms.Padding(3);
            this.ModsTab.Size = new System.Drawing.Size(723, 721);
            this.ModsTab.TabIndex = 0;
            this.ModsTab.Text = "Mods";
            this.ModsTab.UseVisualStyleBackColor = true;
            // 
            // AssetsTab
            // 
            this.AssetsTab.Location = new System.Drawing.Point(4, 26);
            this.AssetsTab.Name = "AssetsTab";
            this.AssetsTab.Padding = new System.Windows.Forms.Padding(3);
            this.AssetsTab.Size = new System.Drawing.Size(1139, 696);
            this.AssetsTab.TabIndex = 1;
            this.AssetsTab.Text = "Assets";
            this.AssetsTab.UseVisualStyleBackColor = true;
            // 
            // SortByHarmony
            // 
            this.SortByHarmony.AutoSize = true;
            this.SortByHarmony.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.SortByHarmony.Location = new System.Drawing.Point(515, 38);
            this.SortByHarmony.Margin = new System.Windows.Forms.Padding(2);
            this.SortByHarmony.Name = "SortByHarmony";
            this.SortByHarmony.Size = new System.Drawing.Size(116, 27);
            this.SortByHarmony.TabIndex = 1;
            this.SortByHarmony.Text = "Sort By Harmony";
            this.SortByHarmony.UseVisualStyleBackColor = true;
            this.SortByHarmony.Click += new System.EventHandler(this.SortByHarmony_Click);
            // 
            // RandomizeOrder
            // 
            this.RandomizeOrder.AutoSize = true;
            this.RandomizeOrder.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.RandomizeOrder.Location = new System.Drawing.Point(515, 94);
            this.RandomizeOrder.Margin = new System.Windows.Forms.Padding(2);
            this.RandomizeOrder.Name = "RandomizeOrder";
            this.RandomizeOrder.Size = new System.Drawing.Size(122, 27);
            this.RandomizeOrder.TabIndex = 1;
            this.RandomizeOrder.Text = "Randomize Order";
            this.RandomizeOrder.UseVisualStyleBackColor = true;
            this.RandomizeOrder.Click += new System.EventHandler(this.RandomizeOrder_Click);
            // 
            // ReverseOrder
            // 
            this.ReverseOrder.AutoSize = true;
            this.ReverseOrder.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ReverseOrder.Location = new System.Drawing.Point(515, 66);
            this.ReverseOrder.Margin = new System.Windows.Forms.Padding(2);
            this.ReverseOrder.Name = "ReverseOrder";
            this.ReverseOrder.Size = new System.Drawing.Size(103, 27);
            this.ReverseOrder.TabIndex = 1;
            this.ReverseOrder.Text = "Reverse Order";
            this.ReverseOrder.UseVisualStyleBackColor = true;
            this.ReverseOrder.Click += new System.EventHandler(this.ReverseOrder_Click);
            // 
            // EnableAll
            // 
            this.EnableAll.AutoSize = true;
            this.EnableAll.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.EnableAll.Location = new System.Drawing.Point(515, 133);
            this.EnableAll.Margin = new System.Windows.Forms.Padding(2);
            this.EnableAll.Name = "EnableAll";
            this.EnableAll.Size = new System.Drawing.Size(75, 27);
            this.EnableAll.TabIndex = 1;
            this.EnableAll.Text = "Enable All";
            this.EnableAll.UseVisualStyleBackColor = true;
            this.EnableAll.Click += new System.EventHandler(this.EnableAll_Click);
            // 
            // DisableAll
            // 
            this.DisableAll.AutoSize = true;
            this.DisableAll.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.DisableAll.Location = new System.Drawing.Point(515, 161);
            this.DisableAll.Margin = new System.Windows.Forms.Padding(2);
            this.DisableAll.Name = "DisableAll";
            this.DisableAll.Size = new System.Drawing.Size(79, 27);
            this.DisableAll.TabIndex = 1;
            this.DisableAll.Text = "Disable All";
            this.DisableAll.UseVisualStyleBackColor = true;
            this.DisableAll.Click += new System.EventHandler(this.DisableAll_Click);
            // 
            // IncludeAll
            // 
            this.IncludeAll.AutoSize = true;
            this.IncludeAll.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.IncludeAll.Location = new System.Drawing.Point(515, 189);
            this.IncludeAll.Margin = new System.Windows.Forms.Padding(2);
            this.IncludeAll.Name = "IncludeAll";
            this.IncludeAll.Size = new System.Drawing.Size(77, 27);
            this.IncludeAll.TabIndex = 1;
            this.IncludeAll.Text = "Include All";
            this.IncludeAll.UseVisualStyleBackColor = true;
            this.IncludeAll.Click += new System.EventHandler(this.IncludeAll_Click);
            // 
            // ExcludeAll
            // 
            this.ExcludeAll.AutoSize = true;
            this.ExcludeAll.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ExcludeAll.Location = new System.Drawing.Point(515, 217);
            this.ExcludeAll.Margin = new System.Windows.Forms.Padding(2);
            this.ExcludeAll.Name = "ExcludeAll";
            this.ExcludeAll.Size = new System.Drawing.Size(80, 27);
            this.ExcludeAll.TabIndex = 1;
            this.ExcludeAll.Text = "Exclude All";
            this.ExcludeAll.UseVisualStyleBackColor = true;
            this.ExcludeAll.Click += new System.EventHandler(this.ExcludeAll_Click);
            // 
            // LoadProfile
            // 
            this.LoadProfile.AutoSize = true;
            this.LoadProfile.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.LoadProfile.Location = new System.Drawing.Point(515, 638);
            this.LoadProfile.Margin = new System.Windows.Forms.Padding(2);
            this.LoadProfile.Name = "LoadProfile";
            this.LoadProfile.Size = new System.Drawing.Size(88, 27);
            this.LoadProfile.TabIndex = 1;
            this.LoadProfile.Text = "Load Profile";
            this.LoadProfile.UseVisualStyleBackColor = true;
            this.LoadProfile.Click += new System.EventHandler(this.LoadProfile_Click);
            // 
            // SaveProfile
            // 
            this.SaveProfile.AutoSize = true;
            this.SaveProfile.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.SaveProfile.Location = new System.Drawing.Point(515, 607);
            this.SaveProfile.Margin = new System.Windows.Forms.Padding(2);
            this.SaveProfile.Name = "SaveProfile";
            this.SaveProfile.Size = new System.Drawing.Size(72, 27);
            this.SaveProfile.TabIndex = 1;
            this.SaveProfile.Text = "Save As...";
            this.SaveProfile.UseVisualStyleBackColor = true;
            this.SaveProfile.Click += new System.EventHandler(this.SaveProfile_Click);
            // 
            // ReloadMods
            // 
            this.ReloadMods.AutoSize = true;
            this.ReloadMods.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ReloadMods.Location = new System.Drawing.Point(515, 576);
            this.ReloadMods.Margin = new System.Windows.Forms.Padding(2);
            this.ReloadMods.Name = "ReloadMods";
            this.ReloadMods.Size = new System.Drawing.Size(97, 27);
            this.ReloadMods.TabIndex = 4;
            this.ReloadMods.Text = "Reload Mods";
            this.ReloadMods.UseVisualStyleBackColor = true;
            this.ReloadMods.Click += new System.EventHandler(this.ReloadMods_Click);
            // 
            // LoadOrderWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(731, 751);
            this.Controls.Add(this.TabContainer);
            this.Name = "LoadOrderWindow";
            this.Text = "LoadOrder";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LoadOrder_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMods)).EndInit();
            this.TabContainer.ResumeLayout(false);
            this.ModsTab.ResumeLayout(false);
            this.ModsTab.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.DataGridView dataGridViewMods;
        private DataGridViewTextBoxColumn LoadIndex;
        private DataGridViewCheckBoxColumn IsIncluded;
        private DataGridViewCheckBoxColumn ModEnabled;
        private DataGridViewTextBoxColumn Description;
        private Button Save;
        private CheckBox AutoSave;
        private TabControl TabContainer;
        private TabPage ModsTab;
        private TabPage AssetsTab;
        private Button ReloadMods;
        private Button SortByHarmony;
        private Button EnableAll;
        private Button LoadProfile;
        private Button DisableAll;
        private Button SaveProfile;
        private Button IncludeAll;
        private Button RandomizeOrder;
        private Button ExcludeAll;
        private Button ReverseOrder;
    }
}