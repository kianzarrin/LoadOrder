namespace LoadOrderTool.Util {
    using Mono.Cecil;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using CO.IO;
    public static class AssemblyUtil {
        public static Assembly LoadDLL(string dllpath) {
            try {
                Assembly asm = Assembly.Load(File.ReadAllBytes(dllpath));
                if (asm != null)
                    Log.Info("Assembly loaded: " + asm);
                else
                    Log.Info("Assembly at " + dllpath + " failed to load.");
                return asm;
            } catch (Exception ex) {
                Log.Info("Assembly at " + dllpath + " failed to load.\n" + ex.Message);
                return null;
            }
        }

        public static AssemblyDefinition ReadAssemblyDefinition(string dllpath) {
            try {
                var r = new MyAssemblyResolver();
                r.AddSearchDirectory(DataLocation.ManagedDLL);
                r.AddSearchDirectory(Path.GetDirectoryName(dllpath));
                var readerParameters = new ReaderParameters {
                    ReadWrite = false,
                    InMemory = true,
                    AssemblyResolver = r,
                };
                r.ReaderParameters = readerParameters;
                var asm = AssemblyDefinition.ReadAssembly(dllpath, readerParameters);

                if (asm != null)
                    Log.Info("Assembly Definition loaded: " + asm, false);
                else
                    Log.Info("Assembly Definition at " + dllpath + " failed to load.", false);
                
                return asm;
            } catch (Exception ex) {
                Log.Info("Assembly Definition at " + dllpath + " failed to load.\n" + ex.Message, false);
                return null;
            }
        }

        public static TypeDefinition FindImplementation(string[] dllPaths, string fullInterfaceName, out string dllPath) {
            foreach (string path in dllPaths) {
                try {
                    var asm = ReadAssemblyDefinition(path);
                    var userMod = asm?.GetExportedImplementation(fullInterfaceName);
                    if (userMod != null) {
                        dllPath = path;
                        return userMod;
                    }
                } catch (Exception ex) {
                    Log.Exception(ex, showInPanel: false);
                }
            }
            dllPath = null;
            return null;
        }

        public static TypeDefinition GetExportedImplementation(this AssemblyDefinition asm, string fullInterfaceName) =>
            asm.MainModule.Types.FirstOrDefault(t =>
                !t.IsAbstract && t.IsPublic && t.ImplementsInterface(fullInterfaceName));
        
        public static bool ImplementsInterface(this TypeDefinition type, string fullInterfaceName) {
            return type.GetAllInterfaces().Any(i => i.FullName == fullInterfaceName);
        }

        public static IEnumerable<TypeReference> GetAllInterfaces(this TypeDefinition type) {
            while (type != null) {
                foreach (var i in type.Interfaces)
                    yield return i.InterfaceType;
                try {
                    type = type.BaseType?.Resolve();
                } catch(AssemblyResolutionException) {
                    Log.Info($"[harmless] GetAllInterfaces({type}) could not resolve {type.BaseType}.", false);
                    type = null;
                } catch (Exception ex) {
                    ex.Log(false);
                    type = null;
                }
            }
        }

        public static byte[] ReadBytesFromGetManifestResource(string name) {
            try {
                using (var stream = typeof(AssemblyUtil).Assembly.GetManifestResourceStream(name)) {
                    byte[] array = new byte[stream.Length];
                    stream.Read(array, 0, array.Length);
                    return array;
                }
            }catch(Exception ex) {
                Log.Exception(ex);
                return null;
            }
        }
    }
}
