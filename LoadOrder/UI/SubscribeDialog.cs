using LoadOrderTool.Util;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CO.PlatformServices;
using CO.Packaging;
using CO.Plugins;
using System.Collections.Generic;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace LoadOrderTool.UI {
    public partial class SubscribeDialog : Form {
        public SubscribeDialog() {
            InitializeComponent();
            tbIDs.DragEnter += tbAssets_DragEnter;
            tbIDs.DragLeave+= tbAssets_DragLeave;
            tbIDs.DragDrop += tbAssets_DragDrop;
            SubscribeAll.SetTooltip("opens CS in mass sub mode and subscribes to all workshop items listed above.");
            btnUnsubscribeAll.SetTooltip("opens CS in mass unsub mode and unsubscribes from all workshop items listed above.");
            btnIncludeAll.SetTooltip("Include/Enable all mods/assets listed above (ignores items that are not downloaded)");
            btnIncludeOnly.SetTooltip("Include/Enable all mods/assets listed above (ignores items that are not downloaded)\n" +
                "and disables are other mods/assets");
            btnReload.SetTooltip("Include/Enable only all mods/assets listed above (exclude everything else - ignores items that are not downloaded)");
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
                    tbIDs.Text += " " + GetMissingIDSFromLSMReport(filePath).Join(" ");
                }
            }
            CleanupTextBox();
        }

        private void tbAssets_DragEnter(object sender, DragEventArgs e) {
            Log.Called();
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

        private void tbAssets_DragLeave(object _, EventArgs __) {
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

        public static IEnumerable<string> GetMissingIDSFromLSMReport(string filePath) {
            Log.Called(filePath);
            var doc = new HtmlDocument();
            doc.Load(filePath);
            var nodes = doc.DocumentNode.SelectNodes("//a[@data-lomtag='missing']");
            return nodes.SelectMany(node => GetHTMLIDs(node.InnerText));
        }

        private void SubscribeAll_Click(object _, EventArgs __) {
            CleanupTextBox();
            var ids = GetIDs(tbIDs.Text);
            ContentUtil.Subscribe(ids);
        }

        private void btnUnsubscribeAll_Click(object sender, EventArgs e) {
            CleanupTextBox();
            var ids = GetIDs(tbIDs.Text);
            ContentUtil.Subscribe(ids, unsub: true);
        }

        private async void btnReload_Click(object _, EventArgs __) {
            await LoadOrderWindow.Instance.ReloadAll();
        }

        private void btnIncludeOnly_Click(object _, EventArgs __) {
            CleanupTextBox();
            var ids = GetIDs(tbIDs.Text).Select(item => new PublishedFileId(ulong.Parse(item))).ToArray();
            var assets = PackageManager.instance.GetAssets();
            var mods = PluginManager.instance.GetMods();
            foreach (var mod in mods) {
                if (mod.IsWorkshop) {
                    bool include = ids.Contains(mod.PublishedFileId);
                    mod.IsIncludedPending = mod.IsEnabledPending = include;
                }
            }
            foreach (var asset in assets) {
                if (asset.IsWorkshop) {
                    bool include = ids.Contains(asset.PublishedFileId);
                    asset.IsIncludedPending = include;
                }
            }
            LoadOrderWindow.Instance.dataGridAssets.Refresh();
            LoadOrderWindow.Instance.dataGridMods.RefreshModList();
        }

        private void CreateProfile_Click(object sender, EventArgs e) {
            btnIncludeOnly_Click(sender, e);
            LoadOrderWindow.Instance.Export_Click(sender, e);
        }

        private void btnCancel_Click(object _, EventArgs __) => Close();

        void CleanupTextBox() => tbIDs.Text = GetIDs(tbIDs.Text).Join(" ");

        private void btnIncludeAll_Click(object _, EventArgs __) {
            CleanupTextBox();
            var ids = GetIDs(tbIDs.Text);
            var assets = PackageManager.instance.GetAssets();
            var mods = PluginManager.instance.GetMods();
            foreach (var id in ids) {
                var publishedFileId = new PublishedFileId(ulong.Parse(id));
                foreach(var mod in mods) {
                    if(mod.IsWorkshop && mod.PublishedFileId == publishedFileId) {
                        mod.IsIncludedPending = mod.IsEnabledPending = true;
                    }
                }
                foreach (var asset in assets) {
                    if (asset.IsWorkshop && asset.PublishedFileId == publishedFileId) {
                        asset.IsIncludedPending = true;
                    }
                }
            }
            LoadOrderWindow.Instance.dataGridAssets.Refresh();
            LoadOrderWindow.Instance.dataGridMods.RefreshModList();
        }
    }
}
