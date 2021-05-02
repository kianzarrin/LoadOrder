namespace LoadOrderTool.UI {
    using System;
    using System.Drawing;
    using System.Reflection;
    using System.Linq;
    using System.Windows.Forms;
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

            return prompt;
        }


    }
}
