namespace LoadOrderIPatch {
    extern alias Injections;
    using LoadOrderIPatch.Patches;
    using Mono.Cecil;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading;

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

        static void CacheWSFilesImpl2() {
            try {
                Log.Called();
                var timer = Stopwatch.StartNew();
                var wsPath = Entry.GamePaths.WorkshopModsPath;
                var files1 = Directory.GetFiles(wsPath, "*.dll", searchOption: SearchOption.AllDirectories)
                    .Where(path => !path.Contains(Path.PathSeparator + "_"));
                var files2 = Directory.GetFiles(wsPath, "*.crp", searchOption: SearchOption.AllDirectories)
                    .Where(path => !Packages.IsPathExcluded(path));
                var files = files1.Concat(files2).ToArray();

                var chunks = files.Chunk(files.Length / 100).ToArray();
                List<Thread> threads = new List<Thread>(chunks.Length);
                foreach(string [] chunk in chunks) {
                    Thread thread = new Thread(CacheFilesThread);
                    threads.Add(thread);
                    thread.Start(chunk);
                }

                foreach(var thread in threads) {
                    thread.Join();
                }

                Log.Info($"caching access to {files.Length} files took {timer.ElapsedMilliseconds}ms");
            } catch (Exception ex) {
                Log.Exception(ex);
            }

            static void CacheFilesThread(object arg) {
                try {
                    var files = arg as string[];
                    foreach (string file in files) {
                        using (var fs = File.OpenRead(file)) { }
                    }
                } catch (Exception ex) {
                    Log.Exception(ex);
                }
            }
        }

        /// <summary>open and close files to cache improve the speed of first time load.</summary>
        /// precondition: all dependant dlls are loaded
        public static void CacheWSFiles() {
            Log.Called();
            new Thread(CacheWSFilesImpl2).Start();
        }

        public static IEnumerable<TValue []> Chunk<TValue>(
                 this IEnumerable<TValue> values,
                 int chunkSize) {
            using (var enumerator = values.GetEnumerator()) {
                while (enumerator.MoveNext()) {
                    yield return GetChunk(enumerator, chunkSize).ToArray();
                }
            }
        }

        private static IEnumerable<T> GetChunk<T>(
                         IEnumerator<T> enumerator,
                         int chunkSize) {
            do {
                yield return enumerator.Current;
            } while (--chunkSize > 0 && enumerator.MoveNext());
        }
    }
}
