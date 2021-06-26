using CO.PlatformServices;
using CO.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using static CO.Plugins.PluginManager;
using CO.Plugins;
using LoadOrderTool.Data;

namespace LoadOrderTool {
    public class ModList : List<PluginInfo> {
        const int DefaultLoadOrder = LoadOrderShared.LoadOrderConfig.DefaultLoadOrder;
        public List<PluginInfo> Filtered;

        public delegate bool PredicateHandler(PluginInfo p);
        public Func<PluginInfo, bool> PredicateCallback { get; set; }

        public ModList(IEnumerable<PluginInfo> list, Func<PluginInfo, bool> predicateCallback) : base(list) {
            PredicateCallback = predicateCallback;
            FilterIn();
        }

        public static ModList GetAllMods(Func<PluginInfo, bool> predicateCallback) {
            return new ModList(PluginManager.instance.GetMods(), predicateCallback);
        }

        public void FilterIn() {
            if (PredicateCallback != null)
                Filtered = this.Where(PredicateCallback).ToList();
            else
                Filtered = this.ToList();
        }

        public static int LoadOrderComparison(PluginInfo p1, PluginInfo p2) =>
            p1.LoadOrder.CompareTo(p2.LoadOrder);

        public static int OldDefaultComparison(PluginInfo p1, PluginInfo p2) {
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
                    (_p.PublishedFileId != PublishedFileId.invalid ? 1 : 2);
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
        public static int GetHarmonyOrder(PluginInfo p) {
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

        public static int HarmonyComparison(PluginInfo p1, PluginInfo p2) {
            var savedOrder1 = p1.LoadOrder;
            var savedOrder2 = p2.LoadOrder;

            // orderless harmony comes first
            if (!p1.HasLoadOrder() && p1.IsHarmonyMod())
                return -1;
            if (!p2.HasLoadOrder() && p2.IsHarmonyMod())
                return +1;

            if (!p1.HasLoadOrder() && !p2.HasLoadOrder()) {
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
                    // builtin first, workshop second, local last
                    static int order(PluginInfo _p) =>
                        _p.isBuiltin ? 0 :
                        (_p.PublishedFileId != PublishedFileId.invalid ? 1 : 2);
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
            foreach (var p in this)
                p.ResetLoadOrder();
        }

        public void SortItemsBy<TKey>(Func<PluginInfo, TKey> selector, bool assending) where TKey : IComparable {
            if (assending)
                Sort((a, b) => Compare(selector(a), selector(b)));
            else
                Sort((a, b) => Compare(selector(b), selector(a)));
        }

        static int Compare<T>(T a, T b) where T : IComparable {
            bool aIsNull = a is null;
            bool bIsNull = b is null;
            if (aIsNull | bIsNull)
                return aIsNull.CompareTo(bIsNull);
            else
                return a.CompareTo(b);
        }

        public void DefaultSort() => Sort(HarmonyComparison);

        public void SortBy(Comparison<PluginInfo> comparison) {
            Sort(comparison);
            for (int i = 0; i < Count; ++i)
                this[i].LoadOrder = i;
        }

        public void ReverseOrder() {
            int lastIndex = Count - 1;
            for (int i = 0; i < Count; ++i)
                this[i].LoadOrder = lastIndex - i;
            this.Sort(LoadOrderComparison);
        }

        private int[] RandomNumber(int count) {
            var orderedList = Enumerable.Range(0, count);
            int randomSeed = RNGCryptoServiceProvider.GetInt32(0, int.MaxValue);
            var rng = new Random(randomSeed);
            return orderedList.OrderBy(c => rng.Next()).ToArray();
        }

        public void RandomizeOrder() {
            int[] order = RandomNumber(Count);
            for (int i = 0; i < Count; ++i)
                this[i].LoadOrder = order[i];
            this.Sort(LoadOrderComparison);
        }


        [Obsolete("Does not work if Load order is not pre-determined", true)]
        public void MoveItem(int oldIndex, int newIndex) {
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

            // work around: hyarmony without load order comes first:
            if (newLoadOrder < DefaultLoadOrder) {
                foreach (var p2 in this) {
                    if (p2 != p && p2.IsHarmonyMod() && !p2.HasLoadOrder())
                        p2.LoadOrder = 0;
                }
            }

            int newIndex = this.FindIndex(item => OldDefaultComparison(item, p) >= 0);
            if (newIndex == -1) newIndex = this.Count - 1; // this is the biggest value;
            this.Remove(p);
            this.Insert(newIndex, p);
            Log.Debug($"newIndex={newIndex} newLoadOrder={p.LoadOrder}");
            for (int i = 1; i < Count; ++i) {
                if (this[i].HasLoadOrder() && this[i].LoadOrder <= this[i - 1].LoadOrder) {
                    this[i].LoadOrder = this[i - 1].LoadOrder + 1;
                }
            }
        }

        public PluginManager.PluginInfo GetPluginInfo(string path) =>
            this.FirstOrDefault(p => p.IncludedPath == path);

        public void LoadFromProfile(LoadOrderProfile profile, bool replace = true)
            => PluginManager.instance.LoadFromProfile(profile, replace);

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
