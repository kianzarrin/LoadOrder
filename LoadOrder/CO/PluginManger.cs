using ColossalFramework.IO;
using ColossalFramework.PlatformServices;
using ICities;
//using ColossalFramework.Threading;
//using ColossalFramework.UI;
//using UnityEngine;
using LoadOrder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;

namespace ColossalFramework.Plugins
{
    public class PluginManager : SingletonLite<PluginManager>
    {
        public static bool TryGetID(string dir, out ulong id)
        {
            string dir2;
            if (dir.StartsWith("_"))
                dir2 = dir.Remove(0, 1);
            else
                dir2 = dir;
            return ulong.TryParse(dir2, out id);
        }

        public static IEnumerable<PublishedFileId> GetSubscribedItems()
        {
            foreach (var dir in Directory.GetDirectories(DataLocation.SteamContentPath))
            {
                if (!TryGetID(dir, out ulong id)) continue;
                //if (!Directory.GetFiles(dir, "*.dll").Any()) continue;
                yield return new PublishedFileId(id);
            }
        }

        public static string GetSubscribedItemPath(PublishedFileId id)
        {
            var ret = Path.Combine(DataLocation.SteamContentPath, id.AsUInt64.ToString());
            if (File.Exists(ret))
                return ret;
            ret = Path.Combine(DataLocation.SteamContentPath, "_" + id.AsUInt64.ToString());
            if (File.Exists(ret))
                return ret;
            return null;
        }

        public enum MessageType
        {
            Error,
            Warning,
            Message
        }

        public enum OverrideState
        {
            None,
            Disabled,
            Enabled
        }

        public delegate void AddMessageHandler(PluginManager.MessageType type, string message);

        public delegate void PluginsChangedHandler();

        public class PluginInfo
        {
            private Type m_UserModImplementation;

            public List<Assembly> m_Assemblies;

            private string m_Path;

            private bool m_IsBuiltin;

            private bool m_Unloaded;

            private string m_CachedName;

            public string dllName => userModImplementation?.Assembly.GetName().Name;

            public bool isBuiltin => this.m_IsBuiltin;

            public string Path => this.m_Path;

            public string dirName => this.m_CachedName;

            public string name => this.m_CachedName;

            public string DisplayText
            {
                get
                {
                    string ret = dllName;
                    var id = publishedFileID;
                    if (id != PublishedFileId.invalid)
                    {
                        ret = $"{id.AsUInt64}: " + ret;
                    }
                    return ret;
                }
            }

            /// <summary>
            /// precondition: all dependent assemblies are loaded.
            /// </summary>
            public bool isCameraScript => GetImplementation(cameraScriptType) != null;

            /// <summary>
            /// precondition: all dependent assemblies are loaded.
            /// </summary>
            public bool HasUserMod => userModImplementation != null;

            public PublishedFileId publishedFileID => this.m_PublishedFileID;

            public int assemblyCount => m_Assemblies.Count;

            public bool IsIncluded
            {
                get => !dirName.StartsWith("_");
                set
                {
                    if (value == IsIncluded)
                        return;
                    string targetPath;
                    if (value)
                        targetPath = m_Path.Substring(1); // drop starting _
                    else
                        targetPath = "_" + m_Path; // add starting _
                    MoveToPath(targetPath);
                }
            }

            public void MoveToPath(string targetPath)
            {
                Directory.Move(Path, targetPath);
                m_Path = targetPath;
            }

            private PublishedFileId m_PublishedFileID = PublishedFileId.invalid;

            private PluginInfo()
            {
            }

            public PluginInfo(string path, bool builtin, PublishedFileId id)
            {
                this.m_Path = path;
                this.m_CachedName = System.IO.Path.GetFileNameWithoutExtension(path);
                this.m_Assemblies = new List<Assembly>();
                this.m_IsBuiltin = builtin;
                this.m_PublishedFileID = id;
            }

            public bool ContainsAssembly(Assembly asm)
            {
                for (int i = 0; i < this.m_Assemblies.Count; i++)
                {
                    if (this.m_Assemblies[i] == asm)
                    {
                        return true;
                    }
                }
                return false;
            }

