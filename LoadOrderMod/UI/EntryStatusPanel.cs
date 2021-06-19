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
    using static KianCommons.ReflectionHelpers;
    using System.Diagnostics;
    using HarmonyLib;

    public static class EntryStatusExtesions {
        public static void UpdateDownloadStatusSprite(this PackageEntry packageEntry) =>
            EntryStatusPanel.UpdateDownloadStatusSprite(packageEntry);
    }

    public class EntryStatusPanel : UIPanel{
        static readonly Vector2 POSITION = new Vector2(1600, 320);
        StatusButton StatusButton => GetComponentInChildren<StatusButton>();
        public override void Awake() {
            try {
                base.Awake();
                anchor = UIAnchorStyle.Top | UIAnchorStyle.Right;
                autoLayoutStart = LayoutStart.TopRight;
                autoLayout = true;
                autoLayoutPadding = new RectOffset(3, 3, 3, 3);
                autoLayoutDirection = LayoutDirection.Horizontal;
                relativePosition = POSITION - new Vector2(160,0);
                size = new Vector2(160, 80);
                var statusButton = AddUIComponent<StatusButton>();
                LogSucceeded();
            } catch(Exception ex) { ex.Log(); }
        }

        public static void UpdateDownloadStatusSprite(PackageEntry packageEntry) {
            try {
                Assertion.NotNull(packageEntry, "packageEntry");
                var ugc = m_EntryDataRef(packageEntry).workshopDetails;
                var status = SteamUtilities.IsUGCUpToDate(ugc, out string reason);
                if (status != DownloadOK) {
                    string m = "$subscribed item not installed properly:" +
                        $"{ugc.publishedFileId} {ugc.title}\n" +
                        $"reason={reason}. " +
                        $"try reinstalling the item.";
                    DebugOutputPanel.AddMessage(ColossalFramework.Plugins.PluginManager.MessageType.Warning, m);
                }

                SetStatus(packageEntry, status, reason);
            } catch (Exception ex) { ex.Log(); }
        }

        public static void SetStatus(PackageEntry packageEntry, SteamUtilities.IsUGCUpToDateResult status, string reason) {
            if (status == DownloadOK) {
                GetStatusPanel(packageEntry)?.StatusButton?.SetStatus(status, reason);
            } else {
                GetorCreateStatusPanel(packageEntry).StatusButton.SetStatus(status, reason);
            }
            Log.Succeeded();
        }

        public static EntryStatusPanel GetorCreateStatusPanel(PackageEntry packageEntry) {
            Assertion.NotNull(packageEntry);
            return packageEntry.GetComponent<EntryStatusPanel>() ?? Create(packageEntry)
                ?? throw new Exception("failed to create panel");
        }

        public static EntryStatusPanel GetStatusPanel(PackageEntry packageEntry) {
            Assertion.NotNull(packageEntry);
            return packageEntry.GetComponent<EntryStatusPanel>();
        }

        static EntryStatusPanel Create(PackageEntry packageEntry) {
            Assertion.Assert(packageEntry, "packageEntry");
            var topPanel = packageEntry.GetComponent<UIPanel>();
            Assertion.Assert(topPanel, "topPanel");
            var ret = topPanel.AddUIComponent<EntryStatusPanel>();
            ret.StatusButton.EntryData = m_EntryDataRef(packageEntry);
            return ret;
        }

        private static AccessTools.FieldRef<PackageEntry, EntryData> m_EntryDataRef =
            AccessTools.FieldRefAccess<PackageEntry, EntryData>("m_EntryData");

    }
}
