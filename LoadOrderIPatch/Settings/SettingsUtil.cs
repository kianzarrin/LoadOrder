using COSettings;
using System.IO;
using System.Runtime.CompilerServices;

namespace LoadOrderIPatch.Settings
{
    internal static class SettingsUtil
    {
        static SettingsUtil()
        {
            userGameSateSettingsFile = new SettingsFile { fileName = "userGameSata" };
            userGameSateSettingsFile.Load();
        }

        public static unsafe int GetLegacyHashCode(string str)
        {
            fixed (char* ptr = str + RuntimeHelpers.OffsetToStringData / 2)
            {
                char* ptr2 = ptr;
                char* ptr3 = ptr2 + str.Length - 1;
                int num = 0;
                while (ptr2 < ptr3)
                {
                    num = (num << 5) - num + (int)(*ptr2);
                    num = (num << 5) - num + (int)ptr2[1];
                    ptr2 += 2;
                }
                ptr3++;
                if (ptr2 < ptr3)
                {
                    num = (num << 5) - num + (int)(*ptr2);
                }
                return num;
            }
        }

        private static string GetSavedEnabledKey(string workingPath)
        {
            var name = Path.GetFileName(workingPath);
            return name + GetLegacyHashCode(workingPath).ToString() + ".enabled";
        }

        private static SettingsFile userGameSateSettingsFile;

        internal static bool GetIsEnabled(string workingPath)
        {
            bool ret = false;
            if (userGameSateSettingsFile.GetValue(GetSavedEnabledKey(workingPath), ref ret))
            {
                return ret;
            }
            return false;
        }
    }
}
