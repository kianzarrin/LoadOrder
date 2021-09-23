namespace LoadOrderTool.UI {
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using LoadOrderTool.Util;
    using LoadOrderTool.UI;

    public class LoadOrderWindowMenuStrip : MenuStrip {
        public ToolStripMenuItem tsmiFile;
        public ToolStripMenuItem tsmiSave;
        public ToolStripMenuItem tsmiExport;
        public ToolStripMenuItem tsmiImport;
        public ToolStripMenuItem tsmiAutoSave;
        public ToolStripMenuItem tsmiOrder;
        public ToolStripMenuItem tsmiResetOrder;
        public ToolStripMenuItem tsmiHarmonyOrder;
        public ToolStripMenuItem tsmiReverseOrder;
        public ToolStripMenuItem tsmiRandomOrder;
        public ToolStripMenuItem tsmiHelp;
        public ToolStripMenuItem tsmiWiki;
        private ToolStripSeparator toolStripSeparator3;
        public ToolStripMenuItem tsmiAbout;
        public ToolStripMenuItem tsmiOpenLogLocation;
        public ToolStripMenuItem tsmiDiscordSupport;
        public ToolStripMenuItem tsmiTools;
        public ToolStripMenuItem tsmiMassSubscribe;

        public ToolStripMenuItem tsmiSync;
        public ToolStripMenuItem tsmiReloadUGC; // reload UGCs from drive.
        public ToolStripMenuItem tsmiResetCache; // reset CS cache + Steam Cache
        public ToolStripMenuItem tsmiResetAllSettings; // rese config + CS cache + steam cache
        public ToolStripMenuItem tsmiReloadSettings; // reload config + CS cache
        public ToolStripMenuItem tsmiUpdateSteamCache; // update Steam Cache


        public LoadOrderWindowMenuStrip() {
            tsmiFile = new ToolStripMenuItem();
            tsmiReloadUGC = new ToolStripMenuItem();
            tsmiSave = new ToolStripMenuItem();
            tsmiAutoSave = new ToolStripMenuItem();
            tsmiExport = new ToolStripMenuItem();
            tsmiImport = new ToolStripMenuItem();
            tsmiOrder = new ToolStripMenuItem();// no need to move if ensuring move the directory for us.
            tsmiResetOrder = new ToolStripMenuItem();
            tsmiHarmonyOrder = new ToolStripMenuItem();
            tsmiReverseOrder = new ToolStripMenuItem();
            tsmiRandomOrder = new ToolStripMenuItem();
            tsmiHelp = new ToolStripMenuItem();
            tsmiWiki = new ToolStripMenuItem();
            tsmiDiscordSupport = new ToolStripMenuItem();
            tsmiOpenLogLocation = new ToolStripMenuItem();
            toolStripSeparator3 = new ToolStripSeparator();
            tsmiAbout = new ToolStripMenuItem();
            tsmiTools = new ToolStripMenuItem();
            tsmiMassSubscribe = new ToolStripMenuItem();

            tsmiSync = new ToolStripMenuItem();
            tsmiResetAllSettings = new ToolStripMenuItem();
            tsmiReloadUGC = new ToolStripMenuItem();
            tsmiReloadSettings = new ToolStripMenuItem();
            tsmiUpdateSteamCache = new ToolStripMenuItem();
            tsmiResetCache = new ToolStripMenuItem();
            tsmiResetAllSettings = new ToolStripMenuItem();

            Items.AddRange(new ToolStripItem[] {
            tsmiFile,
            tsmiSync,
            tsmiOrder,
            tsmiTools,
            tsmiHelp});

            this.ShowItemToolTips = true;

            // 
            // tsmiFile
            // 
            tsmiFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            tsmiSave,
            tsmiAutoSave,
            new ToolStripSeparator(),
            tsmiExport,
            tsmiImport});
            tsmiFile.Name = "tsmiFile";
            tsmiFile.Size = new Size(37, 20);
            tsmiFile.Text = "&File";
            // 
            // tsmiSave
            // 
            tsmiSave.Name = "tsmiSave";
            tsmiSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            tsmiSave.Size = new Size(147, 22);
            tsmiSave.Text = "&Save";
            // 
            // tsmiAutoSave
            // 
            tsmiAutoSave.CheckOnClick = true;
            tsmiAutoSave.Name = "tsmiAutoSave";
            tsmiAutoSave.Size = new Size(147, 22);
            tsmiAutoSave.Text = "&Auto-save";
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
            tsmiRandomOrder});
            tsmiOrder.Name = "tsmiOrder";
            tsmiOrder.Size = new Size(49, 20);
            tsmiOrder.Text = "&Order";
            // 
            // tsmiResetOrder
            // 
            tsmiResetOrder.Name = "tsmiResetOrder";
            tsmiResetOrder.Size = new Size(132, 22);
            tsmiResetOrder.Text = "&ResetOrder";
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
            tsmiReverseOrder.ToolTipText = "revese order";
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
            toolStripSeparator3,
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
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(177, 6);
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
            tsmiMassSubscribe});
            tsmiTools.Name = "tsmiTools";
            tsmiTools.Size = new Size(46, 20);
            tsmiTools.Text = "&Tools";
            // 
            // tsmiMassSubscribe
            // 
            tsmiMassSubscribe.Name = "tsmiMassSubscribe";
            tsmiMassSubscribe.Size = new Size(180, 22);
            tsmiMassSubscribe.Text = "Mass &Subscribe";
            // 
            // tsmiSync
            // 
            tsmiSync.Name = "tsmiSync";
            tsmiSync.Text = "&Sync";
            tsmiSync.DropDownItems.AddRange(new ToolStripItem[] {
                tsmiReloadSettings,
                //new ToolStripSeparator(),
                tsmiReloadUGC,
                //tsmiUpdateSteamCache,
                new ToolStripSeparator(),
                tsmiResetCache,
                tsmiResetAllSettings,
            });
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
            tsmiReloadSettings.Text = "Reload &Settings";
            tsmiReloadSettings.ToolTipText = "Reload settings syncs data that might have been modified inside of game (discarding any unchanged saves).";
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
        
   }
}
