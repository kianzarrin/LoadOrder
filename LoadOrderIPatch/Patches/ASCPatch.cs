using Mono.Cecil;
using Mono.Cecil.Cil;
using Patch.API;
using System;
using System.Linq;
using UnityEngine;
using static LoadOrderIPatch.Commons;
using ILogger = Patch.API.ILogger;


namespace LoadOrderIPatch.Patches {
    public class ASCPatch : IPatch {
        public int PatchOrderAsc { get; } = 99;
        public AssemblyToPatch PatchTarget { get; } = new AssemblyToPatch("Assembly-CSharp", new Version());


        public AssemblyDefinition Execute(
            AssemblyDefinition assemblyDefinition, 
            ILogger logger, 
            string patcherWorkingPath,
            IPaths gamePaths) {
            try
            {
                assemblyDefinition = SubscriptionManagerPatch(assemblyDefinition); // must be called to check if patch loader is effective.
            } catch (Exception ex) { logger.Error(ex.ToString());  }
            return assemblyDefinition;
        }

        /// <summary>
        /// replaces the normal loading process with subscription manger tool
        /// </summary>
        public AssemblyDefinition SubscriptionManagerPatch(AssemblyDefinition ASC)
        {
            Log.StartPatching();
            var module = ASC.Modules.First();
            MethodDefinition mTarget = module.GetMethod("Starter.Awake");
            var instructions = mTarget.Body.Instructions;
            ILProcessor ilProcessor = mTarget.Body.GetILProcessor();
            AssemblyDefinition asm = GetInjectionsAssemblyDefinition(Entry.PatcherWorkingPath);

            /**********************************/
            {
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
                var method = asm.MainModule.GetMethod(
                    "LoadOrderInjections.SteamUtilities.RegisterEvents");
                var call2 = Instruction.Create(OpCodes.Call, module.ImportReference(method));
                ilProcessor.Prefix(call2);
            }
            /**********************************/
            bool sman2 = Environment.GetCommandLineArgs().Any(_arg => _arg == "-sman2");
            if(sman2)
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

    public class DoNothingComponent : MonoBehaviour {
        void Awake() => UnityEngine.Debug.Log("TestComponent.Awake() was called");
        void Start() => UnityEngine.Debug.Log("TestComponent.Start() was called");
        public static void DoNothing() {
            new GameObject().AddComponent<Camera>();
            new GameObject("nop go").AddComponent<DoNothingComponent>();
        }

    }
}