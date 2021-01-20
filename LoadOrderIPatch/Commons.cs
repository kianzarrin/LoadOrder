using Mono.Cecil;
using System.Runtime.CompilerServices;
using ILogger = Patch.API.ILogger;
using System;
using System.Diagnostics;

namespace LoadOrderIPatch {
    internal static class Commons {
        internal const string InjectionsDLL = InjectionsAssemblyName + ".dll";
        internal const string InjectionsAssemblyName = "LoadOrderInjections";
        internal static AssemblyDefinition GetInjectionsAssemblyDefinition(string dir)
            => CecilUtil.GetAssemblyDefinition(dir, InjectionsDLL);

        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ReportSucessFull(this ILogger logger)
        {
            string caller = new StackFrame(1).GetMethod().Name;
            logger.Info($"[LoadOrderIPatch] Sucessfully patched {caller}() !" +
                "\n------------------------------------------------------------------------");
        }
    }
}
