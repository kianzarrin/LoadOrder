using ColossalFramework;
using ColossalFramework.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Patch.API;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using static LoadOrderIPatch.Commons;
using static LoadOrderIPatch.ConfigUtil;
using ILogger = Patch.API.ILogger;

namespace LoadOrderIPatch.Patches {
    extern alias Injections;
    using SteamUtilities = Injections.LoadOrderInjections.SteamUtilities;

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

            bool noReporters = Environment.GetCommandLineArgs().Any(_arg => _arg == "-norep");
            if(noReporters)
                NoReportersPatch(assemblyDefinition);

            if (!poke && Config.SoftDLLDependancy) {
                FindAssemblySoftPatch(assemblyDefinition);
            }
            
            // TODO uncomment after understanding how CS prevents double loading during hot reload
            // NoDoubleLoadPatch(assemblyDefinition);
            
            assemblyDefinition = LoadAssembliesPatch(assemblyDefinition);
            //assemblyDefinition = LoadPluginsPatch(assemblyDefinition); // its loaded in ASCPatch.LoadDLL() instead
            AddAssemlyPatch(assemblyDefinition);
            assemblyDefinition = AddPluginsPatch(assemblyDefinition);
#if DEBUG
            //assemblyDefinition = InsertPrintStackTrace(assemblyDefinition);
#endif
            
            EnsureIncludedExcludedPackagePatch(assemblyDefinition);

            bool noAssets = Environment.GetCommandLineArgs().Any(_arg => _arg == "-noAssets");
            if (noAssets) {
                assemblyDefinition = NoCustomAssetsPatch(assemblyDefinition);
            } else {
                ExcludeAssetFilePatch(assemblyDefinition);
                ExcludeAssetDirPatch(assemblyDefinition);
            }

            LoadPluginPatch(assemblyDefinition);

