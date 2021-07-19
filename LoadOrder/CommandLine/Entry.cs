using System;
using System.Collections.Generic;
using System.Text;
using CO;
using CO.PlatformServices;
using CO.Plugins;
using CO.Packaging;
using CO.IO;
using LoadOrderTool.Util;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Security.Principal;
using LoadOrderShared;
using LoadOrderTool.Data;

namespace LoadOrderTool.CommandLine {
    public static class Entry {
        static LoadOrderConfig Config => ConfigWrapper.instance.Config;
        static LoadOrderProfile Profile;

        public static void Start() {
            Log.Info("command line mode");
            if (Parse.LoadProfile(out string path)) {
                if (!File.Exists(path)) {
                    string path2 = Path.Combine(LoadOrderProfile.DIR, path);
                    if (!File.Exists(path2))
                        throw new Exception("could not find " + path);
                    path = path2;
                }
                var Profile = LoadOrderProfile.Deserialize(path);
            }

            PackageManager.instance.LoadPackages();
            PluginManager.instance.LoadPlugins();
            DLCManager.instance.Load();
            LSMManager.instance.Load();

            if (Profile != null) {
                Log.Info("loading profile ...");
                PluginManager.instance.LoadFromProfile(Profile, replace: true);
                PackageManager.instance.LoadFromProfile(Profile, replace: true);
                //DLCManager.instance.LoadFromProfile(Profile, replace: true);
                //LSMManager.instance.LoadFromProfile(Profile, replace: true);
                Log.Info("saving profile ...");
                ConfigWrapper.instance.SaveConfig();
                Log.Info("Successful!");
            }
        }
    }
}
