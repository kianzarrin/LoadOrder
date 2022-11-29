using ICities;
using KianCommons;
using LoadOrderInjections.Util;
using System;
using System.Linq;
using System.Reflection;
using static ColossalFramework.Plugins.PluginManager;
using static KianCommons.ReflectionHelpers;


namespace LoadOrderInjections.Injections {
    public static class Logs {
        public static void BeforeUserModCtor(PluginInfo p) {
            Log.Info($"adding(instantiating) plugin {p} loadOrder={p.GetLoadOrder()} ");
        
        }
        public static void BeforeCreateUserModInstanceGetExportedTypes(Assembly assembly) {
            Log.Info(ThisMethod + " called for " + assembly);
        }
        public static void AfterUserModCtor(PluginInfo p) {
            string modName = null;
            if (p.instanceCreated) {
                modName = (p.userModInstance as IUserMod)?.Name;
            }

            string dirName = p.name;
            Log.Info($"plugin `{dirName}:{modName}` added successfully!", true);
        }

        public static void BeforeEnable(PluginInfo p) {
            string modName = null;
            if (p.instanceCreated) {
                modName = (p.userModInstance as IUserMod)?.Name;
            }
            Log.Info($"enabling plugin `{modName}` loadOrder={p.GetLoadOrder()} ...", true);
        }
        public static void AfterEnable(PluginInfo p) {
            string modName = null;
            if (p.instanceCreated) {
                modName = (p.userModInstance as IUserMod)?.Name;
            }
            Log.Info($"plugin `{modName}` enabled successfully", true);
        }


        public static Assembly ResolverLog(object sender, ResolveEventArgs args) {
            Log.Info($"Resolver called: sender={sender} name={args.Name}" + Environment.StackTrace);
            return null;
        }
    }
}