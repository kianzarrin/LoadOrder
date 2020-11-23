using HarmonyLib;
using KianCommons;
using System.Linq;
using System.Reflection;

namespace LoadOrderMod.Patches {
    //[HarmonyPatch]
    public static class LoadingProfiler_NewEventPatch {
        static MethodBase TargetMethod() =>
            typeof(LoadingProfiler.Event).GetConstructors().Single();

        static void Prefix(LoadingProfiler.Type type, string name, long time) {
            Log.Info($"Profiler event: type={type} name={name}");
        }
    }
}
