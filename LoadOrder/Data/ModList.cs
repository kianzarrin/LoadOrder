using CO.PlatformServices;
using CO.Plugins;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;
using static CO.Plugins.PluginManager;

namespace LoadOrderTool {
    public class ModList : List<PluginManager.PluginInfo> {
        const int DefaultLoadOrder = LoadOrderShared.LoadOrderConfig.DefaultLoadOrder;
        public List<PluginInfo> Filtered;

        public ModList(IEnumerable<PluginManager.PluginInfo> list) : base(list)
        {
            Filtered = list.ToList();
        }

        public static ModList GetAllMods()
        {
            return new ModList(PluginManager.instance.GetPluginsInfo());
        }

        public void FilterIn(Func<PluginInfo, bool> predicate) {
            Filtered = this.Where(predicate).ToList();
        }

        public static int LoadOrderComparison(PluginInfo p1, PluginInfo p2) =>
            p1.LoadOrder.CompareTo(p2.LoadOrder);

        public static int DefaultComparison(PluginInfo p1, PluginInfo p2)
        {
            var savedOrder1 = p1.LoadOrder;
            var savedOrder2 = p2.LoadOrder;

            // orderless harmony comes first
            if (!p1.HasLoadOrder() && p1.IsHarmonyMod())
                return -1;
            if (!p2.HasLoadOrder() && p2.IsHarmonyMod())
                return +1;

            // if neither have order, use string comparison
            // then builin first, workshop second, local last
            // otherwise use string comparison
            if (!p1.HasLoadOrder() && !p2.HasLoadOrder()) {
                int order(PluginInfo _p) =>
                    _p.isBuiltin ? 0 :
                    (_p.publishedFileID != PublishedFileId.invalid ? 1 : 2);
                if (order(p1) != order(p2))
                    return order(p1) - order(p2);
                return p1.name.CompareTo(p2.name);
            }

            // use assigned or default values
            return savedOrder1 - savedOrder2;
        }

        /// <returns>
        /// harmony mod : 2.0.1.0+
        /// harmony 2 : 2.0.0.0
        /// harmony 1 : 1.*
        /// no harmony : 0
        /// </returns>
        public static int GetHarmonyOrder(PluginInfo p)
        {
            if (p.IsHarmonyMod())
                return 0;
            foreach (var file in p.DllPaths) {
                var name = Path.GetFileNameWithoutExtension(file);
                if (name == "CitiesHarmony.API")
                    return 1;
                if (name == "0Harmony")
                    return 2;
            }
            return 3;
        }

        public static int HarmonyComparison(PluginInfo p1, PluginInfo p2)
        {
            var savedOrder1 = p1.LoadOrder;
            var savedOrder2 = p2.LoadOrder;

            if (!p1.HasLoadOrder() && savedOrder2 == DefaultLoadOrder) {
                // if neither have saved order,
                {
                    // 1st: harmony mod 
                    // 2nd: harmony 2 
                    // 3rd: harmony 1 
                    // 4th: no harmony 
                    var o1 = GetHarmonyOrder(p1);
                    var o2 = GetHarmonyOrder(p2);
                    if (o1 != o2)
                        return o1 - o2;
                }
                {
                    // builin first, workshop second, local last
                    int order(PluginInfo _p) =>
                        _p.isBuiltin ? 0 :
                        (_p.publishedFileID != PublishedFileId.invalid ? 1 : 2);
                    if (order(p1) != order(p2))
                        return order(p1) - order(p2);
                }
                {
                    // use string comparison
                    return p1.name.CompareTo(p2.name);
                }
            }

            // use assigned or default values
            return savedOrder1 - savedOrder2;
        }

        public void ResetLoadOrders() {
            foreach(var p in this) 
                p.ResetLoadOrder();
        }

        public void DefaultSort() {
            Sort(HarmonyComparison);
        }

        public void SortBy(Comparison<PluginInfo> comparison)
        {
            Sort(comparison);
            for (int i = 0; i < Count; ++i)
                this[i].LoadOrder = i;
        }

        public void ReverseOrder()
        {
            int lastIndex = Count - 1;
            for (int i = 0; i < Count; ++i)
                this[i].LoadOrder = lastIndex-i;
            this.Sort(LoadOrderComparison);
        }

        private int[] RandomNumber(int count)
        {
            var orderedList = Enumerable.Range(0, count);
            int randomSeed = RNGCryptoServiceProvider.GetInt32(0, int.MaxValue);
            var rng = new Random(randomSeed);
            return orderedList.OrderBy(c => rng.Next()).ToArray();
        }

        public void RandomizeOrder()
        {
            int[] order = RandomNumber(Count);
            for (int i = 0; i < Count; ++i)
                this[i].LoadOrder = order[i];
            this.Sort(LoadOrderComparison);
        }


        [Obsolete("Does not work if Load order is not pre-determined", true)]
        public void MoveItem(int oldIndex, int newIndex)
        {
            if (oldIndex == newIndex) return;
            newIndex = Math.Clamp(newIndex, 0, Count - 1);
            var item = this[oldIndex];
            this.RemoveAt(oldIndex);
            this.Insert(newIndex, item);

            for (int i = 0; i < Count; ++i)
                this[i].LoadOrder = i;
        }

        public void MoveItem(PluginInfo p, int newLoadOrder) {
            if (p.LoadOrder == newLoadOrder) return;

            p.LoadOrder = newLoadOrder;

            if (!p.HasLoadOrder()) {
                DefaultSort();
                return;
            }

            int newIndex = this.FindIndex(item => DefaultComparison(item, p) >= 0);
            this.Remove(p);
            this.Insert(newIndex, p);
            Log.Debug($"newIndex={newIndex} newLoadOrder={p.LoadOrder}");
            for (int i = 1; i < Count; ++i) {
                if (this[i].HasLoadOrder() && this[i].LoadOrder <= this[i - 1].LoadOrder) {
                    this[i].LoadOrder = this[i - 1].LoadOrder+1;
                    if (!this[i].HasLoadOrder())
                        this[i].LoadOrder++;
                }
            }
        }

        public PluginManager.PluginInfo GetPluginInfo(string path) =>
            this.FirstOrDefault(p => p.ModIncludedPath == path);

        public void LoadFromProfile(LoadOrderProfile profile, bool excludeExtras=true) {
            foreach (var modInfo in this) {
                var modProfile = profile.GetMod(modInfo.ModIncludedPath);
                if (modProfile != null) {
                    modProfile.Write(modInfo);
                } else if(excludeExtras) {
                    Log.Debug("mod profile with path not found: " + modInfo.ModIncludedPath);
                    modInfo.LoadOrder = DefaultLoadOrder;
                    modInfo.IsIncluded = false;
                }
            }

            var missing = profile.Mods.Where(m => GetPluginInfo(m.IncludedPath) == null);
            if (missing.Any()) {
                var strMissing = string.Join('\n', missing.Select(p => p.DisplayText).ToArray());
                MessageBox.Show(strMissing, "Warning! Missing Mods", MessageBoxButtons.OK);
            }
        }

        public void SaveToProfile(LoadOrderProfile profile) {
            var list = new List<LoadOrderProfile.Mod>(this.Count);
            foreach (var pluginInfo in this) {
                var modProfile = new LoadOrderProfile.Mod(pluginInfo);
                list.Add(modProfile);
            }
            profile.Mods = list.ToArray();
        }
    }
}
