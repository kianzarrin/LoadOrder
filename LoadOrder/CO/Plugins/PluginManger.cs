#pragma warning disable 
namespace CO.Plugins {
    using CO.IO;
    using CO.PlatformServices;
    using LoadOrderTool;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.ConstrainedExecution;
    using LoadOrderTool.Util;
    using Mono.Cecil;
    using System.Threading;

    public class PluginManager : SingletonLite<PluginManager> {
        public ConfigWrapper ConfigWrapper;
        LoadOrderShared.LoadOrderConfig Config => ConfigWrapper.Config;

        public enum MessageType {
            Error,
            Warning,
            Message
        }

        public enum OverrideState {
            None,
            Disabled,
            Enabled
        }

        public delegate void AddMessageHandler(PluginManager.MessageType type, string message);

        public delegate void PluginsChangedHandler();

        public class PluginInfo {
            // private Type m_UserModImplementation;
            // public List<Assembly> m_Assemblies;
            // public int assemblyCount => m_Assemblies.Count;

            private TypeDefinition m_UserModImplementation;

            private string m_Path;

            private bool m_IsBuiltin;

            private bool m_Unloaded;

            /// <summary> dir name without prefix. </summary>
            private string m_CachedName;

            public string dllName => userModImplementation?.Module.Assembly.Name.Name;

            public bool isBuiltin => m_IsBuiltin;

            public bool IsLocal => publishedFileID == PublishedFileId.invalid;
            public bool IsWorkshop => !IsLocal;

            public string ModPath => m_Path;

            public string ModIncludedPath {
                get {
                    if (dirName.StartsWith("_"))
                        return Path.Combine(parentDirPath, dirName.Substring(1));
                    else
                        return ModPath;
                }
            }

            /// <summary>
            /// the name of the directory that contains the mod
            /// </summary>
            public string dirName => new DirectoryInfo(ModPath).Name;

            /// <summary>
            /// full path to the parent of the mod's directory.
            /// </summary>
            public string parentDirPath => Directory.GetParent(ModPath).FullName;

            /// <summary> dir name without prefix. </summary>
            public string name => m_CachedName;

            public string DisplayText {
                get {
                    string ret = ModInfo.ModName;
                    if(string.IsNullOrEmpty(ret))
                        ret = dllName;
                    if (publishedFileID != PublishedFileId.invalid)
                        ret = $"{publishedFileID.AsUInt64}: " + ret;
                    return ret;
                }
            }

            public string[] DllPaths =>
                Directory.GetFiles(ModPath, "*.dll", SearchOption.AllDirectories);

            /// <summary>
            /// precondition: all dependent assemblies are loaded.
            /// </summary>
            public bool isCameraScript => GetImplementation(kCameraScript) != null;

            public bool IsHarmonyMod() =>
                name == "2040656402" || dllName == "CitiesHarmony";


            /// <summary>
            /// precondition: all dependent assemblies are loaded.
            /// </summary>
            public bool HasUserMod => userModImplementation != null;

            public PublishedFileId publishedFileID => this.m_PublishedFileID;

            bool isIncludedPending_;
            public bool IsIncludedPending {
                get => isIncludedPending_;
                set {
                    if (isIncludedPending_ != value) {
                        isIncludedPending_ = value;
                        PluginManager.instance.ConfigWrapper.Dirty = true;
                    }
                }
            }

            public bool IsIncluded {
                get => !dirName.StartsWith("_");
                set {
                    isIncludedPending_ = value; 
                    if (value == IsIncluded)
                        return;
                    Log.Debug($"set_IsIncluded current value = {IsIncluded} | target value = {value}");
                    string parentPath = Directory.GetParent(m_Path).FullName;
                    string targetDirname =
                        value
                        ? dirName.Substring(1)  // drop _ prefix
                        : "_" + dirName; // add _ prefix
                    string targetPath = Path.Combine(parentPath, targetDirname);
                    MoveToPath(targetPath);
                }
            }

