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
    using Mono.Cecil;

    public static class LoadingApproach {
        public static Assembly FindDependancySoft(
            AssemblyName name, Dictionary<Assembly, PluginInfo> asms) {
            Log.Info(ThisMethod + ": searching for  " + name, true);

            foreach (Assembly asm in asms.Keys) {
                AssemblyName name2 = asm.GetName();
                if (name.Name == name2.Name)
                {
                    Log.Info($"FindDependancySoft found {asm}");
                    return asm;
                } 
            }

            return null;
        }


        public static void AddAssemblyPrefix(Assembly asm) {
            try {
                if (poke && !breadthFirst) {
                    Log.Info($"Poking {asm} ...", true);

                    var path = GetAssemblyLocation(asm);
                    var cecilTypes = GetAllCecilTypes(path);

                    Log.Info("poking public types individually");
                    foreach (var cecilType in cecilTypes) {
                        if (cecilType.IsPublic || cecilType.IsNestedPublic) {
                            try {
                                asm.GetType(cecilType.FullName, true);
                            } catch (Exception ex) {
                                Log.Exception(ex, "failed to get " + cecilType.FullName, showInPanel:false);
                            }
                        }
                    }
                    
                    Log.Info("calling GetExportedTypes()");
                    asm.GetExportedTypes(); // public only
                    Log.Info("GetExportedTypes sucessfull!");

                    var t = asm.GetImplementationOf(typeof(IUserMod));
                    if(t is not null) { 
                        Log.Info("call static constructor of IUSerMod implementation.");
                        t.CallStaticConstructor();
                        Log.Info("successfully called static constructor of IUserMod.");
                    }

                    Log.Info("poking types individually");
                    foreach (var cecilType in cecilTypes) {
                        try {
                            asm.GetType(cecilType.FullName, true);
                        } catch (Exception ex) {
                            Log.Exception(ex, "failed to get " + cecilType.FullName, showInPanel: false);
                        }
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

        public static IEnumerable<TypeDefinition> GetAllCecilTypes(string path) {
            AssemblyDefinition asm = AssemblyDefinition.ReadAssembly(path);
            foreach(var type in asm.MainModule.Types) {
                yield return type;
                foreach (var nestedType in GetNestedCecilTypesRecursive(type)) {
                    yield return nestedType;
                }
            }
        }

        public static IEnumerable<TypeDefinition> GetNestedCecilTypesRecursive(TypeDefinition type) {
            foreach(var nestedType in type.NestedTypes) {
                yield return nestedType;
                foreach(var nestedType2 in GetNestedCecilTypesRecursive(nestedType)) {
                    yield return nestedType2;
                }
            }
        }

        public static string GetAssemblyLocation(Assembly assembly) {
            var name = assembly.FullName;
            foreach(var pair in m_AssemblyLocations) {
                string roName = pair.Key.FullName;
                string path = pair.Value;
                if (name  == roName) {
                    return pair.Value;
                }
            }
            return null;
        }

        static private Dictionary<Assembly, string> m_AssemblyLocations =
            GetFieldValue(PluginManager.instance, "m_AssemblyLocations") as Dictionary<Assembly, string>;


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
