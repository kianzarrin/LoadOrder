using HarmonyLib;
using KianCommons;
using System.Diagnostics;

namespace LoadOrderMod.Patches {
    [HarmonyPatch(typeof(LoadingManager))]
    [HarmonyPatch("PreLoadLevel")]
    public static class PreLoadLevelPatch {
        static Stopwatch sw_total = new Stopwatch();

        public static void Prefix() {
            Log.Info("LoadingManager.PreLoadLevel() started", true);
            sw_total.Reset();
            sw_total.Start();
        }

        public static void Postfix() {
            sw_total.Stop();
            float secs = sw_total.ElapsedMilliseconds * 0.001f;
            Log.Info($"LoadingManager.PreLoadLevel() finished. total duration = {secs:f3} seconds ", true);
        }
    }
}
