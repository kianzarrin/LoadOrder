namespace LoadOrderTool.UI {
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using static CO.IO.DataLocation.Util;
    using static CO.IO.PathUtils;
    using CO.IO;
    public partial class SelectPathsDialog : Form {
        public SelectPathsDialog() {
            InitializeComponent();
            const string examplePath = @"C:\Program Files (x86)\Steam\steamapps\common\Cities_Skylines\Cities_Data\Cities.exe";
            int w = TextRenderer.MeasureText(examplePath + "XXXXXX", textBoxCitiesPath.Font).Width;
            textBoxCitiesPath.Width = textBoxSteamPath.Width = w;
            textBoxCitiesPath.TextChanged += OnTextChanged;
            textBoxSteamPath.TextChanged += OnTextChanged;
            buttonCitiesPath.Click += ButtonCitiesPath_Click;
            buttonSteamPath.Click += ButtonSteamPath_Click;
            OKButton.Click += OKButton_Click;
            ExitButton.Click += ExitButton_Click;
        }

        public string CitiesPath {
            get => textBoxCitiesPath.Text;

            set {
                try {
                    string path = value;
                    if (!IsCitiesExePath(path)) {
                        path = RealPath(ToCitiesExePath(path));
                    }
                    textBoxCitiesPath.Text = path;
                } catch (Exception ex) {
                    Log.Exception(ex, showInPanel: false);
                }
            }
        }

        public string SteamPath {
            get => textBoxSteamPath.Text;

            set { 
                try {
                    string path = value;
                    if (!IsSteamExePath(path)) {
                        path = RealPath(ToSteamExePath(path));
                    }
                    textBoxSteamPath.Text = path;
                } catch (Exception ex) {
                    Log.Exception(ex, showInPanel: false);
                }
            }
        }

        public void GetCitiesPath() {
            if (!IsCitiesExePath(CitiesPath)) {
                string path = ToCitiesExePath(SteamPath);
                if (IsCitiesExePath(path))
                    CitiesPath = path;
            }
        }

        public void GetSteamPath() {
            if (!IsSteamPath(SteamPath)) {
                string path = ToSteamExePath(CitiesPath);
                if (IsSteamExePath(path))
                    SteamPath = path;
            }
        }

        public static string ToSteamExePath(string path) {
            if (IsSteamExePath(path))
                return path;
            else if (IsSteamPath(path))
                return Path.Combine(path, DataLocation.SteamExe);
            else
                return Path.Combine(ToSteamPath(path), DataLocation.SteamExe);
        }
        public static string ToCitiesExePath(string path) {
            if (IsCitiesExePath(path))
                return path;
            else if (IsGamePath(path))
                return Path.Combine(path, DataLocation.CitiesExe);
            else
                return Path.Combine(ToGamePath(path), DataLocation.CitiesExe);
        }

        private void ButtonSteamPath_Click(object sender, EventArgs e) {
            using (var ofd = new OpenFileDialog()) {
                ofd.Title = $"Find {DataLocation.SteamExe}";
                ofd.Filter = $"{DataLocation.SteamExe}|{DataLocation.SteamExe}";
                ofd.CheckFileExists = true;
                ofd.Multiselect = false;
                var path = textBoxSteamPath.Text;
                if (Directory.Exists(path))
                    ofd.InitialDirectory = path;
                else if (File.Exists(path))
                    ofd.InitialDirectory = Path.GetDirectoryName(path);
                if (ofd.ShowDialog() == DialogResult.OK)
                    textBoxSteamPath.Text = ofd.FileName;
            }
        }

        private void ButtonCitiesPath_Click(object sender, EventArgs e) {
            using (var ofd = new OpenFileDialog()) {
                ofd.Title = "Find " + DataLocation.CitiesExe;
                ofd.Filter = $"{DataLocation.CitiesExe}|{DataLocation.CitiesExe}";
                ofd.CheckFileExists = true;
                ofd.Multiselect = false;
                var path = textBoxCitiesPath.Text;
                if (Directory.Exists(path))
                    ofd.InitialDirectory = path;
                else if (File.Exists(path))
                    ofd.InitialDirectory = Path.GetDirectoryName(path);
                if (ofd.ShowDialog() == DialogResult.OK)
                    textBoxCitiesPath.Text = ofd.FileName;
            }
        }

        private void ExitButton_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void OKButton_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.OK;
            Close();
        }

        private async void OnTextChanged(object sender, EventArgs e) {
            await OnTextChanged(sender);
        }

        async Task OnTextChanged(object sender) {
            if (sender == textBoxCitiesPath && await Task.Run(()=>IsCitiesExePath(textBoxCitiesPath.Text))) {
                await Task.Run(GetSteamPath);
            } else if (sender == textBoxSteamPath && await Task.Run(() => IsSteamExePath(textBoxSteamPath.Text))) {
                await Task.Run(GetCitiesPath);
            }
            OKButton.Enabled = await Task.Run(() => IsCitiesExePath(textBoxCitiesPath.Text) && IsSteamExePath(textBoxSteamPath.Text));
        }

    }
}
