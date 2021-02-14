namespace LoadOrderInjections.Injections {
    using System;
    using System.Reflection;
    using KianCommons;
    using ColossalFramework.Plugins;
    using System.Linq;

    public static class AddAssembly {
        public static void Prefix(Assembly asm) {
            try {
                Log.Info($"Poking {asm.Name()} ...", true);
                Log.Info("calling GetExportedTypes() for " + asm);
                asm.GetExportedTypes(); // public only
                Log.Info("GetExportedTypes sucessfull!");


                //var asmRO = AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies()
                //    .First(_a => _a.FullName == asm.FullName);
                //Log.Info("calling GetTypes() on asmRO: " + asmRO);
                //var types = asmRO.GetTypes(); // including non-public
                //Log.Info("asmRO.GetTypes() sucessfull!");

                //// this way we can understand what is causing the problem:
                //Log.Info("getting every type individutally from assembly.");
                //foreach(var type in types) {
                //    try {
                //        asm.GetType(type.FullName, true);
                //    } catch(Exception ex) {
                //        throw new TypeLoadException($"asm.GetType({type.FullName}) failed", ex);
                //    }
                //}
                //Log.Info("asmRO.GetTypes() sucessfull!");

                Log.Info("calling asm.GetTypes()");
                asm.GetTypes(); // including non-public
                Log.Info("GetTypes() sucessfull!");
            } catch (Exception ex) {
                Log.Exception(ex);
            }
        }

    }
}
