namespace LoadOrderTool.Data {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CO;

    public class DLCList {
        public List<DLCManager.DLCInfo> Original { get; private set; }
        public List<DLCManager.DLCInfo> Filtered { get; private set; }

        public Action<DLCList> FilterCallBack;

        public DLCList(IEnumerable<DLCManager.DLCInfo> items, Action<DLCList> filterCallBack) {
            Original = new List<DLCManager.DLCInfo>(items);
            FilterCallBack = filterCallBack;
            FilterItems();
        }

        public void SortItemsBy<TKey>(Func<DLCManager.DLCInfo, TKey> selector, bool assending) where TKey : IComparable {
            if (assending)
                Original.Sort((a, b) => Compare(selector(a), selector(b)));
            else
                Original.Sort((a, b) => Compare(selector(b), selector(a)));
        }

        static int Compare<T>(T a, T b) where T : IComparable {
            bool aIsNull = a is null;
            bool bIsNull = b is null;
            if (aIsNull | bIsNull)
                return aIsNull.CompareTo(bIsNull);
            else
                return a.CompareTo(b);
        }

        public void FilterItems() =>
            FilterCallBack?.Invoke(this);

        public void FilterItems(Func<DLCManager.DLCInfo, bool> predicate) =>
            Filtered = Original.Where(predicate).ToList();

    }

    public class DLCManager :  SingletonLite<DLCManager> {
        static ConfigWrapper ConfigWrapper => ConfigWrapper.instance;

        public List<DLCInfo> DLCs = new List<DLCInfo>();

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
            DLCs = LoadImpl(ConfigWrapper.Config.ExcludedDLCs);
        }

        public void Save() {
            ConfigWrapper.Config.ExcludedDLCs = SaveImpl();
        }

    }
}
