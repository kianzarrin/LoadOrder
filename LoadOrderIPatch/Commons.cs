namespace LoadOrderIPatch {
    using Mono.Cecil;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using ILogger = Patch.API.ILogger;

    internal static class Commons {
        internal const string InjectionsDLL = InjectionsAssemblyName + ".dll";
        internal const string InjectionsAssemblyName = "LoadOrderInjections";
        internal static AssemblyDefinition GetInjectionsAssemblyDefinition(string dir)
            => CecilUtil.ReadAssemblyDefinition(Path.Combine(dir, InjectionsDLL));
    }
}
