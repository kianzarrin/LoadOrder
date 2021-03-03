using KianCommons;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using static ColossalFramework.Plugins.PluginManager;
using LoadOrderShared;
using ColossalFramework;
using ColossalFramework.IO;
using ColossalFramework.Plugins;
using ColossalFramework.PlatformServices;

namespace LoadOrderInjections.Util {
    internal static class LoadOrderUtil {
        public static LoadOrderConfig Config;

        static LoadOrderUtil() {
            Config = LoadOrderConfig.Deserialize(DataLocation.localApplicationData);
        }

        internal static int GetLoadOrder(this PluginInfo p) {
            var mod = p.GetModConfig();
            if (mod == null)
                return LoadOrderConfig.DefaultLoadOrder;
            return mod.LoadOrder;
        }

        internal static LoadOrderShared.ModInfo GetModConfig(this PluginInfo p) =>
            Config?.Mods?.FirstOrDefault(_mod => _mod.Path == p.modPath);

        public static bool HasArg(string arg) =>
            Environment.GetCommandLineArgs().Any(_arg => _arg == arg);
        public static bool breadthFirst = HasArg("-phased");
        public static bool poke = HasArg("-poke");

        internal const string LoadOrderSettingsFile = "LoadOrder";

        internal static string DllName(this PluginInfo p) => p.userModInstance?.GetType()?.Assembly?.GetName()?.Name;
        internal static bool IsHarmonyMod(this PluginInfo p) => p.name == "2040656402" || p.name == "CitiesHarmony";
        internal static bool IsLSM(this PluginInfo p) => p.name == "667342976" || p.name == "LoadingScreenMod";

        internal static Assembly GetLSMAssembly() =>
            AppDomain.CurrentDomain.GetAssemblies()
            .FirstOrDefault(_asm => _asm.GetName().Name == "LoadingScreenMod");
    }
}
