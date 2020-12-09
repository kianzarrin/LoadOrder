using HarmonyLib;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using static KianCommons.Patches.TranspilerUtils;
using static LoadOrderMod.Util.LoadOrderUtil;
using KianCommons;

namespace LoadOrderMod.Patches {
    [HarmonyPatch]
    public static class LoadLevelCompletePatch {
        static MethodBase TargetMethod() {
            return GetCoroutineMoveNext(typeof(LoadingManager), "LoadLevelComplete");
        }
        public static void Postfix() {
            LoadingManager.instance.QueueLoadingAction(OnLoadingFinished());
        }
        private static IEnumerator OnLoadingFinished() {
            Log.Info("LoadOrderMod:GAME LOADING HAS FINISHED", true);
            yield return 0;
        }
    }
}
