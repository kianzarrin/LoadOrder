namespace LoadOrderShared {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public static class UGCListTransfer {
        const string FILE_NAME = "UGCListTransfer.txt";
        const string DELIM = ";";
        static string GetFilePath(string localLOMData) => Path.Combine(localLOMData, FILE_NAME);
        public static void SendList(IEnumerable<ulong> ids, string localLOMData) {
            if (ids == null) throw new ArgumentNullException("ids");
            static bool IsValid(ulong id) => id != 0 && id != ulong.MaxValue;
            var ids2 = ids.Where(IsValid).ToArray();
            var text = string.Join(DELIM, ids);

            File.WriteAllText(GetFilePath(localLOMData), text);
        }

        public static List<ulong> GetList(string localLOMData) {
            string text = File.ReadAllText(GetFilePath(localLOMData));
            var ids = text.Split(DELIM);
            return ToNumber(ids);
        }

        public static List<ulong> ToNumber(IEnumerable<string> ids) {
            List<ulong> ret = new List<ulong>();
            foreach (var strId in ids) {
                if (ulong.TryParse(strId, out ulong id))
                    ret.Add(id);
            }
            return ret;
        }
    }
}
