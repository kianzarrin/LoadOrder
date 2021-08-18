using System;

namespace LoadOrderTool.CommandLine {
    public static class Parse {
        public static bool CommandLine = ParseCommandLine("c|commandLine", out _);

        public static bool Linux = ParseCommandLine("linux", out _);

        public static bool LoadProfile(out string path) => ParseCommandLine("loadProfile", out path);
        

        /// <summary>
        /// if no match was found, value=null and returns false.
        /// if a match is found, value="" or string after --prototype= and returns true.
        /// </summary>
        public static bool ParseCommandLine(string prototypes, out string value) {
            foreach (string arg in Environment.GetCommandLineArgs()) {
                foreach (string prototype in prototypes.Split("|")) {
                    if (MatchCommandLineArg(arg, prototype, out string val)) {
                        value = val;
                        return true;
                    }
                }
            }
            value = null;
            return false;
        }

        /// <summary>
        /// matches one arg with one prototype
        /// </summary>
        public static bool MatchCommandLineArg(string arg, string prototype, out string value) {
            if (arg == "-" + prototype) {
                value = "";
                return true;
            } else if (arg.StartsWith($"--{prototype}=")) {
                int i = prototype.Length + 3;
                if (arg.Length > i)
                    value = arg.Substring(i);
                else
                    value = "";
                return true;
            } else {
                value = null;
                return false;
            }
        }
    }
}
