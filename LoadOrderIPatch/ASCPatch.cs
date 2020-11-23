using Mono.Cecil;
using Mono.Cecil.Cil;
using Patch.API;
using System;
using System.IO;
using System.Linq;
using UnityEngine;
using ILogger = Patch.API.ILogger;

namespace LoadOrderIPatch.Patches {
    public class ASCPatch : IPatch {
        public int PatchOrderAsc { get; } = 100;
        public AssemblyToPatch PatchTarget { get; } = new AssemblyToPatch("Assembly-CSharp", new Version()); private ILogger logger_;
        private string workingPath_;
        public AssemblyDefinition Execute(AssemblyDefinition assemblyDefinition, ILogger logger, string patcherWorkingPath)
        {
            logger_ = logger;
            workingPath_ = patcherWorkingPath;

            assemblyDefinition = ImproveLoggingPatch(assemblyDefinition);
            return assemblyDefinition;
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