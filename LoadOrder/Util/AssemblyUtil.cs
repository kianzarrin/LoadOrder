namespace LoadOrderTool.Util {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;
    using System.Reflection;
    using System.Linq;
    using Mono.Cecil;

    public static class AssemblyUtil {
        public static Assembly LoadDLL(string dllpath) {
            try {
                Assembly asm = Assembly.Load(File.ReadAllBytes(dllpath));
                if(asm!=null)
                    Log.Info("Assembly loaded: " + asm);
                else
                    Log.Info("Assembly at " + dllpath + " failed to load.");
                return asm;
            } catch (Exception ex) {
                Log.Info("Assembly at " + dllpath + " failed to load.\n" + ex.Message);
                return null;
            }
        }

        public static AssemblyDefinition LoadAssemblyDefinition(string dllpath) {
            try {
                var asm = AssemblyDefinition.ReadAssembly(dllpath);
                if (asm != null)
                    Log.Info("Assembly Definition loaded: " + asm);
                else
                    Log.Info("Assembly Definition at " + dllpath + " failed to load.");
                return asm;
            } catch (Exception ex) {
                Log.Info("Assembly Definition at " + dllpath + " failed to load.\n" + ex.Message);
                return null;
            }
        }

        public static TypeDefinition FindImplementation(string[] dllPaths, string fullInterfaceName) {
            foreach(string dllpath in dllPaths) {
                try {
                    var asm = LoadAssemblyDefinition(dllpath);
                    var userMod = asm?.GetImplementation(fullInterfaceName);
                    if (userMod != null)
                        return userMod;
                } catch (Exception ex) {
                    Log.Exception(ex, showInPanel: false);
                }
            }
            return null;
        }

        public static TypeDefinition GetImplementation(this AssemblyDefinition asm, string fullInterfaceName) =>
            asm.MainModule.Types.FirstOrDefault(t => t.Implements(fullInterfaceName));
        
        public static bool Implements(this TypeDefinition type, string fullInterfaceName) =>
            type.Interfaces.Any(_interface => _interface.InterfaceType.FullName == fullInterfaceName);
    }
}