            return assemblyDefinition;
        }

        // avoid memory leak by skipping reporters.
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

        /// <summary>
        /// loads assembly with symbols
        /// </summary>
        public void LoadPluginPatch(AssemblyDefinition CM) {
            Log.StartPatching();
            var module = CM.MainModule;
            var tPluginManager = module.GetType("ColossalFramework.Plugins", "PluginManager");

            MethodDefinition mTarget = tPluginManager.GetMethod("LoadPlugin");
            var ilprocessor = mTarget.Body.GetILProcessor();
            var instructions = mTarget.Body.Instructions;

            string fpsMethod = "LoadOrScanAndPatch";
            bool touchedByFPS = instructions.Any(code => code.Calls(fpsMethod));
            if (touchedByFPS) {
                Log.Info("ignoring LoadPluginPatch because FPSBooster already loads symbols");
                return;
            }

            var mrInjection = module.ImportReference(
                GetType().GetMethod(nameof(LoadPlugingWithSymbols)));

            ilprocessor.Prefix(
                Instruction.Create(OpCodes.Ldarg_1),
                Instruction.Create(OpCodes.Call, mrInjection),
                Instruction.Create(OpCodes.Ret));

            Log.Successful();
        }

        public static Assembly LoadPlugingWithSymbols(string dllPath) {
            try {
                Assembly assembly;
                string symPath = dllPath + ".mdb";
                if (File.Exists(symPath)) {
                    CODebugBase<InternalLogChannel>.Log(InternalLogChannel.Mods, "Loading " + dllPath + "\nSymbols " + symPath);
                    assembly = Assembly.Load(File.ReadAllBytes(dllPath), File.ReadAllBytes(symPath));
                } else {
                    CODebugBase<InternalLogChannel>.Log(InternalLogChannel.Mods, "Loading " + dllPath);
                    assembly = Assembly.Load(File.ReadAllBytes(dllPath));
                }
                if (assembly != null) {
                    CODebugBase<InternalLogChannel>.Log(InternalLogChannel.Mods, "Assembly " + assembly.FullName + " loaded.");
                } else {
                    CODebugBase<InternalLogChannel>.Error(InternalLogChannel.Mods, "Assembly at " + dllPath + " failed to load.");
                }
                return assembly;
            } catch (Exception ex) {
                CODebugBase<InternalLogChannel>.Error(InternalLogChannel.Mods, "Assembly at " + dllPath + " failed to load.\n" + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// if the assembly already exists in the current domain, 
        /// do not double load it.
        /// </summary>
        /// <param name="CM"></param>
        public void NoDoubleLoadPatch(AssemblyDefinition CM) {
            Log.StartPatching();
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

            Log.Successful();
        }

        /// <summary>
        /// Load dependant dll even if the version does not match.
        /// </summary>
        /// <param name="CM"></param>
        public void FindAssemblySoftPatch(AssemblyDefinition CM) {
            Log.StartPatching();
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

            Log.Successful();
        }

        public void AddAssemlyPatch(AssemblyDefinition CM) {
            Log.StartPatching();
            var module = CM.MainModule;
            Log.Info("PluginInfo is :" +  module.Types.FirstOrDefault(t => t.Name.EndsWith("PluginManager")).FullName);

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

            Log.Successful();
        }

        public AssemblyDefinition NoCustomAssetsPatch(AssemblyDefinition CM)
        {
            Log.StartPatching();
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

                Log.Info("patching " + mTarget.Name);
                if (loadPath) {
                    // skip method only if path is asset path
                    var ret = instructions.Last();
                    var LdArgPath = mTarget.GetLDArg("path");
                    var mIsAssetPath = GetType().GetMethod(nameof(IsAssetPath));
                    var callIsAssetPath = Instruction.Create(OpCodes.Call, module.ImportReference(mIsAssetPath));
                    var skipIfAsset = Instruction.Create(OpCodes.Brtrue, ret);
                    ilProcessor.Prefix(LdArgPath, callIsAssetPath, skipIfAsset);
                } else {
                    // return to skip method.
                    var ret = Instruction.Create(OpCodes.Ret);
                    ilProcessor.Prefix(ret);
                }
            }

            Log.Successful();
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
            Log.StartPatching();
            var cm = CM.MainModule;
            var type = cm.GetType("ColossalFramework.Packaging.PackageManager");
            var mTargets = type.Methods.Where(_m => _m.Name == "LoadPackages");

            foreach (var mTarget in mTargets) {
                ILProcessor ilProcessor = mTarget.Body.GetILProcessor();
                var instructions = mTarget.Body.Instructions;

                var callGetFiles = instructions.FirstOrDefault(_c => _c.Calls("GetFiles"));
                if( callGetFiles != null) {
                    Log.Info($"patching {mTarget}");
                    var mCheckFiles = GetType().GetMethod(nameof(CheckFiles));
                    var callCheckFiles = Instruction.Create(OpCodes.Call, cm.ImportReference(mCheckFiles));
                    ilProcessor.InsertBefore(callGetFiles, callCheckFiles);
                }
            }
            Log.Successful();
        }

        public static string CheckFiles(string path) {
            SteamUtilities.EnsureIncludedOrExcludedFiles(path);
            return path;
        }

        public void ExcludeAssetFilePatch(AssemblyDefinition CM) {
            try {
                Log.StartPatching();
                var module = CM.MainModule;
                var type = module.GetType("ColossalFramework.Packaging.PackageManager");

                //void Update(string path)
                //void Update(PublishedFileId id, string path)
                var mTargets = type.Methods.Where(_m => _m.Name == "Update").ToList();

                // LoadPackages(string path, bool)
                var mLoadPackages = type.Methods.Single(_m =>
                   _m.Name == "LoadPackages" && _m.HasParameter("path"));

                mTargets.Add(mLoadPackages);


                foreach (var mTarget in mTargets) {
                    ILProcessor ilProcessor = mTarget.Body.GetILProcessor();
                    var instructions = mTarget.Body.Instructions;
                    var first = instructions.First();
                    var last = instructions.Last();

                    // skip method only if path is asset path
                    var LdArgPath = mTarget.GetLDArg("path");
                    var mIsExcluded = typeof(Packages).GetMethod(nameof(Packages.IsFileExcluded));
                    var callIsExcluded = Instruction.Create(OpCodes.Call, module.ImportReference(mIsExcluded));
                    var skipIfExcluded = Instruction.Create(OpCodes.Brtrue, last); // goto to return.
                    ilProcessor.Prefix(new[] { LdArgPath, callIsExcluded, skipIfExcluded });
                }

                Log.Successful();
            }catch(Exception ex) {
                Log.Exception(ex);
            }
        }

        public void ExcludeAssetDirPatch(AssemblyDefinition CM) {
            Log.StartPatching();
            var cm = CM.MainModule;
            var type = cm.GetType("ColossalFramework.Packaging.PackageManager");
            var mTarget = type.Methods.Single(
                _m => _m.Name == "LoadPackages" && _m.HasParameter("path"));
            ILProcessor ilProcessor = mTarget.Body.GetILProcessor();
            var instructions = mTarget.Body.Instructions;
            var ret = instructions.Last();

            var loadPath = mTarget.GetLDArg("path");
            var mIsExcluded = GetType().GetMethod(nameof(IsDirExcluded));
            var callIsExcluded = Instruction.Create(OpCodes.Call, cm.ImportReference(mIsExcluded));
            var skipIfExcluded = Instruction.Create(OpCodes.Brtrue, ret);
            ilProcessor.Prefix(loadPath, callIsExcluded, skipIfExcluded);
            Log.Successful();
        }

        public static bool IsDirExcluded(string path) {
            return string.IsNullOrEmpty(path) || path[0] == '_';
        }

#if DEBUG
        // get the stack trace for debugging purposes.
        // modify this mehtod to print the desired stacktrace. 
        public AssemblyDefinition InsertPrintStackTrace(AssemblyDefinition CM)
        {
            Log.StartPatching();
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

            Log.Info("PlatformServiceBehaviour_Awake_Patch applied successfully!");
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
            Log.StartPatching();
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
            Log.Successful();
            return CM;
        }

        /// <summary>
        /// loads LoadOrderInjections.dll at the beggning of PluginManger.LoadPlugins()
        /// </summary>
        public AssemblyDefinition LoadPluginsPatch(AssemblyDefinition CM)
        {
            Log.StartPatching();
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
            Log.Successful();
            return CM;
        }

        /// <summary>
        /// inserts time stamps about adding/enabling plugins.
        /// </summary>
        public AssemblyDefinition AddPluginsPatch(AssemblyDefinition CM)
        {
            Log.StartPatching();
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

            Log.Successful();
            return CM;
        }
    }
}