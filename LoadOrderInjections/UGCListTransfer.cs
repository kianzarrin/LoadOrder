namespace LoadOrderShared {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public static class UGCListTransfer {
        const string FILE_NAME = "UGCListTransfer.txt";
        const string DELIM = ";";
        static string GetFilePath(string localLoadOrderDir) => Path.Combine(localLoadOrderDir, FILE_NAME);
        public static void SendList(IEnumerable<ulong> ids, string localLoadOrderDir) {
            if (ids == null) throw new ArgumentNullException("ids");
            static bool IsValid(ulong id) => id != 0 && id != ulong.MaxValue;
            var ids2 = ids.Where(IsValid).ToArray();
            var text = string.Join(DELIM, ids);

            File.WriteAllText(GetFilePath(localLoadOrderDir), text);
        }

        public static List<ulong> GetList(string localLoadOrderDir) {
            string text = File.ReadAllText(GetFilePath(localLoadOrderDir));
            var ids = text.Split(DELIM);

            List<ulong> ret = new List<ulong>(ids.Length);
            foreach (var strId in ids) {
                if (ulong.TryParse(strId, out ulong id))
                    ret.Add(id);
            }

            return ret;
        }
    }
}
