namespace LoadOrderTool.Data {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AssetInfo = CO.Packaging.PackageManager.AssetInfo;

    public class AssetList {
        public List<AssetInfo> Original { get; private set; }
        public List<AssetInfo> Filtered { get; private set; }

        public AssetList(IEnumerable<AssetInfo> items) {
            Original = new List<AssetInfo>(items);
            Filtered = new List<AssetInfo>(items);
        }

        public void SortItemsBy<TKey>(Func<AssetInfo, TKey> selector, bool assending) where TKey : IComparable {
            if (assending)
                Original.Sort((a, b) => selector(a).CompareTo(selector(b)));
            else
                Original.Sort((a, b) => selector(b).CompareTo(selector(a)));
        }

        public void FilterItems(Func<AssetInfo, bool> predicate) {
            Filtered = Original.Where(predicate).ToList();
        }
    }
}
