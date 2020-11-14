using Mono.Cecil;
using Mono.Cecil.Cil;
using Patch.API;
using System;
using System.Linq;
using System.IO;

namespace LoadOrderIPatch.Patches {
    public class ACP : IPatch {
        public int PatchOrderAsc { get; } = 7;
        public AssemblyToPatch PatchTarget { get; } = new AssemblyToPatch("ColossalManaged", new Version(0, 3, 0, 0));
        private ILogger logger_;
        private string workingPath_;
        public AssemblyDefinition Execute(AssemblyDefinition assemblyDefinition, ILogger logger, string patcherWorkingPath) {
            logger_ = logger;
            workingPath_ = patcherWorkingPath;

            assemblyDefinition = AddPluginsPatch(assemblyDefinition);
            return assemblyDefinition;
        }

        public AssemblyDefinition GetAssemblyDefinition(string fileName)
            => Util.GetAssemblyDefinition(workingPath_, fileName);


        public AssemblyDefinition AddPluginsPatch(AssemblyDefinition CO)
        {
            var tPluginManager = CO.MainModule.Types
                .First(_t => _t.FullName == "ColossalFramework.Plugins.PluginManager");
            var mAddPlugins = tPluginManager.Methods
                .First(_m => _m.Name == "AddPlugins");
            
            AssemblyDefinition LoadOrderMod = GetAssemblyDefinition("LoadOrderMod.dll");
            var tAddPluginsPatch = LoadOrderMod.MainModule.Types
                .First(_t => _t.Name == "AddPluginsPatch");
            var mAddPlugginsSorted = tAddPluginsPatch.Methods
                .First(_m => _m.Name == "AddPlugginsSorted");

            ILProcessor ilProcessor = mAddPlugins.Body.GetILProcessor();
            Instruction loadArg0 = Instruction.Create(OpCodes.Ldarg_0); 
            Instruction CallAddPlugginsSorted = Instruction.Create(OpCodes.Call, mAddPlugginsSorted);
            Instruction retInstr = Instruction.Create(OpCodes.Ret); 
            Instruction first = mAddPlugins.Body.Instructions.First();
            ilProcessor.InsertBefore(loadArg0, first); // load pluggins
            ilProcessor.InsertAfter(loadArg0, CallAddPlugginsSorted); 
            ilProcessor.InsertAfter(CallAddPlugginsSorted, retInstr); // skip stock code.
            logger_.Info("AddPluginsPatch applied!");

            return CO;
        }
    }
}