            public bool ContainsAssembly(AssemblyName asmName)
            {
                foreach (Assembly assembly in this.m_Assemblies)
                {
                    if (assembly.GetName().FullName == asmName.FullName)
                    {
                        return true;
                    }
                }
                return false;
            }

            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
            public unsafe int GetLegacyHashCode(string str)
            {
                fixed (char* ptr = str + RuntimeHelpers.OffsetToStringData / 2)
                {
                    char* ptr2 = ptr;
                    char* ptr3 = ptr2 + str.Length - 1;
                    int num = 0;
                    while (ptr2 < ptr3)
                    {
                        num = (num << 5) - num + (int)(*ptr2);
                        num = (num << 5) - num + (int)ptr2[1];
                        ptr2 += 2;
                    }
                    ptr3++;
                    if (ptr2 < ptr3)
                    {
                        num = (num << 5) - num + (int)(*ptr2);
                    }
                    return num;
                }
            }

            private string savedEnabledKey_ => name + GetLegacyHashCode(Path).ToString() + ".enabled";
            public SavedBool SavedEnabled => new SavedBool(savedEnabledKey_, assetStateSettingsFile, false);
            public bool isEnabled
            {
                get => SavedEnabled.value;
                set => SavedEnabled.value = value;
            }

            private string savedLoadIndexKey_ => dllName + ".Order";
            public SavedInt SavedOrder => new SavedInt(savedLoadIndexKey_, LoadOrderSettingsFile, 0);
            public int LoadOrder
            {
                get => SavedOrder.value;
                set => SavedOrder.value = value; 
            }


            public override string ToString()
            {
                return $"PluginInfo: path={Path} included={IsIncluded} enabled={isEnabled} DisplayText={DisplayText} " +
                    $"cachedName={m_CachedName} assemblies=" + assembliesString;
            }

            public string assembliesString
            {
                get
                {
                    int assemblyCount = this.assemblyCount;
                    string strCount = (assemblyCount > 0) ? string.Empty : "No assemblies";
                    for (int i = 0; i < assemblyCount; i++)
                    {
                        AssemblyName asmName = this.m_Assemblies[i].GetName();
                        strCount = string.Concat(new object[]
                        {
                            strCount,
                            asmName.Name,
                            "[",
                            asmName.Version,
                            "]"
                        });
                        if (i < assemblyCount - 1)
                        {
                            strCount += ", ";
                        }
                    }
                    return strCount;
                }
            }


