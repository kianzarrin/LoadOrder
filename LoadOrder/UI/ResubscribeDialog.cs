namespace LoadOrderTool.UI {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;
    using Util;
    public partial class ResubscribeDialog : Form {
        public ResubscribeDialog() {
            InitializeComponent();
            pictureBox1.Image = pictureBox2.Image = ResourceUtil.GetImage("next-arrow.png");
            pictureBox1.Enabled = btnSubscribeAll.Enabled = pictureBox2.Enabled = btnFinish.Enabled = false;
        }
    }
}
