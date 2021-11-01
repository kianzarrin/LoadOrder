namespace LoadOrderIPatch.Patches {
    using Mono.Cecil;
    using Patch.API;
    using System;
    public class Entry : IPatch {
        public static ILogger Logger { get; set; }
        public static IPaths GamePaths { get; private set; }
        public static string PatcherWorkingPath { get; private set; }

        public int PatchOrderAsc => 0;
        public AssemblyToPatch PatchTarget => null;

        public AssemblyDefinition Execute(
            AssemblyDefinition assemblyDefinition,
            ILogger logger,
            string patcherWorkingPath,
            IPaths gamePaths) {
            try {
                Logger = logger;
                GamePaths = gamePaths;
                PatcherWorkingPath = patcherWorkingPath;
                Log.Init();
            } catch (Exception ex) {
                Log.Exception(ex);
            }
            return assemblyDefinition;
        }
    }
}
