using HarmonyLib;
using System;
using System.Reflection.Emit;
using System.Collections.Generic;
using KianCommons;
using static KianCommons.Patches.TranspilerUtils;
using ICities;
using System.Reflection;
using System.Diagnostics;
using ColossalFramework.Plugins;
using System.Linq;
using LoadOrderMod.Util;

namespace LoadOrderMod.Patches {
    [HarmonyPatch(typeof(LoadingWrapper))]
    [HarmonyPatch("OnLevelLoaded")]
    public static class LoadingWrapperOnLevelLoadedpatch {
        public delegate void Handler();
        static Stopwatch sw = new Stopwatch();
        static Stopwatch sw_total = new Stopwatch();

        public static ILoadingExtension BeforeOnLevelLoaded(ILoadingExtension loadingExtension) {
            Log.Info($"calling {loadingExtension}.OnLevelLoaded()", copyToGameLog: true);
#if DEBUG
            var asm = loadingExtension.GetType().Assembly;
            var p = PluginManager.instance.GetPluginsInfo().Single(_p => _p.ContainsAssembly(asm));
            Log.Debug($"loadOrder={p.GetLoadOrder()}", true);
#endif

            sw.Reset();
            sw.Start();
            return loadingExtension;
        }
        public static void AfterOnLevelLoaded() {
            sw.Stop();
            float secs = sw.ElapsedMilliseconds * 0.001f;
            Log.Info($"OnLevelLoaded() successful. duration = {secs:f3} seconds", copyToGameLog: true);
        }

        static MethodInfo mBeforeOnLevelLoaded_ = typeof(LoadingWrapperOnLevelLoadedpatch).GetMethod(nameof(BeforeOnLevelLoaded))
            ?? throw new Exception("mBeforeOnLevelLoaded_ is null");
        static MethodInfo mAfterOnLevelLoaded_ = typeof(LoadingWrapperOnLevelLoadedpatch).GetMethod(nameof(AfterOnLevelLoaded))
            ?? throw new Exception("mAfterOnLevelLoaded_ is null");
        static MethodInfo mOnLevelLoaded_ = typeof(ILoadingExtension).GetMethod(nameof(ILoadingExtension.OnLevelLoaded))
            ?? throw new Exception("mAfterOnLevelLoaded_ is null");

        public static void Prefix() {
            Log.Info("LoadingWrapper.OnLevelLoaded() started", true);
            sw_total.Reset();
            sw_total.Start();
        }
        public static IEnumerable<CodeInstruction> Transpiler(ILGenerator il, IEnumerable<CodeInstruction> instructions) {
            try {
                List<CodeInstruction> codes = instructions.ToCodeList();
                var LDArg_Mode = new CodeInstruction(OpCodes.Ldarg_1);
                var CallVirt_OnLevelLoaded = new CodeInstruction(OpCodes.Callvirt, mOnLevelLoaded_); //callvirt instance void [ICities]ICities.ILoadingExtension::OnLevelLoaded(valuetype [ICities]ICities.LoadMode)
                var Call_BeforeOnLevelLoaded = new CodeInstruction(OpCodes.Call, mBeforeOnLevelLoaded_);
                var Call_AfterOnLevelLoaded = new CodeInstruction(OpCodes.Call, mAfterOnLevelLoaded_);

                int index = SearchInstruction(codes, LDArg_Mode, 0);
                InsertInstructions(codes, new[] { Call_BeforeOnLevelLoaded }, index);

                int index2 = SearchInstruction(codes, CallVirt_OnLevelLoaded, index);
                InsertInstructions(codes, new[] { Call_AfterOnLevelLoaded }, index2+1, moveLabels:false); // insert after.

                return codes;
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                throw e;
            }
        }

        public static void Postfix() {
            sw_total.Stop();
            float secs = sw_total.ElapsedMilliseconds * 0.001f;
            Log.Info($"LoadingWrapper.OnLevelLoaded() finished. total duration = {secs:f3} seconds ", true);
        }
    }
}
