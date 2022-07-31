namespace LoadOrderMod.Patches.ContentManager {
    using ColossalFramework;
    using ColossalFramework.PlatformServices;
    using HarmonyLib;
    using KianCommons;
    using LoadOrderMod.UI.EntryAction;
    using LoadOrderMod.UI.EntryStatus;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [HarmonyPatch(typeof(PackageEntry))]
    static class PackageEntryPatch {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(PackageEntry.SetEntry))]
        static void SetEntry_Postfix(PackageEntry __instance, EntryData data) {
            //Log.Called($"entry: {data.publishedFileId} {data.entryName}");
            EntryStatusPanel.UpdateDownloadStatusSprite(__instance);
            EntryActionPanel.Update(__instance);
            __instance.component.eventMouseUp -= CopyWSItemsToCopyBoard;
            __instance.component.eventMouseUp += CopyWSItemsToCopyBoard;

        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(PackageEntry.Reset))]
        static void Reset_prefix(PackageEntry __instance) {
            //Log.Called(__instance.entryName);
            __instance.component.eventClick -= CopyWSItemsToCopyBoard;
            EntryStatusPanel.RemoveDownloadStatusSprite(__instance);
            EntryActionPanel.Remove(__instance);
        }

        private static void CopyWSItemsToCopyBoard(ColossalFramework.UI.UIComponent component, ColossalFramework.UI.UIMouseEventParameter eventParam) {
            try {
                Log.Called();
                var packageEntry = component.GetComponentInParent<PackageEntry>();
                if (eventParam.buttons == ColossalFramework.UI.UIMouseButton.Right) {
                    if (packageEntry.asset.Instantiate<SaveGameMetaData>() is SaveGameMetaData saveGameMetaData) {
                        HashSet<string> ids = new();
                        foreach (var item in saveGameMetaData.mods) {
                            if (item.modWorkshopID != 0 && item.modWorkshopID != PublishedFileId.invalid.AsUInt64) {
                                ids.Add(item.modWorkshopID.ToString());
                            }
                        }
                        foreach (var item in saveGameMetaData.assets) {
                            if (item.modWorkshopID != 0 && item.modWorkshopID != PublishedFileId.invalid.AsUInt64) {
                                ids.Add(item.modWorkshopID.ToString());
                            }
                        }
                        var text = string.Join(" ", ids.ToArray());
                        Log.Info("copied to clip board: " + text);
                        Clipboard.text = text;
                    }
                }
            } catch (Exception ex) {
                ex.Log();
            }
        }
    }

}
