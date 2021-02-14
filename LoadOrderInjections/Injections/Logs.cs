using ICities;
using KianCommons;
using LoadOrderInjections.Util;
using System;
using System.Reflection;
using static ColossalFramework.Plugins.PluginManager;
using static KianCommons.ReflectionHelpers;

namespace LoadOrderInjections.Injections {
    public static class Logs {
        public static void PreCreateUserModInstance(PluginInfo p) {
            var m_UserModInstance = GetFieldValue(p, "m_UserModInstance");
            if (m_UserModInstance != null)
                return; // too late. already instanciated.

            BeforeUserModCtor(p);
            if (p.userModInstance == null)
                return; // failed to instanciate.
            AfterUserModCtor(p);
        }
        public static void BeforeUserModCtor(PluginInfo p) {
            Log.Info($"adding plugin {p} loadOrder={p.GetLoadOrder()} ...", true);
        }
        public static void AfterUserModCtor(PluginInfo p) {
            string modName = (p.userModInstance as IUserMod)?.Name;
            string dirName = p.name;
            Log.Info($"plugin `{dirName}:{modName}` added sucessfully!", true);
        }

        public static void BeforeEnable(PluginInfo p) {
            string modName = (p.userModInstance as IUserMod)?.Name;
            Log.Info($"enabling plugin `{modName}` loadOrder={p.GetLoadOrder()} ...", true);
        }
        public static void AfterEnable(PluginInfo p) {
            string modName = (p.userModInstance as IUserMod)?.Name;
            Log.Info($"plugin `{modName}` enabled sucessfully", true);
        }

        public static Assembly ResolverLog(object sender, ResolveEventArgs args) {
            Log.Info($"Resolver called: sender={sender} name={args.Name}" + Environment.StackTrace);
            return null;
        }
    }
}