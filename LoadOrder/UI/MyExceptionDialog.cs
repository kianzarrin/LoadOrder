namespace LoadOrderTool.UI {
    using System;
    using System.IO;
    using System.Reflection;
    using System.Linq;
    using System.Windows.Forms;
    using LoadOrderTool.Util;

    public static class ThreadExceptionDialogUtil {
        public static object GetFieldValue(this ThreadExceptionDialog d, string field) {
            return typeof(ThreadExceptionDialog)
                .GetField(field, BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(d);
        }

        public static Label Label(this ThreadExceptionDialog d) => 
            d.GetFieldValue("message") as Label;

        public static TextBox Details(this ThreadExceptionDialog d) => 
            d.Controls.OfType<TextBox>().First();

        public static Button ContinueButton(this ThreadExceptionDialog d) =>
            d.GetFieldValue("continueButton") as Button;
        public static Button QuitButton(this ThreadExceptionDialog d) =>
            d.GetFieldValue("quitButton") as Button;
        public static Button DetailsButton(this ThreadExceptionDialog d) =>
            d.GetFieldValue("detailsButton") as Button;

        private static int scaledButtonAlignmentWidth(this ThreadExceptionDialog d) =>
            (int)d.GetFieldValue("scaledButtonAlignmentWidth");

        static void MoveRight(Button button, int value) {
            var loc = button.Location;
            loc.X += value;
            button.Location = loc;
        }

        public static ThreadExceptionDialog Create(Exception t, string details) {
            var prompt = new ThreadExceptionDialog(t);
            prompt.Text = t.GetType().Name;
            prompt.Label().Text = t.Message;

            var detailsBox = prompt.Details();
            detailsBox.Text = details;
            detailsBox.Height *= 2;

            int expand = (int)(detailsBox.Width * 0.5f);
            prompt.Width += expand;
            MoveRight(prompt.ContinueButton(), expand);
            MoveRight(prompt.QuitButton(), expand);
            detailsBox.Width += expand;

            var btnLogFile = prompt.AddButton("Log File", 0);
            btnLogFile.Click += BtnLogFile_Click;

            var btnCopy = prompt.AddButton("Copy", 1);
            btnCopy.Click += (_,__)=> Clipboard.SetText(details);

            return prompt;
        }

        public static Button AddButton(this ThreadExceptionDialog d, string text, int index) {
            var bounds = d.DetailsButton().Bounds;
            bounds.X += d.scaledButtonAlignmentWidth() * (index+1);
            var btnLogFile = new Button {
                Text = text,
                FlatStyle = FlatStyle.Standard,
                Visible = true,
            };
            d.Controls.Add(btnLogFile);
            btnLogFile.Bounds = bounds;
            return btnLogFile;
        }

        private static void BtnLogFile_Click(object sender, EventArgs e) =>
            ContentUtil.OpenPath(Log.LogFilePath);
        
    }
}
