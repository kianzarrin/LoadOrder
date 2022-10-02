using CO.PlatformServices;
using CO.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using static CO.Plugins.PluginManager;
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

        /// <summary>
        /// 1st: harmony mod 
        /// 2nd: harmony 2 
        /// 3rd: harmony 1
        /// 4th: not harmony
        /// </summary>
        /// <returns>
        /// harmony mod : 0
        /// harmony 2   : 1
        /// harmony 1   : 2
        /// not harmony : 3
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

        /// <summary>
        /// if parent folder is a number, it counts as WS
        /// </summary>
        static bool GetWSID(string name, out uint id) {
            bool isWS = uint.TryParse(name, out id) && 
                id != 0 &&
                id != PublishedFileId.invalid.AsUInt64;
            if (!isWS) id = 0;
            return isWS;
        }


        public static int HarmonyComparison(PluginInfo p1, PluginInfo p2) {
            int ret = p1.LoadOrder.CompareTo(p2.LoadOrder);
            if (ret != 0) return ret; // if both 1000(default) then go to next line

            ret = GetHarmonyOrder(p1).CompareTo(GetHarmonyOrder(p2));
            if (ret != 0) return ret;

            // WS mod comes before local mod.
            ret = -GetWSID(p1.name, out uint id1).CompareTo(GetWSID(p2.name, out uint id2));
            if (ret != 0) return ret;

            
            ret = id1.CompareTo(id2); // compare WS mods
            if (ret != 0) return ret;

            ret = p1.name.CompareTo(p2.name); // compare local mods
            return ret;
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

        public void MoveItem(PluginInfo movingItem, int newLoadOrder) {
            if (movingItem.LoadOrder == newLoadOrder) return;

            movingItem.LoadOrder = newLoadOrder;

            if (!movingItem.HasLoadOrder()) {
                DefaultSort();
                return;
            }

            // work around: harmony without load order comes first:
            if (newLoadOrder < DefaultLoadOrder) {
                foreach (var p2 in this) {
                    if (p2 != movingItem && p2.IsHarmonyMod() && !p2.HasLoadOrder())
                        p2.LoadOrder = 0;
                }
            }

            this.Remove(movingItem);
            // find the first item that is bigger than the moving item (that item will move one index down).
            int newIndex = this.FindIndex(otherItem => HarmonyComparison(otherItem, movingItem) >= 0);
            if (newIndex == -1) {
                this.Add(movingItem); // add to end
            } else {
                this.Insert(newIndex, movingItem);
            }
            Log.Debug($"newIndex={newIndex} newLoadOrder={movingItem.LoadOrder}");
            // if next item has order equal to current item shift it down.
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

        public void SaveToProfile(LoadOrderProfile profile)
            => PluginManager.instance.SaveToProfile(profile);
    }
}
