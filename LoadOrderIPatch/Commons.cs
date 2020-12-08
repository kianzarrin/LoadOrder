using Mono.Cecil;
using Mono.Cecil.Cil;
using Patch.API;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using ILogger = Patch.API.ILogger;
using static LoadOrderIPatch.Commons;
namespace LoadOrderIPatch {
    internal static class Commons {
        internal const string InjectionsDLL = InjectionsAssemblyName + ".dll";
        internal const string InjectionsAssemblyName = "LoadOrderInjections";
        internal static AssemblyDefinition GetInjectionsAssemblyDefinition(string dir) 
            => CecilUtil.GetAssemblyDefinition(dir, InjectionsDLL);
    }
}
