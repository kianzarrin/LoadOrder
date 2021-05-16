namespace LoadOrderMod {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ICities;
    using ColossalFramework;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// record info in case of hot reload
    /// </summary>
    public class AssetDataExtension : AssetDataExtensionBase {

        public static Dictionary<object, Dictionary<string, byte[]>> AssetToUserData =
            new Dictionary<object, Dictionary<string, byte[]>>();

        internal static void Init() {
            AssetToUserData.Clear();
        }

        public override void OnAssetLoaded(string name, object asset, Dictionary<string, byte[]> userData) {
            AssetToUserData.Add(asset, userData);
        }
    }
}
