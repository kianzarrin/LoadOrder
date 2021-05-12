namespace LoadOrderIPatch.Patches {
    using System;
    using Mono.Cecil;
    using Patch.API;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Entry : IPatch {
        public static ILogger logger;
        public static IPaths gamePaths;

        public int PatchOrderAsc => 0;
        public AssemblyToPatch PatchTarget => null;

        public AssemblyDefinition Execute(
            AssemblyDefinition assemblyDefinition,
            ILogger logger,
            string patcherWorkingPath,
            IPaths gamePaths) {
            if (IsDebugMono()) {
                logger.Info("Warning! Debug mono is slow! use Load order tool to change it.");
            }

            return assemblyDefinition;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static bool IsDebugMono() {
            try {
                string file = new StackFrame(true).GetFileName();
                return file?.EndsWith(".cs") ?? false;
            } catch (Exception ex) {
                logger.Error(ex.ToString());
                return false;
            }
        }

    }
}
