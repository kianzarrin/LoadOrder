using Mono.Cecil;
using Mono.Cecil.Cil;
using Patch.API;
using System;
using System.Linq;
using UnityEngine;
using ILogger = Patch.API.ILogger;

namespace LoadOrderIPatch.Patches {
    public class CMPatch : IPatch {
        public int PatchOrderAsc { get; } = 99;
        public AssemblyToPatch PatchTarget { get; } = new AssemblyToPatch("ColossalManaged", new Version(0, 3, 0, 0));

        public AssemblyDefinition Execute(
            AssemblyDefinition assemblyDefinition, 
            ILogger logger, 
            string patcherWorkingPath,
            IPaths gamePaths) {
            try
            {
                Entry.Logger = logger;
                //BootPatch(assemblyDefinition);
                NoReportersPatch(assemblyDefinition);
            } catch (Exception ex) { logger.Error(ex.ToString());  }
            return assemblyDefinition;
        }

        /// <summary>
        /// replaces the normal loading process with subscription manger tool
        /// </summary>
        public void BootPatch(AssemblyDefinition CM)
        {
            Log.StartPatching();
            var module = CM.Modules.First();
            var type = module.GetType("ColossalFramework", "BootStrapper");
            MethodDefinition mTarget = type.Methods.Where(m => m.Name == "Boot" && m.Parameters.Count == 3).Single();
            Log.Info($"patching {mTarget} ...");

            var instructions = mTarget.Body.Instructions;
            ILProcessor ilProcessor = mTarget.Body.GetILProcessor();

            /**********************************/
            Instruction ret = Instruction.Create(OpCodes.Ret);
            Instruction pointer = instructions.First(_c => _c.Calls("DisplayStatus"));
            ilProcessor.InsertAfter(pointer, ret);
        
            Log.Successful();
        }

        public void NoReportersPatch(AssemblyDefinition CM) {
            Log.StartPatching();
            var module = CM.Modules.First();
            MethodDefinition mTarget = module.GetMethod("ColossalFramework.Packaging.PackageManager.CreateReporter");
            Log.Info($"patching {mTarget} ...");

            var instructions = mTarget.Body.Instructions;
            ILProcessor ilProcessor = mTarget.Body.GetILProcessor();

            /**********************************/
            Instruction ret = Instruction.Create(OpCodes.Ret);
            ilProcessor.Prefix(ret);

            Log.Successful();
        }

    }
}