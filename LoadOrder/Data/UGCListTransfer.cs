namespace LoadOrderShared {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public static class UGCListTransfer {
        const string FILE_NAME = "UGCListTransfer.txt";
        static string GetFilePath(string localLOMData) => Path.Combine(localLOMData, FILE_NAME);
        public static void SendList(IEnumerable<ulong> ids, string localLOMData) {
            if (ids == null) throw new ArgumentNullException("ids");
            static bool IsValid(ulong id) => id != 0 && id != ulong.MaxValue;
            var ids2 = ids.Where(IsValid).Select(id=>id.ToString()).ToArray();
            var text = string.Join(";", ids2);

            File.WriteAllText(GetFilePath(localLOMData), text);
        }

        public static List<ulong> GetList(string localLOMData) =>
            GetListFromFile(GetFilePath(localLOMData));


        public static List<ulong> GetListFromFile(string filePath) {
            string text = File.ReadAllText(filePath);
            var ids = text.Split(';');
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
