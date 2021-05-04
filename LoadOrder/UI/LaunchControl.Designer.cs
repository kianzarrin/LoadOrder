using System.Windows.Forms;

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
            this.radioButtonMainMenu = new System.Windows.Forms.RadioButton();
            this.radioButtonAssetEditor = new System.Windows.Forms.RadioButton();
            this.radioButtonLoadSave = new System.Windows.Forms.RadioButton();
            this.textBoxSavePath = new System.Windows.Forms.TextBox();
            this.buttonSavePath = new System.Windows.Forms.Button();
            this.radioButtonNewGame = new System.Windows.Forms.RadioButton();
            this.textBoxMapPath = new System.Windows.Forms.TextBox();
            this.buttonMapPath = new System.Windows.Forms.Button();
            this.checkBoxPoke = new System.Windows.Forms.CheckBox();
            this.checkBoxPhased = new System.Windows.Forms.CheckBox();
            this.buttonLaunch = new System.Windows.Forms.Button();
            this.labelCommand = new System.Windows.Forms.Label();
            this.flowLayoutPanelLoadMode = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanelTopLevel.SuspendLayout();
            this.flowLayoutPanelExclusions.SuspendLayout();
            this.tableLayoutPanelLunchMode.SuspendLayout();
            this.flowLayoutPanelLoadMode.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanelTopLevel
            // 
            this.flowLayoutPanelTopLevel.Controls.Add(this.flowLayoutPanelExclusions);
            this.flowLayoutPanelTopLevel.Controls.Add(this.checkBoxLHT);
            this.flowLayoutPanelTopLevel.Controls.Add(this.tableLayoutPanelLunchMode);
            this.flowLayoutPanelTopLevel.Controls.Add(this.flowLayoutPanelLoadMode);
            this.flowLayoutPanelTopLevel.Controls.Add(this.buttonLaunch);
            this.flowLayoutPanelTopLevel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelTopLevel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanelTopLevel.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanelTopLevel.Name = "flowLayoutPanelTopLevel";
            this.flowLayoutPanelTopLevel.Size = new System.Drawing.Size(599, 339);
            this.flowLayoutPanelTopLevel.TabIndex = 0;
            this.flowLayoutPanelTopLevel.WrapContents = false;
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
            this.flowLayoutPanelExclusions.Size = new System.Drawing.Size(295, 27);
            this.flowLayoutPanelExclusions.TabIndex = 0;
            this.flowLayoutPanelExclusions.WrapContents = false;
            // 
            // checkBoxNoAssets
            // 
            this.checkBoxNoAssets.AutoSize = true;
            this.checkBoxNoAssets.Location = new System.Drawing.Point(3, 3);
            this.checkBoxNoAssets.Name = "checkBoxNoAssets";
            this.checkBoxNoAssets.Size = new System.Drawing.Size(86, 21);
            this.checkBoxNoAssets.TabIndex = 0;
            this.checkBoxNoAssets.Text = "No Assets";
            this.checkBoxNoAssets.UseVisualStyleBackColor = true;
            // 
            // checkBoxNoMods
            // 
            this.checkBoxNoMods.AutoSize = true;
            this.checkBoxNoMods.Location = new System.Drawing.Point(95, 3);
            this.checkBoxNoMods.Name = "checkBoxNoMods";
            this.checkBoxNoMods.Size = new System.Drawing.Size(83, 21);
            this.checkBoxNoMods.TabIndex = 1;
            this.checkBoxNoMods.Text = "No Mods";
            this.checkBoxNoMods.UseVisualStyleBackColor = true;
            // 
            // checkBoxNoWorkshop
            // 
            this.checkBoxNoWorkshop.AutoSize = true;
            this.checkBoxNoWorkshop.Location = new System.Drawing.Point(184, 3);
            this.checkBoxNoWorkshop.Name = "checkBoxNoWorkshop";
            this.checkBoxNoWorkshop.Size = new System.Drawing.Size(108, 21);
            this.checkBoxNoWorkshop.TabIndex = 2;
            this.checkBoxNoWorkshop.Text = "No Workshop";
            this.checkBoxNoWorkshop.UseVisualStyleBackColor = true;
            // 
            // checkBoxLHT
            // 
            this.checkBoxLHT.AutoSize = true;
            this.checkBoxLHT.Location = new System.Drawing.Point(3, 48);
            this.checkBoxLHT.Margin = new System.Windows.Forms.Padding(3, 3, 3, 15);
            this.checkBoxLHT.Name = "checkBoxLHT";
            this.checkBoxLHT.Padding = new System.Windows.Forms.Padding(3);
            this.checkBoxLHT.Size = new System.Drawing.Size(128, 27);
            this.checkBoxLHT.TabIndex = 1;
            this.checkBoxLHT.Text = "Left Hand Traffic";
            this.checkBoxLHT.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelLunchMode
            // 
            this.tableLayoutPanelLunchMode.AutoSize = true;
            this.tableLayoutPanelLunchMode.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanelLunchMode.ColumnCount = 3;
            this.tableLayoutPanelLunchMode.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelLunchMode.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelLunchMode.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelLunchMode.Controls.Add(this.radioButtonMainMenu, 0, 0);
            this.tableLayoutPanelLunchMode.Controls.Add(this.radioButtonAssetEditor, 0, 1);
            this.tableLayoutPanelLunchMode.Controls.Add(this.radioButtonLoadSave, 0, 2);
            this.tableLayoutPanelLunchMode.Controls.Add(this.textBoxSavePath, 1, 2);
            this.tableLayoutPanelLunchMode.Controls.Add(this.buttonSavePath, 2, 2);
            this.tableLayoutPanelLunchMode.Controls.Add(this.radioButtonNewGame, 0, 3);
            this.tableLayoutPanelLunchMode.Controls.Add(this.textBoxMapPath, 1, 3);
            this.tableLayoutPanelLunchMode.Controls.Add(this.buttonMapPath, 2, 3);
            this.tableLayoutPanelLunchMode.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelLunchMode.Location = new System.Drawing.Point(3, 93);
            this.tableLayoutPanelLunchMode.Margin = new System.Windows.Forms.Padding(3, 3, 3, 15);
            this.tableLayoutPanelLunchMode.Name = "tableLayoutPanelLunchMode";
            this.tableLayoutPanelLunchMode.RowCount = 4;
            this.tableLayoutPanelLunchMode.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelLunchMode.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelLunchMode.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelLunchMode.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelLunchMode.Size = new System.Drawing.Size(422, 120);
            this.tableLayoutPanelLunchMode.TabIndex = 2;
            // 
            // radioButtonMainMenu
            // 
            this.radioButtonMainMenu.AutoSize = true;
            this.radioButtonMainMenu.Location = new System.Drawing.Point(3, 3);
            this.radioButtonMainMenu.Name = "radioButtonMainMenu";
            this.radioButtonMainMenu.Size = new System.Drawing.Size(92, 21);
            this.radioButtonMainMenu.TabIndex = 5;
            this.radioButtonMainMenu.TabStop = true;
            this.radioButtonMainMenu.Text = "Main Menu";
            this.radioButtonMainMenu.UseVisualStyleBackColor = true;
            // 
            // radioButtonAssetEditor
            // 
            this.radioButtonAssetEditor.AutoSize = true;
            this.radioButtonAssetEditor.Location = new System.Drawing.Point(3, 30);
            this.radioButtonAssetEditor.Name = "radioButtonAssetEditor";
            this.radioButtonAssetEditor.Size = new System.Drawing.Size(96, 21);
            this.radioButtonAssetEditor.TabIndex = 5;
            this.radioButtonAssetEditor.TabStop = true;
            this.radioButtonAssetEditor.Text = "Asset Editor";
            this.radioButtonAssetEditor.UseVisualStyleBackColor = true;
            // 
            // radioButtonLoadSave
            // 
            this.radioButtonLoadSave.AutoSize = true;
            this.radioButtonLoadSave.Location = new System.Drawing.Point(3, 57);
            this.radioButtonLoadSave.Name = "radioButtonLoadSave";
            this.radioButtonLoadSave.Size = new System.Drawing.Size(86, 21);
            this.radioButtonLoadSave.TabIndex = 5;
            this.radioButtonLoadSave.TabStop = true;
            this.radioButtonLoadSave.Text = "Load Save";
            this.radioButtonLoadSave.UseVisualStyleBackColor = true;
            // 
            // textBoxSavePath
            // 
            this.textBoxSavePath.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxSavePath.Location = new System.Drawing.Point(105, 57);
            this.textBoxSavePath.Name = "textBoxSavePath";
            this.textBoxSavePath.Size = new System.Drawing.Size(281, 25);
            this.textBoxSavePath.TabIndex = 0;
            // 
            // buttonSavePath
            // 
            this.buttonSavePath.AutoSize = true;
            this.buttonSavePath.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonSavePath.Location = new System.Drawing.Point(392, 57);
            this.buttonSavePath.Name = "buttonSavePath";
            this.buttonSavePath.Size = new System.Drawing.Size(27, 27);
            this.buttonSavePath.TabIndex = 3;
            this.buttonSavePath.Text = "…";
            this.buttonSavePath.UseVisualStyleBackColor = true;
            this.buttonSavePath.Click += new System.EventHandler(this.buttonSavePath_Click);
            // 
            // radioButtonNewGame
            // 
            this.radioButtonNewGame.AutoSize = true;
            this.radioButtonNewGame.Location = new System.Drawing.Point(3, 90);
            this.radioButtonNewGame.Name = "radioButtonNewGame";
            this.radioButtonNewGame.Size = new System.Drawing.Size(96, 21);
            this.radioButtonNewGame.TabIndex = 5;
            this.radioButtonNewGame.TabStop = true;
            this.radioButtonNewGame.Text = "Asset Editor";
            this.radioButtonNewGame.UseVisualStyleBackColor = true;
            // 
            // textBoxMapPath
            // 
            this.textBoxMapPath.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxMapPath.Location = new System.Drawing.Point(105, 90);
            this.textBoxMapPath.Name = "textBoxMapPath";
            this.textBoxMapPath.Size = new System.Drawing.Size(281, 25);
            this.textBoxMapPath.TabIndex = 0;
            // 
            // buttonMapPath
            // 
            this.buttonMapPath.AutoSize = true;
            this.buttonMapPath.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonMapPath.Location = new System.Drawing.Point(392, 90);
            this.buttonMapPath.Name = "buttonMapPath";
            this.buttonMapPath.Size = new System.Drawing.Size(27, 27);
            this.buttonMapPath.TabIndex = 4;
            this.buttonMapPath.Text = "…";
            this.buttonMapPath.UseVisualStyleBackColor = true;
            this.buttonMapPath.Click += new System.EventHandler(this.buttonMapPath_Click);
            // 
            // checkBoxPoke
            // 
            this.checkBoxPoke.AutoSize = true;
            this.checkBoxPoke.Location = new System.Drawing.Point(78, 3);
            this.checkBoxPoke.Name = "checkBoxPoke";
            this.checkBoxPoke.Size = new System.Drawing.Size(55, 21);
            this.checkBoxPoke.TabIndex = 3;
            this.checkBoxPoke.Text = "Poke";
            this.checkBoxPoke.UseVisualStyleBackColor = true;
            // 
            // checkBoxPhased
            // 
            this.checkBoxPhased.AutoSize = true;
            this.checkBoxPhased.Location = new System.Drawing.Point(3, 3);
            this.checkBoxPhased.Name = "checkBoxPhased";
            this.checkBoxPhased.Size = new System.Drawing.Size(69, 21);
            this.checkBoxPhased.TabIndex = 4;
            this.checkBoxPhased.Text = "Phased";
            this.checkBoxPhased.UseVisualStyleBackColor = true;
            // 
            // buttonLaunch
            // 
            this.buttonLaunch.AutoSize = true;
            this.buttonLaunch.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonLaunch.Location = new System.Drawing.Point(3, 281);
            this.buttonLaunch.Margin = new System.Windows.Forms.Padding(3, 20, 3, 3);
            this.buttonLaunch.Name = "buttonLaunch";
            this.buttonLaunch.Size = new System.Drawing.Size(139, 27);
            this.buttonLaunch.TabIndex = 5;
            this.buttonLaunch.Text = "Launch Cites Skylines";
            this.buttonLaunch.UseVisualStyleBackColor = true;
            this.buttonLaunch.Click += new System.EventHandler(this.buttonLaunch_Click);
            // 
            // labelCommand
            // 
            this.labelCommand.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelCommand.Location = new System.Drawing.Point(0, 322);
            this.labelCommand.Name = "labelCommand";
            this.labelCommand.Size = new System.Drawing.Size(599, 17);
            this.labelCommand.TabIndex = 6;
            this.labelCommand.Text = "command";
            // 
            // flowLayoutPanelLoadMode
            // 
            this.flowLayoutPanelLoadMode.AutoSize = true;
            this.flowLayoutPanelLoadMode.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanelLoadMode.Controls.Add(this.checkBoxPoke);
            this.flowLayoutPanelLoadMode.Controls.Add(this.checkBoxPhased);
            this.flowLayoutPanelLoadMode.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanelLoadMode.Location = new System.Drawing.Point(3, 231);
            this.flowLayoutPanelLoadMode.Name = "flowLayoutPanelLoadMode";
            this.flowLayoutPanelLoadMode.Size = new System.Drawing.Size(136, 27);
            this.flowLayoutPanelLoadMode.TabIndex = 6;
            // 
            // LaunchControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelCommand);
            this.Controls.Add(this.flowLayoutPanelTopLevel);
            this.Name = "LaunchControl";
            this.Size = new System.Drawing.Size(599, 339);
            this.flowLayoutPanelTopLevel.ResumeLayout(false);
            this.flowLayoutPanelTopLevel.PerformLayout();
            this.flowLayoutPanelExclusions.ResumeLayout(false);
            this.flowLayoutPanelExclusions.PerformLayout();
            this.tableLayoutPanelLunchMode.ResumeLayout(false);
            this.tableLayoutPanelLunchMode.PerformLayout();
            this.flowLayoutPanelLoadMode.ResumeLayout(false);
            this.flowLayoutPanelLoadMode.PerformLayout();
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
        private System.Windows.Forms.CheckBox checkBoxPoke;
        private System.Windows.Forms.CheckBox checkBoxPhased;
        private System.Windows.Forms.Label labelCommand;
        private RadioButton radioButtonMainMenu;
        private RadioButton radioButtonAssetEditor;
        private RadioButton radioButtonLoadSave;
        private RadioButton radioButtonNewGame;
        private Button buttonLaunch;
        private FlowLayoutPanel flowLayoutPanelLoadMode;
    }
}
