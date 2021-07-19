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

    public class ConfigWrapper : SingletonLite<ConfigWrapper> {
        public LoadOrderConfig Config;
        public LoadingScreenMod.Settings LSMConfig;

        bool dirty_;
        public bool Dirty {
            get => dirty_;
            set {
                dirty_ = value;
                if (LoadOrderWindow.Instance != null)
                    LoadOrderWindow.Instance.Dirty = value;
            }
        }

        Thread m_SaveThread;
        bool m_Run = true;
        object m_LockObject = new object();

        public ConfigWrapper() {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            Config = LoadOrderConfig.Deserialize(DataLocation.localApplicationData)
                ?? new LoadOrderConfig();
            Log.Info($"LoadOrderConfig.Deserialize took {sw.ElapsedMilliseconds}ms");
            LSMConfig = LoadingScreenMod.Settings.Deserialize();
            if(!CommandLine.Parse.CommandLine)
                StartSaveThread();
        }

        ~ConfigWrapper() => Terminate();

        public void Terminate() {
            m_Run = false;
            lock (m_LockObject)
                Monitor.Pulse(m_LockObject);
            Log.Info("LoadOrderConfig terminated");
        }

        public bool AutoSave {
            get {
                if(m_SaveThread == null)
                    return false; // command line
                else 
                    return LoadOrderToolSettings.Instace.AutoSave;
            }
            set {
                LoadOrderToolSettings.Instace.AutoSave = value;
                LoadOrderToolSettings.Instace.Serialize();
                if(LoadOrderWindow.Instance != null)
                    LoadOrderWindow.Instance.menuStrip.tsmiAutoSave.Checked = value;
            }
        }

        public bool Paused { get; set; } = false;
        public void Suspend() => Paused = true;
        public void Resume() => Paused = false;

        public void ResetAllConfig() {
            AutoSave = false;

            Config = new LoadOrderConfig {
                WorkShopContentPath = DataLocation.WorkshopContentPath,
                GamePath = DataLocation.GamePath,
                SteamPath = DataLocation.SteamPath,
            };

            Config.Serialize(DataLocation.localApplicationData);

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
            AutoSave = LoadOrderToolSettings.Instace.AutoSave;
        }

        public void SaveConfig() {
            if (!AutoSave) {
                SaveConfigImpl();
            } else if (Thread.CurrentThread != m_SaveThread) {
                Dirty = true;
            }
        }

        private void SaveConfigImpl() {
            Dirty = false;
            ManagerList.instance.Save(); // saves but not serialize
            PluginManager.instance.ApplyPendingValues(); // saves to game config and moves folders.
            Config.Serialize(DataLocation.localApplicationData);
            LSMConfig.Serialize();
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
                    if (AutoSave && Dirty)
                        SaveConfigImpl();
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
