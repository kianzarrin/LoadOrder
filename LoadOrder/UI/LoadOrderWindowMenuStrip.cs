namespace LoadOrderTool.UI {
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using LoadOrderTool.Util;
    using LoadOrderTool.UI;
    using LoadOrderTool.Data;
    using System.Linq;

    public class LoadOrderWindowMenuStrip : MenuStrip {
        public ToolStripMenuItem tsmiFile;
        public ToolStripMenuItem tsmiSave;
        public ToolStripMenuItem tsmiExport;
        public ToolStripMenuItem tsmiImport;
        public ToolStripMenuItem tsmiAutoSync;
        public ToolStripMenuItem tsmiOrder;
        public ToolStripMenuItem tsmiResetOrder;
        public ToolStripMenuItem tsmiHarmonyOrder;
        public ToolStripMenuItem tsmiReverseOrder;
        public ToolStripMenuItem tsmiWSIDOrder;
        public ToolStripMenuItem tsmiRandomOrder;
        public ToolStripMenuItem tsmiHelp;
        public ToolStripMenuItem tsmiWiki;
        private ToolStripSeparator toolStripSeparator1;
        public ToolStripMenuItem tsmiAbout;
        public ToolStripMenuItem tsmiOpenLogLocation;
        public ToolStripMenuItem tsmiDiscordSupport;
        public ToolStripMenuItem tsmiTools;
        public ToolStripMenuItem tsmiMassSubscribe;
        public ToolStripMenuItem tsmiAdvanced;
        public ToolStripMenuItem tsmiReDownload;

        public ToolStripMenuItem tsmiReloadUGC; // reload UGCs from drive.
        public ToolStripMenuItem tsmiResetCache; // reset CS cache + Steam Cache
        public ToolStripMenuItem tsmiResetAllSettings; // reset config + CS cache + steam cache
        public ToolStripMenuItem tsmiReloadSettings; // reload config + CS cache
        public ToolStripMenuItem tsmiUpdateSteamCache; // update Steam Cache


        public LoadOrderWindowMenuStrip() {
            tsmiFile = new ToolStripMenuItem();
            tsmiReloadUGC = new ToolStripMenuItem();
            tsmiSave = new ToolStripMenuItem();
            tsmiAutoSync = new ToolStripMenuItem();
            tsmiExport = new ToolStripMenuItem();
            tsmiImport = new ToolStripMenuItem();
            tsmiOrder = new ToolStripMenuItem();// no need to move if ensuring move the directory for us.
            tsmiResetOrder = new ToolStripMenuItem();
            tsmiHarmonyOrder = new ToolStripMenuItem();
            tsmiReverseOrder = new ToolStripMenuItem();
            tsmiWSIDOrder = new ToolStripMenuItem();
            tsmiRandomOrder = new ToolStripMenuItem();
            tsmiHelp = new ToolStripMenuItem();
            tsmiWiki = new ToolStripMenuItem();
            tsmiDiscordSupport = new ToolStripMenuItem();
            tsmiOpenLogLocation = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            tsmiAbout = new ToolStripMenuItem();
            tsmiTools = new ToolStripMenuItem();
            tsmiMassSubscribe = new ToolStripMenuItem();
            tsmiAdvanced = new ToolStripMenuItem();
            tsmiReDownload = new ToolStripMenuItem();

            tsmiResetAllSettings = new ToolStripMenuItem();
            tsmiReloadUGC = new ToolStripMenuItem();
            tsmiReloadSettings = new ToolStripMenuItem();
            tsmiUpdateSteamCache = new ToolStripMenuItem();
            tsmiResetCache = new ToolStripMenuItem();
            tsmiResetAllSettings = new ToolStripMenuItem();

            Items.AddRange(new ToolStripItem[] {
            tsmiFile,
            tsmiOrder,
            tsmiTools,
            tsmiHelp});

            this.ShowItemToolTips = true;

            // 
            // tsmiFile
            // 
            tsmiFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                tsmiReloadSettings,
                tsmiSave,
                tsmiAutoSync,
                new ToolStripSeparator(),
                tsmiReloadUGC,
                toolStripSeparator1,
                tsmiResetCache,
                tsmiResetAllSettings,
                new ToolStripSeparator(),
                tsmiExport,
                tsmiImport,
            });
            tsmiFile.Name = "tsmiFile";
            tsmiFile.Size = new Size(37, 20);
            tsmiFile.Text = "&File";
            // 
            // tsmiSave
            // 
            tsmiSave.Name = "tsmiSave";
            tsmiSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            tsmiSave.Size = new Size(147, 22);
            tsmiSave.Text = "&Save settings";
            tsmiSave.ToolTipText = "Save settings to game";
            tsmiSave.Image = ResourceUtil.GetImage("arrow-right.png");
            // 
            // tsmiAutoSync
            // 
            tsmiAutoSync.CheckOnClick = true;
            tsmiAutoSync.Name = "tsmiAutoSync";
            tsmiAutoSync.Size = new Size(147, 22);
            tsmiAutoSync.Text = "&Auto-sync";
            // 
            // tsmiExport
            // 
            tsmiExport.Name = "tsmiExport";
            tsmiExport.Size = new Size(147, 22);
            tsmiExport.Text = "&Export ...";
            // 
            // tsmiImport
            // 
            tsmiImport.Name = "tsmiImport";
            tsmiImport.Size = new Size(147, 22);
            tsmiImport.Text = "&Import ...";
            // 
            // tsmiOrder
            // 
            tsmiOrder.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            tsmiResetOrder,
            tsmiHarmonyOrder,
            tsmiReverseOrder,
            tsmiWSIDOrder,
            tsmiRandomOrder});
            tsmiOrder.Name = "tsmiOrder";
            tsmiOrder.Size = new Size(49, 20);
            tsmiOrder.Text = "&Order";
            // 
            // tsmiResetOrder
            // 
            tsmiResetOrder.Name = "tsmiResetOrder";
            tsmiResetOrder.Size = new Size(132, 22);
            tsmiResetOrder.Text = "&ResetOrder (recommended)";
            // 
            // tsmiHarmonyOrder
            // 
            tsmiHarmonyOrder.Name = "tsmiHarmonyOrder";
            tsmiHarmonyOrder.Size = new Size(132, 22);
            tsmiHarmonyOrder.Text = "&Harmony";
            tsmiHarmonyOrder.ToolTipText = "sort by harmony";
            // 
            // tsmiReverseOrder
            // 
            tsmiReverseOrder.Name = "tsmiReverseOrder";
            tsmiReverseOrder.Size = new Size(132, 22);
            tsmiReverseOrder.Text = "Re&verse";
            tsmiReverseOrder.ToolTipText = "reverse order";
            // 
            // tsmiWSIDOrder
            // 
            tsmiWSIDOrder.Name = "tsmiWSIDOrder";
            tsmiWSIDOrder.Size = new Size(132, 22);
            tsmiWSIDOrder.Text = "&ID";
            tsmiWSIDOrder.ToolTipText = "sort by id then by name";
            // 
            // tsmiRandomOrder
            // 
            tsmiRandomOrder.Name = "tsmiRandomOrder";
            tsmiRandomOrder.Size = new Size(132, 22);
            tsmiRandomOrder.Text = "Ra&ndom";
            tsmiRandomOrder.ToolTipText = "Random order";
            // 
            // tsmiHelp
            // 
            tsmiHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            tsmiWiki,
            tsmiDiscordSupport,
            tsmiOpenLogLocation,
            new ToolStripSeparator(),
            tsmiAbout});
            tsmiHelp.Name = "tsmiHelp";
            tsmiHelp.Size = new Size(44, 20);
            tsmiHelp.Text = "&Help";
            // 
            // tsmiWiki
            // 
            tsmiWiki.Name = "tsmiWiki";
            tsmiWiki.Size = new Size(180, 22);
            tsmiWiki.Text = "&Wiki";
            // 
            // tsmiDiscordSupport
            // 
            tsmiDiscordSupport.Name = "tsmiDiscordSupport";
            tsmiDiscordSupport.Size = new Size(180, 22);
            tsmiDiscordSupport.Text = "Discord &Support";
            // 
            // tsmiOpenLogLocation
            // 
            tsmiOpenLogLocation.Name = "tsmiOpenLogLocation";
            tsmiOpenLogLocation.Size = new Size(180, 22);
            tsmiOpenLogLocation.Text = "Open &Log Location";
            // 
            // tsmiAbout
            // 
            tsmiAbout.Name = "tsmiAbout";
            tsmiAbout.Size = new Size(180, 22);
            tsmiAbout.Text = "&About";
            // 
            // tsmiTools
            // 
            tsmiTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                tsmiAdvanced,
                tsmiMassSubscribe,
                tsmiReDownload,
            });
            tsmiTools.Name = "tsmiTools";
            tsmiTools.Size = new Size(46, 20);
            tsmiTools.Text = "&Tools";
            // 
            // tsmiAdvanced
            // 
            tsmiAdvanced.Name = "tsmiAdvanced";
            tsmiAdvanced.Size = new Size(180, 22);
            tsmiAdvanced.Text = "Show &Advanced options";
            tsmiAdvanced.ToolTipText = "Show &Advanced options";
            tsmiAdvanced.CheckOnClick = true;
            // 
            // tsmiMassSubscribe
            // 
            tsmiMassSubscribe.Name = "tsmiMassSubscribe";
            tsmiMassSubscribe.Size = new Size(180, 22);
            tsmiMassSubscribe.Text = "Mass &Subscribe";
            // 
            // tsmiReDownload
            // 
            tsmiReDownload.Name = "tsmiReDownload";
            tsmiReDownload.Text = "&Redownload broken downloads [Experimental]";
            // 
            // tsmiReloadUGC
            //
            tsmiReloadUGC.Name = "tsmiReloadUGC";
            tsmiReloadUGC.Text = "&Reload Mods/Assets";
            tsmiReloadUGC.ToolTipText = "Reload syncs mods/assets that might have been subbed/unsubbed";
            // 
            // tsmiReloadSettings
            //
            tsmiReloadSettings.Name = "tsmiReloadSettings";
            tsmiReloadSettings.Text = "Load &Settings";
            tsmiReloadSettings.ToolTipText = "syncs back data that might have been modified inside of game (discarding any unsaved changes).";
            tsmiReloadSettings.Image = ResourceUtil.GetImage("arrow-left.png");
            // 
            // tsmiUpdateSteamCache
            //
            tsmiUpdateSteamCache.Name = "tsmiUpdateSteamCache";
            tsmiUpdateSteamCache.Text = "Update Steam Data";
            tsmiUpdateSteamCache.Visible = false;
            // 
            // tsmiResetCache
            //
            tsmiResetCache.Name = "tsmiResetCache";
            tsmiResetCache.Text = "Reset Cache";
            tsmiResetCache.ToolTipText = "Reset Cache resets all data (author names) collected from steam or data collected when you launch CS (asset names/tags)";
            // 
            // tsmiResetAllSettings
            // 
            tsmiResetAllSettings.Name = "tsmiResetAllSettings";
            tsmiResetAllSettings.Text = "Reset all Settings";
            tsmiResetAllSettings.ToolTipText = "reset all settings deletes all cache/setting files (not profiles) and starts from scratch";
            //// 
            //// tsmi
            ////
            //tsmi.Name = "tsmi";
            //tsmi.Text = "&";
            //tsmi.ToolTipText = "";
            tsmiWiki.Click += TsmiWiki_Click;
            tsmiDiscordSupport.Click += TsmiDiscordSupport_Click;
            tsmiOpenLogLocation.Click += TsmiOpenLogLocation_Click;
            tsmiAbout.Click += TsmiAbout_Click;
            tsmiMassSubscribe.Click += TsmiMassSubscribe_Click;
            tsmiReDownload.Click += TsmiReDownload_Click;
            tsmiAdvanced.CheckedChanged += TsmiAdvanced_CheckedChanged;

            OnAdvancedChanged();
        }

        public void OnAdvancedChanged() {
            tsmiAdvanced.Checked = ConfigWrapper.instance.Advanced;
            var advancedItems = new ToolStripItem[] {
                tsmiRandomOrder, tsmiHarmonyOrder, tsmiReverseOrder, tsmiWSIDOrder, tsmiReloadSettings, tsmiResetCache, tsmiResetAllSettings, toolStripSeparator1 };
            foreach (var item in advancedItems)
                item.Visible = ConfigWrapper.instance.Advanced;
#if NO_CO_STEAM_API
            tsmiMassSubscribe.Visible = false;
#endif
        }

        private void TsmiAdvanced_CheckedChanged(object sender, EventArgs e) {
            ConfigWrapper.instance.Advanced = tsmiAdvanced.Checked;
        }

        private void TsmiDiscordSupport_Click(object sender, EventArgs e) =>
            ContentUtil.OpenURL("https://discord.gg/tTYS6XnmBb");

        private void TsmiOpenLogLocation_Click(object sender, EventArgs e) =>
            ContentUtil.OpenPath(Log.LogFilePath);

        private void TsmiAbout_Click(object sender, EventArgs e) =>
            new AboutBox().ShowDialog();

        private void TsmiWiki_Click(object sender, EventArgs e) =>
            ContentUtil.OpenURL("https://github.com/kianzarrin/LoadOrder/wiki");
        
        private void TsmiMassSubscribe_Click(object sender, EventArgs e) =>
            new SubscribeDialog().Show();

        private async void TsmiReDownload_Click(object sender, EventArgs e) {
            try {
                var existingIDs = ContentUtil.GetSubscribedItems();
                var steamItems = ConfigWrapper.instance.SteamCache.Items;
                var ids = steamItems
                    .Where(item=>item.Status.IsBroken() && existingIDs.Contains(item.PublishedFileId))
                    .Select(item => item.ID)
                    .Concat(ContentUtil.GetMissingDirItems()) // missing root dir
                    .Distinct()
                    .ToArray();
                SteamUtil.ReDownload(ids);

                var res = MessageBox.Show(
                    "You can monitor download progress in steam client. Wait for steam to finish downloading. Then press OK to refresh everything.\n" +
                    "should this not work the first time please try again", "Wait for download");
                ConfigWrapper.instance.CSCache.MissingDir = new ulong[0];
                LoadOrderWindow.Instance.DownloadWarningLabel.Visible = false;
                foreach(var item in steamItems) {
                    if (item.Status.IsBroken() && item.DTO != null) {
                        item.RefreshIsUpToDate();
                    }
                }
                await LoadOrderWindow.Instance.ReloadAll(); // reload to fix included/excluded, paths, ...
            } catch (Exception ex) { ex.Log(); }
        }
    }
}
