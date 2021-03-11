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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.TabContainer = new System.Windows.Forms.TabControl();
            this.ModsTab = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.ComboBoxIncluded = new System.Windows.Forms.ComboBox();
            this.ComboBoxEnabled = new System.Windows.Forms.ComboBox();
            this.ComboBoxWS = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TextFilterMods = new System.Windows.Forms.TextBox();
            this.ModsButtons = new System.Windows.Forms.Panel();
            this.SortByHarmony = new System.Windows.Forms.Button();
            this.ReverseOrder = new System.Windows.Forms.Button();
            this.ExcludeAllMods = new System.Windows.Forms.Button();
            this.EnableAllMods = new System.Windows.Forms.Button();
            this.RandomizeOrder = new System.Windows.Forms.Button();
            this.DisableAllMods = new System.Windows.Forms.Button();
            this.IncludeAllMods = new System.Windows.Forms.Button();
            this.dataGridViewMods = new System.Windows.Forms.DataGridView();
            this.LoadIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsIncluded = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ModEnabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AssetsTab = new System.Windows.Forms.TabPage();
            this.AssetTabSplitter = new System.Windows.Forms.SplitContainer();
            this.AssetsActionsPanel = new System.Windows.Forms.Panel();
            this.ExcludeAllAssets = new System.Windows.Forms.Button();
            this.IncludeAllAssets = new System.Windows.Forms.Button();
            this.CheckedListBoxAssets = new System.Windows.Forms.CheckedListBox();
            this.AutoSave = new System.Windows.Forms.CheckBox();
            this.ReloadAll = new System.Windows.Forms.Button();
            this.Save = new System.Windows.Forms.Button();
            this.LoadProfile = new System.Windows.Forms.Button();
            this.SaveProfile = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.TabContainer.SuspendLayout();
            this.ModsTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.ModsButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMods)).BeginInit();
            this.AssetsTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AssetTabSplitter)).BeginInit();
            this.AssetTabSplitter.Panel2.SuspendLayout();
            this.AssetTabSplitter.SuspendLayout();
            this.AssetsActionsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.TabContainer);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.AutoSave);
            this.splitContainer1.Panel2.Controls.Add(this.ReloadAll);
            this.splitContainer1.Panel2.Controls.Add(this.Save);
            this.splitContainer1.Panel2.Controls.Add(this.LoadProfile);
            this.splitContainer1.Panel2.Controls.Add(this.SaveProfile);
            this.splitContainer1.Size = new System.Drawing.Size(806, 646);
            this.splitContainer1.SplitterDistance = 610;
            this.splitContainer1.TabIndex = 6;
            // 
            // TabContainer
            // 
            this.TabContainer.Controls.Add(this.ModsTab);
            this.TabContainer.Controls.Add(this.AssetsTab);
            this.TabContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabContainer.Location = new System.Drawing.Point(0, 0);
            this.TabContainer.Name = "TabContainer";
            this.TabContainer.SelectedIndex = 0;
            this.TabContainer.Size = new System.Drawing.Size(806, 610);
            this.TabContainer.TabIndex = 6;
            // 
            // ModsTab
            // 
            this.ModsTab.BackColor = System.Drawing.SystemColors.Control;
            this.ModsTab.Controls.Add(this.splitContainer2);
            this.ModsTab.Location = new System.Drawing.Point(4, 26);
            this.ModsTab.Name = "ModsTab";
            this.ModsTab.Padding = new System.Windows.Forms.Padding(3);
            this.ModsTab.Size = new System.Drawing.Size(798, 580);
            this.ModsTab.TabIndex = 0;
            this.ModsTab.Text = "Mods";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(3, 3);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer3);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.ModsButtons);
            this.splitContainer2.Panel2.Controls.Add(this.dataGridViewMods);
            this.splitContainer2.Size = new System.Drawing.Size(792, 574);
            this.splitContainer2.SplitterDistance = 31;
            this.splitContainer2.TabIndex = 3;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.flowLayoutPanel1);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.TextFilterMods);
            this.splitContainer3.Size = new System.Drawing.Size(792, 31);
            this.splitContainer3.SplitterDistance = 422;
            this.splitContainer3.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.ComboBoxIncluded);
            this.flowLayoutPanel1.Controls.Add(this.ComboBoxEnabled);
            this.flowLayoutPanel1.Controls.Add(this.ComboBoxWS);
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(422, 31);
            this.flowLayoutPanel1.TabIndex = 0;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // ComboBoxIncluded
            // 
            this.ComboBoxIncluded.FormattingEnabled = true;
            this.ComboBoxIncluded.Location = new System.Drawing.Point(3, 3);
            this.ComboBoxIncluded.Name = "ComboBoxIncluded";
            this.ComboBoxIncluded.Size = new System.Drawing.Size(121, 25);
            this.ComboBoxIncluded.TabIndex = 1;
            // 
            // ComboBoxEnabled
            // 
            this.ComboBoxEnabled.FormattingEnabled = true;
            this.ComboBoxEnabled.Location = new System.Drawing.Point(130, 3);
            this.ComboBoxEnabled.Name = "ComboBoxEnabled";
            this.ComboBoxEnabled.Size = new System.Drawing.Size(121, 25);
            this.ComboBoxEnabled.TabIndex = 3;
            // 
            // ComboBoxWS
            // 
            this.ComboBoxWS.FormattingEnabled = true;
            this.ComboBoxWS.Location = new System.Drawing.Point(257, 3);
            this.ComboBoxWS.Name = "ComboBoxWS";
            this.ComboBoxWS.Size = new System.Drawing.Size(121, 25);
            this.ComboBoxWS.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(384, 0);
            this.label1.MinimumSize = new System.Drawing.Size(0, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 25);
            this.label1.TabIndex = 5;
            this.label1.Text = "Filter:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TextFilterMods
            // 
            this.TextFilterMods.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextFilterMods.Location = new System.Drawing.Point(0, 0);
            this.TextFilterMods.Name = "TextFilterMods";
            this.TextFilterMods.Size = new System.Drawing.Size(366, 25);
            this.TextFilterMods.TabIndex = 0;
            // 
            // ModsButtons
            // 
            this.ModsButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ModsButtons.Controls.Add(this.SortByHarmony);
            this.ModsButtons.Controls.Add(this.ReverseOrder);
            this.ModsButtons.Controls.Add(this.ExcludeAllMods);
            this.ModsButtons.Controls.Add(this.EnableAllMods);
            this.ModsButtons.Controls.Add(this.RandomizeOrder);
            this.ModsButtons.Controls.Add(this.DisableAllMods);
            this.ModsButtons.Controls.Add(this.IncludeAllMods);
            this.ModsButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.ModsButtons.Location = new System.Drawing.Point(654, 0);
            this.ModsButtons.Name = "ModsButtons";
            this.ModsButtons.Size = new System.Drawing.Size(138, 539);
            this.ModsButtons.TabIndex = 3;
            // 
            // SortByHarmony
            // 
            this.SortByHarmony.AutoSize = true;
            this.SortByHarmony.Location = new System.Drawing.Point(9, 1);
            this.SortByHarmony.Margin = new System.Windows.Forms.Padding(2);
            this.SortByHarmony.Name = "SortByHarmony";
            this.SortByHarmony.Size = new System.Drawing.Size(122, 27);
            this.SortByHarmony.TabIndex = 1;
            this.SortByHarmony.Text = "Sort By Harmony";
            this.SortByHarmony.UseVisualStyleBackColor = true;
            // 
            // ReverseOrder
            // 
            this.ReverseOrder.AutoSize = true;
            this.ReverseOrder.Location = new System.Drawing.Point(9, 29);
            this.ReverseOrder.Margin = new System.Windows.Forms.Padding(2);
            this.ReverseOrder.Name = "ReverseOrder";
            this.ReverseOrder.Size = new System.Drawing.Size(122, 27);
            this.ReverseOrder.TabIndex = 1;
            this.ReverseOrder.Text = "Reverse Order";
            this.ReverseOrder.UseVisualStyleBackColor = true;
            // 
            // ExcludeAllMods
            // 
            this.ExcludeAllMods.AutoSize = true;
            this.ExcludeAllMods.Location = new System.Drawing.Point(9, 180);
            this.ExcludeAllMods.Margin = new System.Windows.Forms.Padding(2);
            this.ExcludeAllMods.Name = "ExcludeAllMods";
            this.ExcludeAllMods.Size = new System.Drawing.Size(122, 27);
            this.ExcludeAllMods.TabIndex = 1;
            this.ExcludeAllMods.Text = "Exclude All";
            this.ExcludeAllMods.UseVisualStyleBackColor = true;
            // 
            // EnableAllMods
            // 
            this.EnableAllMods.AutoSize = true;
            this.EnableAllMods.Location = new System.Drawing.Point(9, 96);
            this.EnableAllMods.Margin = new System.Windows.Forms.Padding(2);
            this.EnableAllMods.Name = "EnableAllMods";
            this.EnableAllMods.Size = new System.Drawing.Size(122, 27);
            this.EnableAllMods.TabIndex = 1;
            this.EnableAllMods.Text = "Enable All";
            this.EnableAllMods.UseVisualStyleBackColor = true;
            // 
            // RandomizeOrder
            // 
            this.RandomizeOrder.AutoSize = true;
            this.RandomizeOrder.Location = new System.Drawing.Point(9, 57);
            this.RandomizeOrder.Margin = new System.Windows.Forms.Padding(2);
            this.RandomizeOrder.Name = "RandomizeOrder";
            this.RandomizeOrder.Size = new System.Drawing.Size(122, 27);
            this.RandomizeOrder.TabIndex = 1;
            this.RandomizeOrder.Text = "Randomize Order";
            this.RandomizeOrder.UseVisualStyleBackColor = true;
            // 
            // DisableAllMods
            // 
            this.DisableAllMods.AutoSize = true;
            this.DisableAllMods.Location = new System.Drawing.Point(9, 124);
            this.DisableAllMods.Margin = new System.Windows.Forms.Padding(2);
            this.DisableAllMods.Name = "DisableAllMods";
            this.DisableAllMods.Size = new System.Drawing.Size(122, 27);
            this.DisableAllMods.TabIndex = 1;
            this.DisableAllMods.Text = "Disable All";
            this.DisableAllMods.UseVisualStyleBackColor = true;
            // 
            // IncludeAllMods
            // 
            this.IncludeAllMods.AutoSize = true;
            this.IncludeAllMods.Location = new System.Drawing.Point(9, 152);
            this.IncludeAllMods.Margin = new System.Windows.Forms.Padding(2);
            this.IncludeAllMods.Name = "IncludeAllMods";
            this.IncludeAllMods.Size = new System.Drawing.Size(122, 27);
            this.IncludeAllMods.TabIndex = 1;
            this.IncludeAllMods.Text = "Include All";
            this.IncludeAllMods.UseVisualStyleBackColor = true;
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
            this.dataGridViewMods.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewMods.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewMods.MultiSelect = false;
            this.dataGridViewMods.Name = "dataGridViewMods";
            this.dataGridViewMods.RowHeadersVisible = false;
            this.dataGridViewMods.Size = new System.Drawing.Size(792, 539);
            this.dataGridViewMods.TabIndex = 1;
            this.dataGridViewMods.Text = "Mods";
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
            // AssetsTab
            // 
            this.AssetsTab.BackColor = System.Drawing.SystemColors.Control;
            this.AssetsTab.Controls.Add(this.AssetTabSplitter);
            this.AssetsTab.Location = new System.Drawing.Point(4, 26);
            this.AssetsTab.Name = "AssetsTab";
            this.AssetsTab.Padding = new System.Windows.Forms.Padding(3);
            this.AssetsTab.Size = new System.Drawing.Size(798, 580);
            this.AssetsTab.TabIndex = 1;
            this.AssetsTab.Text = "Assets";
            // 
            // AssetTabSplitter
            // 
            this.AssetTabSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AssetTabSplitter.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.AssetTabSplitter.Location = new System.Drawing.Point(3, 3);
            this.AssetTabSplitter.Name = "AssetTabSplitter";
            this.AssetTabSplitter.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // AssetTabSplitter.Panel2
            // 
            this.AssetTabSplitter.Panel2.Controls.Add(this.AssetsActionsPanel);
            this.AssetTabSplitter.Panel2.Controls.Add(this.CheckedListBoxAssets);
            this.AssetTabSplitter.Size = new System.Drawing.Size(792, 574);
            this.AssetTabSplitter.SplitterDistance = 30;
            this.AssetTabSplitter.TabIndex = 0;
            // 
            // AssetsActionsPanel
            // 
            this.AssetsActionsPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.AssetsActionsPanel.Controls.Add(this.ExcludeAllAssets);
            this.AssetsActionsPanel.Controls.Add(this.IncludeAllAssets);
            this.AssetsActionsPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.AssetsActionsPanel.Location = new System.Drawing.Point(712, 0);
            this.AssetsActionsPanel.Name = "AssetsActionsPanel";
            this.AssetsActionsPanel.Size = new System.Drawing.Size(80, 540);
            this.AssetsActionsPanel.TabIndex = 5;
            // 
            // ExcludeAllAssets
            // 
            this.ExcludeAllAssets.AutoSize = true;
            this.ExcludeAllAssets.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ExcludeAllAssets.Location = new System.Drawing.Point(1, 28);
            this.ExcludeAllAssets.Margin = new System.Windows.Forms.Padding(2);
            this.ExcludeAllAssets.Name = "ExcludeAllAssets";
            this.ExcludeAllAssets.Size = new System.Drawing.Size(80, 27);
            this.ExcludeAllAssets.TabIndex = 1;
            this.ExcludeAllAssets.Text = "Exclude All";
            this.ExcludeAllAssets.UseVisualStyleBackColor = true;
            // 
            // IncludeAllAssets
            // 
            this.IncludeAllAssets.AutoSize = true;
            this.IncludeAllAssets.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.IncludeAllAssets.Location = new System.Drawing.Point(1, 0);
            this.IncludeAllAssets.Margin = new System.Windows.Forms.Padding(2);
            this.IncludeAllAssets.Name = "IncludeAllAssets";
            this.IncludeAllAssets.Size = new System.Drawing.Size(77, 27);
            this.IncludeAllAssets.TabIndex = 1;
            this.IncludeAllAssets.Text = "Include All";
            this.IncludeAllAssets.UseVisualStyleBackColor = true;
            // 
            // CheckedListBoxAssets
            // 
            this.CheckedListBoxAssets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CheckedListBoxAssets.FormattingEnabled = true;
            this.CheckedListBoxAssets.Location = new System.Drawing.Point(0, 0);
            this.CheckedListBoxAssets.Name = "CheckedListBoxAssets";
            this.CheckedListBoxAssets.Size = new System.Drawing.Size(792, 540);
            this.CheckedListBoxAssets.TabIndex = 4;
            // 
            // AutoSave
            // 
            this.AutoSave.AutoSize = true;
            this.AutoSave.Location = new System.Drawing.Point(413, 5);
            this.AutoSave.Name = "AutoSave";
            this.AutoSave.Size = new System.Drawing.Size(85, 21);
            this.AutoSave.TabIndex = 8;
            this.AutoSave.Text = "Auto-save";
            this.AutoSave.UseVisualStyleBackColor = true;
            // 
            // ReloadAll
            // 
            this.ReloadAll.AutoSize = true;
            this.ReloadAll.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ReloadAll.Location = new System.Drawing.Point(0, 1);
            this.ReloadAll.Margin = new System.Windows.Forms.Padding(2);
            this.ReloadAll.Name = "ReloadAll";
            this.ReloadAll.Size = new System.Drawing.Size(77, 27);
            this.ReloadAll.TabIndex = 9;
            this.ReloadAll.Text = "Reload All";
            this.ReloadAll.UseVisualStyleBackColor = true;
            // 
            // Save
            // 
            this.Save.AutoSize = true;
            this.Save.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Save.Location = new System.Drawing.Point(307, 1);
            this.Save.Margin = new System.Windows.Forms.Padding(2);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(101, 27);
            this.Save.TabIndex = 7;
            this.Save.Text = "Save To Game";
            this.Save.UseVisualStyleBackColor = true;
            // 
            // LoadProfile
            // 
            this.LoadProfile.AutoSize = true;
            this.LoadProfile.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.LoadProfile.Location = new System.Drawing.Point(200, 1);
            this.LoadProfile.Margin = new System.Windows.Forms.Padding(2);
            this.LoadProfile.Name = "LoadProfile";
            this.LoadProfile.Size = new System.Drawing.Size(88, 27);
            this.LoadProfile.TabIndex = 5;
            this.LoadProfile.Text = "Load Profile";
            this.LoadProfile.UseVisualStyleBackColor = true;
            // 
            // SaveProfile
            // 
            this.SaveProfile.AutoSize = true;
            this.SaveProfile.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.SaveProfile.Location = new System.Drawing.Point(126, 1);
            this.SaveProfile.Margin = new System.Windows.Forms.Padding(2);
            this.SaveProfile.Name = "SaveProfile";
            this.SaveProfile.Size = new System.Drawing.Size(72, 27);
            this.SaveProfile.TabIndex = 6;
            this.SaveProfile.Text = "Save As...";
            this.SaveProfile.UseVisualStyleBackColor = true;
            // 
            // LoadOrderWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(806, 646);
            this.Controls.Add(this.splitContainer1);
            this.MinimumSize = new System.Drawing.Size(524, 334);
            this.Name = "LoadOrderWindow";
            this.Text = "LoadOrder";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LoadOrder_FormClosing);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.TabContainer.ResumeLayout(false);
            this.ModsTab.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ModsButtons.ResumeLayout(false);
            this.ModsButtons.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMods)).EndInit();
            this.AssetsTab.ResumeLayout(false);
            this.AssetTabSplitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.AssetTabSplitter)).EndInit();
            this.AssetTabSplitter.ResumeLayout(false);
            this.AssetsActionsPanel.ResumeLayout(false);
            this.AssetsActionsPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private SplitContainer splitContainer1;
        private TabControl TabContainer;
        private TabPage ModsTab;
        private TabPage AssetsTab;
        private CheckBox AutoSave;
        private Button ReloadAll;
        private Button Save;
        private Button LoadProfile;
        private Button SaveProfile;
        private SplitContainer splitContainer2;
        private Panel ModsButtons;
        private Button SortByHarmony;
        private Button ReverseOrder;
        private Button ExcludeAllMods;
        private Button EnableAllMods;
        private Button RandomizeOrder;
        private Button DisableAllMods;
        private Button IncludeAllMods;
        public DataGridView dataGridViewMods;
        private DataGridViewTextBoxColumn LoadIndex;
        private DataGridViewCheckBoxColumn IsIncluded;
        private DataGridViewCheckBoxColumn ModEnabled;
        private DataGridViewTextBoxColumn Description;
        private SplitContainer AssetTabSplitter;
        private Panel AssetsActionsPanel;
        private Button ExcludeAllAssets;
        private Button IncludeAllAssets;
        private CheckedListBox CheckedListBoxAssets;
        private SplitContainer splitContainer3;
        private FlowLayoutPanel flowLayoutPanel1;
        private ComboBox ComboBoxIncluded;
        private ComboBox ComboBoxEnabled;
        private TextBox TextFilterMods;
        private ComboBox ComboBoxWS;
        private Label label1;
    }
}