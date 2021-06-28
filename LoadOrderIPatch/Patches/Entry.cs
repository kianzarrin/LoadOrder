namespace LoadOrderIPatch.Patches {
    using System;
    using Mono.Cecil;
    using Patch.API;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

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

    }
}
