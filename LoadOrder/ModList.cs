using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using System.Transactions;

namespace LoadOrder
{
    public class ModDataItem
    {
        public int LoadOrder;
        public bool ModEnabled;
        public string Text;
    }
    public class ModList : List<ModDataItem>
    {
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
        }

        //public static ModList GetAllMods()
        //{

        //}
    }



}
