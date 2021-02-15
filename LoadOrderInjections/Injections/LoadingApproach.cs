namespace LoadOrderInjections.Injections {
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Reflection;
    using ColossalFramework.Plugins;
    using static ColossalFramework.Plugins.PluginManager;
    using KianCommons;
    using static KianCommons.ReflectionHelpers;
    using System.Runtime.CompilerServices;
    using ICities;
    using static LoadOrderInjections.Util.LoadOrderUtil;
    public static class LoadingApproach {
        public static void AddAssemblyPrefix(Assembly asm) {
            try {
                if (poke && !breadthFirst) {
                    Log.Info($"Poking {asm.Name()} ...", true);
                    Log.Info("calling GetExportedTypes() for " + asm);
                    asm.GetExportedTypes(); // public only
                    Log.Info("GetExportedTypes sucessfull!");

                    var t = asm.GetImplementationOf(typeof(IUserMod));
                    if(t is not null) { 
                        Log.Info("call static constructor of IUSerMod implementation.");
                        t.CallStaticConstructor();
                        Log.Info("successfully called static constructor of IUserMod.");
                    }

                    Log.Info("calling asm.GetTypes()");
                    asm.GetTypes(); // including non-public
                    Log.Info("GetTypes() sucessfull!");
                }
            } catch (Exception ex) {
                Log.Exception(ex);
            }
        }

        public static void AddPluginsPrefix(Dictionary<string, PluginManager.PluginInfo> plugins) {
            if (breadthFirst) {
                Log.Info("Getting Exported Types", true);
                foreach (var plugin in plugins.Values) {
                    try {
                        foreach (var t in plugin.GetExportedTypes()) {
                            if (cameraScriptType.IsAssignableFrom(t)) {
                                InvokeSetter(plugin, nameof(plugin.isCameraScript), true);
                            }
                        }
                    } catch (Exception ex) {
                        Log.Exception(ex, "get exported types failed for " + plugin);
                    }
                }

                Log.Info("activating static constructors of IUSerMod implementations", true);
                foreach (var plugin in plugins.Values) {
                    try {
                        plugin.GetImplementationOf(typeof(IUserMod))?.CallStaticConstructor();
                    } catch (Exception ex) {
                        Log.Exception(ex, "get exported types failed for " + plugin);
                    }
                }

                Log.Info("Instanciating IUserMod Implementations", true);
                foreach (var plugin in plugins.Values) {
                    try {
                        Logs.PreCreateUserModInstance(plugin);
                    } catch (Exception ex) {
                        Log.Exception(ex, "Instanciating IUserMod Implementation failed for " + plugin);
                    }
                }

                if (poke) {
                    Log.Info("calling GetTypes() ...");
                    foreach (var plugin in plugins.Values) {
                        try {
                            plugin.GetAllTypes();
                        } catch (Exception ex) {
                            Log.Exception(ex, "GetType() failed for " + plugin);
                        }
                    }
                    Log.Info("GetTypes() sucessfull!");
                }
            }
        }

        public static IEnumerable<Type> GetExportedTypes(this PluginInfo plugin) {
            foreach(var asm in plugin.GetAssemblies())
                foreach(var type in asm.GetExportedTypes())
                    yield return type;
        }

        public static IEnumerable<Type> GetAllTypes(this PluginInfo plugin) {
            foreach (var asm in plugin.GetAssemblies())
                foreach (var type in asm.GetTypes())
                    yield return type;
        }

        public static Type GetImplementationOf(this Assembly asm, Type type) {
            foreach (var type2 in asm.GetExportedTypes()) {
                if (type2.IsClass && !type2.IsAbstract) {
                    if (type2.GetInterfaces().Contains(type))
                        return type2;
                }
            }
            return null;
        }

        public static Type GetImplementationOf(this PluginInfo plugin, Type type) {
            foreach (Assembly asm in plugin.GetAssemblies()) { 
                var ret = asm.GetImplementationOf(type);
                if (ret is not null)
                    return ret;
            }
            return null;
        }

        public static void CallStaticConstructor(this Type type) =>
            RuntimeHelpers.RunClassConstructor(type.TypeHandle);
        
        public static object Instanciate(this Type type) {
            var c = type.GetConstructor(new Type[] { });
            return c.Invoke(null);
        }

        public static T CreateImplementation<T>(this PluginInfo plugin) {
            return (T)plugin.GetImplementationOf(typeof(T)).Instanciate();
        }
    }
}
