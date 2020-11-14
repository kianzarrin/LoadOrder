
using COSettings.Plugins;
using System;
using System.Collections.Generic;

namespace LoadOrderTool
{
    public class ModList : List<PluginManager.PluginInfo>
    {
        public ModList(IEnumerable<PluginManager.PluginInfo> list) : base(list) {
            SortByOrder();
        }

        public static ModList GetAllMods()
        {
            var mods = new ModList(PluginManager.instance.GetPluginsInfo());
            return mods;
        }
        public void SortByOrder()
        {
            for (int i = 0; i < Count - 1;)
            {
                int j = i + 1;
                if (this[i].LoadOrder > this[j].LoadOrder)
                {
                    (this[i], this[j]) = (this[j], this[i]);
                    i = 0; // restart
                }
                else
                {
                    ++i;
                }
            }
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
