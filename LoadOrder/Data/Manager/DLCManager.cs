using System;
using System.Collections.Generic;
using CO;
using System.Linq;

namespace LoadOrderTool.Data {
    public class DLCManager : SingletonLite<DLCManager>, IDataManager {
        static ConfigWrapper ConfigWrapper => ConfigWrapper.instance;

        public List<DLCInfo> DLCs = new List<DLCInfo>();

        public bool IsLoading { get; private set; }
        public bool IsLoaded { get; private set; }
        public event Action EventLoaded;

        public class DLCInfo {

            public CO.DLC DLC;
            public string Text;
            public DLCType DLCType;

            bool isIncluded_ = true;
            public bool IsIncluded {
                get => isIncluded_;
                set {
                    isIncluded_ = value;
                    ConfigWrapper.Dirty = true;
                }
            }

            public DLCInfo(CO.DLC dlc) {
                DLC = dlc;
                var att = dlc.GetDLCInfo();
                if (att != null) {
                    Text = att.Text;
                    DLCType = att.Type;
                }
            }
        }


        string[] SaveImpl() {
            return DLCs
                .Where(item => !item.IsIncluded)
                .Select(item => item.DLC.ToString())
                .ToArray();
        }

        static List<DLCInfo> LoadImpl(string[] excluded) {
            CO.DLC[] dlcValues = Enum.GetValues(typeof(CO.DLC)) as CO.DLC[];
            var dlcs = dlcValues.
                Where(item => item != DLC.None)
                .Select(item => new DLCInfo(item));

            if (excluded != null && excluded.Any()) {
                foreach (var dlc in dlcs) {
                    dlc.IsIncluded = !excluded.Contains(dlc.DLC.ToString());
                }
            }

            return dlcs.ToList();
        }

        public void Load() {
            try {
                Log.Called();
                IsLoading = true;
                IsLoaded = false;
                DLCs = LoadImpl(ConfigWrapper.Config.ExcludedDLCs);
            } catch (Exception ex) { ex.Log(); }
            try { EventLoaded?.Invoke(); } catch (Exception ex) { ex.Log(); }
        }

        public void Save() {
            try {
                Log.Called();
                ConfigWrapper.Config.ExcludedDLCs = SaveImpl();
            } catch (Exception ex) { ex.Log(); }
        }

        public void LoadFromProfile(LoadOrderProfile profile, bool replace = true) {
            foreach (var item in DLCs) {
                bool included = !profile.ExcludedDLCs.Contains(item.DLC);
                if (replace) {
                    item.IsIncluded = included;
                } else {
                    item.IsIncluded |= included;
                }
            }
        }

        public void SaveToProfile(LoadOrderProfile profile) {
            profile.ExcludedDLCs = DLCs
                .Where(item => !item.IsIncluded)
                .Select(item => item.DLC)
                .ToArray();
        }
    }

}