            public void MoveToPath(string targetPath) {
                try {
                    Log.Debug($"moving mod from {m_Path} to {targetPath}");
                    Directory.Move(m_Path, targetPath);
                    if (Directory.Exists(targetPath))
                        Log.Debug($"move successful!");
                    else {
                        Log.Debug($"FAILED!");
                        throw new Exception($"failed to move directory from {m_Path} to {targetPath}");
                    }
                    m_Path = targetPath;
                } catch (Exception ex) {
                    if (userModImplementation.Name == "LoadOrderMod")
                        Log.Error("Cannot move Load Order because LoadOrderTool is in use");
                    else
                        Log.Exception(ex);
                }
            }

            private PublishedFileId m_PublishedFileID = PublishedFileId.invalid;

            private PluginInfo() {
            }

            public PluginInfo(string path, bool builtin, PublishedFileId id) {
                this.m_Path = path;
                this.m_CachedName = Path.GetFileNameWithoutExtension(path);
                if (m_CachedName.StartsWith("_"))
                    m_CachedName = m_CachedName.Substring(1);
                //this.m_Assemblies = new List<Assembly>();
                this.m_IsBuiltin = builtin;
                this.m_PublishedFileID = id;
                this.ModInfo = PluginManager.instance.Config.Mods.FirstOrDefault(
                    _mod => _mod.Path == ModIncludedPath);
                this.ModInfo ??= new LoadOrderShared.ModInfo {
                    Path = ModIncludedPath,
                    LoadOrder = LoadOrderShared.LoadOrderConfig.DefaultLoadOrder,
                };
                isIncludedPending_ = IsIncluded;
                isEnabledPending_ = isEnabled;
            }

            //public bool ContainsAssembly(Assembly asm)
            //{
            //    for (int i = 0; i < this.m_Assemblies.Count; i++) {
            //        if (this.m_Assemblies[i] == asm) {
            //            return true;
            //        }
            //    }
            //    return false;
            //}

            //public bool ContainsAssembly(AssemblyName asmName)
            //{
            //    foreach (Assembly assembly in this.m_Assemblies) {
            //        if (assembly.GetName().FullName == asmName.FullName) {
            //            return true;
            //        }
            //    }
            //    return false;
            //}

            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
            public unsafe int GetLegacyHashCode(string str) {
                fixed (char* ptr = str + RuntimeHelpers.OffsetToStringData / 2) {
                    char* ptr2 = ptr;
                    char* ptr3 = ptr2 + str.Length - 1;
                    int num = 0;
                    while (ptr2 < ptr3) {
                        num = (num << 5) - num + (int)(*ptr2);
                        num = (num << 5) - num + (int)ptr2[1];
                        ptr2 += 2;
                    }
                    ptr3++;
                    if (ptr2 < ptr3) {
                        num = (num << 5) - num + (int)(*ptr2);
                    }
                    return num;
                }
            }

            bool isEnabledPending_;
            public bool IsEnabledPending {
                get => isEnabledPending_;
                set {
                    if (isEnabledPending_ != value) {
                        isEnabledPending_ = value;
                        PluginManager.instance.ConfigWrapper.Dirty = true;
                    }
                }
            }
            public string savedEnabledKey_ =>
                name + GetLegacyHashCode(ModIncludedPath).ToString() + ".enabled";
            public SavedBool SavedEnabled => new SavedBool(savedEnabledKey_, assetStateSettingsFile, def: false, autoUpdate: true);
            public bool isEnabled {
                get => SavedEnabled.value;
                set => SavedEnabled.value = isEnabledPending_ = value;
            }

            public LoadOrderShared.ModInfo ModInfo { get; private set; }

            public void ResetLoadOrder() => LoadOrder = LoadOrderShared.LoadOrderConfig.DefaultLoadOrder;

            public bool HasLoadOrder() => LoadOrder != LoadOrderShared.LoadOrderConfig.DefaultLoadOrder;


            public int LoadOrder {
                get => ModInfo.LoadOrder;
                set {
                    ModInfo.LoadOrder = value;
                    PluginManager.instance.ConfigWrapper.Dirty = true;
                }

            }

            public override string ToString() {
                return $"PluginInfo: path={ModPath} included={IsIncludedPending} enabled={IsEnabledPending} DisplayText={DisplayText} " +
                    $"cachedName={m_CachedName}"; /*assemblies={assembliesString}"*/
            }

