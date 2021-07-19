namespace LoadOrderMod.Patches.Startup {
    using HarmonyLib;
    using KianCommons;
    using ColossalFramework;
    using ColossalFramework.PlatformServices;
    using LoadOrderMod.Settings;
    using System.Linq;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    [HarmonyPatch(typeof(SteamHelper), nameof(SteamHelper.IsDLCOwned))]
    public static class IsDLCOwnedPatch {
        static SteamHelper.DLC[] ExcludedDLCs;
        static void Prepair(MethodBase original) {
            if (original != null) return;
            var dlcs = new List<SteamHelper.DLC>();
            foreach(string item in ConfigUtil.Config.ExcludedDLCs) {
                if(item == "MusicDLCs") {
                    dlcs.Add(SteamHelper.DLC.RadioStation1);
                    dlcs.Add(SteamHelper.DLC.RadioStation2);
                    dlcs.Add(SteamHelper.DLC.RadioStation3);
                    dlcs.Add(SteamHelper.DLC.RadioStation4);
                    dlcs.Add(SteamHelper.DLC.RadioStation5);
                    dlcs.Add(SteamHelper.DLC.RadioStation6);
                    dlcs.Add(SteamHelper.DLC.RadioStation7);
                    dlcs.Add(SteamHelper.DLC.RadioStation9);
                    dlcs.Add(SteamHelper.DLC.RadioStation10);
                    dlcs.Add(SteamHelper.DLC.RadioStation11);
                } else {
                    dlcs.Add((SteamHelper.DLC)Enum.Parse(typeof(SteamHelper.DLC), item));
                }
            }
        }


        static void Postfix(SteamHelper.DLC dlc, ref bool __result) {
            if (__result) {
                __result = __result && !ExcludedDLCs.Contains(dlc);
            }
        }
    }
}
