using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LoadOrderTool.UI {
    public partial class ProgressWindow : Form {
        public static ProgressWindow Instance;
        public ProgressWindow() {
            InitializeComponent();
            Instance = this;
            this.TopMost = true;
        }


        public void SetProgress(int percent, string message) {
            progressBar1.Value = percent;
            Text = message;
        }

        protected override void OnClosed(EventArgs e) {
            Instance = null;
            base.OnClosed(e);
        }


    }
}
