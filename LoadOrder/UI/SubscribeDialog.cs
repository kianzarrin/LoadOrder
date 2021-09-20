using LoadOrderTool.Util;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace LoadOrderTool.UI {
    public partial class SubscribeDialog : Form {
        public SubscribeDialog() {
            InitializeComponent();
        }

        private void tbAssets_DragDrop(object sender, DragEventArgs e) {
            tbIDs.BackColor = Color.White;
            if (e.Data.GetDataPresent(DataFormats.Html)) {
                var ids = GetHTMLIDs(e.Data.GetData(DataFormats.Html).ToString());
                tbIDs.Text += " " + ids.Join(" ");
            } else if (e.Data.GetDataPresent(DataFormats.Text)) {
                var ids = GetIDs(e.Data.GetData(DataFormats.Text).ToString());
                tbIDs.Text += " " + ids.Join(" ");
            } else if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                foreach(var filePath in  (e.Data as DataObject).GetFileDropList()) {
                    tbIDs.Text += " " + GetIDsFromFile(filePath).Join(" ");
                }
            }
            CleanupTextBox();
        }

        private void tbAssets_DragEnter(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.Text) ||
                e.Data.GetDataPresent(DataFormats.Html) ||
                e.Data.GetDataPresent(DataFormats.FileDrop)) {
                e.Effect = DragDropEffects.Copy;
                tbIDs.BackColor = Color.LightGreen;
            } else {
                e.Effect = DragDropEffects.None;
                tbIDs.BackColor = Color.Red;
            }

        }

        private void tbAssets_DragLeave(object sender, EventArgs e) {
            tbIDs.BackColor = Color.White;
        }

        public static string[] GetIDs(string text) {
            var chars = text.ToCharArray();
            for (int i = 0; i < chars.Length; ++i) {
                var c = chars[i];
                bool isNum = '0' <= c && c <='9';
                if (!isNum)
                    chars[i] = ' ';
            }
            text = new string(chars);
            var entries = text.Split(" ", options: StringSplitOptions.RemoveEmptyEntries);
            return entries.Distinct().ToArray();
        }

        public static string[] GetHTMLIDs(string html) {
            Log.Called(html);
            // https://steamcommunity.com/sharedfiles/filedetails/\?id=([0-9]+)
            string pattern = "https://steamcommunity.com/sharedfiles/filedetails/\\?id=([0-9]+)";
            MatchCollection mc = Regex.Matches(html, pattern);
            return mc.Select(m => m.Groups[1].Value).ToArray();
        }


        public static string[] GetIDsFromFile(string filePath) {
            Log.Called(filePath);
            string text = File.ReadAllText(filePath);
            int i1 = text.IndexOf("The following custom assets are used in this city");
            int i2 = text.IndexOf("The following enabled assets are currently unnecessary");
            text = text.Substring(i1, i2 - i1); // cout out the desired text;
            return GetHTMLIDs(text);
        }

        private void SubscribeAll_Click(object sender, EventArgs e) {
            CleanupTextBox();
            var ids = GetIDs(tbIDs.Text);
            ContentUtil.Subscribe(ids);
        }

        private void btnCancel_Click(object sender, EventArgs e) => Close();

        void CleanupTextBox() => tbIDs.Text = GetIDs(tbIDs.Text).Join(" ");
        
    }
}
