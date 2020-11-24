using HarmonyLib;
using ICities;
using KianCommons;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using static KianCommons.Patches.TranspilerUtils;
namespace LoadOrderMod.Patches._AssetDataWrapper {
    [HarmonyPatch(typeof(AssetDataWrapper))]
    [HarmonyPatch(nameof(AssetDataWrapper.OnAssetLoaded))]
    public static class OnAssetLoadedPatch {
        static Stopwatch sw = new Stopwatch();
        static Stopwatch sw_total = new Stopwatch();

        public static IAssetDataExtension BeforeOnAssetLoaded(IAssetDataExtension extension) {
            Log.Info($"calling {extension}.OnAssetLoaded()", copyToGameLog: true);
            sw.Reset();
            sw.Start();
            return extension;
        }
        public static void AfterOnAssetLoaded() {
            sw.Stop();
            var ms = sw.ElapsedMilliseconds;
            Log.Info($"OnAssetLoaded() successful! duration = {ms:#,0}ms", copyToGameLog: true);
        }

        static MethodInfo mBeforeOnAssetLoaded_ =
            GetMethod(typeof(OnAssetLoadedPatch), nameof(BeforeOnAssetLoaded));
        static MethodInfo mAfterOnAssetLoaded_ =
            GetMethod(typeof(OnAssetLoadedPatch), nameof(AfterOnAssetLoaded));
        static MethodInfo mOnAssetLoaded_ =
            GetMethod(typeof(IAssetDataExtension), nameof(IAssetDataExtension.OnAssetLoaded));


        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
            foreach (var code in instructions) {
                if (code.Calls(mOnAssetLoaded_)) {
                    yield return new CodeInstruction(OpCodes.Call, mBeforeOnAssetLoaded_);
                    yield return code;
                    yield return new CodeInstruction(OpCodes.Call, mAfterOnAssetLoaded_);
                } else {
                    yield return code;
                }
            }
        }
        public static void Prefix() {
            Log.Info("AssetDataWrapper.OnAssetLoaded() started", true);
            sw_total.Reset();
            sw_total.Start();
        }
        public static void Postfix() {
            sw_total.Stop();
            var ms = sw_total.ElapsedMilliseconds;
            Log.Info($"AssetDataWrapper.OnAssetLoaded() finished. total duration = {ms:#,0}ms", true);
        }
    }
}
