namespace LoadOrderTool {
    using System;
    using System.Collections.Generic;
    using CO;
    using CO.Packaging;

    public class AssetList :List<PackageManager.AssetInfo> {
        public static AssetList GetAllAssets() {
            var ret =  new AssetList();
            ret.AddRange(PackageManager.instance.GetAssets());
            return ret;
        }
    }
}
