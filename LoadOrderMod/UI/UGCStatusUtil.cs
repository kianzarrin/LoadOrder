namespace LoadOrderMod.UI {
    extern alias Injections;
    using KianCommons;
    using System;
    using static Injections.LoadOrderInjections.SteamUtilities.IsUGCUpToDateResult;
    using SteamUtilities = Injections.LoadOrderInjections.SteamUtilities;

    public static class UGCStatusUtil {
        public static void UpdateDownloadStatusSprite(this EntryData entryData) {
            try {
                var ugc = entryData.workshopDetails;
                var res = SteamUtilities.IsUGCUpToDate(ugc, out string reason);
                switch (res) {
                    case OK:
                        break;
                    case NotDownloaded:
                        break;
                    case PartiallyDownloaded:
                        break;
                    case OutOfDate:
                        break;
                    default:
                        Log.Exception(new Exception("unreachable code"));
                        break;
                }
                if (res != OK) {
                    string m = "$subscribed item not installed properly:" +
                        $"{ugc.publishedFileId} {ugc.title}\n" +
                        $"reason={reason}. " +
                        $"try reinstalling the item.";
                    DebugOutputPanel.AddMessage(ColossalFramework.Plugins.PluginManager.MessageType.Warning, m);
                }
            } catch (Exception ex) {
                ex.Log();
            }
        }
    }
}
