namespace LoadOrderTool.Util {
    using CO.IO;
    using CO.Packaging;
    using CO.Plugins;
    using LoadOrderTool;
    using System;
    using System.Threading;

    public class ConfigWrapper {
        public LoadOrderShared.LoadOrderConfig Config;
        public bool Dirty = false;

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
            lock (m_LockObject) {
                Monitor.Pulse(m_LockObject);
            }
            Log.Info("LoadOrderConfig terminated");
        }

        public bool AutoSave { get; set; } = false;

        public void Suspend() => m_SaveThread.Suspend();
        public void Resume() => m_SaveThread.Resume();

        public void SaveConfig() {
            if (!AutoSave) {
                SaveConfigImpl();
            } else if (Thread.CurrentThread != m_SaveThread) {
                Dirty = true;
            }
        }

        private void SaveConfigImpl() {
            Dirty = false;
            Config.Serialize(DataLocation.localApplicationData);
            PluginManager.instance.ApplyPendingValues();
            PackageManager.instance.ApplyPendingValues();
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
                    lock (m_LockObject) {
                        Monitor.Wait(m_LockObject, 100);
                    }
                }

                Log.Info("Load Order Config Monitor Exiting...");
            } catch (Exception ex) {
                Log.Exception(ex);
            }
        }


    }
}
