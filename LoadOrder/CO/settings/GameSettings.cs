using LoadOrderTool;
using System;
using System.Collections.Generic;
using System.Threading;


namespace CO {
    public class GameSettings : SingletonLite<GameSettings> {
        public static readonly string extension = ".cgs";

        private Dictionary<string, SettingsFile> m_SettingsFiles = new Dictionary<string, SettingsFile>();

        private static bool m_Run;

        private static Thread m_SaveThread;


        private static object m_LockObject = new object();

        public static void AddSettingsFile(params SettingsFile[] settingsFiles) {
            instance.InternalAddSettingsFile(settingsFiles);
        }

        private void InternalAddSettingsFile(params SettingsFile[] settingsFiles) {
            lock (this.m_SettingsFiles) {
                for (int i = 0; i < settingsFiles.Length; i++) {
                    try {
                        settingsFiles[i].Load();
                        this.m_SettingsFiles.Add(settingsFiles[i].fileName, settingsFiles[i]);
                    } catch (Exception ex) {
                        new Exception($"could not load {settingsFiles[i]} (maybe try launching CS?)",ex).Log();
                    }
                }
            }
        }

        public static SettingsFile FindSettingsFileByName(string name) {
            return instance.InternalFindSettingsFileByName(name);
        }

        internal SettingsFile InternalFindSettingsFileByName(string name) {
            SettingsFile result;
            if (!m_SettingsFiles.TryGetValue(name, out result)) {
                // auto add missing setting files.
                result = new SettingsFile() { fileName = name };
                InternalAddSettingsFile(result);
            }
            return result;
        }

        public static void SaveAll() {
            instance.InternalSaveAll();
        }

        public void InternalSaveAll() {
            lock (this.m_SettingsFiles) {
                foreach (SettingsFile settingsFile in this.m_SettingsFiles.Values) {
                    if (settingsFile.isDirty) {
                        settingsFile.Save();
                    }
                }
            }
        }

        public static void ClearAll() {
            ClearAll(false);
        }

        public static void ClearAll(bool systemToo) {
            instance.InternalClearAll(systemToo);
        }

        private void InternalClearAll(bool systemToo) {
            foreach (SettingsFile settingsFile in this.m_SettingsFiles.Values) {
                if (!settingsFile.isSystem || systemToo) {
                    settingsFile.Delete();
                }
            }
        }

        private static void MonitorSave() {
            try {
                while (GameSettings.m_Run) {
                    GameSettings.SaveAll();
                    lock (GameSettings.m_LockObject) {
                        Monitor.Wait(GameSettings.m_LockObject, 100);
                    }
                }
                GameSettings.SaveAll();
                Log.Info("GameSettings Monitor Exiting...");
            } catch (Exception ex) {
                Log.Exception(ex);
            }
        }

        public GameSettings() {
            Log.Info("GameSettings Monitor Started...");
            GameSettings.m_SaveThread = new Thread(new ThreadStart(GameSettings.MonitorSave));
            GameSettings.m_SaveThread.Name = "SaveSettingsThread";
            GameSettings.m_SaveThread.IsBackground = true;
            GameSettings.m_Run = true;
            GameSettings.m_SaveThread.Start();
        }

        ~GameSettings() => Terminate();

        public void Terminate() {
            GameSettings.m_Run = false;
            lock (GameSettings.m_LockObject)
                Monitor.Pulse(GameSettings.m_LockObject);
            Log.Info("GameSettings terminated");
        }
    }
}