            //public string assembliesString {
            //    get {
            //        int assemblyCount = this.assemblyCount;
            //        string strCount = (assemblyCount > 0) ? string.Empty : "No assemblies";
            //        for (int i = 0; i < assemblyCount; i++) {
            //            AssemblyName asmName = this.m_Assemblies[i].GetName();
            //            strCount = string.Concat(new object[]
            //            {
            //                strCount,
            //                asmName.Name,
            //                "[",
            //                asmName.Version,
            //                "]"
            //            });
            //            if (i < assemblyCount - 1) {
            //                strCount += ", ";
            //            }
            //        }
            //        return strCount;
            //    }
            //}

            public TypeDefinition userModImplementation =>
                m_UserModImplementation ??= GetImplementation(PluginManager.kUserMod);

            public TypeDefinition GetImplementation(string fullInterfaceName) =>
                AssemblyUtil.FindImplementation(DllPaths, fullInterfaceName);

            public void ApplyPendingValues() {
                IsIncluded = isIncludedPending_;
                isEnabled = isEnabledPending_;
            }
        }

        //public Type userModImplementation {
        //        get {
        //            return m_UserModImplementation =
        //                m_UserModImplementation ??
        //                GetImplementation(PluginManager.userModType);
        //        }
        //    }
        //    /// <summary>
        //    /// precondition: all dependent assemblies are loaded.            
        //    /// </summary>
        //    public Type GetImplementation(Type type)
        //    {
        //        for (int i = 0; i < this.m_Assemblies.Count; i++) {
        //            try {
        //                var asm = m_Assemblies[i];
        //                if (!asm.GetReferencedAssemblies().Any(_asm => _asm?.Name == "ICities")) {
        //                    // this also filters out non-CS mods.
        //                    continue;
        //                }
        //                foreach (Type type2 in this.m_Assemblies[i].GetExportedTypes()) {
        //                    if (type2.IsClass && !type2.IsAbstract) {
        //                        Type[] interfaces = type2.GetInterfaces();
        //                        if (interfaces.Contains(type)) {
        //                            return type2;
        //                        }
        //                    }
        //                }
        //            } catch (Exception ex) {
        //                Log.Exception(new Exception("this can happen if not all dependencies are laoded", ex));
        //            }
        //        }
        //        return null;
        //    }
        //}

        public static readonly string kModExtension = ".dll";

        public static readonly string kSourceExtension = ".cs";

        public static bool noWorkshop;

        //private Dictionary<Assembly, string> m_AssemblyLocations = new Dictionary<Assembly, string>();

        //private Dictionary<string, PluginInfo> m_Plugins = new Dictionary<string, PluginInfo>();
        private List<PluginInfo> m_Plugins = new List<PluginInfo>();


        public static string assetStateSettingsFile => "userGameState";

        //public static Type userModType => Type.GetType("ICities.IUserMod, ICities");
        public const string kUserMod = "ICities.IUserMod";

        //public static Type cameraScriptType => Type.GetType("ICities.ICameraExtension, ICities");
        public const string kCameraScript = "ICities.ICameraExtension";


        public static event PluginManager.AddMessageHandler eventLogMessage;

        public event PluginManager.PluginsChangedHandler eventPluginsChanged;

        public event PluginManager.PluginsChangedHandler eventPluginsStateChanged;

        public IEnumerable<PluginInfo> GetPluginsInfo() =>
            m_Plugins.Where(p => p.HasUserMod);

        public IEnumerable<PluginInfo> GetCameraPluginInfos() =>
             m_Plugins.Where(p => p.isCameraScript);

        public int modCount => GetPluginsInfo().Count();

