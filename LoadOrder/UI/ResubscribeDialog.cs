namespace LoadOrderTool.UI {
    using CO.IO;
    using CO.PlatformServices;
    using LoadOrderShared;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;
    using Util;
    using LoadOrderTool.Data;

    public partial class ResubscribeDialog : Form {
        public ResubscribeDialog() {
            InitializeComponent();
            try {
                tbIDs.Text = ManagerList.GetBrokenDownloads().Join(" ");
                btnSubscribeAll.Enabled = btnFinish.Enabled = false;
                pictureBox1.BackgroundImage = ResourceUtil.GetImage("next-arrow.png");
                pictureBox2.BackgroundImage = ResourceUtil.GetImage("next-arrow.png");
            } catch (Exception ex) { ex.Log(); }
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);

        }

        private void btnUnsubscribeAll_Click(object sender, EventArgs e) {
            try {
                CleanupTextBox();
                var ids = SubscribeDialog.GetIDs(tbIDs.Text);
                tbIDs.Enabled = false;
                UGCListTransfer.SendList(UGCListTransfer.ToNumber(ids), DataLocation.LocalLOMData, cbMissing.Checked);
                //ContentUtil.Execute(DataLocation.SteamPath, DataLocation.SteamExe, $"-applaunch 255710 -unsubscribe");
                btnSubscribeAll.Enabled = true;
                pictureBox1.BackColor = Color.Transparent;
            } catch (Exception ex) { ex.Log(); }
        }

        void CleanupTextBox() => tbIDs.Text = SubscribeDialog.GetIDs(tbIDs.Text).Join(" ");

        private void btnSubscribeAll_Click(object sender, EventArgs e) {
            CleanupTextBox();
            var ids = SubscribeDialog.GetIDs(tbIDs.Text);
            UGCListTransfer.SendList(UGCListTransfer.ToNumber(ids), DataLocation.LocalLOMData, cbMissing.Checked);
            //ContentUtil.Execute(DataLocation.SteamPath, DataLocation.SteamExe, $"-applaunch 255710 -subscribe");
            MessageBox.Show("wait for steam to download assets", "wait");
            ContentUtil.Execute(DataLocation.SteamPath, DataLocation.SteamExe, $"steam://open/downloads").WaitForExit();
            btnFinish.Enabled = true;
            pictureBox2.BackColor = Color.Transparent;
        }

        private void btnFinish_Click(object sender, EventArgs e) {
            try {
                ReloadAll();
            } catch (Exception ex) { ex.Log(); }
        }
        private async void ReloadAll() {
            try {
                await LoadOrderWindow.Instance.ReloadAll();
            } catch (Exception ex) { ex.Log(); }
        }
    }
}
