namespace LoadOrderTool.UI {
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using CO.IO;
    using System.IO;
    using System.Diagnostics;

    public partial class LaunchControl : UserControl {
        public LaunchControl() {
            InitializeComponent();
            foreach (var c in this.GetAll<TextBox>())
                c.TextChanged += UpdateCommand;

            foreach (var c in this.GetAll<CheckBox>())
                c.CheckedChanged += UpdateCommand;

            foreach (var c in this.GetAll<RadioButton>())
                c.CheckedChanged += UpdateCommand;

            checkBoxLHT.SetTooltip("Traffic drives on left.");
            textBoxSavePath.SetTooltip("leave empty to continue last save. enter save name or its full path to load it.");
            textBoxMapPath.SetTooltip("leave empty to load the first map. enter map name or its full path to load it.");
            checkBoxPoke.SetTooltip("depth-first: poke mods to find potential type resultion problems.");
            checkBoxPhased.SetTooltip("breadth-frist: load mods in phases to avoid potential type resultion problems.");
            
            UpdateCommand(null, null);
        }

        private void flowLayoutPanelTopLevel_SizeChanged(object sender, EventArgs e) =>
            AutoSizeLaunchTable();

        private void flowLayoutPanelTopLevel_VisibleChanged(object sender, EventArgs e) =>
            AutoSizeLaunchTable();

        private void AutoSizeLaunchTable() =>
            tableLayoutPanelLunchMode.Width = flowLayoutPanelTopLevel.Width;

        private void UpdateCommand(object sender, EventArgs e) {
            labelCommand.Text = "Cities.exe " + string.Join(" ", GetCommandArgs(sender as Button));
        }

        private string[] GetCommandArgs(Button launchButton) {
            List<string> args = new List<string>();

            if (checkBoxNoWorkshop.Checked)
                args.Add("-noWorkshop");
            if (checkBoxNoAssets.Checked)
                args.Add("-noAssets");
            if (checkBoxNoMods.Checked)
                args.Add("-disableMods");
            if (checkBoxLHT.Checked)
                args.Add("-LHT");
            if (checkBoxPhased.Checked)
                args.Add("-pahsed");
            if (checkBoxPoke.Checked)
                args.Add("-poke");

            if (radioButtonMainMenu.Checked) {
                ;
            } else if (radioButtonAssetEditor.Checked) {
                args.Add("-editor");
            } else if (radioButtonNewGame.Checked) {
                string path = textBoxMapPath.Text;
                if (string.IsNullOrEmpty(path))
                    args.Add("-newGame");
                else
                    args.Add("--newGame=" + path);
            } else if (radioButtonLoadSave.Checked) {
                string path = textBoxSavePath.Text;
                if (string.IsNullOrEmpty(path))
                    args.Add("-continuelastsave");
                else
                    args.Add("--loadSave=" + path);
            }

            return args.ToArray();
        }

        private void buttonSavePath_Click(object sender, EventArgs e) {
            var file = OpenCRP(DataLocation.saveLocation, "Load saved game");
            if (!string.IsNullOrEmpty(file))
                textBoxSavePath.Text = file;
        }


        private void buttonMapPath_Click(object sender, EventArgs e) {
            var file = OpenCRP(DataLocation.mapLocation, "Load map");
            if (!string.IsNullOrEmpty(file))
                textBoxMapPath.Text = file;
        }
        private static string OpenCRP(string InitialDirectory, string title) {
            using (var ofd = new OpenFileDialog()) {
                ofd.Filter = "*.crp";
                ofd.Multiselect = false;
                ofd.CheckPathExists = true;
                ofd.AddExtension = true;
                ofd.InitialDirectory = InitialDirectory;
                ofd.Title = title;
                ofd.CustomPlaces.Add(DataLocation.saveLocation);
                ofd.CustomPlaces.Add(DataLocation.mapLocation);
                ofd.CustomPlaces.Add(DataLocation.WorkshopContentPath);
                if (ofd.ShowDialog() == DialogResult.OK) {
                    return ofd.FileName;
                }
            }
            return null;

        }

        private void buttonLoadSave_Click(object sender, EventArgs e) {
            Lunch(sender);
        }

        private void buttonNewGame_Click(object sender, EventArgs e) {
            Lunch(sender);
        }

        private void buttonLoadAsset_Click(object sender, EventArgs e) {
            Lunch(sender);
        }

        private void Lunch(object sender) {
            var args = GetCommandArgs(sender as Button);
            Execute(DataLocation.GamePath, "Cities.exe", string.Join(" ", args));
        }


        static Process Execute(string dir, string exeFile, string args) {
            try {
                ProcessStartInfo startInfo = new ProcessStartInfo {
                    WorkingDirectory = dir,
                    FileName = exeFile,
                    Arguments = args,
                    WindowStyle = ProcessWindowStyle.Normal,
                    UseShellExecute = true,
                    CreateNoWindow = false,
                };
                Log.Info($"Executing ...\n" +
                    $"\tWorkingDirectory={dir}\n" +
                    $"\tFileName={exeFile}\n" +
                    $"\tArguments={args}");
                Process process = new Process { StartInfo = startInfo };
                process.Start();
                process.OutputDataReceived += (_,e)=> Log.Info(e.Data);
                process.ErrorDataReceived += (_, e) => Log.Warning(e.Data);
                process.Exited += (_, e) => Log.Info("process exited with code " + process.ExitCode);
                return process;
            }catch(Exception ex) {
                Log.Exception(ex);
                return null;
            }
        }

    }
}
