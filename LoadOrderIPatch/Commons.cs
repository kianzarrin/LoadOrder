using Mono.Cecil;
using System.Runtime.CompilerServices;
using ILogger = Patch.API.ILogger;
using System.IO;
using System.Diagnostics;
using System;

namespace LoadOrderIPatch {
    internal static class Commons {
        internal const string InjectionsDLL = InjectionsAssemblyName + ".dll";
        internal const string InjectionsAssemblyName = "LoadOrderInjections";
        internal static AssemblyDefinition GetInjectionsAssemblyDefinition(string dir)
            => CecilUtil.ReadAssemblyDefinition(Path.Combine(dir, InjectionsDLL));

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
    }
}
