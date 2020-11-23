using HarmonyLib;
using KianCommons;
using System.Diagnostics;

namespace LoadOrderMod.Patches {
    //[HarmonyPatch(typeof(LoadingManager))]
    //[HarmonyPatch("SetSceneProgress")]
    public static class SetSceneProgressPatch {
        public static void Prefix(float progress) {
            Log.Info($"LoadingManager.SetSceneProgress(progress={progress}) Called ", true);
        }
    }
}
