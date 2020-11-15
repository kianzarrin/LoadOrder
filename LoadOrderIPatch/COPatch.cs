using Mono.Cecil;
using Mono.Cecil.Cil;
using Patch.API;
using System;
using System.IO;
using System.Linq;

namespace LoadOrderIPatch.Patches {
    public class COPatch : IPatch {
        public int PatchOrderAsc { get; } = 10;
        public AssemblyToPatch PatchTarget { get; } = new AssemblyToPatch("ColossalManaged", new Version(0, 3, 0, 0));
        private ILogger logger_;
        private string workingPath_;
        public AssemblyDefinition Execute(AssemblyDefinition assemblyDefinition, ILogger logger, string patcherWorkingPath) {
            logger_ = logger;
            workingPath_ = patcherWorkingPath;

            assemblyDefinition = LoadAssembliesPatch(assemblyDefinition);
            return assemblyDefinition;
        }

        //public AssemblyDefinition GetAssemblyDefinition(string fileName)
        //    => Util.GetAssemblyDefinition(workingPath_, fileName);

        //public AssemblyDefinition AddPluginsPatch(AssemblyDefinition CO)
        //{
        //    var tPluginManager = CO.MainModule.Types
        //        .First(_t => _t.FullName == "ColossalFramework.Plugins.PluginManager");
        //    var mAddPlugins = tPluginManager.Methods
        //        .First(_m => _m.Name == "AddPlugins");

        //    DefaultAssemblyResolver resolver = new DefaultAssemblyResolver();
        //    resolver.AddSearchDirectory(workingPath_);
        //    var dllPath = Path.Combine(workingPath_, "LoadOrderMod.dll");
        //    var readerParams = new ReaderParameters { AssemblyResolver = resolver };
        //    AssemblyDefinition LoadOrderMod = AssemblyDefinition.ReadAssembly(dllPath, readerParams);

        //    var tAddPluginsPatch = LoadOrderMod.MainModule.Types
        //        .First(_t => _t.Name == "AddPluginsPatch");
        //    var mdAddPlugginsSorted = tAddPluginsPatch.Methods
        //        .First(_m => _m.Name == "AddPlugginsSorted");
        //    MethodReference mrAddPlugginsSorted = tPluginManager.Module.ImportReference(mdAddPlugginsSorted);

        //    ILProcessor ilProcessor = mAddPlugins.Body.GetILProcessor();
        //    Instruction loadArg1 = Instruction.Create(OpCodes.Ldarg_1); 
        //    Instruction CallAddPlugginsSorted = Instruction.Create(OpCodes.Call, mrAddPlugginsSorted);
        //    Instruction retInstr = Instruction.Create(OpCodes.Ret); 
        //    Instruction first = mAddPlugins.Body.Instructions.First();
        //    ilProcessor.InsertBefore(first, loadArg1); // load pluggins
        //    ilProcessor.InsertAfter(loadArg1, CallAddPlugginsSorted); 
        //    ilProcessor.InsertAfter(CallAddPlugginsSorted, retInstr); // skip stock code.
        //    logger_.Info("AddPluginsPatch applied!");

        //    return CO;
        //}


        public AssemblyDefinition LoadAssembliesPatch(AssemblyDefinition CO)
        {
            var tPluginManager = CO.MainModule.Types
                .First(_t => _t.FullName == "ColossalFramework.Plugins.PluginManager");
            var mAddPlugins = tPluginManager.Methods
                .First(_m => _m.Name == "LoadAssemblies");

            DefaultAssemblyResolver resolver = new DefaultAssemblyResolver();
            resolver.AddSearchDirectory(workingPath_);
            var dllPath = Path.Combine(workingPath_, "LoadOrderMod.dll");
            var readerParams = new ReaderParameters { AssemblyResolver = resolver };
            AssemblyDefinition LoadOrderMod = AssemblyDefinition.ReadAssembly(dllPath, readerParams);

            var tSortPlugins = LoadOrderMod.MainModule.Types
                .First(_t => _t.Name == "SortPlugins");
            var mdSort = tSortPlugins.Methods
                .First(_m => _m.Name == "Sort");
            
            MethodReference mrInjection = tPluginManager.Module.ImportReference(mdSort);
            ILProcessor ilProcessor = mAddPlugins.Body.GetILProcessor();

            Instruction loadArg1 = Instruction.Create(OpCodes.Ldarg_1);
            Instruction CallInjection = Instruction.Create(OpCodes.Call, mrInjection);
            Instruction first = mAddPlugins.Body.Instructions.First();
            ilProcessor.InsertBefore(first, loadArg1); // load pluggins
            ilProcessor.InsertAfter(loadArg1, CallInjection);
            logger_.Info("LoadAssembliesPatch applied successfully!");
            return CO;
        }


    }
}