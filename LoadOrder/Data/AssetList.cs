namespace LoadOrderTool.Data {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AssetInfo = CO.Packaging.PackageManager.AssetInfo;

    public class AssetList {
        public List<AssetInfo> Original { get; private set; }
        public List<AssetInfo> Filtered { get; private set; }

        public Action<AssetList> FilterCallBack;

        public AssetList(IEnumerable<AssetInfo> items, Action<AssetList> filterCallBack) {
            Original = new List<AssetInfo>(items);
            FilterCallBack = filterCallBack;
            FilterItems();
        }

        public void SortItemsBy<TKey>(Func<AssetInfo, TKey> selector, bool assending) where TKey : IComparable {
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

        public void FilterItems(Func<AssetInfo, bool> predicate) => 
            Filtered = Original.Where(predicate).ToList();
    }
}
