namespace LoadOrderTool.UI {
    partial class LaunchControl {
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
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.flowLayoutPanelTopLevel = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanelExclusions = new System.Windows.Forms.FlowLayoutPanel();
            this.checkBoxNoAssets = new System.Windows.Forms.CheckBox();
            this.checkBoxNoMods = new System.Windows.Forms.CheckBox();
            this.checkBoxNoWorkshop = new System.Windows.Forms.CheckBox();
            this.checkBoxLHT = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanelLunchMode = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxSavePath = new System.Windows.Forms.TextBox();
            this.textBoxMapPath = new System.Windows.Forms.TextBox();
            this.buttonSavePath = new System.Windows.Forms.Button();
            this.buttonMapPath = new System.Windows.Forms.Button();
            this.buttonLoadSave = new System.Windows.Forms.Button();
            this.buttonNewGame = new System.Windows.Forms.Button();
            this.buttonLoadAsset = new System.Windows.Forms.Button();
            this.checkBoxPoke = new System.Windows.Forms.CheckBox();
            this.checkBoxPhased = new System.Windows.Forms.CheckBox();
            this.labelCommand = new System.Windows.Forms.Label();
            this.flowLayoutPanelTopLevel.SuspendLayout();
            this.flowLayoutPanelExclusions.SuspendLayout();
            this.tableLayoutPanelLunchMode.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanelTopLevel
            // 
            this.flowLayoutPanelTopLevel.Controls.Add(this.flowLayoutPanelExclusions);
            this.flowLayoutPanelTopLevel.Controls.Add(this.checkBoxLHT);
            this.flowLayoutPanelTopLevel.Controls.Add(this.tableLayoutPanelLunchMode);
            this.flowLayoutPanelTopLevel.Controls.Add(this.checkBoxPoke);
            this.flowLayoutPanelTopLevel.Controls.Add(this.checkBoxPhased);
            this.flowLayoutPanelTopLevel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelTopLevel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanelTopLevel.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanelTopLevel.Name = "flowLayoutPanelTopLevel";
            this.flowLayoutPanelTopLevel.Size = new System.Drawing.Size(477, 301);
            this.flowLayoutPanelTopLevel.TabIndex = 0;
            this.flowLayoutPanelTopLevel.SizeChanged += new System.EventHandler(this.flowLayoutPanelTopLevel_SizeChanged);
            this.flowLayoutPanelTopLevel.VisibleChanged += new System.EventHandler(this.flowLayoutPanelTopLevel_VisibleChanged);
            // 
            // flowLayoutPanelExclusions
            // 
            this.flowLayoutPanelExclusions.AutoSize = true;
            this.flowLayoutPanelExclusions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanelExclusions.Controls.Add(this.checkBoxNoAssets);
            this.flowLayoutPanelExclusions.Controls.Add(this.checkBoxNoMods);
            this.flowLayoutPanelExclusions.Controls.Add(this.checkBoxNoWorkshop);
            this.flowLayoutPanelExclusions.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanelExclusions.Margin = new System.Windows.Forms.Padding(3, 3, 3, 15);
            this.flowLayoutPanelExclusions.Name = "flowLayoutPanelExclusions";
            this.flowLayoutPanelExclusions.Size = new System.Drawing.Size(442, 27);
            this.flowLayoutPanelExclusions.TabIndex = 0;
            this.flowLayoutPanelExclusions.WrapContents = false;
            // 
            // checkBoxNoAssets
            // 
            this.checkBoxNoAssets.AutoSize = true;
            this.checkBoxNoAssets.Location = new System.Drawing.Point(3, 3);
            this.checkBoxNoAssets.Name = "checkBoxNoAssets";
            this.checkBoxNoAssets.Size = new System.Drawing.Size(135, 21);
            this.checkBoxNoAssets.TabIndex = 0;
            this.checkBoxNoAssets.Text = "checkBoxNoAssets";
            this.checkBoxNoAssets.UseVisualStyleBackColor = true;
            // 
            // checkBoxNoMods
            // 
            this.checkBoxNoMods.AutoSize = true;
            this.checkBoxNoMods.Location = new System.Drawing.Point(144, 3);
            this.checkBoxNoMods.Name = "checkBoxNoMods";
            this.checkBoxNoMods.Size = new System.Drawing.Size(132, 21);
            this.checkBoxNoMods.TabIndex = 1;
            this.checkBoxNoMods.Text = "checkBoxNoMods";
            this.checkBoxNoMods.UseVisualStyleBackColor = true;
            // 
            // checkBoxNoWorkshop
            // 
            this.checkBoxNoWorkshop.AutoSize = true;
            this.checkBoxNoWorkshop.Location = new System.Drawing.Point(282, 3);
            this.checkBoxNoWorkshop.Name = "checkBoxNoWorkshop";
            this.checkBoxNoWorkshop.Size = new System.Drawing.Size(157, 21);
            this.checkBoxNoWorkshop.TabIndex = 2;
            this.checkBoxNoWorkshop.Text = "checkBoxNoWorkshop";
            this.checkBoxNoWorkshop.UseVisualStyleBackColor = true;
            // 
            // checkBoxLHT
            // 
            this.checkBoxLHT.AutoSize = true;
            this.checkBoxLHT.Location = new System.Drawing.Point(3, 48);
            this.checkBoxLHT.Margin = new System.Windows.Forms.Padding(3, 3, 3, 15);
            this.checkBoxLHT.Name = "checkBoxLHT";
            this.checkBoxLHT.Padding = new System.Windows.Forms.Padding(3);
            this.checkBoxLHT.Size = new System.Drawing.Size(108, 27);
            this.checkBoxLHT.TabIndex = 1;
            this.checkBoxLHT.Text = "checkBoxLHT";
            this.checkBoxLHT.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelLunchMode
            // 
            this.tableLayoutPanelLunchMode.AutoSize = true;
            this.tableLayoutPanelLunchMode.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanelLunchMode.ColumnCount = 3;
            this.tableLayoutPanelLunchMode.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelLunchMode.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelLunchMode.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelLunchMode.Controls.Add(this.textBoxSavePath, 0, 0);
            this.tableLayoutPanelLunchMode.Controls.Add(this.textBoxMapPath, 0, 1);
            this.tableLayoutPanelLunchMode.Controls.Add(this.buttonSavePath, 1, 0);
            this.tableLayoutPanelLunchMode.Controls.Add(this.buttonMapPath, 1, 1);
            this.tableLayoutPanelLunchMode.Controls.Add(this.buttonLoadSave, 2, 0);
            this.tableLayoutPanelLunchMode.Controls.Add(this.buttonNewGame, 2, 1);
            this.tableLayoutPanelLunchMode.Controls.Add(this.buttonLoadAsset, 2, 2);
            this.tableLayoutPanelLunchMode.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelLunchMode.Location = new System.Drawing.Point(3, 93);
            this.tableLayoutPanelLunchMode.Margin = new System.Windows.Forms.Padding(3, 3, 3, 15);
            this.tableLayoutPanelLunchMode.Name = "tableLayoutPanelLunchMode";
            this.tableLayoutPanelLunchMode.RowCount = 3;
            this.tableLayoutPanelLunchMode.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelLunchMode.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelLunchMode.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelLunchMode.Size = new System.Drawing.Size(442, 99);
            this.tableLayoutPanelLunchMode.TabIndex = 2;
            // 
            // textBoxSavePath
            // 
            this.textBoxSavePath.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxSavePath.Location = new System.Drawing.Point(3, 3);
            this.textBoxSavePath.Name = "textBoxSavePath";
            this.textBoxSavePath.Size = new System.Drawing.Size(281, 25);
            this.textBoxSavePath.TabIndex = 0;
            this.textBoxSavePath.Text = "textBoxSavePath";
            // 
            // textBoxMapPath
            // 
            this.textBoxMapPath.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxMapPath.Location = new System.Drawing.Point(3, 36);
            this.textBoxMapPath.Name = "textBoxMapPath";
            this.textBoxMapPath.Size = new System.Drawing.Size(281, 25);
            this.textBoxMapPath.TabIndex = 0;
            this.textBoxMapPath.Text = "textBoxMapPath";
            // 
            // buttonSavePath
            // 
            this.buttonSavePath.AutoSize = true;
            this.buttonSavePath.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonSavePath.Location = new System.Drawing.Point(290, 3);
            this.buttonSavePath.Name = "buttonSavePath";
            this.buttonSavePath.Size = new System.Drawing.Size(27, 27);
            this.buttonSavePath.TabIndex = 3;
            this.buttonSavePath.Text = "…";
            this.buttonSavePath.UseVisualStyleBackColor = true;
            // 
            // buttonMapPath
            // 
            this.buttonMapPath.AutoSize = true;
            this.buttonMapPath.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonMapPath.Location = new System.Drawing.Point(290, 36);
            this.buttonMapPath.Name = "buttonMapPath";
            this.buttonMapPath.Size = new System.Drawing.Size(27, 27);
            this.buttonMapPath.TabIndex = 4;
            this.buttonMapPath.Text = "…";
            this.buttonMapPath.UseVisualStyleBackColor = true;
            // 
            // buttonLoadSave
            // 
            this.buttonLoadSave.AutoSize = true;
            this.buttonLoadSave.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonLoadSave.Location = new System.Drawing.Point(323, 3);
            this.buttonLoadSave.Name = "buttonLoadSave";
            this.buttonLoadSave.Size = new System.Drawing.Size(112, 27);
            this.buttonLoadSave.TabIndex = 6;
            this.buttonLoadSave.Text = "buttonLoadSave";
            this.buttonLoadSave.UseVisualStyleBackColor = true;
            // 
            // buttonNewGame
            // 
            this.buttonNewGame.AutoSize = true;
            this.buttonNewGame.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonNewGame.Location = new System.Drawing.Point(323, 36);
            this.buttonNewGame.Name = "buttonNewGame";
            this.buttonNewGame.Size = new System.Drawing.Size(116, 27);
            this.buttonNewGame.TabIndex = 7;
            this.buttonNewGame.Text = "buttonNewGame";
            this.buttonNewGame.UseVisualStyleBackColor = true;
            // 
            // buttonLoadAsset
            // 
            this.buttonLoadAsset.Location = new System.Drawing.Point(323, 69);
            this.buttonLoadAsset.Name = "buttonLoadAsset";
            this.buttonLoadAsset.Size = new System.Drawing.Size(116, 27);
            this.buttonLoadAsset.TabIndex = 8;
            this.buttonLoadAsset.Text = "buttonLoadAsset";
            this.buttonLoadAsset.UseVisualStyleBackColor = true;
            // 
            // checkBoxPoke
            // 
            this.checkBoxPoke.AutoSize = true;
            this.checkBoxPoke.Location = new System.Drawing.Point(3, 210);
            this.checkBoxPoke.Name = "checkBoxPoke";
            this.checkBoxPoke.Size = new System.Drawing.Size(108, 21);
            this.checkBoxPoke.TabIndex = 3;
            this.checkBoxPoke.Text = "checkBoxPoke";
            this.checkBoxPoke.UseVisualStyleBackColor = true;
            // 
            // checkBoxPhased
            // 
            this.checkBoxPhased.AutoSize = true;
            this.checkBoxPhased.Location = new System.Drawing.Point(3, 237);
            this.checkBoxPhased.Name = "checkBoxPhased";
            this.checkBoxPhased.Size = new System.Drawing.Size(122, 21);
            this.checkBoxPhased.TabIndex = 4;
            this.checkBoxPhased.Text = "checkBoxPhased";
            this.checkBoxPhased.UseVisualStyleBackColor = true;
            // 
            // labelCommand
            // 
            this.labelCommand.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelCommand.Location = new System.Drawing.Point(0, 284);
            this.labelCommand.Name = "labelCommand";
            this.labelCommand.Size = new System.Drawing.Size(477, 17);
            this.labelCommand.TabIndex = 6;
            this.labelCommand.Text = "command";
            // 
            // LaunchControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelCommand);
            this.Controls.Add(this.flowLayoutPanelTopLevel);
            this.Name = "LaunchControl";
            this.Size = new System.Drawing.Size(477, 301);
            this.flowLayoutPanelTopLevel.ResumeLayout(false);
            this.flowLayoutPanelTopLevel.PerformLayout();
            this.flowLayoutPanelExclusions.ResumeLayout(false);
            this.flowLayoutPanelExclusions.PerformLayout();
            this.tableLayoutPanelLunchMode.ResumeLayout(false);
            this.tableLayoutPanelLunchMode.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelTopLevel;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelExclusions;
        private System.Windows.Forms.CheckBox checkBoxNoAssets;
        private System.Windows.Forms.CheckBox checkBoxNoMods;
        private System.Windows.Forms.CheckBox checkBoxNoWorkshop;
        private System.Windows.Forms.CheckBox checkBoxLHT;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelLunchMode;
        private System.Windows.Forms.TextBox textBoxSavePath;
        private System.Windows.Forms.TextBox textBoxMapPath;
        private System.Windows.Forms.Button buttonSavePath;
        private System.Windows.Forms.Button buttonMapPath;
        private System.Windows.Forms.Button buttonLoadSave;
        private System.Windows.Forms.Button buttonNewGame;
        private System.Windows.Forms.Button buttonLoadAsset;
        private System.Windows.Forms.CheckBox checkBoxPoke;
        private System.Windows.Forms.CheckBox checkBoxPhased;
        private System.Windows.Forms.Label labelCommand;
    }
}
