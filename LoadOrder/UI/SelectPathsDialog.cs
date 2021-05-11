namespace LoadOrderTool.UI {
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using static CO.IO.DataLocation.Util;
    using static CO.IO.PathUtils;

    public partial class SelectPathsDialog : Form {
        public SelectPathsDialog() {
            InitializeComponent();
            const string examplePath = @"C:\Program Files (x86)\Steam\steamapps\common\Cities_Skylines\Cities_Data\Cities.exe";
            int w = TextRenderer.MeasureText(examplePath + "XXXXXX", textBoxCitiesPath.Font).Width;
            textBoxCitiesPath.Width = textBoxSteamPath.Width = w;
            textBoxCitiesPath.TextChanged += TextChanged;
            textBoxSteamPath.TextChanged += TextChanged;
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
                return Path.Combine(path, "Steam.exe");
            else
                return Path.Combine(ToSteamPath(path), "Steam.exe");
        }
        public static string ToCitiesExePath(string path) {
            if (IsCitiesExePath(path))
                return path;
            else if (IsGamePath(path))
                return Path.Combine(path, "Cities.exe");
            else
                return Path.Combine(ToGamePath(path), "Cities.exe");
        }

        private void ButtonSteamPath_Click(object sender, EventArgs e) {
            using (var ofd = new OpenFileDialog()) {
                ofd.Title = "Find Steam.exe";
                ofd.Filter = "Steam.exe|Steam.exe";
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
                ofd.Title = "Find Cities.exe";
                ofd.Filter = "Cities.exe|Cities.exe";
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

        private void TextChanged(object sender, EventArgs e) {
            Task.Run(() => TextChanged(sender));
        }

        async Task TextChanged(object sender) {
            if (sender == textBoxCitiesPath && IsCitiesExePath(textBoxCitiesPath.Text)) {
                GetSteamPath();
            } else if (sender == textBoxSteamPath && IsSteamExePath(textBoxSteamPath.Text)) {
                GetCitiesPath();
            }
            OKButton.Enabled = IsCitiesExePath(textBoxCitiesPath.Text) && IsSteamExePath(textBoxSteamPath.Text);
        }

    }
}
