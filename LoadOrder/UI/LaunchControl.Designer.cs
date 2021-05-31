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
            this.flowLayoutPanelLoadMode = new System.Windows.Forms.FlowLayoutPanel();
            this.checkBoxPoke = new System.Windows.Forms.CheckBox();
            this.checkBoxPhased = new System.Windows.Forms.CheckBox();
            this.flowLayoutPanelLauncher = new System.Windows.Forms.FlowLayoutPanel();
            this.labelLauncher = new System.Windows.Forms.Label();
            this.radioButtonSteamExe = new System.Windows.Forms.RadioButton();
            this.radioButtonCitiesExe = new System.Windows.Forms.RadioButton();
            this.flowLayoutPanelMono = new System.Windows.Forms.FlowLayoutPanel();
            this.labelMono = new System.Windows.Forms.Label();
            this.radioButtonReleaseMono = new System.Windows.Forms.RadioButton();
            this.radioButtonDebugMono = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanelExtraArgs = new System.Windows.Forms.TableLayoutPanel();
            this.labelExtraArgs = new System.Windows.Forms.Label();
            this.textBoxExtraArgs = new System.Windows.Forms.TextBox();
            this.buttonLaunch = new System.Windows.Forms.Button();
            this.lblNote = new System.Windows.Forms.Label();
            this.labelCommand = new System.Windows.Forms.Label();
            this.flowLayoutPanelAssetEditorOptions = new System.Windows.Forms.FlowLayoutPanel();
            this.checkBoxNewAsset = new System.Windows.Forms.CheckBox();
            this.checkBoxLSM = new System.Windows.Forms.CheckBox();
            this.flowLayoutPanelTopLevel.SuspendLayout();
            this.flowLayoutPanelExclusions.SuspendLayout();
            this.tableLayoutPanelLunchMode.SuspendLayout();
            this.flowLayoutPanelLoadMode.SuspendLayout();
            this.flowLayoutPanelLauncher.SuspendLayout();
            this.flowLayoutPanelMono.SuspendLayout();
            this.tableLayoutPanelExtraArgs.SuspendLayout();
            this.flowLayoutPanelAssetEditorOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanelTopLevel
            // 
            this.flowLayoutPanelTopLevel.Controls.Add(this.flowLayoutPanelExclusions);
            this.flowLayoutPanelTopLevel.Controls.Add(this.checkBoxLHT);
            this.flowLayoutPanelTopLevel.Controls.Add(this.tableLayoutPanelLunchMode);
            this.flowLayoutPanelTopLevel.Controls.Add(this.flowLayoutPanelLoadMode);
            this.flowLayoutPanelTopLevel.Controls.Add(this.flowLayoutPanelLauncher);
            this.flowLayoutPanelTopLevel.Controls.Add(this.flowLayoutPanelMono);
            this.flowLayoutPanelTopLevel.Controls.Add(this.tableLayoutPanelExtraArgs);
            this.flowLayoutPanelTopLevel.Controls.Add(this.buttonLaunch);
            this.flowLayoutPanelTopLevel.Controls.Add(this.lblNote);
            this.flowLayoutPanelTopLevel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelTopLevel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanelTopLevel.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanelTopLevel.Name = "flowLayoutPanelTopLevel";
            this.flowLayoutPanelTopLevel.Size = new System.Drawing.Size(508, 499);
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
            this.tableLayoutPanelLunchMode.Controls.Add(this.flowLayoutPanelAssetEditorOptions, 1, 1);
            this.tableLayoutPanelLunchMode.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelLunchMode.Location = new System.Drawing.Point(3, 93);
            this.tableLayoutPanelLunchMode.Margin = new System.Windows.Forms.Padding(3, 3, 3, 15);
            this.tableLayoutPanelLunchMode.Name = "tableLayoutPanelLunchMode";
            this.tableLayoutPanelLunchMode.RowCount = 4;
            this.tableLayoutPanelLunchMode.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelLunchMode.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelLunchMode.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelLunchMode.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelLunchMode.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelLunchMode.Size = new System.Drawing.Size(422, 126);
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
            this.radioButtonLoadSave.Location = new System.Drawing.Point(3, 63);
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
            this.textBoxSavePath.Location = new System.Drawing.Point(105, 63);
            this.textBoxSavePath.Name = "textBoxSavePath";
            this.textBoxSavePath.Size = new System.Drawing.Size(281, 25);
            this.textBoxSavePath.TabIndex = 0;
            // 
            // buttonSavePath
            // 
            this.buttonSavePath.AutoSize = true;
            this.buttonSavePath.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonSavePath.Location = new System.Drawing.Point(392, 63);
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
            this.radioButtonNewGame.Location = new System.Drawing.Point(3, 96);
            this.radioButtonNewGame.Name = "radioButtonNewGame";
            this.radioButtonNewGame.Size = new System.Drawing.Size(90, 21);
            this.radioButtonNewGame.TabIndex = 5;
            this.radioButtonNewGame.TabStop = true;
            this.radioButtonNewGame.Text = "New Game";
            this.radioButtonNewGame.UseVisualStyleBackColor = true;
            // 
            // textBoxMapPath
            // 
            this.textBoxMapPath.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxMapPath.Location = new System.Drawing.Point(105, 96);
            this.textBoxMapPath.Name = "textBoxMapPath";
            this.textBoxMapPath.Size = new System.Drawing.Size(281, 25);
            this.textBoxMapPath.TabIndex = 0;
            // 
            // buttonMapPath
            // 
            this.buttonMapPath.AutoSize = true;
            this.buttonMapPath.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonMapPath.Location = new System.Drawing.Point(392, 96);
            this.buttonMapPath.Name = "buttonMapPath";
            this.buttonMapPath.Size = new System.Drawing.Size(27, 27);
            this.buttonMapPath.TabIndex = 4;
            this.buttonMapPath.Text = "…";
            this.buttonMapPath.UseVisualStyleBackColor = true;
            this.buttonMapPath.Click += new System.EventHandler(this.buttonMapPath_Click);
            // 
            // flowLayoutPanelLoadMode
            // 
            this.flowLayoutPanelLoadMode.AutoSize = true;
            this.flowLayoutPanelLoadMode.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanelLoadMode.Controls.Add(this.checkBoxPoke);
            this.flowLayoutPanelLoadMode.Controls.Add(this.checkBoxPhased);
            this.flowLayoutPanelLoadMode.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanelLoadMode.Location = new System.Drawing.Point(3, 237);
            this.flowLayoutPanelLoadMode.Name = "flowLayoutPanelLoadMode";
            this.flowLayoutPanelLoadMode.Size = new System.Drawing.Size(136, 27);
            this.flowLayoutPanelLoadMode.TabIndex = 6;
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
            // flowLayoutPanelLauncher
            // 
            this.flowLayoutPanelLauncher.AutoSize = true;
            this.flowLayoutPanelLauncher.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanelLauncher.Controls.Add(this.labelLauncher);
            this.flowLayoutPanelLauncher.Controls.Add(this.radioButtonSteamExe);
            this.flowLayoutPanelLauncher.Controls.Add(this.radioButtonCitiesExe);
            this.flowLayoutPanelLauncher.Location = new System.Drawing.Point(3, 282);
            this.flowLayoutPanelLauncher.Margin = new System.Windows.Forms.Padding(3, 15, 3, 3);
            this.flowLayoutPanelLauncher.Name = "flowLayoutPanelLauncher";
            this.flowLayoutPanelLauncher.Size = new System.Drawing.Size(250, 27);
            this.flowLayoutPanelLauncher.TabIndex = 7;
            this.flowLayoutPanelLauncher.WrapContents = false;
            // 
            // labelLauncher
            // 
            this.labelLauncher.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelLauncher.AutoSize = true;
            this.labelLauncher.Location = new System.Drawing.Point(3, 5);
            this.labelLauncher.Name = "labelLauncher";
            this.labelLauncher.Size = new System.Drawing.Size(67, 17);
            this.labelLauncher.TabIndex = 2;
            this.labelLauncher.Text = "Launcher: ";
            // 
            // radioButtonSteamExe
            // 
            this.radioButtonSteamExe.AutoSize = true;
            this.radioButtonSteamExe.Location = new System.Drawing.Point(76, 3);
            this.radioButtonSteamExe.Name = "radioButtonSteamExe";
            this.radioButtonSteamExe.Size = new System.Drawing.Size(85, 21);
            this.radioButtonSteamExe.TabIndex = 1;
            this.radioButtonSteamExe.TabStop = true;
            this.radioButtonSteamExe.Text = "Steam.exe";
            this.radioButtonSteamExe.UseVisualStyleBackColor = true;
            // 
            // radioButtonCitiesExe
            // 
            this.radioButtonCitiesExe.AutoSize = true;
            this.radioButtonCitiesExe.Location = new System.Drawing.Point(167, 3);
            this.radioButtonCitiesExe.Name = "radioButtonCitiesExe";
            this.radioButtonCitiesExe.Size = new System.Drawing.Size(80, 21);
            this.radioButtonCitiesExe.TabIndex = 0;
            this.radioButtonCitiesExe.TabStop = true;
            this.radioButtonCitiesExe.Text = "Cities.exe";
            this.radioButtonCitiesExe.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanelMono
            // 
            this.flowLayoutPanelMono.AutoSize = true;
            this.flowLayoutPanelMono.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanelMono.Controls.Add(this.labelMono);
            this.flowLayoutPanelMono.Controls.Add(this.radioButtonReleaseMono);
            this.flowLayoutPanelMono.Controls.Add(this.radioButtonDebugMono);
            this.flowLayoutPanelMono.Location = new System.Drawing.Point(3, 312);
            this.flowLayoutPanelMono.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.flowLayoutPanelMono.Name = "flowLayoutPanelMono";
            this.flowLayoutPanelMono.Size = new System.Drawing.Size(403, 27);
            this.flowLayoutPanelMono.TabIndex = 8;
            this.flowLayoutPanelMono.WrapContents = false;
            // 
            // labelMono
            // 
            this.labelMono.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelMono.AutoSize = true;
            this.labelMono.Location = new System.Drawing.Point(3, 5);
            this.labelMono.Name = "labelMono";
            this.labelMono.Size = new System.Drawing.Size(50, 17);
            this.labelMono.TabIndex = 2;
            this.labelMono.Text = "Mono: ";
            // 
            // radioButtonReleaseMono
            // 
            this.radioButtonReleaseMono.AutoSize = true;
            this.radioButtonReleaseMono.Location = new System.Drawing.Point(59, 3);
            this.radioButtonReleaseMono.Name = "radioButtonReleaseMono";
            this.radioButtonReleaseMono.Size = new System.Drawing.Size(155, 21);
            this.radioButtonReleaseMono.TabIndex = 0;
            this.radioButtonReleaseMono.TabStop = true;
            this.radioButtonReleaseMono.Text = "Release Mode (faster)";
            this.radioButtonReleaseMono.UseVisualStyleBackColor = true;
            // 
            // radioButtonDebugMono
            // 
            this.radioButtonDebugMono.AutoSize = true;
            this.radioButtonDebugMono.Location = new System.Drawing.Point(220, 3);
            this.radioButtonDebugMono.Name = "radioButtonDebugMono";
            this.radioButtonDebugMono.Size = new System.Drawing.Size(180, 21);
            this.radioButtonDebugMono.TabIndex = 1;
            this.radioButtonDebugMono.TabStop = true;
            this.radioButtonDebugMono.Text = "Debug Mode (better logs)";
            this.radioButtonDebugMono.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelExtraArgs
            // 
            this.tableLayoutPanelExtraArgs.AutoSize = true;
            this.tableLayoutPanelExtraArgs.ColumnCount = 2;
            this.tableLayoutPanelExtraArgs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelExtraArgs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelExtraArgs.Controls.Add(this.labelExtraArgs, 0, 0);
            this.tableLayoutPanelExtraArgs.Controls.Add(this.textBoxExtraArgs, 1, 0);
            this.tableLayoutPanelExtraArgs.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelExtraArgs.Location = new System.Drawing.Point(3, 345);
            this.tableLayoutPanelExtraArgs.Name = "tableLayoutPanelExtraArgs";
            this.tableLayoutPanelExtraArgs.RowCount = 1;
            this.tableLayoutPanelExtraArgs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelExtraArgs.Size = new System.Drawing.Size(422, 31);
            this.tableLayoutPanelExtraArgs.TabIndex = 10;
            // 
            // labelExtraArgs
            // 
            this.labelExtraArgs.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelExtraArgs.AutoSize = true;
            this.labelExtraArgs.Location = new System.Drawing.Point(3, 7);
            this.labelExtraArgs.Name = "labelExtraArgs";
            this.labelExtraArgs.Size = new System.Drawing.Size(104, 17);
            this.labelExtraArgs.TabIndex = 0;
            this.labelExtraArgs.Text = "Extra Arguments";
            // 
            // textBoxExtraArgs
            // 
            this.textBoxExtraArgs.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxExtraArgs.Location = new System.Drawing.Point(113, 3);
            this.textBoxExtraArgs.Name = "textBoxExtraArgs";
            this.textBoxExtraArgs.Size = new System.Drawing.Size(306, 25);
            this.textBoxExtraArgs.TabIndex = 1;
            // 
            // buttonLaunch
            // 
            this.buttonLaunch.AutoSize = true;
            this.buttonLaunch.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonLaunch.Location = new System.Drawing.Point(3, 384);
            this.buttonLaunch.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.buttonLaunch.Name = "buttonLaunch";
            this.buttonLaunch.Size = new System.Drawing.Size(139, 27);
            this.buttonLaunch.TabIndex = 5;
            this.buttonLaunch.Text = "Launch Cities Skylines";
            this.buttonLaunch.UseVisualStyleBackColor = true;
            this.buttonLaunch.Click += new System.EventHandler(this.buttonLaunch_Click);
            // 
            // lblNote
            // 
            this.lblNote.AutoSize = true;
            this.lblNote.Location = new System.Drawing.Point(3, 414);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(245, 17);
            this.lblNote.TabIndex = 9;
            this.lblNote.Text = "?: hover over components for more info.";
            // 
            // labelCommand
            // 
            this.labelCommand.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelCommand.Location = new System.Drawing.Point(0, 482);
            this.labelCommand.Name = "labelCommand";
            this.labelCommand.Size = new System.Drawing.Size(508, 17);
            this.labelCommand.TabIndex = 6;
            this.labelCommand.Text = "command";
            // 
            // flowLayoutPanelAssetEditorOptions
            // 
            this.flowLayoutPanelAssetEditorOptions.AutoSize = true;
            this.flowLayoutPanelAssetEditorOptions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanelAssetEditorOptions.Controls.Add(this.checkBoxNewAsset);
            this.flowLayoutPanelAssetEditorOptions.Controls.Add(this.checkBoxLSM);
            this.flowLayoutPanelAssetEditorOptions.Location = new System.Drawing.Point(105, 30);
            this.flowLayoutPanelAssetEditorOptions.Name = "flowLayoutPanelAssetEditorOptions";
            this.flowLayoutPanelAssetEditorOptions.Size = new System.Drawing.Size(117, 27);
            this.flowLayoutPanelAssetEditorOptions.TabIndex = 6;
            // 
            // checkBoxNewAsset
            // 
            this.checkBoxNewAsset.AutoSize = true;
            this.checkBoxNewAsset.Location = new System.Drawing.Point(3, 3);
            this.checkBoxNewAsset.Name = "checkBoxNewAsset";
            this.checkBoxNewAsset.Size = new System.Drawing.Size(53, 21);
            this.checkBoxNewAsset.TabIndex = 0;
            this.checkBoxNewAsset.Text = "New";
            this.checkBoxNewAsset.UseVisualStyleBackColor = true;
            // 
            // checkBoxLSM
            // 
            this.checkBoxLSM.AutoSize = true;
            this.checkBoxLSM.Location = new System.Drawing.Point(62, 3);
            this.checkBoxLSM.Name = "checkBoxLSM";
            this.checkBoxLSM.Size = new System.Drawing.Size(52, 21);
            this.checkBoxLSM.TabIndex = 1;
            this.checkBoxLSM.Text = "LSM";
            this.checkBoxLSM.UseVisualStyleBackColor = true;
            // 
            // LaunchControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelCommand);
            this.Controls.Add(this.flowLayoutPanelTopLevel);
            this.Name = "LaunchControl";
            this.Size = new System.Drawing.Size(508, 499);
            this.flowLayoutPanelTopLevel.ResumeLayout(false);
            this.flowLayoutPanelTopLevel.PerformLayout();
            this.flowLayoutPanelExclusions.ResumeLayout(false);
            this.flowLayoutPanelExclusions.PerformLayout();
            this.tableLayoutPanelLunchMode.ResumeLayout(false);
            this.tableLayoutPanelLunchMode.PerformLayout();
            this.flowLayoutPanelLoadMode.ResumeLayout(false);
            this.flowLayoutPanelLoadMode.PerformLayout();
            this.flowLayoutPanelLauncher.ResumeLayout(false);
            this.flowLayoutPanelLauncher.PerformLayout();
            this.flowLayoutPanelMono.ResumeLayout(false);
            this.flowLayoutPanelMono.PerformLayout();
            this.tableLayoutPanelExtraArgs.ResumeLayout(false);
            this.tableLayoutPanelExtraArgs.PerformLayout();
            this.flowLayoutPanelAssetEditorOptions.ResumeLayout(false);
            this.flowLayoutPanelAssetEditorOptions.PerformLayout();
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
        private FlowLayoutPanel flowLayoutPanelLauncher;
        private RadioButton radioButtonSteamExe;
        private RadioButton radioButtonCitiesExe;
        private FlowLayoutPanel flowLayoutPanelMono;
        private RadioButton radioButtonReleaseMono;
        private RadioButton radioButtonDebugMono;
        private Label lblNote;
        private Label labelLauncher;
        private Label labelMono;
        private TableLayoutPanel tableLayoutPanelExtraArgs;
        private Label labelExtraArgs;
        private TextBox textBoxExtraArgs;
        private FlowLayoutPanel flowLayoutPanelAssetEditorOptions;
        private CheckBox checkBoxNewAsset;
        private CheckBox checkBoxLSM;
    }
}
