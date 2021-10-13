namespace LoadOrderIPatch {
    using ColossalFramework.IO;
    using LoadOrderShared;
    using System;
    using System.Linq;
    using LoadOrderIPatch.Patches;
    using System.IO;

    public static class ConfigUtil {
        internal static LoadOrderConfig config_;
        public static LoadOrderConfig Config =>
            config_ ??=
                LoadOrderConfig.Deserialize(Entry.LocalLOMData)
                ?? new LoadOrderConfig();
        
        public static bool HasArg(string arg) =>
            Environment.GetCommandLineArgs().Any(_arg => _arg == arg);
        public static bool breadthFirst = HasArg("-phased");
        public static bool poke = HasArg("-poke");
    }
}
