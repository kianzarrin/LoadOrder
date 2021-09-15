using CO;
using CO.Packaging;
using CO.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public void ResetCache();

    }

    public class ManagerList : SingletonLite<ManagerList>, IDataManager {
        public static IEnumerable<IWSItem> GetItems() =>
            PackageManager.instance.GetAssets()
            .Concat<IWSItem>(PluginManager.instance.GetMods());

        public static IEnumerable<IWSItem> GetWSItems() =>
            GetItems().Where(item => item.IsWorkshop);

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
            try {
                Log.Called();
                if (IsLoaded)
                    EventLoaded?.Invoke();
            } catch (Exception ex) { ex.Log(); }
        }

        public void Load() {
            try {
                Log.Called();
                foreach (IDataManager m in Managers) {
                    m.EventLoaded += OnLoadCallBack;
                    m.Load();
                }
            } catch (Exception ex) { ex.Log(); }
        }

        public void LoadFromProfile(LoadOrderProfile profile, bool replace = true) {
            try {
                Log.Called();
                foreach (IDataManager m in Managers)
                    m.LoadFromProfile(profile, replace);
            } catch (Exception ex) { ex.Log(); }

        }

        public void Save() {
            try {
                Log.Called();
                foreach (IDataManager m in Managers)
                    m.Save();
            } catch (Exception ex) { ex.Log(); }
        }

        public void SaveToProfile(LoadOrderProfile profile) {
            try {
                Log.Called();
                foreach (IDataManager m in Managers)
                    m.SaveToProfile(profile);
            } catch (Exception ex) { ex.Log(); }
        }

        public void ResetCache() {
            try {
                Log.Called();
                foreach (IDataManager m in Managers)
                    m.ResetCache();
            } catch (Exception ex) { ex.Log(); }
        }
    }
}
