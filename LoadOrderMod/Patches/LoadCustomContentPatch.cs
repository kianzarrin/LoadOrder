using HarmonyLib;
using KianCommons;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using static KianCommons.Patches.TranspilerUtils;

namespace LoadOrderMod.Patches {
    public static class LoadCustomContentPatch {
        static IEnumerable<MethodBase> TargetMethods() {
            yield return GetMethod(typeof(LoadingManager), "LoadCustomContent");
            var tAssetLoader = Type.GetType("LoadingScreenMod.AssetLoader , LoadingScreenMod", throwOnError: false);
            if (tAssetLoader != null) {
                yield return GetCoroutineMoveNext(tAssetLoader, "LoadCustomContent");
            }
        }
        static Stopwatch sw_total = new Stopwatch();

        public static void Prefix() {
            Log.Info("LoadCustomContent() started", true);
            sw_total.Reset();
            sw_total.Start();
        }

        public static void Postfix() {
            sw_total.Stop();
            float ms = sw_total.ElapsedMilliseconds;
            Log.Info($"LoadCustomContent() finished. total duration = {ms:#,0}ms ", true);
        }
    }
}