            public Type userModImplementation
            {
                get
                {
                    return m_UserModImplementation =
                        m_UserModImplementation ??
                        GetImplementation(PluginManager.userModType);
                }
            }
            /// <summary>
            /// precondition: all dependent assemblies are loaded.            
            /// </summary>
            public Type GetImplementation(Type type)
            {
                for (int i = 0; i < this.m_Assemblies.Count; i++)
                {
                    try
                    {
                        var asm = m_Assemblies[i];
                        if (!asm.GetReferencedAssemblies().Any(_asm => _asm?.Name == "ICities"))
                        {
                            // this also filters out non-CS mods.
                            continue;
                        }
                        foreach (Type type2 in this.m_Assemblies[i].GetExportedTypes())
                        {
                            if (type2.IsClass && !type2.IsAbstract)
                            {
                                Type[] interfaces = type2.GetInterfaces();
                                if (interfaces.Contains(type))
                                {
                                    return type2;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Exception(new Exception("this can happen if not all dependencies are laoded", ex));
                    }
                }
                return null;
            }
        }

        public static readonly string kModExtension = ".dll";

        public static readonly string kSourceExtension = ".cs";

        public static bool noWorkshop;

        //private Dictionary<Assembly, string> m_AssemblyLocations = new Dictionary<Assembly, string>();

        //private Dictionary<string, PluginInfo> m_Plugins = new Dictionary<string, PluginInfo>();
        private List<PluginInfo> m_Plugins = new List<PluginInfo>();


        private static int sUniqueCompilationID;

        private static string[] m_AdditionalAssemblies;
         
        public static string assetStateSettingsFile => "userGameState";
        public static string LoadOrderSettingsFile => "LoadOrder";

        public static Type userModType { get; set; } = typeof(IUserMod);

        public static Type cameraScriptType { get; set; } = typeof(ICameraExtension);

        public static event PluginManager.AddMessageHandler eventLogMessage;

        public event PluginManager.PluginsChangedHandler eventPluginsChanged;

        public event PluginManager.PluginsChangedHandler eventPluginsStateChanged;

        public IEnumerable<PluginInfo> GetPluginsInfo() =>
            m_Plugins.Where(p => p.HasUserMod);

        public IEnumerable<PluginInfo> GetCameraPluginInfos() =>
             m_Plugins.Where(p => p.isCameraScript);

        public int modCount => GetPluginsInfo().Count();

        public int enabledModCount =>
            GetPluginsInfo().Count(p => p.isEnabled);

        public void LoadPlugins()
        {
            string builtinModsPath = Path.Combine(DataLocation.gameContentPath, "Mods");
            string addonsModsPath = DataLocation.modsPath;
            m_Plugins = new List<PluginInfo>();

            try
            {
                this.LoadPluginInfosAtPath(builtinModsPath, true);
                this.LoadPluginInfosAtPath(addonsModsPath, false);
                if (!PluginManager.noWorkshop)
                {
                    this.LoadWorkshopPluginInfos();
                }
                this.LoadAssemblies();
                Log.Debug($"{m_Plugins.Count} pluggins loaded.");

                // purge plugins without a IUserMod Implementation. this also filters out non-cs mods.
                // all dependent assemblies must be loaded before doing this.
                m_Plugins = m_Plugins.Where(p => p.HasUserMod).ToList();
                Log.Debug($"{m_Plugins.Count} pluggins remained after purging non-cs or non-mods.");
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
            finally
            {
                //this.CreateReporters(modsPath);
            }
        }

        private void LoadPluginInfosAtPath(string path, bool builtin)
        {
            string[] directories = Directory.GetDirectories(path);
            foreach (string dir in directories)
            {
                //if (!Path.GetFileName(dir).StartsWith("_"))
                m_Plugins.Add(new PluginInfo(dir, builtin, PublishedFileId.invalid));
                //else
                //    PluginManager.LogMessage(PluginManager.MessageType.Message, "Inactive mod path found: " + Path.GetFileName(dir));

            }
        }

        private void LoadWorkshopPluginInfos()
        {
            var subscribedItems = GetSubscribedItems();
            if (subscribedItems != null)
            {
                foreach (var id in subscribedItems)
                {
                    string subscribedItemPath = GetSubscribedItemPath(id);
                    if (subscribedItemPath != null && Directory.Exists(subscribedItemPath))
                    {
                        m_Plugins.Add(new PluginInfo(subscribedItemPath, false, id));
                    }
                }
            }
        }

        private void LoadAssemblies()
        {
            foreach (PluginInfo pluginInfo in m_Plugins)
            {
                string modPath = pluginInfo.Path;
                string[] files = Directory.GetFiles(modPath, "*.dll", SearchOption.AllDirectories);
                foreach (string dllpath in files)
                {
                    try
                    {
                        Assembly asm = Assembly.LoadFrom(dllpath);
                        pluginInfo.m_Assemblies.Add(asm);
                        //this.m_AssemblyLocations[asm] = dllpath;
                        Log.Info("Assembly loaded: " + asm);
                    }
                    catch (Exception ex)
                    {
                        Log.Info("Assembly at " + dllpath + " failed to load.\n" + ex.Message);
                    }
                }
            }
        }

        //public static void SetAdditionalAssemblies(params string[] assemblies)
        //{
        //    PluginManager.m_AdditionalAssemblies = assemblies;
        //}

        //public static void CompileScripts()
        //{
        //    PluginManager.CompileScripts(DataLocation.modsPath, PluginManager.m_AdditionalAssemblies);
        //}

        //public static void CompileScripts(string modPath, params string[] additionalAssemblies)
        //{
        //    try
        //    {
        //        string[] directories = Directory.GetDirectories(modPath);
        //        foreach (string text in directories)
        //        {
        //            if (!Path.GetFileName(text).StartsWith("_"))
        //            {
        //                string text2 = Path.Combine(text, "Source");
        //                if (Directory.Exists(text2))
        //                {
        //                    PluginManager.CompileSourceInFolder(text2, text, additionalAssemblies);
        //                }
        //                else
        //                {
        //                    PluginManager.LogMessage(PluginManager.MessageType.Message, "No source files found: " + Path.GetFileName(text));
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        PluginManager.LogMessage(PluginManager.MessageType.Error, ex.ToString());
        //    }
        //}

        //public static void CompileSourceInFolder(string sourcePath, string outputPath, params string[] additionalAssemblies)
        //{
        //    try
        //    {
        //        string[] files = Directory.GetFiles(sourcePath, "*.cs", SearchOption.AllDirectories);
        //        if (files.Length > 0)
        //        {
        //            PluginManager.CompileSource(files, Path.Combine(outputPath, Path.GetFileName(outputPath) + ".dll"), additionalAssemblies);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        PluginManager.LogMessage(PluginManager.MessageType.Error, ex.ToString());
        //    }
        //}

        private static void LogMessage(PluginManager.MessageType type, string message)
        {
            if (PluginManager.eventLogMessage != null)
            {
                PluginManager.eventLogMessage(type, message);
            }
            if (type == PluginManager.MessageType.Error)
            {
                Log.Error(message);
                return;
            }
            if (type == PluginManager.MessageType.Warning)
            {
                Log.Error("WARNING: " + message);
                return;
            }
            if (type == PluginManager.MessageType.Message)
            {
                Log.Info(message);
            }
        }

        //private static bool CompileSource(string[] sourceFiles, string output, params string[] additionalAssemblies)
        //{
        //    Log.Info("Compiling " + string.Join(", ", sourceFiles));
        //    ColossalCSharpCodeProvider colossalCSharpCodeProvider = new ColossalCSharpCodeProvider();
        //    CompilerParameters compilerParameters = new CompilerParameters();
        //    foreach (string text in additionalAssemblies)
        //    {
        //        if (Path.IsPathRooted(text))
        //        {
        //            compilerParameters.ReferencedAssemblies.Add(text);
        //        }
        //        else
        //        {
        //            compilerParameters.ReferencedAssemblies.Add(DataLocation.ManagedDLL);
        //        }
        //    }
        //    string text2 = Path.Combine(DataLocation.addonsPath, "Temp");
        //    if (!Directory.Exists(text2))
        //    {
        //        Directory.CreateDirectory(text2);
        //    }
        //    string text3 = Path.Combine(text2, Path.GetFileNameWithoutExtension(output) + PluginManager.sUniqueCompilationID.ToString() + ".dll");
        //    PluginManager.sUniqueCompilationID++;
        //    compilerParameters.ReferencedAssemblies.Add(typeof(PluginManager).Assembly.Location);
        //    compilerParameters.OutputAssembly = text3;
        //    compilerParameters.GenerateInMemory = false;
        //    compilerParameters.TreatWarningsAsErrors = false;
        //    CompilerResults compilerResults = colossalCSharpCodeProvider.CompileAssemblyFromFile(compilerParameters, sourceFiles);
        //    if (compilerResults.Errors.HasErrors)
        //    {
        //        PluginManager.LogMessage(PluginManager.MessageType.Error, "Errors building assembly '" + output + "'");
        //        foreach (CompilerError compilerError in compilerResults.Errors)
        //        {
        //            if (!compilerError.IsWarning)
        //                PluginManager.LogMessage(PluginManager.MessageType.Error, compilerError.ToString());
        //        }
        //    }
        //    else
        //    {
        //        if (compilerResults.Errors.HasWarnings)
        //        {
        //            PluginManager.LogMessage(PluginManager.MessageType.Warning, "Warnings building assembly '" + output + "'");
        //        }
        //        foreach (object obj2 in compilerResults.Errors)
        //        {
        //            CompilerError compilerError2 = (CompilerError)obj2;
        //            if (compilerError2.IsWarning)
        //            {
        //                PluginManager.LogMessage(PluginManager.MessageType.Warning, compilerError2.ToString());
        //            }
        //        }
        //        string text4 = Path.Combine(DataLocation.modsPath, output);
        //        PluginManager.LogMessage(PluginManager.MessageType.Message, "Assembly '" + text4 + "' compiled successfully");
        //        if (File.Exists(text4))
        //        {
        //            File.Replace(text3, text4, null);
        //        }
        //        else
        //        {
        //            File.Move(text3, text4);
        //        }
        //    }
        //    DirectoryUtils.DeleteDirectory(text2);
        //    return !compilerResults.Errors.HasErrors;
        //}

        public PluginInfo FindPluginInfo(Assembly asmRO) =>
            m_Plugins.FirstOrDefault(p => p.isEnabled && p.ContainsAssembly(asmRO));


        public PluginManager()
        {
        }

        static PluginManager()
        {
            if (GameSettings.FindSettingsFileByName(assetStateSettingsFile) == null)
            {
                GameSettings.AddSettingsFile(new SettingsFile[] { new SettingsFile() { fileName = assetStateSettingsFile } });
            }
            if (GameSettings.FindSettingsFileByName(LoadOrderSettingsFile) == null)
            {
                GameSettings.AddSettingsFile(new SettingsFile[] { new SettingsFile() { fileName = LoadOrderSettingsFile } });
            }
        }
    }
}
