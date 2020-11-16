using Mono.Cecil;
using Mono.Cecil.Cil;
using Patch.API;
using System;
using System.IO;
using System.Linq;

namespace LoadOrderIPatch.Patches {
    public class COPatch : IPatch {
        public int PatchOrderAsc { get; } = 100;
        public AssemblyToPatch PatchTarget { get; } = new AssemblyToPatch("ColossalManaged", new Version(0, 3, 0, 0));
        private ILogger logger_;
        private string workingPath_;
        public AssemblyDefinition Execute(AssemblyDefinition assemblyDefinition, ILogger logger, string patcherWorkingPath) {
            logger_ = logger;
            workingPath_ = patcherWorkingPath;

            assemblyDefinition = LoadAssembliesPatch(assemblyDefinition); 
                assemblyDefinition = LoadPluginsPatch(assemblyDefinition); 
            return assemblyDefinition;
        }

        public AssemblyDefinition GetAssemblyDefinition(string fileName)
            => Util.GetAssemblyDefinition(workingPath_, fileName);

        public AssemblyDefinition LoadAssembliesPatch(AssemblyDefinition CO)
        {
            var tPluginManager = CO.MainModule.Types
                .First(_t => _t.FullName == "ColossalFramework.Plugins.PluginManager");
            MethodDefinition mTarget = tPluginManager.Methods
                .First(_m => _m.Name == "LoadAssemblies");


            AssemblyDefinition LoadOrderMod = GetAssemblyDefinition("LoadOrderMod.dll");
            var tSortPlugins = LoadOrderMod.MainModule.Types
                .First(_t => _t.Name == "SortPlugins");
            MethodDefinition mdSort = tSortPlugins.Methods
                .First(_m => _m.Name == "Sort");
            
            MethodReference mrInjection = tPluginManager.Module.ImportReference(mdSort);
            ILProcessor ilProcessor = mTarget.Body.GetILProcessor();

            Instruction loadArg1 = Instruction.Create(OpCodes.Ldarg_1);
            Instruction callInjection = Instruction.Create(OpCodes.Call, mrInjection);
            Instruction first = mTarget.Body.Instructions.First();
            ilProcessor.InsertBefore(first, loadArg1); // load pluggins arg
            ilProcessor.InsertAfter(loadArg1, callInjection); 
            logger_.Info("LoadAssembliesPatch applied successfully!");
            return CO;
        }

        public AssemblyDefinition LoadPluginsPatch(AssemblyDefinition CO)
        {
            var tPluginManager = CO.MainModule.Types
                .First(_t => _t.FullName == "ColossalFramework.Plugins.PluginManager");
            MethodDefinition mTarget = tPluginManager.Methods
                .First(_m => _m.Name == "LoadPlugins");

            MethodDefinition mInjection = tPluginManager.Methods
                .First(_m => _m.Name == "LoadPlugin");

            var dllPath = Path.Combine(workingPath_, "LoadOrderMod.dll");

            Instruction loadThis = Instruction.Create(OpCodes.Ldarg_0);
            Instruction loadDllPath = Instruction.Create(OpCodes.Ldstr, dllPath);
            Instruction callInjection = Instruction.Create(OpCodes.Call, mInjection);

            Instruction first = mTarget.Body.Instructions.First();
            ILProcessor ilProcessor = mTarget.Body.GetILProcessor();
            ilProcessor.InsertBefore(first, loadThis); // load pluggins arg
            ilProcessor.InsertAfter(loadThis, loadDllPath);
            ilProcessor.InsertAfter(loadDllPath, callInjection);
            logger_.Info("LoadPluginsPatch applied successfully!");
            return CO;
        }

        public AssemblyDefinition AddPluginsPatch(AssemblyDefinition CO)
        {
            var tPluginManager = CO.MainModule.Types
                .First(_t => _t.FullName == "ColossalFramework.Plugins.PluginManager");
            MethodDefinition mTarget = tPluginManager.Methods
                .First(_m => _m.Name == "AddPlugins");

            AssemblyDefinition LoadOrderMod = GetAssemblyDefinition("LoadOrderMod.dll");
            var tLogs = LoadOrderMod.MainModule.Types
                .First(_t => _t.Name == "Logs");

            MethodDefinition mdPreCreateUserModInstance = tLogs.Methods
                .First(_m => _m.Name == "PreCreateUserModInstance");
            MethodReference mrPreCreateUserModInstance = tPluginManager.Module.ImportReference(mdPreCreateUserModInstance);

            MethodDefinition mdBeforeEnable = tLogs.Methods
                .First(_m => _m.Name == "BeforeEnable");
            MethodReference mrBeforeEnable = tPluginManager.Module.ImportReference(mdBeforeEnable);

            MethodDefinition mdAfterEnable = tLogs.Methods
                .First(_m => _m.Name == "AfterEnable");
            MethodReference mrAfterEnable = tPluginManager.Module.ImportReference(mdAfterEnable);

            // find instructions
            var codes = mTarget.Body.Instructions.ToList();
            Instruction getUserModInstance = codes.First(_c => (_c.Operand as MethodDefinition).Name == "get_userModInstance");
            Instruction InvokeOnEnabled = codes.First(_c => (_c.Operand as MethodDefinition).Name == "Invoke");

            Instruction LoadPluginInfo = getUserModInstance.Previous.Duplicate();
            Instruction callPreCreateUserModInstance = Instruction.Create(OpCodes.Call, mrPreCreateUserModInstance);
            Instruction callBeforeEnable = Instruction.Create(OpCodes.Call, mrBeforeEnable);
            Instruction callAfterEnable = Instruction.Create(OpCodes.Call, mrAfterEnable);

            ILProcessor ilProcessor = mTarget.Body.GetILProcessor();
            ilProcessor.InsertBefore(first, loadArg1); // load pluggins arg
            ilProcessor.InsertAfter(loadArg1, callInjection);
            logger_.Info("AddPluginsPatch applied successfully!");
            return CO;
        }

        //PreCreateUserModInstance
    }
}