namespace LoadOrderMod.Patches.ContentManager {
    using HarmonyLib;
    using KianCommons;
    using KianCommons.Patches;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Reflection.Emit;
    using static KianCommons.ReflectionHelpers;
    using ColossalFramework.Plugins;
    using ColossalFramework.Packaging;
    using LoadOrderMod.Util;
    using LoadOrderMod.Settings;


    [HarmonyPatch(typeof(EntryData))]
    static class EntryData_OnReceivedPatches {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(EntryData.OnDetailsReceived))]
        static void OnDetailsReceived_Postfix(EntryData __instance) => __instance.OnAuthorRecieved();

        [HarmonyPostfix]
        [HarmonyPatch("OnNameReceived")]
        static void OnNameReceived_Postfix(EntryData __instance) => __instance.OnAuthorRecieved();

        static void OnAuthorRecieved(this EntryData entryData) {
            try {
                string author = entryData.authorName;
                if (author.IsAuthorNameValid()) {
                    entryData.asset?.SetAuthor(author);
                    entryData.pluginInfo?.SetAuthor(author);
                }
            } catch(Exception ex) {
                Log.Exception(ex);
            }
        }
    }
}
