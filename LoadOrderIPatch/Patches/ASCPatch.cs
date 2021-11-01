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
            if(false){
                var method = asm.MainModule.GetMethod(
                    "LoadOrderInjections.SubscriptionManager.PostBootAction");
                var call1 = Instruction.Create(OpCodes.Call, module.ImportReference(method));
                Instruction CallBoot = instructions.First(_c => _c.Calls("Boot"));
                Instruction BranchTarget = instructions.Last();// return
                Instruction BrTrueEnd = Instruction.Create(OpCodes.Brtrue, BranchTarget);

                ilProcessor.InsertAfter(CallBoot, call1, BrTrueEnd);
            }

            /**********************************/
            {
                var method = asm.MainModule.GetMethod("LoadOrderInjections.SubscriptionManager.DoNothing");
                var callInjection = Instruction.Create(OpCodes.Call, module.ImportReference(method));
                Instruction brLast = Instruction.Create(OpCodes.Br, instructions.Last()); // return
                ilProcessor.Prefix(callInjection, brLast);
            }

            Log.Successful();
            return ASC;
        }
    }
}