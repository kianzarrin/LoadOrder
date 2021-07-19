using CO.IO;
using LoadOrderTool.Data;
using System;
using System.IO;
using System.Windows.Forms;

namespace LoadOrderTool.UI {
    public partial class LSMControl : UserControl {
        static ConfigWrapper ConfigWrapper => ConfigWrapper.instance;

        public LSMControl() {
            InitializeComponent();
            rbSkip.SetTooltip("Turn on for LSM to use skip file");
            tbSkipPath.SetTooltip("Path to skip file");
            bSkipPath.SetTooltip("Choose Skip file");

            LSMManager.instance.Load();
            LoadValues();

            bSkipPath.Click += BSkipPath_Click;
            tbSkipPath.TextChanged += TbSkipPath_TextChanged;
            rbSkip.CheckedChanged += RbSkip_CheckedChanged;
        }

        public void LoadValues() {
            tbSkipPath.Text = ConfigWrapper.LSMConfig.skipFile;
            rbSkip.Checked = LSMManager.instance.SkipPrefabs; // off if file does not exist or LSM toggle is off.
        }

        private void RbSkip_CheckedChanged(object sender, EventArgs e) {
            if (rbSkip.Checked) {
                LSMManager.instance.SkipPath = tbSkipPath.Text;
                rbSkip.Checked = LSMManager.instance.SkipPrefabs; // turn back off if file does not exist.
            } else {
                LSMManager.instance.SkipPath = null;
            }
        }

        private void TbSkipPath_TextChanged(object sender, EventArgs e) {
            LSMManager.instance.SkipPath = tbSkipPath.Text;
            rbSkip.Checked = LSMManager.instance.SkipPrefabs;
        }

        private void BSkipPath_Click(object sender, EventArgs e) {
            using (var ofd = new OpenFileDialog()) {
                ofd.Multiselect = false;
                ofd.CheckPathExists = true;
                ofd.AddExtension = true;
                ofd.Title = "OpenSkipFile";

                string skipPath = Path.Combine(DataLocation.mapLocation, "SkippedPrefabs");
                ofd.CustomPlaces.Add(DataLocation.localApplicationData);
                if (Directory.Exists(skipPath)) {
                    ofd.CustomPlaces.Add(skipPath);
                    ofd.InitialDirectory = skipPath;
                } else {
                    ofd.InitialDirectory = DataLocation.localApplicationData;
                }

                if (ofd.ShowDialog() == DialogResult.OK) {
                    tbSkipPath.Text = ofd.FileName;
                }
            }
        }
    }
}
