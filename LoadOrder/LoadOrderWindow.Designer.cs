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
            this.FilePanel = new System.Windows.Forms.FlowLayoutPanel();
            this.ReloadAll = new System.Windows.Forms.Button();
            this.SaveProfile = new System.Windows.Forms.Button();
            this.LoadProfile = new System.Windows.Forms.Button();
            this.Save = new System.Windows.Forms.Button();
            this.AutoSave = new System.Windows.Forms.CheckBox();
            this.TabContainer = new System.Windows.Forms.TabControl();
            this.ModsTab = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelModFilters = new System.Windows.Forms.TableLayoutPanel();
            this.ComboBoxIncluded = new System.Windows.Forms.ComboBox();
            this.ComboBoxEnabled = new System.Windows.Forms.ComboBox();
            this.ComboBoxWS = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TextFilterMods = new System.Windows.Forms.TextBox();
            this.dataGridViewMods = new System.Windows.Forms.DataGridView();
            this.LoadIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsIncluded = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ModEnabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ModActionPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.ResetOrder = new System.Windows.Forms.Button();
            this.SortByHarmony = new System.Windows.Forms.Button();
            this.ReverseOrder = new System.Windows.Forms.Button();
            this.ExcludeAllMods = new System.Windows.Forms.Button();
            this.EnableAllMods = new System.Windows.Forms.Button();
            this.RandomizeOrder = new System.Windows.Forms.Button();
            this.DisableAllMods = new System.Windows.Forms.Button();
            this.IncludeAllMods = new System.Windows.Forms.Button();
            this.AssetsTab = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelAssetFilters = new System.Windows.Forms.TableLayoutPanel();
            this.ComboBoxAssetIncluded = new System.Windows.Forms.ComboBox();
            this.ComboBoxAssetWS = new System.Windows.Forms.ComboBox();
            this.ComboBoxAssetTags = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TextFilterAsset = new System.Windows.Forms.TextBox();
            this.dataGridAssets = new System.Windows.Forms.DataGridView();
            this.cIncluded = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cAuthor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cTags = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AssetsActionPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.ExcludeAllAssets = new System.Windows.Forms.Button();
            this.IncludeAllAssets = new System.Windows.Forms.Button();
            this.FilePanel.SuspendLayout();
            this.TabContainer.SuspendLayout();
            this.ModsTab.SuspendLayout();
            this.tableLayoutPanelModFilters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMods)).BeginInit();
            this.ModActionPanel.SuspendLayout();
            this.AssetsTab.SuspendLayout();
            this.tableLayoutPanelAssetFilters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridAssets)).BeginInit();
            this.AssetsActionPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // FilePanel
            // 
            this.FilePanel.AutoSize = true;
            this.FilePanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.FilePanel.Controls.Add(this.ReloadAll);
            this.FilePanel.Controls.Add(this.SaveProfile);
            this.FilePanel.Controls.Add(this.LoadProfile);
            this.FilePanel.Controls.Add(this.Save);
            this.FilePanel.Controls.Add(this.AutoSave);
            this.FilePanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.FilePanel.Location = new System.Drawing.Point(0, 615);
            this.FilePanel.Name = "FilePanel";
            this.FilePanel.Size = new System.Drawing.Size(806, 31);
            this.FilePanel.TabIndex = 1;
            // 
            // ReloadAll
            // 
            this.ReloadAll.AutoSize = true;
            this.ReloadAll.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ReloadAll.Location = new System.Drawing.Point(2, 2);
            this.ReloadAll.Margin = new System.Windows.Forms.Padding(2);
            this.ReloadAll.Name = "ReloadAll";
            this.ReloadAll.Size = new System.Drawing.Size(77, 27);
            this.ReloadAll.TabIndex = 0;
            this.ReloadAll.Text = "Reload All";
            this.ReloadAll.UseVisualStyleBackColor = true;
            // 
            // SaveProfile
            // 
            this.SaveProfile.AutoSize = true;
            this.SaveProfile.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.SaveProfile.Location = new System.Drawing.Point(83, 2);
            this.SaveProfile.Margin = new System.Windows.Forms.Padding(2);
            this.SaveProfile.Name = "SaveProfile";
            this.SaveProfile.Size = new System.Drawing.Size(97, 27);
            this.SaveProfile.TabIndex = 1;
            this.SaveProfile.Text = "Export Profile";
            this.SaveProfile.UseVisualStyleBackColor = true;
           // 
            // LoadProfile
            // 
            this.LoadProfile.AutoSize = true;
            this.LoadProfile.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.LoadProfile.Location = new System.Drawing.Point(184, 2);
            this.LoadProfile.Margin = new System.Windows.Forms.Padding(2);
            this.LoadProfile.Name = "LoadProfile";
            this.LoadProfile.Size = new System.Drawing.Size(98, 27);
            this.LoadProfile.TabIndex = 2;
            this.LoadProfile.Text = "Import Profile";
            this.LoadProfile.UseVisualStyleBackColor = true;
            // 
            // Save
            // 
            this.Save.AutoSize = true;
            this.Save.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Save.Location = new System.Drawing.Point(286, 2);
            this.Save.Margin = new System.Windows.Forms.Padding(2);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(101, 27);
            this.Save.TabIndex = 3;
            this.Save.Text = "Save To Game";
            this.Save.UseVisualStyleBackColor = true;
            // 
            // AutoSave
            // 
            this.AutoSave.AutoSize = true;
            this.AutoSave.Location = new System.Drawing.Point(392, 3);
            this.AutoSave.Name = "AutoSave";
            this.AutoSave.Size = new System.Drawing.Size(86, 21);
            this.AutoSave.TabIndex = 4;
            this.AutoSave.Text = "Auto-Save";
            this.AutoSave.UseVisualStyleBackColor = true;
            // 
            // TabContainer
            // 
            this.TabContainer.Controls.Add(this.ModsTab);
            this.TabContainer.Controls.Add(this.AssetsTab);
            this.TabContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabContainer.Location = new System.Drawing.Point(0, 0);
            this.TabContainer.Name = "TabContainer";
            this.TabContainer.SelectedIndex = 0;
            this.TabContainer.Size = new System.Drawing.Size(806, 615);
            this.TabContainer.TabIndex = 0;
            // 
            // ModsTab
            // 
            this.ModsTab.BackColor = System.Drawing.SystemColors.Control;
            this.ModsTab.Controls.Add(this.dataGridViewMods);
            this.ModsTab.Controls.Add(this.ModActionPanel);
            this.ModsTab.Controls.Add(this.tableLayoutPanelModFilters);
            this.ModsTab.Location = new System.Drawing.Point(4, 26);
            this.ModsTab.Name = "ModsTab";
            this.ModsTab.Padding = new System.Windows.Forms.Padding(3);
            this.ModsTab.Size = new System.Drawing.Size(798, 585);
            this.ModsTab.TabIndex = 0;
            this.ModsTab.Text = "Mods";
            // 
            // tableLayoutPanelModFilters
            // 
            this.tableLayoutPanelModFilters.AutoSize = true;
            this.tableLayoutPanelModFilters.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanelModFilters.ColumnCount = 5;
            this.tableLayoutPanelModFilters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelModFilters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelModFilters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelModFilters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelModFilters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelModFilters.Controls.Add(this.ComboBoxIncluded);
            this.tableLayoutPanelModFilters.Controls.Add(this.ComboBoxEnabled);
            this.tableLayoutPanelModFilters.Controls.Add(this.ComboBoxWS);
            this.tableLayoutPanelModFilters.Controls.Add(this.label1);
            this.tableLayoutPanelModFilters.Controls.Add(this.TextFilterMods);
            this.tableLayoutPanelModFilters.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelModFilters.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.AddColumns;
            this.tableLayoutPanelModFilters.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelModFilters.Name = "tableLayoutPanelModFilters";
            this.tableLayoutPanelModFilters.RowCount = 1;
            this.tableLayoutPanelModFilters.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelModFilters.Size = new System.Drawing.Size(654, 31);
            this.tableLayoutPanelModFilters.TabIndex = 0;
            // 
            // ComboBoxIncluded
            // 
            this.ComboBoxIncluded.FormattingEnabled = true;
            this.ComboBoxIncluded.Location = new System.Drawing.Point(3, 3);
            this.ComboBoxIncluded.Name = "ComboBoxIncluded";
            this.ComboBoxIncluded.Size = new System.Drawing.Size(121, 25);
            this.ComboBoxIncluded.TabIndex = 0;
            // 
            // ComboBoxEnabled
            // 
            this.ComboBoxEnabled.FormattingEnabled = true;
            this.ComboBoxEnabled.Location = new System.Drawing.Point(130, 3);
            this.ComboBoxEnabled.Name = "ComboBoxEnabled";
            this.ComboBoxEnabled.Size = new System.Drawing.Size(121, 25);
            this.ComboBoxEnabled.TabIndex = 1;
            // 
            // ComboBoxWS
            // 
            this.ComboBoxWS.FormattingEnabled = true;
            this.ComboBoxWS.Location = new System.Drawing.Point(257, 3);
            this.ComboBoxWS.Name = "ComboBoxWS";
            this.ComboBoxWS.Size = new System.Drawing.Size(121, 25);
            this.ComboBoxWS.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(384, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Filter:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TextFilterMods
            // 
            this.TextFilterMods.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextFilterMods.Location = new System.Drawing.Point(429, 3);
            this.TextFilterMods.Name = "TextFilterMods";
            this.TextFilterMods.Size = new System.Drawing.Size(222, 25);
            this.TextFilterMods.TabIndex = 4;
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
            this.dataGridViewMods.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewMods.MultiSelect = false;
            this.dataGridViewMods.Name = "dataGridViewMods";
            this.dataGridViewMods.RowHeadersVisible = false;
            this.dataGridViewMods.Size = new System.Drawing.Size(654, 579);
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
            this.Description.ReadOnly = true;
            this.Description.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Description.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ModActionPanel
            // 
            this.ModActionPanel.FlowDirection = FlowDirection.TopDown;
            this.ModActionPanel.AutoSize = true;
            this.ModActionPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ModActionPanel.Controls.Add(this.ResetOrder);
            this.ModActionPanel.Controls.Add(this.SortByHarmony);
            this.ModActionPanel.Controls.Add(this.ReverseOrder);
            this.ModActionPanel.Controls.Add(this.RandomizeOrder);
            this.ModActionPanel.Controls.Add(this.IncludeAllMods);
            this.ModActionPanel.Controls.Add(this.ExcludeAllMods);
            this.ModActionPanel.Controls.Add(this.EnableAllMods);
            this.ModActionPanel.Controls.Add(this.DisableAllMods);
            this.ModActionPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.ModActionPanel.Name = "ModActionPanel";
            // 
            // ResetOrder
            // 
            this.ResetOrder.AutoSize = true;
            this.ResetOrder.Location = new System.Drawing.Point(9, 85);
            this.ResetOrder.Margin = new System.Windows.Forms.Padding(2);
            this.ResetOrder.Name = "ResetOrder";
            this.ResetOrder.Size = new System.Drawing.Size(122, 27);
            this.ResetOrder.TabIndex = 0;
            this.ResetOrder.Text = "Reset Order";
            this.ResetOrder.UseVisualStyleBackColor = true;
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
            this.ReverseOrder.TabIndex = 2;
            this.ReverseOrder.Text = "Reverse Order";
            this.ReverseOrder.UseVisualStyleBackColor = true;
            // 
            // ExcludeAllMods
            // 
            this.ExcludeAllMods.AutoSize = true;
            this.ExcludeAllMods.Location = new System.Drawing.Point(9, 227);
            this.ExcludeAllMods.Margin = new System.Windows.Forms.Padding(2);
            this.ExcludeAllMods.Name = "ExcludeAllMods";
            this.ExcludeAllMods.Size = new System.Drawing.Size(122, 27);
            this.ExcludeAllMods.TabIndex = 3;
            this.ExcludeAllMods.Text = "Exclude All";
            this.ExcludeAllMods.UseVisualStyleBackColor = true;
            // 
            // EnableAllMods
            // 
            this.EnableAllMods.AutoSize = true;
            this.EnableAllMods.Location = new System.Drawing.Point(9, 143);
            this.EnableAllMods.Margin = new System.Windows.Forms.Padding(2);
            this.EnableAllMods.Name = "EnableAllMods";
            this.EnableAllMods.Size = new System.Drawing.Size(122, 27);
            this.EnableAllMods.TabIndex = 4;
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
            this.RandomizeOrder.TabIndex = 5;
            this.RandomizeOrder.Text = "Randomize Order";
            this.RandomizeOrder.UseVisualStyleBackColor = true;
            // 
            // DisableAllMods
            // 
            this.DisableAllMods.AutoSize = true;
            this.DisableAllMods.Location = new System.Drawing.Point(9, 171);
            this.DisableAllMods.Margin = new System.Windows.Forms.Padding(2);
            this.DisableAllMods.Name = "DisableAllMods";
            this.DisableAllMods.Size = new System.Drawing.Size(122, 27);
            this.DisableAllMods.TabIndex = 6;
            this.DisableAllMods.Text = "Disable All";
            this.DisableAllMods.UseVisualStyleBackColor = true;
            // 
            // IncludeAllMods
            // 
            this.IncludeAllMods.AutoSize = true;
            this.IncludeAllMods.Location = new System.Drawing.Point(9, 199);
            this.IncludeAllMods.Margin = new System.Windows.Forms.Padding(2);
            this.IncludeAllMods.Name = "IncludeAllMods";
            this.IncludeAllMods.Size = new System.Drawing.Size(122, 27);
            this.IncludeAllMods.TabIndex = 7;
            this.IncludeAllMods.Text = "Include All";
            this.IncludeAllMods.UseVisualStyleBackColor = true;
            IncludeAllMods.Padding += new Padding(0, 20, 0, 0);
            // 
            // AssetsTab
            // 
            this.AssetsTab.BackColor = System.Drawing.SystemColors.Control;
            this.AssetsTab.Controls.Add(this.dataGridAssets);
            this.AssetsTab.Controls.Add(this.AssetsActionPanel);
            this.AssetsTab.Controls.Add(this.tableLayoutPanelAssetFilters);
            this.AssetsTab.Location = new System.Drawing.Point(4, 26);
            this.AssetsTab.Name = "AssetsTab";
            this.AssetsTab.Padding = new System.Windows.Forms.Padding(3);
            this.AssetsTab.Size = new System.Drawing.Size(798, 585);
            this.AssetsTab.TabIndex = 1;
            this.AssetsTab.Text = "Assets";
            // 
            // tableLayoutPanelAssetFilters
            // 
            this.tableLayoutPanelAssetFilters.AutoSize = true;
            this.tableLayoutPanelAssetFilters.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanelAssetFilters.ColumnCount = 5;
            this.tableLayoutPanelAssetFilters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelAssetFilters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelAssetFilters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelAssetFilters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelAssetFilters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelAssetFilters.Controls.Add(this.ComboBoxAssetIncluded);
            this.tableLayoutPanelAssetFilters.Controls.Add(this.ComboBoxAssetWS);
            this.tableLayoutPanelAssetFilters.Controls.Add(this.ComboBoxAssetTags);
            this.tableLayoutPanelAssetFilters.Controls.Add(this.label2);
            this.tableLayoutPanelAssetFilters.Controls.Add(this.TextFilterAsset);
            this.tableLayoutPanelAssetFilters.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelAssetFilters.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.AddColumns;
            this.tableLayoutPanelAssetFilters.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelAssetFilters.Name = "tableLayoutPanelAssetFilters";
            this.tableLayoutPanelAssetFilters.RowCount = 1;
            this.tableLayoutPanelAssetFilters.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelAssetFilters.Size = new System.Drawing.Size(712, 31);
            this.tableLayoutPanelAssetFilters.TabIndex = 0;
            // 
            // ComboBoxAssetIncluded
            // 
            this.ComboBoxAssetIncluded.FormattingEnabled = true;
            this.ComboBoxAssetIncluded.Location = new System.Drawing.Point(3, 3);
            this.ComboBoxAssetIncluded.Name = "ComboBoxAssetIncluded";
            this.ComboBoxAssetIncluded.Size = new System.Drawing.Size(121, 25);
            this.ComboBoxAssetIncluded.TabIndex = 0;
            // 
            // ComboBoxAssetWS
            // 
            this.ComboBoxAssetWS.FormattingEnabled = true;
            this.ComboBoxAssetWS.Location = new System.Drawing.Point(130, 3);
            this.ComboBoxAssetWS.Name = "ComboBoxAssetWS";
            this.ComboBoxAssetWS.Size = new System.Drawing.Size(121, 25);
            this.ComboBoxAssetWS.TabIndex = 1;
            // 
            // ComboBoxAssetTags
            // 
            this.ComboBoxAssetTags.FormattingEnabled = true;
            this.ComboBoxAssetTags.Location = new System.Drawing.Point(257, 3);
            this.ComboBoxAssetTags.Name = "ComboBoxAssetTags";
            this.ComboBoxAssetTags.Size = new System.Drawing.Size(121, 25);
            this.ComboBoxAssetTags.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(384, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Filter:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TextFilterAsset
            // 
            this.TextFilterAsset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextFilterAsset.Location = new System.Drawing.Point(429, 3);
            this.TextFilterAsset.Name = "TextFilterAsset";
            this.TextFilterAsset.Size = new System.Drawing.Size(280, 25);
            this.TextFilterAsset.TabIndex = 4;
            // 
            // dataGridAssets
            // 
            this.dataGridAssets.AllowUserToAddRows = false;
            this.dataGridAssets.AllowUserToDeleteRows = false;
            this.dataGridAssets.AllowUserToOrderColumns = true;
            this.dataGridAssets.AllowUserToResizeRows = false;
            this.dataGridAssets.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dataGridAssets.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dataGridAssets.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dataGridAssets.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridAssets.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cIncluded,
            this.cID,
            this.cName,
            this.cAuthor,
            this.cDate,
            this.cTags});
            this.dataGridAssets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridAssets.Location = new System.Drawing.Point(3, 3);
            this.dataGridAssets.Name = "dataGridAssets";
            this.dataGridAssets.RowHeadersVisible = false;
            this.dataGridAssets.RowTemplate.Height = 27;
            this.dataGridAssets.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridAssets.Size = new System.Drawing.Size(712, 579);
            this.dataGridAssets.TabIndex = 0;
            // 
            // cIncluded
            // 
            this.cIncluded.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cIncluded.HeaderText = "Included";
            this.cIncluded.Name = "cIncluded";
            this.cIncluded.Width = 63;
            // 
            // cID
            // 
            this.cID.HeaderText = "ID";
            this.cID.Name = "cID";
            this.cID.ReadOnly = true;
            this.cID.Width = 45;
            // 
            // cName
            // 
            this.cName.HeaderText = "Name";
            this.cName.Name = "cName";
            this.cName.ReadOnly = true;
            this.cName.Width = 68;
            // 
            // cAuthor
            // 
            this.cAuthor.HeaderText = "Author";
            this.cAuthor.Name = "cAuthor";
            this.cAuthor.ReadOnly = true;
            this.cAuthor.Width = 72;
            // 
            // cDate
            // 
            this.cDate.HeaderText = "Date";
            this.cDate.Name = "cDate";
            this.cDate.ReadOnly = true;
            this.cDate.Width = 60;
            // 
            // cTags
            // 
            this.cTags.HeaderText = "Tags";
            this.cTags.Name = "cTags";
            this.cTags.ReadOnly = true;
            this.cTags.Width = 60;
            // 
            // AssetsActionPanel
            // 
            this.AssetsActionPanel.FlowDirection = FlowDirection.TopDown;
            this.AssetsActionPanel.AutoSize = true;
            this.AssetsActionPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.AssetsActionPanel.Controls.Add(this.IncludeAllAssets);
            this.AssetsActionPanel.Controls.Add(this.ExcludeAllAssets);
            this.AssetsActionPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.AssetsActionPanel.Name = "AssetsActionPanel";
            // 
            // ExcludeAllAssets
            // 
            this.ExcludeAllAssets.AutoSize = true;
            this.ExcludeAllAssets.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ExcludeAllAssets.Location = new System.Drawing.Point(1, 28);
            this.ExcludeAllAssets.Margin = new System.Windows.Forms.Padding(2);
            this.ExcludeAllAssets.Name = "ExcludeAllAssets";
            this.ExcludeAllAssets.Size = new System.Drawing.Size(80, 27);
            this.ExcludeAllAssets.TabIndex = 0;
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
            // LoadOrderWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(806, 646);
            this.Controls.Add(this.TabContainer);
            this.Controls.Add(this.FilePanel);
            this.MinimumSize = new System.Drawing.Size(524, 334);
            this.Name = "LoadOrderWindow";
            this.Text = "LoadOrder";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LoadOrderWindow_FormClosing);
            this.FilePanel.ResumeLayout(false);
            this.FilePanel.PerformLayout();
            this.TabContainer.ResumeLayout(false);
            this.ModsTab.ResumeLayout(false);
            this.ModsTab.PerformLayout();
            this.tableLayoutPanelModFilters.ResumeLayout(false);
            this.tableLayoutPanelModFilters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMods)).EndInit();
            this.ModActionPanel.ResumeLayout(false);
            this.ModActionPanel.PerformLayout();
            this.AssetsTab.ResumeLayout(false);
            this.AssetsTab.PerformLayout();
            this.tableLayoutPanelAssetFilters.ResumeLayout(false);
            this.tableLayoutPanelAssetFilters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridAssets)).EndInit();
            this.AssetsActionPanel.ResumeLayout(false);
            this.AssetsActionPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private FlowLayoutPanel FilePanel;
        private TabControl TabContainer;
        private TabPage ModsTab;
        private TabPage AssetsTab;
        private TableLayoutPanel tableLayoutPanelModFilters;
        private TableLayoutPanel tableLayoutPanelAssetFilters;
        private CheckBox AutoSave;
        private Button ReloadAll;
        private Button Save;
        private Button LoadProfile;
        private Button SaveProfile;
        private FlowLayoutPanel ModActionPanel;
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
        private FlowLayoutPanel AssetsActionPanel;
        private Button ExcludeAllAssets;
        private Button IncludeAllAssets;
        private ComboBox ComboBoxIncluded;
        private ComboBox ComboBoxEnabled;
        private TextBox TextFilterMods;
        private ComboBox ComboBoxWS;
        private Label label1;
        private ComboBox ComboBoxAssetIncluded;
        private ComboBox ComboBoxAssetWS;
        private ComboBox ComboBoxAssetTags;
        private Label label2;
        private TextBox TextFilterAsset;
        private DataGridView dataGridAssets;
        private DataGridViewCheckBoxColumn cIncluded;
        private DataGridViewTextBoxColumn cID;
        private DataGridViewTextBoxColumn cName;
        private DataGridViewTextBoxColumn cAuthor;
        private DataGridViewTextBoxColumn cDate;
        private DataGridViewTextBoxColumn cTags;
        private Button ResetOrder;
    }
}