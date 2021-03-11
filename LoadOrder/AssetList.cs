namespace LoadOrderTool {
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using CO;
    using CO.Packaging;

    public class AssetList :List<PackageManager.AssetInfo> {
        public List<PackageManager.AssetInfo> Filtered;

        public AssetList(IEnumerable<PackageManager.AssetInfo> list) : base(list) {
            Filtered = list.ToList();
        }

        public static AssetList GetAllAssets() {
            return  new AssetList(PackageManager.instance.GetAssets());
        }

        public void FilterIn(Func<PackageManager.AssetInfo,bool> predicate) {
            Filtered = this.Where(predicate).ToList();
        }

    }
}
