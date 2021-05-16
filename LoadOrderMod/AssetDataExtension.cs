namespace LoadOrderMod {
    using System.Collections.Generic;
    using ICities;
    using System;

    /// <summary>
    /// record user data to aid hot reload.
    /// </summary>
    public class LOMAssetDataExtension : AssetDataExtensionBase {
        public static Dictionary<PrefabInfo, Dictionary<string, byte[]>> Assets2UserData =
            new Dictionary<PrefabInfo, Dictionary<string, byte[]>>();

        internal static void Init() => Assets2UserData.Clear();
        internal static void Release() => Assets2UserData.Clear();

        public override void OnAssetLoaded(string name, object asset, Dictionary<string, byte[]> userData) =>
            OnAssetLoadedImpl(name, asset as PrefabInfo, userData);

        internal static void OnAssetLoadedImpl(string name, PrefabInfo asset, Dictionary<string, byte[]> userData) {
            if(asset != null)
                Assets2UserData.Add(asset, userData);
        }

        /// <summary>
        /// code to be used by other mods
        /// </summary>
        public static void HotReload() {
            var assets2UserData = Type.GetType("LoadOrderMod.LOMAssetDataExtension, LoadOrderMod", throwOnError:false)
                ?.GetField("Assets2UserData")
                ?.GetValue(null)
                as Dictionary<PrefabInfo, Dictionary<string, byte[]>>;

            if( null == assets2UserData) {
                UnityEngine.Debug.LogWarning("Could not hot reload assets because LoadOrderMod was not found");
                return;
            }

            foreach(var asset2UserData in assets2UserData) {
                var asset = asset2UserData.Key;
                var userData = asset2UserData.Value;
                OnAssetLoadedImpl(asset.name, asset, userData);
            }
        }
    }
}
