namespace LoadOrderIPatch.Patches {
    extern alias Injections;
    using Inject = Injections.LoadOrderInjections.Injections;
    using SteamUtilities = Injections.LoadOrderInjections.SteamUtilities;

    using System;
    using Mono.Cecil;
    using Patch.API;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.IO;
    using System.Linq;

    public class Entry : IPatch {
        public static ILogger Logger { get; private set; }
        public static IPaths GamePaths { get; private set; }

        public int PatchOrderAsc => 0;
        public AssemblyToPatch PatchTarget => null;

        public AssemblyDefinition Execute(
            AssemblyDefinition assemblyDefinition,
            ILogger logger,
            string patcherWorkingPath,
            IPaths gamePaths) {
            Logger = logger;
            GamePaths = gamePaths;
            Log.Init();

            var args = Environment.GetCommandLineArgs();
            Log.Info("comamnd line args are: " + string.Join(" ", args));
            
            if (IsDebugMono())
                Log.Warning("Debug mono is slow! use Load order tool to change it.");


            return assemblyDefinition;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static bool IsDebugMono() {
            try {
                string file = new StackFrame(true).GetFileName();
                return file?.EndsWith(".cs") ?? false;
            } catch (Exception ex) {
                Logger.Error(ex.ToString());
                return false;
            }
        }

        static void CacheWSFilesImpl() {
            try {
                Log.Called();
                var timer = Stopwatch.StartNew();
                var wsPath = GamePaths.WorkshopModsPath;
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
                        if (File.Exists(path) && !Inject.Packages.IsPathExcluded(path)) {
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
        public static void CacheWSFiles() {
            Log.Called();
            new Thread(CacheWSFilesImpl).Start();
        }

    }
}
