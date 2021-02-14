using ColossalFramework.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Patch.API;
using System;
using System.IO;
using System.Linq;
using static LoadOrderIPatch.Commons;
using ILogger = Patch.API.ILogger;

namespace LoadOrderIPatch.Patches {
    public class CMPatch : IPatch {
        public int PatchOrderAsc { get; } = 100;
        public AssemblyToPatch PatchTarget { get; } = new AssemblyToPatch("ColossalManaged", new Version(0, 3, 0, 0));
        private ILogger logger_;
        private string workingPath_;
        public AssemblyDefinition Execute(
            AssemblyDefinition assemblyDefinition, 
            ILogger logger, 
            string patcherWorkingPath,
            IPaths gamePaths) {
            logger_ = logger;
            workingPath_ = patcherWorkingPath;

            assemblyDefinition = LoadAssembliesPatch(assemblyDefinition); 
            //assemblyDefinition = LoadPluginsPatch(assemblyDefinition); // its loaded in ASCPatch.LoadDLL() instead
            assemblyDefinition = AddPluginsPatch(assemblyDefinition);
#if DEBUG
            //assemblyDefinition = InsertPrintStackTrace(assemblyDefinition);
#endif
            bool noAssets = Environment.GetCommandLineArgs().Any(_arg => _arg == "-noAssets");
            if (noAssets) {
                assemblyDefinition = NoCustomAssetsPatch(assemblyDefinition);
            }


            bool poke = Environment.GetCommandLineArgs().Any(_arg => _arg == "-poke");
            if(poke)
                PokeAssemlyPatch(assemblyDefinition);

            return assemblyDefinition;
        }

        public void PokeAssemlyPatch(AssemblyDefinition CM) {
            logger_.LogStartPatching();
            var module = CM.MainModule;
            logger_.Info("PluginInfo is :" +  module.Types.FirstOrDefault(t => t.Name.EndsWith("PluginManager")).FullName);

            var type1 = module.GetType("ColossalFramework.Plugins.PluginManager");
            var type2 = type1.NestedTypes.Single(_t => _t.Name == "PluginInfo");
            var mTarget = type2.GetMethod("AddAssembly");

            ILProcessor ilProcessor = mTarget.Body.GetILProcessor();
            var instructions = mTarget.Body.Instructions;
            var first = instructions.First();

            var loi = GetInjectionsAssemblyDefinition(workingPath_);
            var mInjection = loi.MainModule.GetMethod("LoadOrderInjections.Injections.AddAssembly.Prefix");
            var mrInjection = module.ImportReference(mInjection);

            var callInjection = Instruction.Create(OpCodes.Call, mrInjection);
            var ldAsm = Instruction.Create(OpCodes.Ldarg_1);
            ilProcessor.InsertBefore(first, callInjection);
            ilProcessor.InsertBefore(callInjection, ldAsm);

            logger_.LogSucessfull();
        }

        public AssemblyDefinition NoCustomAssetsPatch(AssemblyDefinition CM)
        {
            logger_.LogStartPatching();
            var module = CM.MainModule;
            var type = module.GetType("ColossalFramework.Packaging.PackageManager");
            var mTargets = type.Methods.Where(_m =>
                _m.Name.StartsWith("Load") && _m.Name.EndsWith("Packages")); //Load*Packages
            foreach (var mTarget in mTargets) {
                bool top = mTarget.Name == "LoadPackages" && mTarget.Parameters.Count == 0;
                if (top) continue;
                bool loadPath = mTarget.HasParameter("path");
                ILProcessor ilProcessor = mTarget.Body.GetILProcessor();
                var instructions = mTarget.Body.Instructions;

                var first = instructions.First();
                var last = instructions.Last();
                // return to skip method.
                if (loadPath) {
                    logger_.Info("patching LoadPackages(string path,bool)");
                    // skip method only if path is asset path
                    var LdArgPath = mTarget.GetLDArg("path");
                    var mIsAssetPath = GetType().GetMethod(nameof(IsAssetPath));
                    var callIsAssetPath = Instruction.Create(OpCodes.Call, module.ImportReference(mIsAssetPath));
                    var skipIfAsset = Instruction.Create(OpCodes.Brtrue, last); 
                    ilProcessor.InsertBefore(first, LdArgPath);
                    ilProcessor.InsertAfter(LdArgPath, callIsAssetPath);
                    ilProcessor.InsertAfter(callIsAssetPath, skipIfAsset);
                } else {
                    var ret = Instruction.Create(OpCodes.Ret);
                    ilProcessor.InsertBefore(first, ret);
                }
            }

            logger_.LogSucessfull();
            return CM;
        }

