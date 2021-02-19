using Mono.Cecil;
using System.Runtime.CompilerServices;
using ILogger = Patch.API.ILogger;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.IO;
using Patch.API;

namespace LoadOrderIPatch {
    internal static class Commons {
        public static ILogger Logger;
        internal const string InjectionsDLL = InjectionsAssemblyName + ".dll";
        internal const string InjectionsAssemblyName = "LoadOrderInjections";
        internal static AssemblyDefinition GetInjectionsAssemblyDefinition(string dir)
            => CecilUtil.GetAssemblyDefinition(dir, InjectionsDLL);

        public static bool IsDebugMono {
            get {
                string file = new StackFrame(fNeedFileInfo: true).GetFileName();
                Logger.Info($"testing is debug mono: file={file}");
                return !string.IsNullOrEmpty(file);
            }
        }

        public static Assembly LoadDLL(string dllPath) {
            void Log(string _m) => Logger.Info(_m);
            try {
                Assembly assembly;
                string symPath = dllPath + ".mdb";
                if (File.Exists(symPath)) {
                    Log("\nLoading " + dllPath + "\nSymbols " + symPath);
                    assembly = Assembly.Load(File.ReadAllBytes(dllPath), File.ReadAllBytes(symPath));
                } else {
                    Log("Loading " + dllPath);
                    assembly = Assembly.Load(File.ReadAllBytes(dllPath));
                }
                if (assembly != null) {
                    Log("Assembly " + assembly.FullName + " loaded.\n");
                } else {
                    Log("Assembly at " + dllPath + " failed to load.\n");
                }
                return assembly;
            } catch (Exception ex) {
                Logger.Error("Assembly at " + dllPath + " failed to load.\n" + ex.ToString());
                return null;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void LogSucessfull(this ILogger logger)
        {
            string caller = new StackFrame(1).GetMethod().Name;
            logger.Info($"[LoadOrderIPatch] Sucessfully applied {caller}!");
                //+ "\n------------------------------------------------------------------------");
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void LogStartPatching(this ILogger logger)
        {
            string caller = new StackFrame(1).GetMethod().Name;
            logger.Info($"[LoadOrderIPatch] {caller} started ...");
        }

        public static bool HasArg(string arg) =>
            Environment.GetCommandLineArgs().Any(_arg => _arg == arg);
        public static bool breadthFirst = HasArg("-phased");
        public static bool poke = HasArg("-poke");
    }
}
