namespace LoadOrderTool.UI {
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using LoadOrderTool.Util;

    public class LoadOrderWindowMenuStrip : MenuStrip {
        public ToolStripMenuItem tsmiFile;
        public ToolStripMenuItem tsmiReload;
        private ToolStripSeparator toolStripSeparator1;
        public ToolStripMenuItem tsmiSave;
        private ToolStripSeparator toolStripSeparator2;
        public ToolStripMenuItem tsmiExport;
        public ToolStripMenuItem tsmiImport;
        public ToolStripMenuItem tsmiAutoSave;
        public ToolStripMenuItem tsmiResetSettings;
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
        public ToolStripMenuItem tsniTools;
        public ToolStripMenuItem tsmiMassSubscribe;

        public LoadOrderWindowMenuStrip() {
            tsmiFile = new ToolStripMenuItem();
            tsmiResetSettings = new ToolStripMenuItem();
            tsmiReload = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            tsmiSave = new ToolStripMenuItem();
            tsmiAutoSave = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            tsmiExport = new ToolStripMenuItem();
            tsmiImport = new ToolStripMenuItem();
            tsmiOrder = new ToolStripMenuItem();
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
            tsniTools = new ToolStripMenuItem();
            tsmiMassSubscribe = new ToolStripMenuItem();

            Items.AddRange(new ToolStripItem[] {
            tsmiFile,
            tsmiOrder,
            tsniTools,
            tsmiHelp});

            // 
            // tsmiFile
            // 
            tsmiFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            tsmiResetSettings,
            tsmiReload,
            toolStripSeparator1,
            tsmiSave,
            tsmiAutoSave,
            toolStripSeparator2,
            tsmiExport,
            tsmiImport});
            tsmiFile.Name = "tsmiFile";
            tsmiFile.Size = new Size(37, 20);
            tsmiFile.Text = "&File";
            // 
            // tsmiResetSettings
            // 
            tsmiResetSettings.Name = "tsmiResetSettings";
            tsmiResetSettings.Size = new Size(147, 22);
            tsmiResetSettings.Text = "Reset &Settings";
            // 
            // tsmiReload
            // 
            tsmiReload.Name = "tsmiReload";
            tsmiReload.Size = new Size(147, 22);
            tsmiReload.Text = "&Reload Items";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(144, 6);
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
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(144, 6);
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
            // tsniTools
            // 
            tsniTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            tsmiMassSubscribe});
            tsniTools.Name = "tsniTools";
            tsniTools.Size = new Size(46, 20);
            tsniTools.Text = "&Tools";
            // 
            // tsmiMassSubscribe
            // 
            tsmiMassSubscribe.Name = "tsmiMassSubscribe";
            tsmiMassSubscribe.Size = new Size(180, 22);
            tsmiMassSubscribe.Text = "Mass &Subscribe";

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
