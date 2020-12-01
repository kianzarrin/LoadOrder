using Mono.Cecil;
using Mono.Cecil.Cil;
using Patch.API;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using ILogger = Patch.API.ILogger;

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
            assemblyDefinition = SubscriptionManagerPatch(assemblyDefinition);
            
            var dllPath = Path.Combine(workingPath_, "LoadOrderMod.dll");
            LoadDLL(dllPath);

            return assemblyDefinition;
        }

        private Assembly LoadDLL(string dllPath)
        {
            try {
                Assembly assembly;
                string symPath = dllPath + ".mdb";
                if (File.Exists(symPath)) {
                    logger_.Info("\nLoading " + dllPath + "\nSymbols " + symPath);
                    assembly = Assembly.Load(File.ReadAllBytes(dllPath), File.ReadAllBytes(symPath));
                } else {
                    logger_.Info("Loading " + dllPath);
                    assembly = Assembly.Load(File.ReadAllBytes(dllPath));
                }
                if (assembly != null) {
                    logger_.Info("Assembly " + assembly.FullName + " loaded.\n");
                } else {
                    logger_.Info("Assembly at " + dllPath + " failed to load.\n");
                }
                return assembly;
            } catch (Exception ex) {
                logger_.Info("Assembly at " + dllPath + " failed to load.\n" + ex.ToString());
                return null;
            }
        }

        public AssemblyDefinition GetAssemblyDefinition(string fileName)
            => CecilUtil.GetAssemblyDefinition(workingPath_, fileName);

        public AssemblyDefinition SubscriptionManagerPatch(AssemblyDefinition ASC)
        {
            logger_.Info("SubManagerPatch.Execute() called ...");
            var module = ASC.Modules.First();
            var tPluginManager = module.Types
                .First(_t => _t.FullName == "Starter");
            MethodDefinition mTarget = tPluginManager.Methods
                .First(_m => _m.Name == "Awake");
            var instructions = mTarget.Body.Instructions;
            ILProcessor ilProcessor = mTarget.Body.GetILProcessor();

            AssemblyDefinition asmMod = GetAssemblyDefinition("LoadOrderMod.dll");
            var tSortPlugins = asmMod.MainModule.Types
                .First(_t => _t.Name == "SubscriptionManager");
            MethodDefinition mdPostBootAction = tSortPlugins.Methods
                .First(_m => _m.Name == "PostBootAction");
            var mrInjection = module.ImportReference(mdPostBootAction);

            Instruction CallBoot = instructions.First(_c => _c.Calls("Boot"));
            Instruction callInjection = Instruction.Create(OpCodes.Call, mrInjection);
            Instruction BranchTarget = instructions.Last();// return
            Instruction BrTrueEnd = Instruction.Create(OpCodes.Brtrue, BranchTarget);

            ilProcessor.InsertAfter(CallBoot, callInjection); // load pluggins arg
            ilProcessor.InsertAfter(callInjection, BrTrueEnd);
            logger_.Info("SubscriptionManagerPatch applied successfully!");
            return ASC;
        }

        public AssemblyDefinition LoadPluginsPatch(AssemblyDefinition ASC)
        {
            var tPluginManager = ASC.MainModule.Types
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
            return ASC;
        }

        public AssemblyDefinition ImproveLoggingPatch(AssemblyDefinition ASC)
        {
            ModuleDefinition module = ASC.Modules.First();
            var entryPoint = GetEntryPoint(module);
            var mInjection = GetType().GetMethod(nameof(ApplyGameLoggingImprovements));
            var mrInjection = module.ImportReference(mInjection);

            Instruction first = entryPoint.Body.Instructions.First();
            Instruction callInjection = Instruction.Create(OpCodes.Call, mrInjection);

            ILProcessor ilProcessor = entryPoint.Body.GetILProcessor();
            ilProcessor.InsertBefore(first, callInjection);

            logger_.Info("ImproveLoggingPatch applied successfully!");
            return ASC;
        }

        private MethodDefinition GetEntryPoint(ModuleDefinition module)
        {
            TypeDefinition type = module.Types.FirstOrDefault(t => t.Name == "Starter")
                ?? throw new Exception("Starter not found");
            return type.Methods.FirstOrDefault(method => method.Name.Equals("Awake"))
                ?? throw new Exception("Starter.Awake() not found");
        }

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
        //PreCreateUserModInstance
    }
}