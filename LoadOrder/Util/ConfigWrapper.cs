namespace LoadOrderTool.Util {
    using CO.IO;
    using CO.Packaging;
    using CO.Plugins;
    using LoadOrderTool;
    using System;
    using System.Threading;

    public class ConfigWrapper {
        public LoadOrderShared.LoadOrderConfig Config;

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
            Config = LoadOrderShared.LoadOrderConfig.Deserialize(DataLocation.localApplicationData)
                ?? new LoadOrderShared.LoadOrderConfig();
            StartSaveThread();

        }
        ~ConfigWrapper() {
            m_Run = false;
            lock (m_LockObject)
                Monitor.Pulse(m_LockObject);
            
            Log.Info("LoadOrderConfig terminated");
        }

        public bool AutoSave {
            get => Config.ToolSettings.AutoSave;
            set => Config.ToolSettings.AutoSave = value;
        }

        public bool Paused { get; set; } = false;
        public void Suspend() => Paused = true;
        public void Resume() => Paused = false;

        public void SaveConfig() {
            if (!AutoSave) {
                SaveConfigImpl();
            } else if (Thread.CurrentThread != m_SaveThread) {
                Dirty = true;
            }
        }

        private void SaveConfigImpl() {
            Dirty = false;
            PluginManager.instance.ApplyPendingValues();
            PackageManager.instance.ApplyPendingValues();
            Config.Serialize(DataLocation.localApplicationData);
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
