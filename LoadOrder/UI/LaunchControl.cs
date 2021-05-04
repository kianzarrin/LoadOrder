namespace LoadOrderTool.UI {
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using CO.IO;
    using System.IO;
    using System.Diagnostics;
    using LoadOrderTool.Data;

    public partial class LaunchControl : UserControl {
        public LoadOrderToolSettings settings_ => LoadOrderToolSettings.Instace;

        public LaunchControl() {
            InitializeComponent();

            LoadSettings();
            UpdateCommand();

            foreach (var c in this.GetAll<TextBox>())
                c.TextChanged += Update;

            foreach (var c in this.GetAll<CheckBox>())
                c.CheckedChanged += Update;

            foreach (var c in this.GetAll<RadioButton>())
                c.CheckedChanged += Update;

            checkBoxLHT.SetTooltip("Traffic drives on left.");
            textBoxSavePath.SetTooltip("leave empty to continue last save. enter save name or its full path to load it.");
            textBoxMapPath.SetTooltip("leave empty to load the first map. enter map name or its full path to load it.");
            checkBoxPoke.SetTooltip("depth-first: poke mods to find potential type resultion problems.");
            checkBoxPhased.SetTooltip("breadth-frist: load mods in phases to avoid potential type resultion problems.");

        }

        void LoadSettings() {
              checkBoxNoAssets.Checked = settings_.NoAssets;
            checkBoxNoMods.Checked = settings_.NoMods;
            checkBoxNoWorkshop.Checked = settings_.NoWorkshop;
            
            checkBoxLHT.Checked = settings_.LHT;

            switch (settings_.AutoLoad) {
                case 0:
                    radioButtonMainMenu.Checked = true;
                    break;
                case 1:
                    radioButtonAssetEditor.Checked = true;
                    break;
                case 2:
                    radioButtonLoadSave.Checked = true;
                    break;
                case 3:
                    radioButtonNewGame.Checked = true;
                    break;
                default:
                    radioButtonMainMenu.Checked = true;
                    Log.Error("Unexpected settings_.AutoLoad=" + settings_.AutoLoad);
                    break;
            }

            textBoxSavePath.Text = settings_.SavedGamePath;
            textBoxMapPath.Text = settings_.MapPath;

            checkBoxPhased.Checked = settings_.Phased;
            checkBoxPoke.Checked = settings_.Poke;
        }

        void SaveSettings() {
            settings_.NoAssets = checkBoxNoAssets.Checked ;
            settings_.NoMods = checkBoxNoMods.Checked;
            settings_.NoWorkshop = checkBoxNoWorkshop.Checked;

            settings_.LHT = checkBoxLHT.Checked;

            if (radioButtonMainMenu.Checked)
                settings_.AutoLoad = 0;
            else if (radioButtonAssetEditor.Checked)
                settings_.AutoLoad = 1;
            else if (radioButtonLoadSave.Checked)
                settings_.AutoLoad = 2;
            else if (radioButtonNewGame.Checked)
                settings_.AutoLoad = 3;
            else
                settings_.AutoLoad = 0;

            settings_.SavedGamePath = textBoxSavePath.Text;
            settings_.MapPath = textBoxMapPath.Text;

            settings_.Phased = checkBoxPhased.Checked;
            settings_.Poke = checkBoxPoke.Checked;

            settings_.Serialize();
        }

        private void Update(object sender, EventArgs e) {
            UpdateCommand();
            SaveSettings();
        }

        private void UpdateCommand() {
            labelCommand.Text = "Cities.exe " + string.Join(" ", GetCommandArgs());
        }

        private string[] GetCommandArgs() {
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
                ofd.Filter = "crp file (*.crp)|*.crp";
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

        private void buttonLaunch_Click(object sender, EventArgs e) {
            Launch();
        }

        private void Launch() {
            var args = GetCommandArgs();
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
