using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using CO;
using CO.PlatformServices;
using CO.Packaging;
using CO.Plugins;

namespace LoadOrderTool.Data {
    public interface IDataManager {
        /// <summary>
        /// load or reload data
        /// </summary>
        void Load();
        void LoadFromProfile(LoadOrderProfile profile, bool replace = true);
        bool IsLoading { get; }
        bool IsLoaded { get; }
        event Action EventLoaded;

        /// <summary>
        /// save data to config but do not serialize
        /// </summary>
        void Save();
        void SaveToProfile(LoadOrderProfile profile);
    }

    public class ManagerList : SingletonLite<ManagerList>, IDataManager {
        public IDataManager[] Managers = {
            DLCManager.instance,
            LSMManager.instance,
            PluginManager.instance,
            PackageManager.instance,
        };

        public bool IsLoaded => Managers.All(m => m.IsLoaded);

        public bool IsLoading => Managers.Any(m => m.IsLoading);

        public event Action EventLoaded;

        private void OnLoadCallBack() {
            if (IsLoaded)
                EventLoaded?.Invoke();
        }

        public void Load() {
            foreach (IDataManager m in Managers) {
                m.EventLoaded += OnLoadCallBack;
                m.Load();
            }
        }

        public void LoadFromProfile(LoadOrderProfile profile, bool replace = true) {
            foreach (IDataManager m in Managers)
                m.LoadFromProfile(profile, replace);
        }

        public void Save() {
            foreach (IDataManager m in Managers)
                m.Save();
        }

        public void SaveToProfile(LoadOrderProfile profile) {
            foreach (IDataManager m in Managers)
                m.SaveToProfile(profile);
        }
    }
}
