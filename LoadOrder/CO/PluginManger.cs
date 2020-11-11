using ColossalFramework.IO;
using ColossalFramework.PlatformServices;
using ICities;
//using ColossalFramework.Threading;
//using ColossalFramework.UI;
//using UnityEngine;
using LoadOrder;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ColossalFramework.Plugins
{
    public class PluginManager : SingletonLite<PluginManager>
    {
        public static IEnumerable<PublishedFileId> GetSubscribedItems()
        {
            foreach (var dir in Directory.GetDirectories(DataLocation.SteamContentPath))
            {
                string dir2;
                if (dir.StartsWith("_"))
                    dir2 = dir.Remove(0, 1);
                else
                    dir2 = dir;
                if (!ulong.TryParse(dir2, out ulong id)) continue;
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

            private PluginManager.OverrideState m_OverrideState;

            private string m_CachedName;

            public string m_dllName;

            public string m_textName;

            public string m_finalName;

            private PublishedFileId m_PublishedFileID = PublishedFileId.invalid;

            private PluginInfo()
            {
            }

            public PluginInfo(string path, bool builtin, PublishedFileId id)
            {
                this.m_Path = path;
                this.m_CachedName = Path.GetFileNameWithoutExtension(path);
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

            public void Process()
            {
                isCameraScript = GetImplementation(cameraScriptType) != null;
                isMod = userModImplementation != null;
            }

            public bool isCameraScript { get; private set; }
            public bool isMod { get; private set; }

            public int assemblyCount => m_Assemblies.Count;

            public bool isBuiltin => this.m_IsBuiltin;

            public bool isEnabledNoOverride
            {
                get
                {
                    //if (this.m_Unloaded)
                    //{
                    //    return false;
                    //}
                    if (!string.IsNullOrEmpty(PluginManager.assetStateSettingsFile))
                    {
                        SavedBool savedBool = new SavedBool(this.name + this.modPath.GetHashCode().ToString() + ".enabled", PluginManager.assetStateSettingsFile, false);
                        return savedBool.value;
                    }
                    return true;
                }
            }

            public bool isEnabled
            {
                get
                {
                    if (this.m_Unloaded)
                    {
                        return false;
                    }
                    if (this.m_OverrideState != PluginManager.OverrideState.None)
                    {
                        return this.m_OverrideState == PluginManager.OverrideState.Enabled;
                    }
                    if (!string.IsNullOrEmpty(PluginManager.assetStateSettingsFile))
                    {
                        SavedBool savedBool = new SavedBool(this.name + this.modPath.GetHashCode().ToString() + ".enabled", PluginManager.assetStateSettingsFile, false);
                        return savedBool.value;
                    }
                    return true;
                }
                set
                {
                    this.m_OverrideState = PluginManager.OverrideState.None;
                    if (!string.IsNullOrEmpty(PluginManager.assetStateSettingsFile))
                    {
                        SavedBool savedBool = new SavedBool(this.name + this.modPath.GetHashCode().ToString() + ".enabled", PluginManager.assetStateSettingsFile, false);
                        savedBool.value = value;
                    }
                }
            }

            public PluginManager.OverrideState overrideState
            {
                get
                {
                    return this.m_OverrideState;
                }
                set
                {
                    if (value != this.m_OverrideState)
                    {
                        bool flag = true;
                        if (this.m_OverrideState == PluginManager.OverrideState.None)
                        {
                            if (!string.IsNullOrEmpty(PluginManager.assetStateSettingsFile))
                            {
                                SavedBool savedBool = new SavedBool(this.name + this.modPath.GetHashCode().ToString() + ".enabled", PluginManager.assetStateSettingsFile, false);
                                flag = savedBool.value;
                            }
                        }
                        else
                        {
                            flag = (this.m_OverrideState == PluginManager.OverrideState.Enabled);
                        }
                        this.m_OverrideState = value;
                        bool flag2 = true;
                        if (this.m_OverrideState == PluginManager.OverrideState.None)
                        {
                            if (!string.IsNullOrEmpty(PluginManager.assetStateSettingsFile))
                            {
                                SavedBool savedBool2 = new SavedBool(this.name + this.modPath.GetHashCode().ToString() + ".enabled", PluginManager.assetStateSettingsFile, false);
                                flag2 = savedBool2.value;
                            }
                        }
                        else
                        {
                            flag2 = (this.m_OverrideState == PluginManager.OverrideState.Enabled);
                        }
                        if (flag2 != flag)
                        {

                        }
                    }
                }
            }

            public string name => this.m_CachedName;

            public string modPath => this.m_Path;

            public PublishedFileId publishedFileID => this.m_PublishedFileID;

            public override string ToString()
            {
                int assemblyCount = this.assemblyCount;
                string text = (assemblyCount > 0) ? string.Empty : "No assemblies";
                for (int i = 0; i < assemblyCount; i++)
                {
                    text = text + this.m_Assemblies[i].GetName().Name + ".dll";
                    if (i < assemblyCount - 1)
                    {
                        text += ", ";
                    }
                }
                return string.Format("{0} [{1}]", this.modPath, text);
            }

            public string assembliesString
            {
                get
                {
                    int assemblyCount = this.assemblyCount;
                    string text = (assemblyCount > 0) ? string.Empty : "No assemblies";
                    for (int i = 0; i < assemblyCount; i++)
                    {
                        AssemblyName name = this.m_Assemblies[i].GetName();
                        object obj = text;
                        text = string.Concat(new object[]
                        {
                            obj,
                            name.Name,
                            "[",
                            name.Version,
                            "]"
                        });
                        if (i < assemblyCount - 1)
                        {
                            text += ", ";
                        }
                    }
                    return text;
                }
            }


            public object userModImplementation
            {
                get
                {
                    return m_UserModImplementation =
                        m_UserModImplementation ??
                        GetImplementation(PluginManager.userModType);
                }
            }

            public Type GetImplementation(Type type)
            {
                for (int i = 0; i < this.m_Assemblies.Count; i++)
                {
                    try
                    {
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
                    catch (Exception exception)
                    {
                        Log.Exception(exception);
                    }
                }
                return null;
            }
        }

        public static readonly string kModExtension = ".dll";

        public static readonly string kSourceExtension = ".cs";

        public static bool noWorkshop;

        private Dictionary<Assembly, string> m_AssemblyLocations = new Dictionary<Assembly, string>();

        private Dictionary<string, PluginManager.PluginInfo> m_Plugins = new Dictionary<string, PluginManager.PluginInfo>();

        private static int sUniqueCompilationID;

        private static string[] m_AdditionalAssemblies;

        public static string assetStateSettingsFile { get; set; }

        public static Type userModType { get; set; } = typeof(IUserMod);

        public static Type cameraScriptType { get; set; } = typeof(ICameraExtension);

        public static event PluginManager.AddMessageHandler eventLogMessage;

        public event PluginManager.PluginsChangedHandler eventPluginsChanged;

        public event PluginManager.PluginsChangedHandler eventPluginsStateChanged;

        public IEnumerable<PluginManager.PluginInfo> GetPluginsInfo()
        {
            foreach (KeyValuePair<string, PluginManager.PluginInfo> p in this.m_Plugins)
            {
                KeyValuePair<string, PluginManager.PluginInfo> keyValuePair = p;
                if (!keyValuePair.Value.isCameraScript)
                {
                    KeyValuePair<string, PluginManager.PluginInfo> keyValuePair2 = p;
                    yield return keyValuePair2.Value;
                }
            }
            yield break;
        }

        public IEnumerable<PluginManager.PluginInfo> GetCameraPluginInfos()
        {
            foreach (KeyValuePair<string, PluginManager.PluginInfo> p in this.m_Plugins)
            {
                KeyValuePair<string, PluginManager.PluginInfo> keyValuePair = p;
                if (keyValuePair.Value.isCameraScript)
                {
                    KeyValuePair<string, PluginManager.PluginInfo> keyValuePair2 = p;
                    yield return keyValuePair2.Value;
                }
            }
            yield break;
        }

        public int modCount
        {
            get
            {
                return this.m_Plugins.Count;
            }
        }

        public int enabledModCountNoOverride
        {
            get
            {
                int num = 0;
                foreach (KeyValuePair<string, PluginManager.PluginInfo> keyValuePair in this.m_Plugins)
                {
                    if (keyValuePair.Value.isEnabledNoOverride)
                    {
                        num++;
                    }
                }
                return num;
            }
        }

        public int enabledModCount
        {
            get
            {
                int num = 0;
                foreach (KeyValuePair<string, PluginManager.PluginInfo> keyValuePair in this.m_Plugins)
                {
                    if (keyValuePair.Value.isEnabled)
                    {
                        num++;
                    }
                }
                return num;
            }
        }

        public void LoadPlugins()
        {
            string builtinModsPath = Path.Combine(DataLocation.gameContentPath, "Mods");
            string addonsModsPath = DataLocation.modsPath;
            Dictionary<string, PluginManager.PluginInfo> plugins = new Dictionary<string, PluginManager.PluginInfo>();
            try
            {
                this.LoadPluginInfosAtPath(plugins, builtinModsPath, true);
                this.LoadPluginInfosAtPath(plugins, addonsModsPath, false);
                if (!PluginManager.noWorkshop)
                {
                    this.LoadWorkshopPluginInfos(plugins);
                }
                this.LoadAssemblies(plugins);
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

        private void LoadPluginInfosAtPath(Dictionary<string, PluginManager.PluginInfo> plugins, string path, bool builtin)
        {
            string[] directories = Directory.GetDirectories(path);
            foreach (string dir in directories)
            {
                //if (!Path.GetFileName(dir).StartsWith("_"))
                plugins[dir] = new PluginManager.PluginInfo(dir, builtin, PublishedFileId.invalid);
                //else
                //    PluginManager.LogMessage(PluginManager.MessageType.Message, "Inactive mod path found: " + Path.GetFileName(dir));

            }
        }

        private void LoadWorkshopPluginInfos(Dictionary<string, PluginManager.PluginInfo> plugins)
        {
            var subscribedItems = GetSubscribedItems();
            if (subscribedItems != null)
            {
                foreach (var id in subscribedItems)
                {
                    string subscribedItemPath = GetSubscribedItemPath(id);
                    if (subscribedItemPath != null && Directory.Exists(subscribedItemPath))
                    {
                        plugins[subscribedItemPath] = new PluginManager.PluginInfo(subscribedItemPath, false, id);
                    }
                }
            }
        }

        private void LoadAssemblies(Dictionary<string, PluginManager.PluginInfo> plugins)
        {
            foreach (PluginManager.PluginInfo pluginInfo in plugins.Values)
            {
                string modPath = pluginInfo.modPath;
                string[] files = Directory.GetFiles(modPath, "*.dll", SearchOption.AllDirectories);
                foreach (string dllpath in files)
                {
                    try
                    {
                        Assembly asm = Assembly.Load(dllpath);
                        pluginInfo.m_Assemblies.Add(asm);
                        this.m_AssemblyLocations[asm] = dllpath;
                    }
                    catch (Exception ex)
                    {
                        Log.Error("Assembly at " + dllpath + " failed to load.\n" + ex.ToString());
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

        public PluginManager.PluginInfo FindPluginInfo(Assembly asmRO)
        {
            foreach (PluginManager.PluginInfo pluginInfo in this.m_Plugins.Values)
            {
                if (pluginInfo.isEnabled && pluginInfo.ContainsAssembly(asmRO))
                {
                    return pluginInfo;
                }
            }
            return null;
        }

        public PluginManager()
        {
        }

        static PluginManager()
        {
        }
    }
}
