namespace LoadOrderMod.UI {
    extern alias Injections;
    using KianCommons;
    using System;
    using static Injections.LoadOrderInjections.SteamUtilities.IsUGCUpToDateResult;
    using SteamUtilities = Injections.LoadOrderInjections.SteamUtilities;
    using UnityEngine;
    using ColossalFramework.UI;
    using static KianCommons.ReflectionHelpers;
    using HarmonyLib;
    using ColossalFramework.PlatformServices;

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
                var ugc = m_WorkshopDetails(packageEntry);
                var status = SteamUtilities.IsUGCUpToDate(ugc, out string reason);
                if (status == DownloadOK) {
                    Destroy(GetStatusPanel(packageEntry)?.gameObject);
                } else {
                    GetorCreateStatusPanel(packageEntry).StatusButton.SetStatus(status, reason);
                }
                Log.Succeeded();
            } catch (Exception ex) { ex.Log(); }
        }

        public static void RemoveDownloadStatusSprite(PackageEntry packageEntry) {
            try {
                Assertion.NotNull(packageEntry, "packageEntry");
                Destroy(GetStatusPanel(packageEntry)?.gameObject);
                Log.Succeeded();
            } catch (Exception ex) { ex.Log(); }
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
            ret.StatusButton.UGCDetails = m_WorkshopDetails(packageEntry);
            return ret;
        }

        private static AccessTools.FieldRef<PackageEntry, UGCDetails> m_WorkshopDetails =
            AccessTools.FieldRefAccess<PackageEntry, UGCDetails>("m_WorkshopDetails");

    }
}
