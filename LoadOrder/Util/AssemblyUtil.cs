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
                    Log.Info("Assembly Definition loaded: " + asm);
                else
                    Log.Info("Assembly Definition at " + dllpath + " failed to load.");
                
                return asm;
            } catch (Exception ex) {
                Log.Info("Assembly Definition at " + dllpath + " failed to load.\n" + ex.Message);
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
                } catch(AssemblyResolutionException ex0) {
                    Log.Info($"[harmless] GetAllInterfaces({type}) could not resolve {type.BaseType}.");
                    type = null;
                } catch (Exception ex) {
                    ex.Log(false);
                    type = null;
                }
            }
        }

        public static byte[] GetDebugMono() {
            string[] resources = typeof(AssemblyUtil).Assembly.GetManifestResourceNames();
            foreach (var resource in resources)
                Log.Debug("resource: " + resource);
            string debugMonoName = resources.First(r => r.ToLower().Contains("mono-debug"));
            return ReadBytesFromGetManifestResource(debugMonoName);
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

        public static string DebugMonoPath => Path.Combine(DataLocation.MonoPath, "mono-debug.dll");
        public static string ReleaseMonoPath => Path.Combine(DataLocation.MonoPath, "mono-orig.dll");
        public static string MonoPath => Path.Combine(DataLocation.MonoPath, "mono.dll");

        public static void EnsureDebugMonoWritten() {
            if (File.Exists(DebugMonoPath))
                return;
            var data = GetDebugMono();
            File.WriteAllBytes(DebugMonoPath, data);
        }

        public static void EnsureBReleaseMonoBackedup() {
            if (File.Exists(ReleaseMonoPath))
                return;
            File.Copy(MonoPath, ReleaseMonoPath);
        }

        public static bool FilesEqual(string path1, string path2) {
            return new FileInfo(path1).Length == new FileInfo(path2).Length;
        }

        public static void UseDebugMono() {
            try {
                EnsureBReleaseMonoBackedup();
                EnsureDebugMonoWritten();
                CopyMono(source: DebugMonoPath, dest: MonoPath);

            } catch (Exception ex) {
                Log.Exception(ex);
            }
        }
        public static void UseReleaseMono() {
            try {
                if(File.Exists(ReleaseMonoPath))
                    CopyMono(source: ReleaseMonoPath, dest: MonoPath);
            } catch (Exception ex) {
                Log.Exception(ex);
            }
        }

        public static void CopyMono(string source, string dest) {
            if (FilesEqual(source, dest))
                return; // already the same
            File.Delete(dest);
            File.Copy(sourceFileName: source, destFileName: dest);
        }




    }
}
