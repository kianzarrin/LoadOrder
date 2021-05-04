﻿namespace LoadOrderTool.UI {
    using System;
    using System.IO;
    using System.Windows.Forms;

    public partial class SelectPathsDialog : Form {
        public SelectPathsDialog() {
            InitializeComponent();
            string path = @"C:\Program Files (x86)\Steam\steamapps\common\Cities_Skylines\Cities_Data\Cities.exe";
            int w = TextRenderer.MeasureText(path + "XXXXXXXX", textBoxGamePath.Font).Width;
            textBoxGamePath.Width = textBoxSteamPath.Width = w;
            textBoxGamePath.TextChanged += TextChanged;
            textBoxSteamPath.TextChanged += TextChanged;

            buttonGamePath.Click += ButtonGamePath_Click;
            buttonSteamPath.Click += ButtonSteamPath_Click;

            OKButton.Click += OKButton_Click;
            ExitButton.Click += ExitButton_Click;

            textBoxSteamPath.Text = GetSteamDir();
            textBoxGamePath.Text = GetCitiesPath(textBoxSteamPath.Text);
        }

        public string GamePath {
            get {
                try {
                    return Path.GetDirectoryName(textBoxGamePath.Text);
                } catch (Exception ex) {
                    Log.Exception(ex, "bad path: " + textBoxGamePath.Text, false);
                    return null;
                }
            }

            set {
                try {
                    textBoxGamePath.Text = Path.Combine(value, "Cities.exe");
                } catch (Exception ex) {
                    Log.Exception(ex, showInPanel: false);
                }
            }
        }

        public string WorkshopContentPath {
            get {
                try {
                    string path = Path.GetDirectoryName(textBoxSteamPath.Text);
                    return Path.Combine(path, "steamapps", "workshop", "content", "255710");
                } catch (Exception ex) {
                    Log.Exception(ex, "bad path: " + textBoxSteamPath.Text, false);
                    return null;
                }
            }

            set {
                try {
                    string path = value;
                    path = Directory.GetParent(path).FullName;
                    path = Directory.GetParent(path).FullName;
                    path = Directory.GetParent(path).FullName;
                    path = Directory.GetParent(path).FullName;
                    textBoxSteamPath.Text = Path.Combine(path, "Steam.exe");
                } catch (Exception ex) {
                    Log.Exception(ex, showInPanel: false);
                }
            }
        }

        static string GetSteamDir() {
            try {
                string path = Environment.CurrentDirectory;
                for (int watchdog = 0; watchdog < 20 && Directory.Exists(path); ++watchdog) {
                    path = Directory.GetParent(path).FullName;
                    string ret = Path.Combine(path, "Steam.exe");
                    if (File.Exists(ret))
                        return ret;
                }
            } catch (Exception ex) {
                ex.Log(false);
            }
            return "";
        }

        static string GetCitiesPath(string steamPath) {
            try {
                if (File.Exists(steamPath)) {
                    string path = Path.GetDirectoryName(steamPath);
                    path = Path.Combine(path, "steamapps", "common", "Cities_Skylines", "Cities.exe");
                    if (File.Exists(path))
                        return path;
                }
            } catch (Exception ex) {
                ex.Log(false);
            }
            return "";
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

        private void ButtonGamePath_Click(object sender, EventArgs e) {
            using (var ofd = new OpenFileDialog()) {
                ofd.Title = "Find Cities.exe";
                ofd.Filter = "Cities.exe|Cities.exe";
                ofd.CheckFileExists = true;
                ofd.Multiselect = false;
                var path = textBoxGamePath.Text;
                if (Directory.Exists(path))
                    ofd.InitialDirectory = path;
                else if (File.Exists(path))
                    ofd.InitialDirectory = Path.GetDirectoryName(path);
                if (ofd.ShowDialog() == DialogResult.OK)
                    textBoxGamePath.Text = ofd.FileName;
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

        static bool IsCitiesPath(string path) =>
            path.ToLower().EndsWith("cities.exe") && File.Exists(path);

        static bool IsSteamPath(string path) =>
            path.ToLower().EndsWith("steam.exe") && File.Exists(path);

        private void TextChanged(object sender, EventArgs e) {
            OKButton.Enabled = IsCitiesPath(textBoxGamePath.Text) && IsSteamPath(textBoxSteamPath.Text);
        }
    }
}