using CO.Plugins;
using System;
using System.Collections.Generic;
using static CO.Plugins.PluginManager;
using CO.PlatformServices;

namespace LoadOrderTool
{
    public class ModList : List<PluginManager.PluginInfo>
    {
        public ModList(IEnumerable<PluginManager.PluginInfo> list) : base(list) {
        }

        public static ModList GetAllMods()
        {
            var mods = new ModList(PluginManager.instance.GetPluginsInfo());
            return mods;
        }

        public static int DefaultComparison(PluginInfo p1, PluginInfo p2)
        {
            var savedOrder1 = p1.SavedLoadOrder;
            var savedOrder2 = p2.SavedLoadOrder;

            // orderless harmony comes first
            if (!savedOrder1.exists && p1.IsHarmonyMod())
                return -1;
            if (!savedOrder2.exists && p2.IsHarmonyMod())
                return +1;

            // if neither have order, use string comparison
            // then builin first, workshop second, local last
            // otherwise use string comparison
            if (!savedOrder1.exists && !savedOrder2.exists)
            {
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


        public void DefaultSort()
        {
            Sort(DefaultComparison);
            for (int i = 0; i < Count; ++i)
                this[i].LoadOrder = i;
        }

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


    }



}