        public static bool IsAssetPath(string path)
        {
            return path == DataLocation.assetsPath;
        }

#if DEBUG
        // get the stack trace for debugging purposes.
        // modify this mehtod to print the desired stacktrace. 
        public AssemblyDefinition InsertPrintStackTrace(AssemblyDefinition CM)
        {
            logger_.LogStartPatching();
            var module = CM.MainModule;
            var type = module.GetType("ColossalFramework.PlatformServices.PlatformServiceBehaviour");
            var mTarget = type.Methods.Single(_m => _m.Name == "Awake");
            ILProcessor ilProcessor = mTarget.Body.GetILProcessor();
            var instructions = mTarget.Body.Instructions;
            var first = instructions.First();

            var mInjection = GetType().GetMethod(nameof(LogStackTrace));
            var mrInjection = module.ImportReference(mInjection);
            var callInjection = Instruction.Create(OpCodes.Call, mrInjection);

            ilProcessor.InsertBefore(first, callInjection);

            logger_.Info("PlatformServiceBehaviour_Awake_Patch applied successfully!");
            return CM;
        }
#endif

        public static void LogStackTrace()
        {
            UnityEngine.Debug.Log("[LoadOrderIPatch] stack trace is: " + Environment.StackTrace);
        }


        /// <summary>
        /// Sorts Assembly dictionary (hackish) at the begining of PluginManager.LoadAssemblies()
        /// </summary>
        public AssemblyDefinition LoadAssembliesPatch(AssemblyDefinition CM)
        {
            logger_.LogStartPatching();
            var tPluginManager = CM.MainModule.Types
                .First(_t => _t.FullName == "ColossalFramework.Plugins.PluginManager");
            MethodDefinition mTarget = tPluginManager.Methods
                .First(_m => _m.Name == "LoadAssemblies");

            AssemblyDefinition asm = GetInjectionsAssemblyDefinition(workingPath_);
            var tSortPlugins = asm.MainModule.Types
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
            logger_.LogSucessfull();
            return CM;
        }

        /// <summary>
        /// loads LoadOrderInjections.dll at the beggning of PluginManger.LoadPlugins()
        /// </summary>
        public AssemblyDefinition LoadPluginsPatch(AssemblyDefinition CM)
        {
            logger_.LogStartPatching();
            var tPluginManager = CM.MainModule.Types
                .First(_t => _t.FullName == "ColossalFramework.Plugins.PluginManager");
            MethodDefinition mTarget = tPluginManager.Methods
                .First(_m => _m.Name == "LoadPlugins");

            MethodDefinition mInjection = tPluginManager.Methods
                .First(_m => _m.Name == "LoadPlugin");

            var dllPath = Path.Combine(workingPath_, "LoadOrderInjections.dll");

            Instruction loadThis = Instruction.Create(OpCodes.Ldarg_0);
            Instruction loadDllPath = Instruction.Create(OpCodes.Ldstr, dllPath);
            Instruction callInjection = Instruction.Create(OpCodes.Call, mInjection);

            Instruction first = mTarget.Body.Instructions.First();
            ILProcessor ilProcessor = mTarget.Body.GetILProcessor();
            ilProcessor.InsertBefore(first, loadThis); // load pluggins arg
            ilProcessor.InsertAfter(loadThis, loadDllPath);
            ilProcessor.InsertAfter(loadDllPath, callInjection);
            logger_.LogSucessfull();
            return CM;
        }

        /// <summary>
        /// inserts time stamps about adding/enabling plugins.
        /// </summary>
        public AssemblyDefinition AddPluginsPatch(AssemblyDefinition CM)
        {
            logger_.LogStartPatching();
               var tPluginManager = CM.MainModule.Types
                .First(_t => _t.FullName == "ColossalFramework.Plugins.PluginManager");
            MethodDefinition mTarget = tPluginManager.Methods
                .First(_m => _m.Name == "AddPlugins");

            AssemblyDefinition asm = GetInjectionsAssemblyDefinition(workingPath_);
            var tLogs = asm.MainModule.Types
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
            Instruction getUserModInstance = codes.First(
                _c => (_c.Operand as MethodReference)?.Name == "get_userModInstance");
            Instruction InvokeOnEnabled = codes.First(
                _c => (_c.Operand as MethodReference)?.Name == "Invoke");

            Instruction LoadPluginInfo = getUserModInstance.Previous;
            Instruction callPreCreateUserModInstance = Instruction.Create(OpCodes.Call, mrPreCreateUserModInstance);
            Instruction callBeforeEnable = Instruction.Create(OpCodes.Call, mrBeforeEnable);
            Instruction callAfterEnable = Instruction.Create(OpCodes.Call, mrAfterEnable);

            ILProcessor ilProcessor = mTarget.Body.GetILProcessor();
            ilProcessor.InsertBefore(getUserModInstance, callPreCreateUserModInstance); // insert call
            ilProcessor.InsertBefore(callPreCreateUserModInstance, LoadPluginInfo.Duplicate()); // load argument for the call

            ilProcessor.InsertBefore(InvokeOnEnabled, callBeforeEnable); // insert call
            ilProcessor.InsertBefore(callBeforeEnable, LoadPluginInfo.Duplicate()); // load argument for the call

            ilProcessor.InsertAfter(InvokeOnEnabled, callAfterEnable); // insert call
            ilProcessor.InsertBefore(callAfterEnable, LoadPluginInfo.Duplicate()); // load argument for the call

            logger_.LogSucessfull();
            return CM;
        }
    }
}