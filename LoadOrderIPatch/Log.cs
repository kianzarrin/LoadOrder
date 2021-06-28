namespace LoadOrderIPatch {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mono.Cecil;
    using Patch.API;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using LoadOrderIPatch.Patches;
    using System.IO;

    internal static class Log {
        const string FILENAME = "LoadOrderIPatch.log";
        static string FilePath => Path.Combine(Entry.gamePaths.LogsPath, FILENAME);
        static void Init() {
            if(File.Exists(FilePath))
                File.Delete(FilePath);
        }

        static ILogger logger => Entry.logger;
        static IPaths paths => Entry.gamePaths;

        public static void Info(string text) {
            logger.Info("[LoadOrderIPatch] " + text);
            LogImpl("Info", text);
        }
        public static void Warning(string text) {
            logger.Info("[Warning] [LoadOrderIPatch] " + text);
            LogImpl("Warning", text);
        }

        public static void Error(string text) {
            logger.Error("[LoadOrderIPatch] " + text);
            LogImpl("Error", text + "\n" + Environment.StackTrace);
        }
        public static void Exception(this Exception ex) {
            logger.Error("[Exception] [LoadOrderIPatch] " + ex.Message);
            LogImpl("Exception", ex.ToString() + "\tException logged at:\n" + Environment.StackTrace);
        }

        static void LogImpl(string level, string text) {
            File.AppendAllText(FilePath, $"[{level}] {text}\n");
        }
    }
}