        public void LoadPlugins() {
            string builtinModsPath = Path.Combine(DataLocation.gameContentPath, "Mods");
            string addonsModsPath = DataLocation.modsPath;
            m_Plugins = new List<PluginInfo>();

            try {
                this.LoadPluginInfosAtPath(builtinModsPath, true);
                this.LoadPluginInfosAtPath(addonsModsPath, false);
                if (!PluginManager.noWorkshop) {
                    this.LoadWorkshopPluginInfos();
                }
                //this.LoadAssemblies();
                Log.Debug($"{m_Plugins.Count} pluggins loaded.");

                // purge plugins without a IUserMod Implementation. this also filters out non-cs mods.
                // all dependent assemblies must be loaded before doing this.
                foreach (var p in m_Plugins) {
                    Log.Info($"hasUserMod:{p.HasUserMod} " + p);
                }
                m_Plugins = m_Plugins.Where(p => p.HasUserMod).ToList();
                Log.Debug($"{m_Plugins.Count} pluggins remained after purging non-cs or non-mods.");

                var mods = Config.Mods.ToList();

                foreach (var plugin in m_Plugins) {
                    plugin.ModInfo.AssemblyName = plugin.dllName;
                    if (!mods.Any(_mod => _mod == plugin.ModInfo))
                        mods.Add(plugin.ModInfo);
                }
                Config.Mods = mods.ToArray();
                ConfigWrapper.Dirty = true;

            } catch (Exception ex) {
                Log.Exception(ex);
            } finally {
                //this.CreateReporters(modsPath);
            }
        }

        private void LoadPluginInfosAtPath(string path, bool builtin) {
            ContentUtil.EnsureLocalItemsAt(parentDir: path);
            string[] directories = Directory.GetDirectories(path);
            foreach (string dir in directories) {
                //if (!Path.GetFileName(dir).StartsWith("_"))
                m_Plugins.Add(new PluginInfo(dir, builtin, PublishedFileId.invalid));
                //else
                //    PluginManager.LogMessage(PluginManager.MessageType.Message, "Inactive mod path found: " + Path.GetFileName(dir));

            }
        }

        private void LoadWorkshopPluginInfos() {
            var subscribedItems = ContentUtil.GetSubscribedItems();
            Log.Debug($"subscribed items are: " + string.Join(", ", subscribedItems));
            if (subscribedItems != null) {
                foreach (var id in subscribedItems) {
                    string subscribedItemPath = ContentUtil.GetSubscribedItemPath(id);
                    if (subscribedItemPath != null && Directory.Exists(subscribedItemPath)) {
                        Log.Debug("scanned: " + subscribedItemPath);
                        m_Plugins.Add(new PluginInfo(subscribedItemPath, false, id));
                    } else {
                        Log.Debug("direcotry does not exist: " + subscribedItemPath);
                    }

                }
            }
        }
        public void ApplyPendingValues() {
            foreach(var p in GetPluginsInfo())
                p.ApplyPendingValues();
        }

        //private static int sUniqueCompilationID;
        //private static string[] m_AdditionalAssemblies;
        //private void LoadAssemblies()
        //{
        //    foreach (PluginInfo pluginInfo in m_Plugins) {
        //        string modPath = pluginInfo.ModPath;
        //        string[] files = Directory.GetFiles(modPath, "*.dll", SearchOption.AllDirectories);
        //        foreach (string dllpath in files) {
        //            try {
        //                Assembly asm = AssemblyUtil.LoadDLL(dllpath);
        //                pluginInfo.m_Assemblies.Add(asm);
        //                //this.m_AssemblyLocations[asm] = dllpath;
        //                Log.Info("Assembly loaded: " + asm);
        //            } catch (Exception ex) {
        //                Log.Info("Assembly at " + dllpath + " failed to load.\n" + ex.Message);
        //            }
        //        }
        //    }
        //}

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
            if (PluginManager.eventLogMessage != null) {
                PluginManager.eventLogMessage(type, message);
            }
            if (type == PluginManager.MessageType.Error) {
                Log.Error(message);
                return;
            }
            if (type == PluginManager.MessageType.Warning) {
                Log.Error("WARNING: " + message);
                return;
            }
            if (type == PluginManager.MessageType.Message) {
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

        public PluginManager() => ConfigWrapper = new ConfigWrapper();

        static PluginManager()
        {
            if (GameSettings.FindSettingsFileByName(assetStateSettingsFile) == null) {
                GameSettings.AddSettingsFile(new SettingsFile[] { new SettingsFile() { fileName = assetStateSettingsFile } });
            }
        }
    }
}
