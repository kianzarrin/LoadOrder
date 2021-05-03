namespace LoadOrderTool.UI {
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;

    public partial class LaunchControl : UserControl {
        public LaunchControl() {
            InitializeComponent();
            foreach (var c in this.GetAll<TextBox>())
                c.TextChanged += UpdateCommand;
            
            foreach (var c in this.GetAll<CheckBox>())
                c.CheckedChanged += UpdateCommand;

            foreach (var c in this.GetAll<Button>()) {
                c.MouseHover += UpdateCommand;
                c.Click += UpdateCommand;
            }
        }

        private void flowLayoutPanelTopLevel_SizeChanged(object sender, EventArgs e) =>
            AutoSizeLaunchTable();

        private void flowLayoutPanelTopLevel_VisibleChanged(object sender, EventArgs e) =>
            AutoSizeLaunchTable();

        private void AutoSizeLaunchTable() =>
            tableLayoutPanelLunchMode.Width = flowLayoutPanelTopLevel.Width;

        private void UpdateCommand(object sender, EventArgs e) {
            labelCommand.Text = GetCommand(sender as Button);
        }

        private string GetCommand(Button launchButton) {
            return null;
        }
        
    }
}
