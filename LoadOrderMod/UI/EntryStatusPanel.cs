namespace LoadOrderMod.UI {
    extern alias Injections;
    using KianCommons;
    using System;
    using static Injections.LoadOrderInjections.SteamUtilities.IsUGCUpToDateResult;
    using SteamUtilities = Injections.LoadOrderInjections.SteamUtilities;
    using UnityEngine;
    using UnityEngine.UI;
    using ColossalFramework;
    using ColossalFramework.UI;
    using KianCommons.UI;

    public static class EntryStatusExtesions {
        public static void UpdateDownloadStatusSprite(this EntryData entryData) =>
            EntryStatusPanel.UpdateDownloadStatusSprite(entryData);
    }

    public class EntryStatusPanel : UIPanel{
        static readonly Vector2 POSITION = new Vector2(1595, 320);
        StatusButton StatusButton => GetComponentInChildren<StatusButton>();
        public override void Awake() {
            base.Awake();
            anchor = UIAnchorStyle.Top | UIAnchorStyle.Right;
            autoLayoutStart = LayoutStart.TopRight;
            autoLayout = true;
            autoLayoutPadding = new RectOffset(3, 3, 3, 3);
            autoLayoutDirection = LayoutDirection.Horizontal;
            relativePosition = POSITION;
            AddUIComponent<StatusButton>();
        }

        public static void UpdateDownloadStatusSprite(EntryData entryData) {
            try {
                var ugc = entryData.workshopDetails;
                var status = SteamUtilities.IsUGCUpToDate(ugc, out string reason);
                if (status != OK) {
                    string m = "$subscribed item not installed properly:" +
                        $"{ugc.publishedFileId} {ugc.title}\n" +
                        $"reason={reason}. " +
                        $"try reinstalling the item.";
                    DebugOutputPanel.AddMessage(ColossalFramework.Plugins.PluginManager.MessageType.Warning, m);
                    GetorCreateStatusPanel(entryData).StatusButton.SetStatus(status, reason);
                } else {
                    // optimisation: don't create if not necessary.
                    GetStatusPanel(entryData)?.StatusButton?.SetStatus(status, reason);
                }
                
            } catch (Exception ex) { ex.Log(); }
        }

        public static EntryStatusPanel GetorCreateStatusPanel(EntryData entryData) {
            var packageEntry = entryData.attachedEntry;
            if (!packageEntry) return null;
            return packageEntry.GetComponent<EntryStatusPanel>() ?? Create(packageEntry);
        }

        public static EntryStatusPanel GetStatusPanel(EntryData entryData) {
            var packageEntry = entryData.attachedEntry;
            if (!packageEntry) return null;
            return packageEntry.GetComponent<EntryStatusPanel>();
        }

        static EntryStatusPanel Create(PackageEntry packageEntry) {
            Assertion.Assert(packageEntry, "packageEntry");
            var topPanel = packageEntry.GetComponent<UIPanel>();
            Assertion.Assert(topPanel, "topPanel");
            return topPanel.AddUIComponent<EntryStatusPanel>();
        }

    }
}
