using HarmonyLib;
using KianCommons;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using static KianCommons.Patches.TranspilerUtils;
using static LoadingManager;

namespace LoadOrderMod.Patches {
    [HarmonyPatch]
    public static class SimulationDataReadyPatch {
        static MethodBase TargetMethod() {
            var t = typeof(LoadingManager)
                .GetNestedTypes(ALL)
                .Single(_t => _t.Name.Contains("<LoadLevelCoroutine>"));
            return GetMethod(t, "MoveNext");
        }

        public delegate void Handler();
        static Stopwatch sw = new Stopwatch();
        static Stopwatch sw_total = new Stopwatch();

        public static void SpecialInvoke(SimulationDataReadyHandler e) {
            if (e is null) {
                Log.Exception(new ArgumentNullException("e"));
                return;
            }

            Assertion.AssertNotNull(e);
            Log.Info($"invoking LoadingManager.m_SimulationDataReady()", copyToGameLog: true);
            sw_total.Reset();
            sw_total.Start();

            foreach (SimulationDataReadyHandler @delegate in e.GetInvocationList()) {
                string name = @delegate.Method.FullDescription();
                Log.Info($"invoking " + name, copyToGameLog: true);
                sw.Reset();
                sw.Start();
                try {
                    @delegate();
                    sw.Stop();
                    Log.Info($"{name} successful! " +
                        $"duration = {sw.ElapsedMilliseconds:0,0}ms", copyToGameLog: false);
                } catch (Exception ex) {
                    Log.Exception(ex);
                }
                sw_total.Stop();
                Log.Info($"LoadingManager.m_SimulationDataReady() successful! " +
                    $"total duration = {sw_total.ElapsedMilliseconds:0,0}ms", copyToGameLog: true);
            }
        }

        static MethodInfo mSpecialInvoke = GetMethod(
            typeof(SimulationDataReadyPatch),
            nameof(SpecialInvoke));
        static MethodInfo m_Invoke = GetMethod(
            typeof(SimulationDataReadyHandler),
            nameof(SimulationDataReadyHandler.Invoke));

        public static IEnumerable<CodeInstruction> Transpiler(ILGenerator il, IEnumerable<CodeInstruction> instructions) {
            foreach (var code in instructions) {
                if (code.Calls(m_Invoke))
                    yield return new CodeInstruction(OpCodes.Call, mSpecialInvoke);
                else
                    yield return code;
            }
        }
    }
}
