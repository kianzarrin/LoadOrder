using Mono.Cecil;
using Mono.Cecil.Cil;
using Patch.API;
using System;
using System.Linq;
using static LoadOrderIPatch.Commons;
using ILogger = Patch.API.ILogger;


namespace LoadOrderIPatch.Patches {
    extern alias Injections;

    public class ASCPatch : IPatch {
        public int PatchOrderAsc { get; } = 99;
        public AssemblyToPatch PatchTarget { get; } = new AssemblyToPatch("Assembly-CSharp", new Version());
        private string workingPath_;

        public AssemblyDefinition Execute(
            AssemblyDefinition assemblyDefinition,
            ILogger logger,
            string patcherWorkingPath,
            IPaths gamePaths) {
            try {
                SubscriptionManagerPatch(assemblyDefinition); // must be called to check if patch loader is effective.
            } catch (Exception ex) { logger.Error(ex.ToString()); }
            return assemblyDefinition;
        }

        /// <summary>
        /// replaces the normal loading process with subscription manger tool
        /// </summary>
        public AssemblyDefinition SubscriptionManagerPatch(AssemblyDefinition ASC) {
            Log.StartPatching();
            var module = ASC.Modules.First();
            MethodDefinition mTarget = module.GetMethod("Starter.Awake");
            var instructions = mTarget.Body.Instructions;
            ILProcessor ilProcessor = mTarget.Body.GetILProcessor();
            AssemblyDefinition asm = GetInjectionsAssemblyDefinition();

            /**********************************/


            /**********************************/
            var injectionMethod = asm.MainModule.GetMethod("LoadOrderInjections.DoNothingComponent.DoNothing");
            var callInjection = Instruction.Create(OpCodes.Call, module.ImportReference(injectionMethod));
            Instruction brLast = Instruction.Create(OpCodes.Br, instructions.Last()); // return

            ilProcessor.Prefix(callInjection, brLast);

            Instruction CallBoot = instructions.First(_c => _c.Calls("Boot"));
            ilProcessor.InsertAfter(CallBoot, callInjection, brLast);

            Log.Successful();
            return ASC;
        }
    }
}