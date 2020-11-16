using KianCommons;
using LoadOrderMod.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using static ColossalFramework.Plugins.PluginManager;
using ColossalFramework.PlatformServices;
using ICities;

namespace LoadOrderMod.Injections.CO {
    public static class Logs {
        public static void PreCreateUserModInstance(PluginInfo p) {
            if (p.GetInstances<IUserMod>().Length > 0)
                return;
            BeforeUsrModCtor(p);
            var temp = p.userModInstance;
            AfterUserModCtor(p);
        }
        public static void BeforeUsrModCtor(PluginInfo p) {
            Log.Info($"adding plugin {p} ...", true);
        }
        public static void AfterUserModCtor(PluginInfo p) {
            string modName = (p.userModInstance as IUserMod)?.Name;
            string dirName = p.name;
            Log.Info($"plugin `{dirName}:{modName}` added sucessfully!", false);
        }

        public static void BeforeEnable(PluginInfo p) {
            string modName = (p.userModInstance as IUserMod)?.Name;
            Log.Info($"enabling plugin `{modName}` ...", true);
        }
        public static void AfterEnable(PluginInfo p) {
            string modName = (p.userModInstance as IUserMod)?.Name;
            Log.Info($"plugin `{modName}` enabled sucessfully", true);
        }
    }
}