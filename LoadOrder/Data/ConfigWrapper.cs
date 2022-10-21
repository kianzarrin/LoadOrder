namespace LoadOrderTool.Data {
    using CO.IO;
    using CO.Packaging;
    using CO.Plugins;
    using CO;
    using LoadOrderTool;
    using System;
    using System.Threading;
    using LoadOrderTool.UI;
    using LoadOrderShared;
    using LoadOrderTool.Util;
    using System.IO;

    public class ConfigWrapper : SingletonLite<ConfigWrapper> {
        public LoadOrderConfig Config;
        public SteamCache SteamCache;
        public CSCache CSCache;

        public LoadingScreenMod.Settings LSMConfig;

        bool dirty_;
        public bool Dirty {
            get => dirty_;
            set {
                dirty_ = value;
                if(LoadOrderWindow.Instance != null)
                    LoadOrderWindow.Instance.Dirty = value;
            }
        }

        Thread m_SaveThread;
        bool m_Run = true;
        object m_LockObject = new object();

        public override void Awake() {
            base.Awake();
            var sw = System.Diagnostics.Stopwatch.StartNew();
            Config = LoadOrderConfig.Deserialize(DataLocation.LocalLOMData)
                ?? new LoadOrderConfig();
            SteamCache = SteamCache.Deserialize() ?? new SteamCache();
            ReloadCSCache();
            LSMConfig = LoadingScreenMod.Settings.Deserialize();
            Dirty = false;
            OnSynched();
            Log.Info($"LoadOrderConfig.Deserialize took {sw.ElapsedMilliseconds}ms");
            if(!CommandLine.Parse.CommandLine)
                StartSaveThread();
        }

        ~ConfigWrapper() => Terminate();

        public void Terminate() {
            m_Run = false;
            lock(m_LockObject)
                Monitor.Pulse(m_LockObject);
            Log.Info("LoadOrderConfig terminated");
        }

        public bool AutoSync {
            get {
                if(m_SaveThread == null)
                    return false; // command line
                else
                    return LoadOrderToolSettings.Instace.AutoSync;
            }
            set {
                if (value != LoadOrderToolSettings.Instace.AutoSync) {
                    LoadOrderToolSettings.Instace.AutoSync = value;
                    LoadOrderToolSettings.Instace.Serialize();
                }
                if (LoadOrderWindow.Instance != null) {
                    LoadOrderWindow.Instance.menuStrip.tsmiAutoSync.Checked = value;
                }
            }
        }


        public bool Advanced {
            get => LoadOrderToolSettings.Instace.Advanced;
            set {
                if (LoadOrderToolSettings.Instace.Advanced != value) {
                    LoadOrderToolSettings.Instace.Advanced = value;
                    LoadOrderToolSettings.Instace.Serialize();
                }
                LoadOrderWindow.Instance?.menuStrip?.OnAdvancedChanged();
                LoadOrderWindow.Instance?.launchControl?.OnAdvancedChanged();

            }
        }

        public bool Paused { get; set; } = false;
        public void Suspend() => Paused = true;
        public void Resume() => Paused = false;

        public void ReloadCSCache() => CSCache = CSCache.Deserialize(DataLocation.LocalLOMData) ?? new CSCache();
        public void ResetCSCache() {
            CSCache = new CSCache() {
                WorkShopContentPath = CSCache?.WorkShopContentPath,
                SteamPath = CSCache?.SteamPath,
                GamePath = CSCache?.GamePath,
            };
            CSCache.Serialize(DataLocation.LocalLOMData);
        }

        public void ResetAllConfig() {
            AutoSync = false;

            Config = new LoadOrderConfig {
                WorkShopContentPath = DataLocation.WorkshopContentPath,
                GamePath = DataLocation.GamePath,
                SteamPath = DataLocation.SteamPath,
            };
            Config.Serialize(DataLocation.LocalLOMData);

            ResetCSCache();
            SteamCache = new SteamCache();
            SteamCache.Serialize();

            foreach (var pluginInfo in PluginManager.instance.GetMods()) {
                try {
                    pluginInfo.ResetLoadOrder();
                    pluginInfo.IsIncluded = true;
                } catch (Exception ex) {
                    Log.Exception(ex, pluginInfo.ToString(), false);
                }
            }

            LoadOrderToolSettings.Reset();
            LoadOrderToolSettings.Instace.Serialize();
            
            Dirty = false;
            AutoSync = LoadOrderToolSettings.Instace.AutoSync;
        }

        public void ReloadAllConfig() {
            try {
                Log.Called();
                Assertion.Assert(Paused, "pause config before doing this");
                Dirty = false;
                Config = LoadOrderConfig.Deserialize(DataLocation.LocalLOMData)
                    ?? new LoadOrderConfig();
                ReloadCSCache();
                LSMConfig = LoadingScreenMod.Settings.Deserialize();
                PluginManager.instance.ModStateSettingsFile.Load(); // deserialize
                Log.Succeeded();
                OnSynched();
            } catch(Exception ex) {
                ex.Log();
            }
        }

        DateTime LastSynched = DateTime.MinValue;
        public void OnSynched() => LastSynched = DateTime.UtcNow;

        public void ReloadIfNewer() {
            try {
                Assertion.Assert(Paused, "pause config before doing this");
                DateTime maxUTC = File.GetLastWriteTimeUtc(Path.Combine(DataLocation.LocalLOMData, LoadOrderConfig.FILE_NAME));

                DateTime utc = File.GetLastWriteTimeUtc(Path.Combine(DataLocation.LocalLOMData, CSCache.FILE_NAME));
                if (utc > maxUTC) maxUTC = utc;

                utc = PluginManager.instance?.ModStateSettingsFile?.GetLastWriteTimeUtc() ?? default;
                if (utc > maxUTC) maxUTC = utc;

                utc = File.GetLastWriteTimeUtc(LoadingScreenMod.Settings.FILE_PATH);
                if (utc > maxUTC) maxUTC = utc;

                if(utc > LastSynched) {
                    Paused = true;
                    ReloadAllConfig();

                    ManagerList.instance?.ReloadConfig();
                    LoadOrderWindow.Instance?.launchControl?.LoadSettings();

                    Dirty = false;
                    Paused = false;
                    LoadOrderWindow.Instance?.RefreshAll();
                }

                Log.Succeeded();
            } catch (Exception ex) {
                ex.Log();
            }
        }

        public void SaveConfig() {
            if (!AutoSync) {
                SaveConfigImpl();
            } else if (Thread.CurrentThread != m_SaveThread) {
                Dirty = true;
            }
        }

        private void SaveConfigImpl() {
            Dirty = false;
            ManagerList.instance.Save(); // saves but not serialize
            PluginManager.instance.ApplyPendingValues(); // saves to game config and moves folders.
            Config.Serialize(DataLocation.LocalLOMData);
            SteamCache.Serialize();
            LSMConfig = LSMConfig.SyncAndSerialize();
            OnSynched();
            Log.Info($"SaveConfigImpl() done. (Dirty={Dirty})");
        }

        private void StartSaveThread() {
            Log.Info("Load Order Config Monitor Started...");
            m_SaveThread = new Thread(new ThreadStart(MonitorSave));
            m_SaveThread.Name = "SaveSettingsThread";
            m_SaveThread.IsBackground = true;
            m_SaveThread.Start();
        }

        private void MonitorSave() {
            try {
                while (m_Run) {
                    if(AutoSync) {
                        if (Dirty) {
                            SaveConfigImpl();
                        } else {
                            ReloadIfNewer();
                        }
                    }

                    lock (m_LockObject)
                        Monitor.Wait(m_LockObject, 100);
                    while (m_Run && Paused) {
                        lock (m_LockObject)
                            Monitor.Wait(m_LockObject, 100); // wait here while paused
                    }
                }

                Log.Info("Load Order Config Monitor Exiting...");
            } catch (Exception ex) {
                Log.Exception(ex);
            }
        }
    }
}
