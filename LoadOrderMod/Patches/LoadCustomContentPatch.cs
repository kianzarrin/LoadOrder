using HarmonyLib;
using KianCommons;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using static KianCommons.Patches.TranspilerUtils;
using static LoadOrderMod.Util.LoadOrderUtil;
namespace LoadOrderMod.Patches {
    [HarmonyPatch]
    public static class LoadCustomContentPatch {
        static IEnumerable<MethodBase> TargetMethods() {
            yield return GetCoroutineMoveNext(typeof(LoadingManager), "LoadCustomContent");
            var asm = GetLSMAssembly();
            if (asm != null) {
                var tAssetLoader = asm.GetType("LoadingScreenMod.AssetLoader", throwOnError: true);
                yield return GetCoroutineMoveNext(tAssetLoader, "LoadCustomContent");
            }
        }
        static Stopwatch sw_total = new Stopwatch();
        static Stopwatch sw = new Stopwatch();
        static int counter = 0;

        public static void Prefix() {
            if (counter == 0) {
                sw_total.Reset();
                sw_total.Start();
                Log.Info($"LoadCustomContent() started ...", true);
                Log.Info($"LoadCustomContent.MoveNext() first loop. counter={counter}", false);
            } else {
                Log.Info($"LoadCustomContent.MoveNext() continues. counter={counter}", false);
            }
            sw.Reset();
            sw.Start();
        }

        public static void Postfix(IEnumerator<object> __instance, bool __result) {
            float ms = sw.ElapsedMilliseconds;
            if (__result == false) {
                float ms_total = sw_total.ElapsedMilliseconds;
                Log.Info($"LoadCustomContent.MoveNext() breaked. duration = {ms:#,0}ms ", false);
                Log.Info($"LoadCustomContent() finished! total duration = {ms_total:#,0}ms", true);
            } else {
                object current = __instance.Current;
                Log.Info($"LoadCustomContent.MoveNext() yielded {current ?? "<null>"}. duration = {ms:#,0}ms ", false);
            }
        }
    }
}
