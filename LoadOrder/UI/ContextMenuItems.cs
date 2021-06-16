using CO;
using CO.Packaging;
using CO.Plugins;
using LoadOrderTool.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LoadOrderTool.Data;
using LoadOrderTool.UI;
using CO.PlatformServices;

namespace LoadOrderTool.UI {
    class ContextMenuItems : ContextMenuStrip {
        string path_;
        string url_;

        public ContextMenuItems(string path, PublishedFileId id) {
            path_ = path;
            var itemOpenFile = new ToolStripMenuItem("Open File Location");
            itemOpenFile.Click += ItemOpenFile_Click;
            Items.Add(itemOpenFile);

            if (id != PublishedFileId.invalid) {
                url_ = ContentUtil.GetItemURL(id);
                var itemOpenURL = new ToolStripMenuItem("Open In Workshop");
                itemOpenURL.Click += ItemOpenURL_Click; ;
                Items.Add(itemOpenURL);
            }
        }

        private void ItemOpenURL_Click(object sender, EventArgs e) => ContentUtil.OpenURL(url_);

        private void ItemOpenFile_Click(object sender, EventArgs e) => ContentUtil.OpenPath(path_);
    }
}
