namespace LoadOrderShared {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public static class UGCListTransfer {
        const string FILE_NAME = "UGCListTransfer.txt";
        const string MISSING = "missing";
        static string GetFilePath(string localLOMData) => Path.Combine(localLOMData, FILE_NAME);

        public static void SendList(IEnumerable<ulong> ids, string localLOMData, bool missing) =>
            SendListToFile(ids, GetFilePath(localLOMData), missing);
        public static void SendListToFile(IEnumerable<ulong> ids, string filePath, bool missing) {
            if(ids == null) throw new ArgumentNullException("ids");
            static bool IsValid(ulong id) => id != 0 && id != ulong.MaxValue;

            var ids2 = ids.Where(IsValid).Select(id => id.ToString()).ToList();
            if(missing) ids2.Add(MISSING);

            var text = string.Join(";", ids2.ToArray());

            File.WriteAllText(filePath, text);
        }


        public static List<ulong> GetList(string localLOMData, out bool missing) =>
            GetListFromFile(GetFilePath(localLOMData), out missing);
        public static List<ulong> GetListFromFile(string filePath, out bool missing) {
            string text = File.ReadAllText(filePath);
            var ids = text.Split(';', ',', ' ', '\t').Distinct().ToList();
            missing = ids.Remove(MISSING);
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
