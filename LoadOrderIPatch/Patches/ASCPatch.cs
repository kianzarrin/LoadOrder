using ColossalFramework;
using ColossalFramework.Packaging;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Patch.API;
using System;
using System.Linq;
using UnityEngine;
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
                Entry.Logger = logger;
                SubscriptionManagerPatch(assemblyDefinition); // must be called to check if patch loader is effective.
            } catch (Exception ex) { logger.Error(ex.ToString());  }
            return assemblyDefinition;
        }

        /// <summary>
        /// replaces the normal loading process with subscription manger tool
        /// </summary>
        public void SubscriptionManagerPatch(AssemblyDefinition ASC)
        {
            Log.StartPatching();
            var module = ASC.Modules.First();
            MethodDefinition mTarget = module.GetMethod("Starter.Awake");
            var instructions = mTarget.Body.Instructions;
            ILProcessor ilProcessor = mTarget.Body.GetILProcessor();
            var method = typeof(DoNothingComponent).GetMethod("DoNothing");

            /**********************************/
            {
                var callInjection = Instruction.Create(OpCodes.Call, module.ImportReference(method));
                Instruction brLast = Instruction.Create(OpCodes.Br, instructions.Last()); // return
                //ilProcessor.Prefix(callInjection, brLast);
            }
            {
                var callInjection = Instruction.Create(OpCodes.Call, module.ImportReference(method));
                Instruction brLast = Instruction.Create(OpCodes.Br, instructions.Last()); // return
                Instruction CallBoot = instructions.First(_c => _c.Calls("Boot"));
                ilProcessor.InsertAfter(CallBoot, callInjection, brLast);
            }


            Log.Successful();
        }

    }

    public class DoNothingComponent : MonoBehaviour {
        void Awake() => Debug.Log("DoNothingComponent.Awake() was called");
        void Start() => Debug.Log("DoNothingComponent.Start() was called");
        public static void DoNothing() {
            new GameObject().AddComponent<Camera>();
            new GameObject("nop go").AddComponent<DoNothingComponent>();
        }

    }
}