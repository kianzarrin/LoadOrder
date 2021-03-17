namespace LoadOrderMod.Patches.LoadLevel {
    using HarmonyLib;
    using ICities;
    using KianCommons;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Reflection.Emit;
    using static KianCommons.Patches.TranspilerUtils;

    [HarmonyPatch(typeof(OptionsMainPanel), "AddUserMods")]
    public static class LogOnSettingsUIPatch {
        static Stopwatch sw = new Stopwatch();
        static Stopwatch sw_total = new Stopwatch();

        static IUserMod BeforeSettingsUI(IUserMod userMod) {
            Log.Info("calling OnSettingsUI() for " + userMod.Name, true);
            sw.Reset();
            sw.Start();
            return userMod;
        }
        static void AfterSettingsUI() {
            sw.Stop();
            var ms = sw.ElapsedMilliseconds;
            Log.Info($"OnSettingsUI() successful. duration = {ms:#,0}ms", true);
        }

        static MethodInfo mBeforeSettingsUI =
            typeof(LogOnSettingsUIPatch).GetMethod(nameof(BeforeSettingsUI), true);
        static MethodInfo mAfterSettingsUI =
            typeof(LogOnSettingsUIPatch).GetMethod(nameof(AfterSettingsUI), true);
        static MethodInfo mInvoke =
            typeof(MethodBase).GetMethod(
                nameof(MethodBase.Invoke),
                new[] { typeof(object), typeof(object[]) },
                throwOnError: true);

        static void Prefix() {
            Log.Info("LoadingWrapper.OnLevelLoaded() started", true);
            Log.Flush();
            sw_total.Reset();
            sw_total.Start();
        }

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
            try {
                List<CodeInstruction> codes = instructions.ToCodeList();
                var Call_BeforeSettingsUI = new CodeInstruction(OpCodes.Call, mBeforeSettingsUI);
                var Call_AfterSettingsUI = new CodeInstruction(OpCodes.Call, mAfterSettingsUI);

                int index = codes.Search(c => c.Calls(mInvoke));
                InsertInstructions(codes, new[] { Call_AfterSettingsUI }, index + 1); // insert after.
                InsertInstructions(codes, new[] { Call_BeforeSettingsUI }, index); // insert before

                return codes;
            } catch(Exception e) {
                Log.Error(e.ToString());
                throw e;
            }
        }

        public static void Postfix() {
            sw_total.Stop();
            var ms = sw_total.ElapsedMilliseconds;
            Log.Info($"LoadingWrapper.OnLevelLoaded() finished. total duration = {ms:#,0}ms ", true);
            Log.Flush();
        }
    }
}
