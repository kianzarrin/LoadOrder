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

            FindAssemblySoftPatch(assemblyDefinition);
            
            // TODO uncomment after understanding how CS prevents double loading during hot reload
            // NoDoubleLoadPatch(assemblyDefinition);
            
            assemblyDefinition = LoadAssembliesPatch(assemblyDefinition);
            //assemblyDefinition = LoadPluginsPatch(assemblyDefinition); // its loaded in ASCPatch.LoadDLL() instead
            AddAssemlyPatch(assemblyDefinition);
            assemblyDefinition = AddPluginsPatch(assemblyDefinition);
#if DEBUG
            //assemblyDefinition = InsertPrintStackTrace(assemblyDefinition);
#endif
            bool noAssets = Environment.GetCommandLineArgs().Any(_arg => _arg == "-noAssets");
            if (noAssets) {
                assemblyDefinition = NoCustomAssetsPatch(assemblyDefinition);
            } else {
                EnsureIncludedExcludedPackagePatch(assemblyDefinition);
                ExcludeAssetPatch(assemblyDefinition);
            }


            return assemblyDefinition;
        }

        /// <summary>
        /// if the assembly already exists in the current domain, 
        /// do not double load it.
        /// </summary>
        /// <param name="CM"></param>
        public void NoDoubleLoadPatch(AssemblyDefinition CM) {
            logger_.LogStartPatching();
            var cm = CM.MainModule;
            var mTarget = cm.GetMethod(
                "ColossalFramework.Plugins.PluginManager.LoadPlugin");
            ILProcessor ilProcessor = mTarget.Body.GetILProcessor();
            var instructions = mTarget.Body.Instructions;
            var first = instructions.First();

            var loi = GetInjectionsAssemblyDefinition(workingPath_);
            var mInjection = loi.MainModule.GetMethod(
                "LoadOrderInjections.Injections.LoadingApproach.ExistingAssembly");
            var mrInjection = cm.ImportReference(mInjection);

            var loadDllPath = mTarget.GetLDArg("dllPath");
            var callExistingAssembly = Instruction.Create(OpCodes.Call, mrInjection);
            var storeResult = Instruction.Create(OpCodes.Stloc_0);
            var returnResult = instructions.Last(
                c => c.OpCode == OpCodes.Ldloc_0 && 
                c.Next?.OpCode == OpCodes.Ret);
            var GotoToReturnResultIfNotNull = Instruction.Create(OpCodes.Brfalse, returnResult);

            /*
            result =  ExistingAssembly(dllPath)
            if(result is not null) goto ReturnResult
            ...
            ReturnResult: 
            return result

             */
            ilProcessor.Prefix(loadDllPath, callExistingAssembly, storeResult, GotoToReturnResultIfNotNull);

            logger_.LogSucessfull();
        }

        public void FindAssemblySoftPatch(AssemblyDefinition CM) {
            logger_.LogStartPatching();
            var cm = CM.MainModule;
            var mTarget = cm.GetMethod(
                "ColossalFramework.Plugins.PluginManager.FindPluginAssemblyByName");
            ILProcessor ilProcessor = mTarget.Body.GetILProcessor();
            var instructions = mTarget.Body.Instructions;

            var loi = GetInjectionsAssemblyDefinition(workingPath_);
            var mInjection = loi.MainModule.GetMethod(
                "LoadOrderInjections.Injections.LoadingApproach.FindDependancySoft");
            var mrInjection = cm.ImportReference(mInjection);

            // find return null;
            Instruction ldnull = null;
            for(int i=0; i < instructions.Count; ++i) {
                if(instructions[i].OpCode == OpCodes.Ldnull &&
                   instructions[i+1].OpCode == OpCodes.Ret) {
                    ldnull = instructions[i];
                }
            }

            // replace 'return null' with 'return FindDependancySoft(asmName, asms)'
            var ldarg1 = Instruction.Create(OpCodes.Ldarg_1);
            var ldarg2 = Instruction.Create(OpCodes.Ldarg_2);
            var callInjection = Instruction.Create(OpCodes.Call, mrInjection);
            ilProcessor.Replace(ldnull, ldarg1);
            ilProcessor.InsertAfter(ldarg1, ldarg2);
            ilProcessor.InsertAfter(ldarg2, callInjection);

            logger_.LogSucessfull();
        }

        public void AddAssemlyPatch(AssemblyDefinition CM) {
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

            if (poke) {
                var mInjection = loi.MainModule.GetMethod("LoadOrderInjections.Injections.LoadingApproach.AddAssemblyPrefix");
                var mrInjection = module.ImportReference(mInjection);
                var callInjection = Instruction.Create(OpCodes.Call, mrInjection);
                var ldAsm = Instruction.Create(OpCodes.Ldarg_1);

                ilProcessor.InsertBefore(first, callInjection);
                ilProcessor.InsertBefore(callInjection, ldAsm);
            } 
            
            if (breadthFirst) {
                var callAdd = instructions.First(_c => _c.Calls("Add"));
                var ret = Instruction.Create(OpCodes.Ret);
            }

            var tLogs = loi.MainModule.GetType("LoadOrderInjections.Injections.Logs");
            {
                var mBeforeAddAssembliesGetExportedTypes = tLogs.GetMethod("BeforeAddAssembliesGetExportedTypes");
                var callBeforeAddAssembliesGetExportedTypes = Instruction.Create(
                    OpCodes.Call, module.ImportReference(mBeforeAddAssembliesGetExportedTypes));
                var callGetExportedTypes = instructions.First(_c => _c.Calls("GetExportedTypes"));
                var loadAsm = callGetExportedTypes.Previous.Duplicate();
                ilProcessor.InsertBefore(callGetExportedTypes, callBeforeAddAssembliesGetExportedTypes);
                ilProcessor.InsertBefore(callBeforeAddAssembliesGetExportedTypes, loadAsm);
            }

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
                bool loadPath = mTarget.HasParameter("path"); // LoadPackages(string path,bool)
                ILProcessor ilProcessor = mTarget.Body.GetILProcessor();
                var instructions = mTarget.Body.Instructions;
                var first = instructions.First();
                var last = instructions.Last();

                logger_.Info("patching " + mTarget.Name);
                if (loadPath) {
                    // skip method only if path is asset path
                    var LdArgPath = mTarget.GetLDArg("path");
                    var mIsAssetPath = GetType().GetMethod(nameof(IsAssetPath));
                    var callIsAssetPath = Instruction.Create(OpCodes.Call, module.ImportReference(mIsAssetPath));
                    var skipIfAsset = Instruction.Create(OpCodes.Brtrue, last); // goto to return.
                    ilProcessor.Prefix(LdArgPath, callIsAssetPath, skipIfAsset);
                } else {
                    // return to skip method.
                    var ret = Instruction.Create(OpCodes.Ret);
                    ilProcessor.Prefix(ret);
                }
            }

            logger_.LogSucessfull();
            return CM;
        }

        public static bool IsAssetPath(string path)
        {
            return path == DataLocation.assetsPath;
        }

        /// <summary>
        /// moved folder to _folder if neccessary before calling getfiles.
        /// </summary>
        public void EnsureIncludedExcludedPackagePatch(AssemblyDefinition CM) {
            logger_.LogStartPatching();
            var cm = CM.MainModule;
            var type = cm.GetType("ColossalFramework.Packaging.PackageManager");
            var mTargets = type.Methods.Where(_m => _m.Name == "LoadPackages");

            foreach (var mTarget in mTargets) {
                bool loadPath = mTarget.HasParameter("path"); // LoadPackages(string path,bool)
                ILProcessor ilProcessor = mTarget.Body.GetILProcessor();
                var instructions = mTarget.Body.Instructions;

                foreach( var callGetFiles in instructions.Where(_c => _c.Calls("GetFiles"))) {
                    logger_.Info("patching " + mTarget.Name);
                    var mCheckFiles = GetType().GetMethod(nameof(CheckFiles));
                    var callCheckFiles = Instruction.Create(OpCodes.Call, cm.ImportReference(mCheckFiles));
                    ilProcessor.InsertBefore(callGetFiles, callCheckFiles);
                }
            }
            logger_.LogSucessfull();
        }

        public string CheckFiles(string path) {
            LoadOrderInjections.SteamUtilities.EnsureIncludedOrExcludedFiles(path);
            return path;
        }

        public void ExcludeAssetPatch(AssemblyDefinition CM) {
            logger_.LogStartPatching();
            var module = CM.MainModule;
            var type = module.GetType("ColossalFramework.Packaging.PackageManager");

            //void Update(string path)
            //void Update(PublishedFileId id, string path)
            var mTargets = type.Methods.Where(_m => _m.Name == "Update").ToList();

            // LoadPackages(string path, bool)
            var mLoadPackages =  type.Methods.Single(_m =>
                _m.Name == "LoadPackages" && _m.HasParameter("path"));

            mTargets.Add(mLoadPackages);


            foreach(var mTarget in mTargets) {
                ILProcessor ilProcessor = mTarget.Body.GetILProcessor();
                var instructions = mTarget.Body.Instructions;
                var first = instructions.First();
                var last = instructions.Last();

                // skip method only if path is asset path
                var LdArgPath = mTarget.GetLDArg("path");
                var mIsExcluded = GetType().GetMethod(nameof(IsPathExcluded));
                var callIsExcluded = Instruction.Create(OpCodes.Call, module.ImportReference(mIsExcluded));
                var skipIfExcluded = Instruction.Create(OpCodes.Brtrue, last); // goto to return.
                ilProcessor.Prefix(new[] { LdArgPath, callIsExcluded, skipIfExcluded });
            }

            logger_.LogSucessfull();
        }

        public static bool IsPathExcluded(string path)  => path.StartsWith("_");

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
            ILProcessor ilProcessor = mTarget.Body.GetILProcessor();
            var codes = mTarget.Body.Instructions.ToList();

            AssemblyDefinition asm = GetInjectionsAssemblyDefinition(workingPath_);
            var tLogs = asm.MainModule.Types
                .First(_t => _t.Name == "Logs");

            var tLoadingApproach = asm.MainModule.Types
                .First(_t => _t.Name == "LoadingApproach");

            MethodDefinition mdPreCreateUserModInstance = tLogs.Methods
                .First(_m => _m.Name == "PreCreateUserModInstance");
            MethodReference mrPreCreateUserModInstance = 
                tPluginManager.Module.ImportReference(mdPreCreateUserModInstance);

            MethodDefinition mdAddPluginsPrefix = tLoadingApproach.Methods
                .First(_m => _m.Name == "AddPluginsPrefix");
            MethodReference mrAddPluginsPrefix = 
                tPluginManager.Module.ImportReference(mdAddPluginsPrefix);

            MethodDefinition mdBeforeEnable = tLogs.Methods
                .First(_m => _m.Name == "BeforeEnable");
            MethodReference mrBeforeEnable = tPluginManager.Module.ImportReference(mdBeforeEnable);

            MethodDefinition mdAfterEnable = tLogs.Methods
                .First(_m => _m.Name == "AfterEnable");
            MethodReference mrAfterEnable = tPluginManager.Module.ImportReference(mdAfterEnable);

            // find instructions
            Instruction first = codes.First();

            Instruction getUserModInstance = codes.First(
                _c => (_c.Operand as MethodReference)?.Name == "get_userModInstance");
            Instruction InvokeOnEnabled = codes.First(
                _c => (_c.Operand as MethodReference)?.Name == "Invoke");

            Instruction LoadPlugins = mTarget.GetLDArg("plugins");
            Instruction LoadPluginInfo = getUserModInstance.Previous;
            Instruction callPreCreateUserModInstance = Instruction.Create(OpCodes.Call, mrPreCreateUserModInstance);
            Instruction callAddPluginsPrefix = Instruction.Create(OpCodes.Call, mrAddPluginsPrefix);
            Instruction callBeforeEnable = Instruction.Create(OpCodes.Call, mrBeforeEnable);
            Instruction callAfterEnable = Instruction.Create(OpCodes.Call, mrAfterEnable);

            ilProcessor.InsertBefore(first, callAddPluginsPrefix); // insert call
            ilProcessor.InsertBefore(callAddPluginsPrefix, LoadPlugins.Duplicate()); // load input argument

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