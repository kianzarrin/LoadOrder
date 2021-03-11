namespace LoadOrderTool {
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using CO;
    using CO.Packaging;

    public class AssetList :List<PackageManager.AssetInfo> {
        public List<PackageManager.AssetInfo> Filtered;

        public static AssetList GetAllAssets() {
            var ret =  new AssetList();
            ret.AddRange(PackageManager.instance.GetAssets());
            return ret;
        }

        public void FilterIn(Func<PackageManager.AssetInfo,bool> predicate) {
            Filtered = this.Where(predicate).ToList();
        }

    }
}
