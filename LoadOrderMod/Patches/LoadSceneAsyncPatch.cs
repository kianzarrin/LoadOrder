using HarmonyLib;
using KianCommons;
using System.Diagnostics;
using UnityEngine.SceneManagement;

namespace LoadOrderMod.Patches {
    [HarmonyPatch(typeof(SceneManager))]
    [HarmonyPatch("LoadSceneAsync")]
    [HarmonyPatch(new[] { typeof(string), typeof(LoadSceneMode) })]
    public static class LoadSceneAsyncPatch {
        static Stopwatch sw_total = new Stopwatch();

        public static void Prefix(string sceneName) {
            Log.Info($"SceneManager.LoadSceneAsync({sceneName}) started", true);
            sw_total.Reset();
            sw_total.Start();
        }

        public static void Postfix() {
            sw_total.Stop();
            float ms = sw_total.ElapsedMilliseconds;
            Log.Info($"SceneManager.LoadSceneAsync() finished. total duration = {ms:#,0}ms ", true);
        }
    }
}
