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
            cbSkip.SetTooltip("Turn on for LSM to use skip file");
            tbSkipPath.SetTooltip("Path to skip file");
            bSkipPath.SetTooltip("Choose Skip file");

            LSMManager.instance.Load();
            Populate();

            bSkipPath.Click += BSkipPath_Click;
            tbSkipPath.TextChanged += TbSkipPath_TextChanged;
            cbSkip.CheckedChanged += RbSkip_CheckedChanged;

            cbLoadEnabled.CheckedChanged += CbLoadEnabled_CheckedChanged;
            cbLoadUsed.CheckedChanged += CbLoadUsed_CheckedChanged;
        }



        private void CbLoadEnabled_CheckedChanged(object sender, EventArgs e) {
            LSMManager.instance.LoadEnabled = cbLoadEnabled.Checked;
        }
        private void CbLoadUsed_CheckedChanged(object sender, EventArgs e) {
            LSMManager.instance.LoadUsed = cbLoadUsed.Checked;
        }

        public void Populate() {
            if (UIUtil.DesignMode) return;
            tbSkipPath.Text = LSMManager.instance.SkipPath ?? ConfigWrapper.LSMConfig.skipFile;
            cbSkip.Checked = LSMManager.instance.SkipPrefabs; // off if file does not exist or LSM toggle is off.
            cbLoadEnabled.Checked = LSMManager.instance.LoadEnabled;
            cbLoadUsed.Checked = LSMManager.instance.LoadUsed;
        }

        private void RbSkip_CheckedChanged(object sender, EventArgs e) {
            if (cbSkip.Checked) {
                LSMManager.instance.SkipPath = tbSkipPath.Text;
                cbSkip.Checked = LSMManager.instance.SkipPrefabs; // turn back off if file does not exist.
            } else {
                LSMManager.instance.SkipPath = null;
            }
        }

        private void TbSkipPath_TextChanged(object sender, EventArgs e) {
            LSMManager.instance.SkipPath = tbSkipPath.Text;
            cbSkip.Checked = LSMManager.instance.SkipPrefabs;
        }

        private void BSkipPath_Click(object sender, EventArgs e) {
            using (var ofd = new OpenFileDialog()) {
                ofd.Multiselect = false;
                ofd.CheckPathExists = true;
                ofd.AddExtension = true;
                ofd.Title = "OpenSkipFile";

                string skipPath = Path.Combine(DataLocation.localApplicationData, "SkippedPrefabs");
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
