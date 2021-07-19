namespace LoadOrderTool.Data {
    using CO;
    using System;
    using System.Collections.Generic;
    using System.Linq;

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
}
