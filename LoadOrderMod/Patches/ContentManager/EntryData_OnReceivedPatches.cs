namespace LoadOrderMod.Patches.ContentManager {
    using ColossalFramework.PlatformServices;
    using HarmonyLib;
    using KianCommons;
    using LoadOrderMod.Settings;
    using LoadOrderMod.Util;
    using System;
    using LoadOrderMod.UI;

    [HarmonyPatch(typeof(EntryData))]
    static class EntryData_OnReceivedPatches {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(EntryData.OnDetailsReceived))]
        static void OnDetailsReceived_Postfix(EntryData __instance, UGCDetails details) {
            if (__instance.publishedFileId == details.publishedFileId) {
#if DEBUG
                Log.Called(details.publishedFileId);
#endif
                __instance.OnAuthorRecieved();
                if (__instance.updated != default) {
                    __instance.asset?.SetDate(__instance.updated);
                    __instance.pluginInfo?.SetDate(__instance.updated);
                }
                // TODO: update sprite
                __instance.UpdateDownloadStatusSprite();
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch("OnNameReceived")]
        static void OnNameReceived_Postfix(EntryData __instance, UserID id) {
            if (__instance.workshopDetails.creatorID != UserID.invalid && id == __instance.workshopDetails.creatorID)
                __instance.OnAuthorRecieved();
        }

        static void OnAuthorRecieved(this EntryData entryData) {
            try {
                string author = entryData.authorName;
                if (author.IsAuthorNameValid()) {
                    entryData.asset?.SetAuthor(author);
                    entryData.pluginInfo?.SetAuthor(author);
                }

            } catch (Exception ex) {
                Log.Exception(ex);
            }
        }
    }
}
