using HarmonyLib;
using KianCommons;
using KianCommons.Patches;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using static KianCommons.ReflectionHelpers;
using System.Linq;


namespace LoadOrderMod.Patches.ContentManager {
    [HarmonyPatch(typeof(ContentManagerPanel))]
    public static class OnEnableDisableAllPatch {
        static MethodInfo mEnabledPackageEvents =
            GetMethod(typeof(ContentManagerPanel), "EnablePackageEvents");
        static MethodInfo mFlush =
            new Action(DelayedEventInvoker.Flush).Method;

        [HarmonyPatch("OnEnableAll")]
        [HarmonyPatch("OnDisableAll")]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
            var codes = instructions.ToCodeList();
            //Log.Debug("before:\n" + codes.IL2STR());

            int index = codes.FindIndex(_c => _c.Calls(mEnabledPackageEvents));

            // 0:replace call ContentManagerPanel.EnablePackageEvents 
            codes.ReplaceInstruction(index, new CodeInstruction(OpCodes.Call, mFlush));

            // -2:remove ldarg0     
            // -1:remove ldc.i4.1
            codes.RemoveRange(index - 2, 2);

            //Log.Debug("after:\n"+codes.IL2STR());
            return codes;
        }
    }
}
