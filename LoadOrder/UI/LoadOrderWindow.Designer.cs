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
            this.DLCTab = new System.Windows.Forms.TabPage();
            this.DLCControl = new LoadOrderTool.UI.DLCControl();
            this.LSMTab = new System.Windows.Forms.TabPage();
            //this.LSMControl = new LoadOrderTool.UI.LSMControl();
            this.LaunchTab = new System.Windows.Forms.TabPage();
            this.launchControl = new LoadOrderTool.UI.LaunchControl();
            this.menuStrip = new LoadOrderTool.UI.LoadOrderWindowMenuStrip();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.TabContainer.SuspendLayout();
            this.ModsTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridMods)).BeginInit();
            this.ModActionPanel.SuspendLayout();
            this.tableLayoutPanelModFilters.SuspendLayout();
            this.AssetsTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridAssets)).BeginInit();
            this.AssetsActionPanel.SuspendLayout();
            this.tableLayoutPanelAssetFilters.SuspendLayout();
            DLCTab.SuspendLayout();
            LSMTab.SuspendLayout();
            this.LaunchTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // TabContainer
            // 
            this.TabContainer.Controls.Add(this.ModsTab);
            this.TabContainer.Controls.Add(this.AssetsTab);
            this.TabContainer.Controls.Add(this.DLCTab);
            this.TabContainer.Controls.Add(this.LSMTab);
            this.TabContainer.Controls.Add(this.LaunchTab);
            this.TabContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabContainer.Location = new System.Drawing.Point(0, 24);
            this.TabContainer.Name = "TabContainer";
            this.TabContainer.SelectedIndex = 0;
            this.TabContainer.Size = new System.Drawing.Size(956, 527);
            this.TabContainer.TabIndex = 0;
            // 
            // ModsTab
            // 
            this.ModsTab.BackColor = System.Drawing.SystemColors.Control;
            this.ModsTab.Controls.Add(this.statusStrip);
            this.ModsTab.Controls.Add(this.dataGridMods);
            this.ModsTab.Controls.Add(this.ModActionPanel);
            this.ModsTab.Controls.Add(this.tableLayoutPanelModFilters);
            this.ModsTab.Location = new System.Drawing.Point(4, 24);
            this.ModsTab.Name = "ModsTab";
            this.ModsTab.Padding = new System.Windows.Forms.Padding(3);
            this.ModsTab.Size = new System.Drawing.Size(948, 499);
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
            this.dataGridMods.Location = new System.Drawing.Point(3, 32);
            this.dataGridMods.MultiSelect = false;
            this.dataGridMods.Name = "dataGridMods";
            this.dataGridMods.RowHeadersVisible = false;
            this.dataGridMods.Size = new System.Drawing.Size(863, 464);
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
            this.ModActionPanel.Location = new System.Drawing.Point(866, 32);
            this.ModActionPanel.Name = "ModActionPanel";
            this.ModActionPanel.Size = new System.Drawing.Size(79, 464);
            this.ModActionPanel.TabIndex = 2;
            // 
            // IncludeAllMods
            // 
            this.IncludeAllMods.AutoSize = true;
            this.IncludeAllMods.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.IncludeAllMods.Location = new System.Drawing.Point(2, 2);
            this.IncludeAllMods.Margin = new System.Windows.Forms.Padding(2);
            this.IncludeAllMods.Name = "IncludeAllMods";
            this.IncludeAllMods.Size = new System.Drawing.Size(73, 25);
            this.IncludeAllMods.TabIndex = 7;
            this.IncludeAllMods.Text = "Include All";
            this.IncludeAllMods.UseVisualStyleBackColor = true;
            // 
            // ExcludeAllMods
            // 
            this.ExcludeAllMods.AutoSize = true;
            this.ExcludeAllMods.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ExcludeAllMods.Location = new System.Drawing.Point(2, 31);
            this.ExcludeAllMods.Margin = new System.Windows.Forms.Padding(2);
            this.ExcludeAllMods.Name = "ExcludeAllMods";
            this.ExcludeAllMods.Size = new System.Drawing.Size(75, 25);
            this.ExcludeAllMods.TabIndex = 3;
            this.ExcludeAllMods.Text = "Exclude All";
            this.ExcludeAllMods.UseVisualStyleBackColor = true;
            // 
            // EnableAllMods
            // 
            this.EnableAllMods.AutoSize = true;
            this.EnableAllMods.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.EnableAllMods.Location = new System.Drawing.Point(2, 60);
            this.EnableAllMods.Margin = new System.Windows.Forms.Padding(2);
            this.EnableAllMods.Name = "EnableAllMods";
            this.EnableAllMods.Size = new System.Drawing.Size(69, 25);
            this.EnableAllMods.TabIndex = 4;
            this.EnableAllMods.Text = "Enable All";
            this.EnableAllMods.UseVisualStyleBackColor = true;
            // 
            // DisableAllMods
            // 
            this.DisableAllMods.AutoSize = true;
            this.DisableAllMods.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.DisableAllMods.Location = new System.Drawing.Point(2, 89);
            this.DisableAllMods.Margin = new System.Windows.Forms.Padding(2);
            this.DisableAllMods.Name = "DisableAllMods";
            this.DisableAllMods.Size = new System.Drawing.Size(72, 25);
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
            this.tableLayoutPanelModFilters.Size = new System.Drawing.Size(942, 29);
            this.tableLayoutPanelModFilters.TabIndex = 0;
            // 
            // ComboBoxIncluded
            // 
            this.ComboBoxIncluded.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxIncluded.FormattingEnabled = true;
            this.ComboBoxIncluded.Location = new System.Drawing.Point(3, 3);
            this.ComboBoxIncluded.Name = "ComboBoxIncluded";
            this.ComboBoxIncluded.Size = new System.Drawing.Size(150, 23);
            this.ComboBoxIncluded.TabIndex = 0;
            // 
            // ComboBoxEnabled
            // 
            this.ComboBoxEnabled.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxEnabled.FormattingEnabled = true;
            this.ComboBoxEnabled.Location = new System.Drawing.Point(159, 3);
            this.ComboBoxEnabled.Name = "ComboBoxEnabled";
            this.ComboBoxEnabled.Size = new System.Drawing.Size(150, 23);
            this.ComboBoxEnabled.TabIndex = 1;
            // 
            // ComboBoxWS
            // 
            this.ComboBoxWS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxWS.FormattingEnabled = true;
            this.ComboBoxWS.Location = new System.Drawing.Point(315, 3);
            this.ComboBoxWS.Name = "ComboBoxWS";
            this.ComboBoxWS.Size = new System.Drawing.Size(150, 23);
            this.ComboBoxWS.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(471, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Filter:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TextFilterMods
            // 
            this.TextFilterMods.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextFilterMods.Location = new System.Drawing.Point(513, 3);
            this.TextFilterMods.Name = "TextFilterMods";
            this.TextFilterMods.Size = new System.Drawing.Size(426, 23);
            this.TextFilterMods.TabIndex = 4;
            // 
            // AssetsTab
            // 
            this.AssetsTab.BackColor = System.Drawing.SystemColors.Control;
            this.AssetsTab.Controls.Add(this.dataGridAssets);
            this.AssetsTab.Controls.Add(this.AssetsActionPanel);
            this.AssetsTab.Controls.Add(this.tableLayoutPanelAssetFilters);
            this.AssetsTab.Location = new System.Drawing.Point(4, 24);
            this.AssetsTab.Name = "AssetsTab";
            this.AssetsTab.Padding = new System.Windows.Forms.Padding(3);
            this.AssetsTab.Size = new System.Drawing.Size(948, 499);
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
            this.dataGridAssets.Location = new System.Drawing.Point(3, 32);
            this.dataGridAssets.MultiSelect = false;
            this.dataGridAssets.Name = "dataGridAssets";
            this.dataGridAssets.RowHeadersVisible = false;
            this.dataGridAssets.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridAssets.Size = new System.Drawing.Size(863, 464);
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
            this.AssetsActionPanel.Location = new System.Drawing.Point(866, 32);
            this.AssetsActionPanel.Name = "AssetsActionPanel";
            this.AssetsActionPanel.Size = new System.Drawing.Size(79, 464);
            this.AssetsActionPanel.TabIndex = 1;
            // 
            // IncludeAllAssets
            // 
            this.IncludeAllAssets.AutoSize = true;
            this.IncludeAllAssets.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.IncludeAllAssets.Location = new System.Drawing.Point(2, 2);
            this.IncludeAllAssets.Margin = new System.Windows.Forms.Padding(2);
            this.IncludeAllAssets.Name = "IncludeAllAssets";
            this.IncludeAllAssets.Size = new System.Drawing.Size(73, 25);
            this.IncludeAllAssets.TabIndex = 1;
            this.IncludeAllAssets.Text = "Include All";
            this.IncludeAllAssets.UseVisualStyleBackColor = true;
            // 
            // ExcludeAllAssets
            // 
            this.ExcludeAllAssets.AutoSize = true;
            this.ExcludeAllAssets.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ExcludeAllAssets.Location = new System.Drawing.Point(2, 31);
            this.ExcludeAllAssets.Margin = new System.Windows.Forms.Padding(2);
            this.ExcludeAllAssets.Name = "ExcludeAllAssets";
            this.ExcludeAllAssets.Size = new System.Drawing.Size(75, 25);
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
            this.tableLayoutPanelAssetFilters.Size = new System.Drawing.Size(942, 29);
            this.tableLayoutPanelAssetFilters.TabIndex = 0;
            // 
            // ComboBoxAssetIncluded
            // 
            this.ComboBoxAssetIncluded.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxAssetIncluded.FormattingEnabled = true;
            this.ComboBoxAssetIncluded.Location = new System.Drawing.Point(3, 3);
            this.ComboBoxAssetIncluded.Name = "ComboBoxAssetIncluded";
            this.ComboBoxAssetIncluded.Size = new System.Drawing.Size(150, 23);
            this.ComboBoxAssetIncluded.TabIndex = 0;
            // 
            // ComboBoxAssetWS
            // 
            this.ComboBoxAssetWS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxAssetWS.FormattingEnabled = true;
            this.ComboBoxAssetWS.Location = new System.Drawing.Point(159, 3);
            this.ComboBoxAssetWS.Name = "ComboBoxAssetWS";
            this.ComboBoxAssetWS.Size = new System.Drawing.Size(150, 23);
            this.ComboBoxAssetWS.TabIndex = 1;
            // 
            // ComboBoxAssetTags
            // 
            this.ComboBoxAssetTags.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.ComboBoxAssetTags.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.ComboBoxAssetTags.FormattingEnabled = true;
            this.ComboBoxAssetTags.Location = new System.Drawing.Point(315, 3);
            this.ComboBoxAssetTags.Name = "ComboBoxAssetTags";
            this.ComboBoxAssetTags.Size = new System.Drawing.Size(150, 23);
            this.ComboBoxAssetTags.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(471, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Filter:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TextFilterAsset
            // 
            this.TextFilterAsset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextFilterAsset.Location = new System.Drawing.Point(513, 3);
            this.TextFilterAsset.Name = "TextFilterAsset";
            this.TextFilterAsset.Size = new System.Drawing.Size(426, 23);
            this.TextFilterAsset.TabIndex = 4;

            // 
            // DLCTab
            // 
            this.DLCTab.BackColor = System.Drawing.Color.Transparent;
            this.DLCTab.Controls.Add(this.DLCControl);
            this.DLCTab.Location = new System.Drawing.Point(4, 24);
            this.DLCTab.Name = "DLCTab";
            this.DLCTab.Padding = new System.Windows.Forms.Padding(3);
            this.DLCTab.Size = new System.Drawing.Size(948, 499);
            this.DLCTab.TabIndex = 2;
            this.DLCTab.Text = "DLC";
            // 
            // DLCControl
            // 
            this.DLCControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DLCControl.Name = "DLCControl";
            //this.DLCControl.AutoSize = true;

            // 
            // LSMTab
            // 
            this.LSMTab.BackColor = System.Drawing.Color.Transparent;
            this.LSMTab.Location = new System.Drawing.Point(4, 24);
            this.LSMTab.Name = "LSMTab";
            this.LSMTab.Padding = new System.Windows.Forms.Padding(3);
            this.LSMTab.Size = new System.Drawing.Size(948, 499);
            this.LSMTab.TabIndex = 2;
            this.LSMTab.Text = "LSM";
            //this.LSMTab.Controls.Add(this.LSMControl);
            //// 
            //// LSMControl
            //// 
            //this.LSMControl.Dock = System.Windows.Forms.DockStyle.Fill;
            //this.LSMControl.Location = new System.Drawing.Point(3, 3);
            //this.LSMControl.Name = "LSMControl";
            //this.LSMControl.Size = new System.Drawing.Size(942, 493);
            //this.LSMControl.TabIndex = 0;

            // 
            // LaunchTab
            // 
            this.LaunchTab.BackColor = System.Drawing.Color.Transparent;
            this.LaunchTab.Controls.Add(this.launchControl);
            this.LaunchTab.Location = new System.Drawing.Point(4, 24);
            this.LaunchTab.Name = "LaunchTab";
            this.LaunchTab.Padding = new System.Windows.Forms.Padding(3);
            this.LaunchTab.Size = new System.Drawing.Size(948, 499);
            this.LaunchTab.TabIndex = 2;
            this.LaunchTab.Text = "Launch";
            // 
            // launchControl
            // 
            this.launchControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.launchControl.Location = new System.Drawing.Point(3, 3);
            this.launchControl.Name = "launchControl";
            this.launchControl.Size = new System.Drawing.Size(942, 493);
            this.launchControl.TabIndex = 0;
            // 
            // menuStrip
            // 
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(956, 24);
            this.menuStrip.TabIndex = 2;
            this.menuStrip.Text = "menuStrip";
            // 
            // statusStrip
            // 
            this.statusStrip.Location = new System.Drawing.Point(3, 474);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(863, 22);
            this.statusStrip.TabIndex = 3;
            this.statusStrip.Text = "statusStrip1";
            // 
            // LoadOrderWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(956, 551);
            this.Controls.Add(this.TabContainer);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.MinimumSize = new System.Drawing.Size(524, 299);
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

            this.DLCTab.ResumeLayout(false);
            this.DLCTab.PerformLayout();

            this.LSMTab.ResumeLayout();
            this.LSMTab.PerformLayout();

            this.LaunchTab.ResumeLayout(false);
            this.LaunchTab.PerformLayout();

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
        public LoadOrderWindowMenuStrip menuStrip;

        private TabPage LaunchTab;
        private TabPage DLCTab;
        private TabPage LSMTab;

        private UI.LaunchControl launchControl;
        private UI.DLCControl DLCControl;
        //private UI.LSMControl LSMControl;

        private StatusStrip statusStrip;
    }
}