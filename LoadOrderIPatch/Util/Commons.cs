namespace LoadOrderIPatch {
    extern alias Injections;
    using Inject = Injections.LoadOrderInjections.Injections;
    using SteamUtilities = Injections.LoadOrderInjections.SteamUtilities;

    using Mono.Cecil;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using ILogger = Patch.API.ILogger;
    using LoadOrderIPatch.Patches;
    using System.Linq;
    using System.Threading;
    using LoadOrderShared;

    internal static class Commons {
        internal const string InjectionsDLL = InjectionsAssemblyName + ".dll";
        internal const string InjectionsAssemblyName = "LoadOrderInjections";

        internal static AssemblyDefinition GetInjectionsAssemblyDefinition(string dir)
            => CecilUtil.ReadAssemblyDefinition(Path.Combine(dir, InjectionsDLL));

        public static Assembly LoadDLL(string dllPath) {
            try {
                Assembly assembly;
                string symPath = dllPath + ".mdb";
                if (File.Exists(symPath)) {
                    Log.Info("\nLoading " + dllPath + "\nSymbols " + symPath);
                    assembly = Assembly.Load(File.ReadAllBytes(dllPath), File.ReadAllBytes(symPath));
                } else {
                    Log.Info("Loading " + dllPath);
                    assembly = Assembly.Load(File.ReadAllBytes(dllPath));
                }
                if (assembly != null) {
                    Log.Info("Assembly " + assembly.FullName + " loaded.\n");
                } else {
                    Log.Info("Assembly at " + dllPath + " failed to load.\n");
                }
                return assembly;
            } catch (Exception ex) {
                Log.Error("Assembly at " + dllPath + " failed to load.\n" + ex.ToString());
                return null;
            }
        }

        static void CacheWSFilesImpl() {
            try {
                Log.Called();
                var timer = Stopwatch.StartNew();
                var wsPath = Entry.GamePaths.WorkshopModsPath;
                var res1 = Directory.GetFiles(wsPath, "*.dll", searchOption: SearchOption.AllDirectories)
                    .AsParallel()
                    .Select(path => {
                        if (File.Exists(path)) {
                            using (var fs = File.OpenRead(path)) { }
                        }
                        return path;
                    });
                var res2 = Directory.GetFiles(wsPath, "*.crp", searchOption: SearchOption.AllDirectories)
                    .AsParallel()
                    .Select(path => {
                        if (File.Exists(path) && !Packages.IsPathExcluded(path)) {
                            using (var fs = File.OpenRead(path)) { }
                        }
                        return path;
                    });
                var res = res1.Concat(res2).ToList();
                Log.Info($"caching access to {res.Count} files took {timer.ElapsedMilliseconds}ms");
            } catch (Exception ex) {
                Log.Exception(ex);
            }
        }

        /// <summary>open and close files to cache improve the speed of first time load.</summary>
        /// precondition: all dependant dlls are loaded
        public static void CacheWSFiles() {
            Log.Called();
            new Thread(CacheWSFilesImpl).Start();
        }
    }
}
