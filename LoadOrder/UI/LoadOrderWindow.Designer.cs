namespace LoadOrderTool.UI
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.TabContainer = new System.Windows.Forms.TabControl();
            this.ModsTab = new System.Windows.Forms.TabPage();
            this.dataGridMods = new LoadOrderTool.UI.ModDataGrid();
            this.ModActionPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.IncludeAllMods = new System.Windows.Forms.Button();
            this.ExcludeAllMods = new System.Windows.Forms.Button();
            this.EnableAllMods = new System.Windows.Forms.Button();
            this.DisableAllMods = new System.Windows.Forms.Button();
            this.tableLayoutPanelModFilters = new System.Windows.Forms.TableLayoutPanel();
            this.ComboBoxIncluded = new System.Windows.Forms.ComboBox();
            this.ComboBoxEnabled = new System.Windows.Forms.ComboBox();
            this.ComboBoxWS = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TextFilterMods = new System.Windows.Forms.TextBox();
            this.AssetsTab = new System.Windows.Forms.TabPage();
            this.dataGridAssets = new LoadOrderTool.UI.AssetDataGrid();
            this.AssetsActionPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.IncludeAllAssets = new System.Windows.Forms.Button();
            this.ExcludeAllAssets = new System.Windows.Forms.Button();
            this.tableLayoutPanelAssetFilters = new System.Windows.Forms.TableLayoutPanel();
            this.ComboBoxAssetIncluded = new System.Windows.Forms.ComboBox();
            this.ComboBoxAssetWS = new System.Windows.Forms.ComboBox();
            this.ComboBoxAssetTags = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TextFilterAsset = new System.Windows.Forms.TextBox();
            this.LaunchTab = new System.Windows.Forms.TabPage();
            this.launchControl = new LoadOrderTool.UI.LaunchControl();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tsmiFile = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiResetSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiReload = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiSave = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAutoSave = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiExport = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiImport = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOrder = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiResetOrder = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiHarmonyOrder = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiReverseOrder = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRandomOrder = new System.Windows.Forms.ToolStripMenuItem();
            this.TabContainer.SuspendLayout();
            this.ModsTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridMods)).BeginInit();
            this.ModActionPanel.SuspendLayout();
            this.tableLayoutPanelModFilters.SuspendLayout();
            this.AssetsTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridAssets)).BeginInit();
            this.AssetsActionPanel.SuspendLayout();
            this.tableLayoutPanelAssetFilters.SuspendLayout();
            this.LaunchTab.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TabContainer
            // 
            this.TabContainer.Controls.Add(this.ModsTab);
            this.TabContainer.Controls.Add(this.AssetsTab);
            this.TabContainer.Controls.Add(this.LaunchTab);
            this.TabContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabContainer.Location = new System.Drawing.Point(0, 25);
            this.TabContainer.Name = "TabContainer";
            this.TabContainer.SelectedIndex = 0;
            this.TabContainer.Size = new System.Drawing.Size(956, 600);
            this.TabContainer.TabIndex = 0;
            // 
            // ModsTab
            // 
            this.ModsTab.BackColor = System.Drawing.SystemColors.Control;
            this.ModsTab.Controls.Add(this.dataGridMods);
            this.ModsTab.Controls.Add(this.ModActionPanel);
            this.ModsTab.Controls.Add(this.tableLayoutPanelModFilters);
            this.ModsTab.Location = new System.Drawing.Point(4, 26);
            this.ModsTab.Name = "ModsTab";
            this.ModsTab.Padding = new System.Windows.Forms.Padding(3);
            this.ModsTab.Size = new System.Drawing.Size(948, 570);
            this.ModsTab.TabIndex = 0;
            this.ModsTab.Text = "Mods";
            // 
            // dataGridMods
            // 
            this.dataGridMods.AllowUserToAddRows = false;
            this.dataGridMods.AllowUserToDeleteRows = false;
            this.dataGridMods.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Beige;
            this.dataGridMods.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridMods.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dataGridMods.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dataGridMods.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridMods.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridMods.Location = new System.Drawing.Point(3, 34);
            this.dataGridMods.MultiSelect = false;
            this.dataGridMods.Name = "dataGridMods";
            this.dataGridMods.RowHeadersVisible = false;
            this.dataGridMods.Size = new System.Drawing.Size(858, 533);
            this.dataGridMods.TabIndex = 1;
            // 
            // ModActionPanel
            // 
            this.ModActionPanel.AutoSize = true;
            this.ModActionPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ModActionPanel.Controls.Add(this.IncludeAllMods);
            this.ModActionPanel.Controls.Add(this.ExcludeAllMods);
            this.ModActionPanel.Controls.Add(this.EnableAllMods);
            this.ModActionPanel.Controls.Add(this.DisableAllMods);
            this.ModActionPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.ModActionPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.ModActionPanel.Location = new System.Drawing.Point(861, 34);
            this.ModActionPanel.Name = "ModActionPanel";
            this.ModActionPanel.Size = new System.Drawing.Size(84, 533);
            this.ModActionPanel.TabIndex = 2;
            // 
            // IncludeAllMods
            // 
            this.IncludeAllMods.AutoSize = true;
            this.IncludeAllMods.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.IncludeAllMods.Location = new System.Drawing.Point(2, 2);
            this.IncludeAllMods.Margin = new System.Windows.Forms.Padding(2);
            this.IncludeAllMods.Name = "IncludeAllMods";
            this.IncludeAllMods.Size = new System.Drawing.Size(77, 27);
            this.IncludeAllMods.TabIndex = 7;
            this.IncludeAllMods.Text = "Include All";
            this.IncludeAllMods.UseVisualStyleBackColor = true;
            // 
            // ExcludeAllMods
            // 
            this.ExcludeAllMods.AutoSize = true;
            this.ExcludeAllMods.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ExcludeAllMods.Location = new System.Drawing.Point(2, 33);
            this.ExcludeAllMods.Margin = new System.Windows.Forms.Padding(2);
            this.ExcludeAllMods.Name = "ExcludeAllMods";
            this.ExcludeAllMods.Size = new System.Drawing.Size(80, 27);
            this.ExcludeAllMods.TabIndex = 3;
            this.ExcludeAllMods.Text = "Exclude All";
            this.ExcludeAllMods.UseVisualStyleBackColor = true;
            // 
            // EnableAllMods
            // 
            this.EnableAllMods.AutoSize = true;
            this.EnableAllMods.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.EnableAllMods.Location = new System.Drawing.Point(2, 64);
            this.EnableAllMods.Margin = new System.Windows.Forms.Padding(2);
            this.EnableAllMods.Name = "EnableAllMods";
            this.EnableAllMods.Size = new System.Drawing.Size(75, 27);
            this.EnableAllMods.TabIndex = 4;
            this.EnableAllMods.Text = "Enable All";
            this.EnableAllMods.UseVisualStyleBackColor = true;
            // 
            // DisableAllMods
            // 
            this.DisableAllMods.AutoSize = true;
            this.DisableAllMods.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.DisableAllMods.Location = new System.Drawing.Point(2, 95);
            this.DisableAllMods.Margin = new System.Windows.Forms.Padding(2);
            this.DisableAllMods.Name = "DisableAllMods";
            this.DisableAllMods.Size = new System.Drawing.Size(79, 27);
            this.DisableAllMods.TabIndex = 6;
            this.DisableAllMods.Text = "Disable All";
            this.DisableAllMods.UseVisualStyleBackColor = true;
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
            this.tableLayoutPanelModFilters.Size = new System.Drawing.Size(942, 31);
            this.tableLayoutPanelModFilters.TabIndex = 0;
            // 
            // ComboBoxIncluded
            // 
            this.ComboBoxIncluded.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxIncluded.FormattingEnabled = true;
            this.ComboBoxIncluded.Location = new System.Drawing.Point(3, 3);
            this.ComboBoxIncluded.Name = "ComboBoxIncluded";
            this.ComboBoxIncluded.Size = new System.Drawing.Size(150, 25);
            this.ComboBoxIncluded.TabIndex = 0;
            // 
            // ComboBoxEnabled
            // 
            this.ComboBoxEnabled.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxEnabled.FormattingEnabled = true;
            this.ComboBoxEnabled.Location = new System.Drawing.Point(159, 3);
            this.ComboBoxEnabled.Name = "ComboBoxEnabled";
            this.ComboBoxEnabled.Size = new System.Drawing.Size(150, 25);
            this.ComboBoxEnabled.TabIndex = 1;
            // 
            // ComboBoxWS
            // 
            this.ComboBoxWS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxWS.FormattingEnabled = true;
            this.ComboBoxWS.Location = new System.Drawing.Point(315, 3);
            this.ComboBoxWS.Name = "ComboBoxWS";
            this.ComboBoxWS.Size = new System.Drawing.Size(150, 25);
            this.ComboBoxWS.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(471, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Filter:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TextFilterMods
            // 
            this.TextFilterMods.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextFilterMods.Location = new System.Drawing.Point(516, 3);
            this.TextFilterMods.Name = "TextFilterMods";
            this.TextFilterMods.Size = new System.Drawing.Size(423, 25);
            this.TextFilterMods.TabIndex = 4;
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
            this.AssetsTab.Size = new System.Drawing.Size(948, 570);
            this.AssetsTab.TabIndex = 1;
            this.AssetsTab.Text = "Assets";
            // 
            // dataGridAssets
            // 
            this.dataGridAssets.AllowUserToAddRows = false;
            this.dataGridAssets.AllowUserToDeleteRows = false;
            this.dataGridAssets.AllowUserToOrderColumns = true;
            this.dataGridAssets.AllowUserToResizeRows = false;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Beige;
            this.dataGridAssets.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridAssets.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dataGridAssets.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dataGridAssets.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridAssets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridAssets.Location = new System.Drawing.Point(3, 34);
            this.dataGridAssets.MultiSelect = false;
            this.dataGridAssets.Name = "dataGridAssets";
            this.dataGridAssets.RowHeadersVisible = false;
            this.dataGridAssets.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridAssets.Size = new System.Drawing.Size(858, 533);
            this.dataGridAssets.TabIndex = 0;
            this.dataGridAssets.VirtualMode = true;
            // 
            // AssetsActionPanel
            // 
            this.AssetsActionPanel.AutoSize = true;
            this.AssetsActionPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.AssetsActionPanel.Controls.Add(this.IncludeAllAssets);
            this.AssetsActionPanel.Controls.Add(this.ExcludeAllAssets);
            this.AssetsActionPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.AssetsActionPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.AssetsActionPanel.Location = new System.Drawing.Point(861, 34);
            this.AssetsActionPanel.Name = "AssetsActionPanel";
            this.AssetsActionPanel.Size = new System.Drawing.Size(84, 533);
            this.AssetsActionPanel.TabIndex = 1;
            // 
            // IncludeAllAssets
            // 
            this.IncludeAllAssets.AutoSize = true;
            this.IncludeAllAssets.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.IncludeAllAssets.Location = new System.Drawing.Point(2, 2);
            this.IncludeAllAssets.Margin = new System.Windows.Forms.Padding(2);
            this.IncludeAllAssets.Name = "IncludeAllAssets";
            this.IncludeAllAssets.Size = new System.Drawing.Size(77, 27);
            this.IncludeAllAssets.TabIndex = 1;
            this.IncludeAllAssets.Text = "Include All";
            this.IncludeAllAssets.UseVisualStyleBackColor = true;
            // 
            // ExcludeAllAssets
            // 
            this.ExcludeAllAssets.AutoSize = true;
            this.ExcludeAllAssets.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ExcludeAllAssets.Location = new System.Drawing.Point(2, 33);
            this.ExcludeAllAssets.Margin = new System.Windows.Forms.Padding(2);
            this.ExcludeAllAssets.Name = "ExcludeAllAssets";
            this.ExcludeAllAssets.Size = new System.Drawing.Size(80, 27);
            this.ExcludeAllAssets.TabIndex = 0;
            this.ExcludeAllAssets.Text = "Exclude All";
            this.ExcludeAllAssets.UseVisualStyleBackColor = true;
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
            this.tableLayoutPanelAssetFilters.Size = new System.Drawing.Size(942, 31);
            this.tableLayoutPanelAssetFilters.TabIndex = 0;
            // 
            // ComboBoxAssetIncluded
            // 
            this.ComboBoxAssetIncluded.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxAssetIncluded.FormattingEnabled = true;
            this.ComboBoxAssetIncluded.Location = new System.Drawing.Point(3, 3);
            this.ComboBoxAssetIncluded.Name = "ComboBoxAssetIncluded";
            this.ComboBoxAssetIncluded.Size = new System.Drawing.Size(150, 25);
            this.ComboBoxAssetIncluded.TabIndex = 0;
            // 
            // ComboBoxAssetWS
            // 
            this.ComboBoxAssetWS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxAssetWS.FormattingEnabled = true;
            this.ComboBoxAssetWS.Location = new System.Drawing.Point(159, 3);
            this.ComboBoxAssetWS.Name = "ComboBoxAssetWS";
            this.ComboBoxAssetWS.Size = new System.Drawing.Size(150, 25);
            this.ComboBoxAssetWS.TabIndex = 1;
            // 
            // ComboBoxAssetTags
            // 
            this.ComboBoxAssetTags.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxAssetTags.FormattingEnabled = true;
            this.ComboBoxAssetTags.Location = new System.Drawing.Point(315, 3);
            this.ComboBoxAssetTags.Name = "ComboBoxAssetTags";
            this.ComboBoxAssetTags.Size = new System.Drawing.Size(150, 25);
            this.ComboBoxAssetTags.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(471, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Filter:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TextFilterAsset
            // 
            this.TextFilterAsset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextFilterAsset.Location = new System.Drawing.Point(516, 3);
            this.TextFilterAsset.Name = "TextFilterAsset";
            this.TextFilterAsset.Size = new System.Drawing.Size(423, 25);
            this.TextFilterAsset.TabIndex = 4;
            // 
            // LaunchTab
            // 
            this.LaunchTab.BackColor = System.Drawing.Color.Transparent;
            this.LaunchTab.Controls.Add(this.launchControl);
            this.LaunchTab.Location = new System.Drawing.Point(4, 26);
            this.LaunchTab.Name = "LaunchTab";
            this.LaunchTab.Padding = new System.Windows.Forms.Padding(3);
            this.LaunchTab.Size = new System.Drawing.Size(948, 570);
            this.LaunchTab.TabIndex = 2;
            this.LaunchTab.Text = "Launch";
            // 
            // launchControl
            // 
            this.launchControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.launchControl.Location = new System.Drawing.Point(3, 3);
            this.launchControl.Name = "launchControl";
            this.launchControl.Size = new System.Drawing.Size(942, 564);
            this.launchControl.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiFile,
            this.tsmiOrder});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(956, 25);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // tsmiFile
            // 
            this.tsmiFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiResetSettings,
            this.tsmiReload,
            this.toolStripSeparator1,
            this.tsmiSave,
            this.tsmiAutoSave,
            this.toolStripSeparator2,
            this.tsmiExport,
            this.tsmiImport});
            this.tsmiFile.Name = "tsmiFile";
            this.tsmiFile.Size = new System.Drawing.Size(39, 21);
            this.tsmiFile.Text = "&File";
            // 
            // tsmiResetSettings
            // 
            this.tsmiResetSettings.Name = "tsmiResetSettings";
            this.tsmiResetSettings.Size = new System.Drawing.Size(158, 22);
            this.tsmiResetSettings.Text = "Reset &Settings";
            // 
            // tsmiReload
            // 
            this.tsmiReload.Name = "tsmiReload";
            this.tsmiReload.Size = new System.Drawing.Size(158, 22);
            this.tsmiReload.Text = "&Reload Items";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(155, 6);
            // 
            // tsmiSave
            // 
            this.tsmiSave.Name = "tsmiSave";
            this.tsmiSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.tsmiSave.Size = new System.Drawing.Size(158, 22);
            this.tsmiSave.Text = "&Save";
            // 
            // tsmiAutoSave
            // 
            this.tsmiAutoSave.CheckOnClick = true;
            this.tsmiAutoSave.Name = "tsmiAutoSave";
            this.tsmiAutoSave.Size = new System.Drawing.Size(158, 22);
            this.tsmiAutoSave.Text = "&Auto-save";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(155, 6);
            // 
            // tsmiExport
            // 
            this.tsmiExport.Name = "tsmiExport";
            this.tsmiExport.Size = new System.Drawing.Size(158, 22);
            this.tsmiExport.Text = "&Export ...";
            // 
            // tsmiImport
            // 
            this.tsmiImport.Name = "tsmiImport";
            this.tsmiImport.Size = new System.Drawing.Size(158, 22);
            this.tsmiImport.Text = "&Import ...";
            // 
            // tsmiOrder
            // 
            this.tsmiOrder.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiResetOrder,
            this.tsmiHarmonyOrder,
            this.tsmiReverseOrder,
            this.tsmiRandomOrder});
            this.tsmiOrder.Name = "tsmiOrder";
            this.tsmiOrder.Size = new System.Drawing.Size(55, 21);
            this.tsmiOrder.Text = "&Order";
            // 
            // tsmiResetOrder
            // 
            this.tsmiResetOrder.Name = "tsmiResetOrder";
            this.tsmiResetOrder.Size = new System.Drawing.Size(143, 22);
            this.tsmiResetOrder.Text = "&ResetOrder";
            // 
            // tsmiHarmonyOrder
            // 
            this.tsmiHarmonyOrder.Name = "tsmiHarmonyOrder";
            this.tsmiHarmonyOrder.Size = new System.Drawing.Size(143, 22);
            this.tsmiHarmonyOrder.Text = "&Harmony";
            this.tsmiHarmonyOrder.ToolTipText = "sort by harmony";
            // 
            // tsmiReverseOrder
            // 
            this.tsmiReverseOrder.Name = "tsmiReverseOrder";
            this.tsmiReverseOrder.Size = new System.Drawing.Size(143, 22);
            this.tsmiReverseOrder.Text = "Re&verse";
            this.tsmiReverseOrder.ToolTipText = "revese order";
            // 
            // tsmiRandomOrder
            // 
            this.tsmiRandomOrder.Name = "tsmiRandomOrder";
            this.tsmiRandomOrder.Size = new System.Drawing.Size(143, 22);
            this.tsmiRandomOrder.Text = "Ra&ndom";
            this.tsmiRandomOrder.ToolTipText = "Random order";
            // 
            // LoadOrderWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(956, 625);
            this.Controls.Add(this.TabContainer);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(524, 334);
            this.Name = "LoadOrderWindow";
            this.Text = "LoadOrder";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LoadOrderWindow_FormClosing);
            this.TabContainer.ResumeLayout(false);
            this.ModsTab.ResumeLayout(false);
            this.ModsTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridMods)).EndInit();
            this.ModActionPanel.ResumeLayout(false);
            this.ModActionPanel.PerformLayout();
            this.tableLayoutPanelModFilters.ResumeLayout(false);
            this.tableLayoutPanelModFilters.PerformLayout();
            this.AssetsTab.ResumeLayout(false);
            this.AssetsTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridAssets)).EndInit();
            this.AssetsActionPanel.ResumeLayout(false);
            this.AssetsActionPanel.PerformLayout();
            this.tableLayoutPanelAssetFilters.ResumeLayout(false);
            this.tableLayoutPanelAssetFilters.PerformLayout();
            this.LaunchTab.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private TabControl TabContainer;
        private TabPage ModsTab;
        private TabPage AssetsTab;
        private TableLayoutPanel tableLayoutPanelModFilters;
        private TableLayoutPanel tableLayoutPanelAssetFilters;
        private FlowLayoutPanel ModActionPanel;
        private Button ExcludeAllMods;
        private Button EnableAllMods;
        private Button DisableAllMods;
        private Button IncludeAllMods;
        public UI.ModDataGrid dataGridMods;
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
        private UI.AssetDataGrid dataGridAssets;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem tsmiFile;
        private ToolStripMenuItem tsmiReload;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem tsmiSave;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem tsmiExport;
        private ToolStripMenuItem tsmiImport;
        public ToolStripMenuItem tsmiAutoSave;
        private TabPage LaunchTab;
        private UI.LaunchControl launchControl;
        private ToolStripMenuItem tsmiResetSettings;
        private ToolStripMenuItem tsmiOrder;
        private ToolStripMenuItem tsmiResetOrder;
        private ToolStripMenuItem tsmiHarmonyOrder;
        private ToolStripMenuItem tsmiReverseOrder;
        private ToolStripMenuItem tsmiRandomOrder;
    }
}