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

namespace LoadOrderIPatch.Patches {
    public class ASCPatch : IPatch {
        public int PatchOrderAsc { get; } = 100;
        public AssemblyToPatch PatchTarget { get; } = new AssemblyToPatch("Assembly-CSharp", new Version());
        private ILogger logger_;
        private string workingPath_;

        public AssemblyDefinition Execute(AssemblyDefinition assemblyDefinition, ILogger logger, string patcherWorkingPath)
        {
            logger_ = logger;
            workingPath_ = patcherWorkingPath;

            assemblyDefinition = ImproveLoggingPatch(assemblyDefinition);
            assemblyDefinition = BindEnableDisableAllPatch(assemblyDefinition);
            //assemblyDefinition = NewsFeedPanelPatch(assemblyDefinition); // handled by harmony patch
            LoadDLL(Path.Combine(workingPath_, InjectionsDLL));

            bool sman = Environment.GetCommandLineArgs().Any(_arg => _arg == "-sman");
            //if (sman) 
            {
                assemblyDefinition = SubscriptionManagerPatch(assemblyDefinition);
            }

             // assemblyDefinition = NoQueryPatch(assemblyDefinition); // handled by harmony patch

            return assemblyDefinition;
        }

        public Assembly LoadDLL(string dllPath)
        {
            void Log(string _m) => logger_.Info(_m);
            try {
                Assembly assembly;
                string symPath = dllPath + ".mdb";
                if (File.Exists(symPath)) {
                    Log("\nLoading " + dllPath + "\nSymbols " + symPath);
                    assembly = Assembly.Load(File.ReadAllBytes(dllPath), File.ReadAllBytes(symPath));
                } else {
                    Log("Loading " + dllPath);
                    assembly = Assembly.Load(File.ReadAllBytes(dllPath));
                }
                if (assembly != null) {
                    Log("Assembly " + assembly.FullName + " loaded.\n");
                } else {
                    Log("Assembly at " + dllPath + " failed to load.\n");
                }
                return assembly;
            } catch (Exception ex) {
                logger_.Error("Assembly at " + dllPath + " failed to load.\n" + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// replaces the normal loading process with subscription manger tool
        /// </summary>
        public AssemblyDefinition SubscriptionManagerPatch(AssemblyDefinition ASC)
        {
            logger_.LogStartPatching();
            var module = ASC.Modules.First();
            var tPluginManager = module.Types
                .First(_t => _t.FullName == "Starter");
            MethodDefinition mTarget = tPluginManager.Methods
                .First(_m => _m.Name == "Awake");
            var instructions = mTarget.Body.Instructions;
            ILProcessor ilProcessor = mTarget.Body.GetILProcessor();
            AssemblyDefinition asm = GetInjectionsAssemblyDefinition(workingPath_);

            /**********************************/
            var method = asm.MainModule.GetMethod(
                "LoadOrderInjections.SubscriptionManager.PostBootAction");
            var call1 = Instruction.Create(OpCodes.Call, module.ImportReference(method));
            Instruction CallBoot = instructions.First(_c => _c.Calls("Boot"));
            Instruction BranchTarget = instructions.Last();// return
            Instruction BrTrueEnd = Instruction.Create(OpCodes.Brtrue, BranchTarget);

            ilProcessor.InsertAfter(CallBoot, call1); 
            ilProcessor.InsertAfter(call1, BrTrueEnd);

            /**********************************/
            method = asm.MainModule.GetMethod(
                "LoadOrderInjections.SteamUtilities.RegisterEvents");
            var call2 = Instruction.Create(OpCodes.Call, module.ImportReference(method));
            ilProcessor.InsertBefore(instructions.First(), call2);
            /**********************************/

            logger_.LogSucessfull();
            return ASC;
        }

        /// <summary>
        /// removes call to query which causes steam related errors and puts CollossalManged in an unstable state.
        /// </summary>
        public AssemblyDefinition NoQueryPatch(AssemblyDefinition ASC) {
            logger_.LogStartPatching();
            var module = ASC.Modules.First();
            var targetMethod = module.GetMethod("WorkshopAdPanel.Awake");
            var instructions = targetMethod.Body.Instructions;
            ILProcessor ilProcessor = targetMethod.Body.GetILProcessor();
            Instruction callQuery = instructions.First(_c => _c.Calls("QueryItems"));
            ilProcessor.Remove(callQuery); // the pop instruction after cancels out the load instruction before.
            logger_.LogSucessfull();
            return ASC;
        }

        public AssemblyDefinition LoadPluginsPatch(AssemblyDefinition ASC)
        {
            logger_.LogStartPatching();
            var tPluginManager = ASC.MainModule.Types
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
            return ASC;
        }

        public AssemblyDefinition ImproveLoggingPatch(AssemblyDefinition ASC)
        {
            logger_.LogStartPatching();
            ModuleDefinition module = ASC.Modules.First();
            var entryPoint = GetEntryPoint(module);
            var mInjection = GetType().GetMethod(nameof(ApplyGameLoggingImprovements));
            var mrInjection = module.ImportReference(mInjection);

            Instruction first = entryPoint.Body.Instructions.First();
            Instruction callInjection = Instruction.Create(OpCodes.Call, mrInjection);

            ILProcessor ilProcessor = entryPoint.Body.GetILProcessor();
            ilProcessor.InsertBefore(first, callInjection);

            logger_.LogSucessfull();
            return ASC;
        }

        private MethodDefinition GetEntryPoint(ModuleDefinition module)
            => module.GetMethod("Starter.Awake");

        /// <summary>
        /// Reconfigure Unity logger to remove empty lines of call stack.
        /// Stacktrace is disabled by Unity when game runs in Build mode anyways.
        /// </summary>
        public static void ApplyGameLoggingImprovements()
        {
            Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
            Application.SetStackTraceLogType(LogType.Warning, StackTraceLogType.None);
            Application.SetStackTraceLogType(LogType.Error, StackTraceLogType.ScriptOnly);
            Application.SetStackTraceLogType(LogType.Exception, StackTraceLogType.ScriptOnly);
            Debug.Log("************************** Removed logging stacktrace bloat **************************");
        }


        public AssemblyDefinition BindEnableDisableAllPatch(AssemblyDefinition ASC)
        {
            logger_.LogStartPatching();
            var module = ASC.MainModule;
            var mTarget = module.GetMethod("ContentManagerPanel.BindEnableDisableAll");

            // set disclaimer ID to null. this avoids OnSettingsUI getting called all the time.
            Instruction first = mTarget.Body.Instructions.First();
            Instruction loadNull = Instruction.Create(OpCodes.Ldnull);
            ParameterDefinition pDisclaimerID = mTarget.Parameters.Single(_p => _p.Name == "disclaimerID");
            Instruction storeDisclaimerID = Instruction.Create(OpCodes.Starg, pDisclaimerID);
            
            ILProcessor ilProcessor = mTarget.Body.GetILProcessor();
            ilProcessor.InsertBefore(first, loadNull);
            ilProcessor.InsertAfter(loadNull, storeDisclaimerID);
            logger_.LogSucessfull();

            return ASC;
        }

        public AssemblyDefinition NewsFeedPanelPatch(AssemblyDefinition ASC)
        {
            logger_.LogStartPatching();
            var module = ASC.MainModule;
            Instruction ret = Instruction.Create(OpCodes.Ret);
            {
                var mTarget = module.GetMethod("NewsFeedPanel.RefreshFeed");
                Instruction first = mTarget.Body.Instructions.First();
                ILProcessor ilProcessor = mTarget.Body.GetILProcessor();
                ilProcessor.InsertBefore(first, ret);
            }
            {
                var mTarget = module.GetMethod("NewsFeedPanel.OnFeedNext");
                Instruction first = mTarget.Body.Instructions.First();
                ILProcessor ilProcessor = mTarget.Body.GetILProcessor();
                ilProcessor.InsertBefore(first, ret);
            }

            logger_.LogSucessfull();
            return ASC;
        }

    }
}