using System;
using System.IO;
using System.Linq;
using CO;

namespace LoadOrderTool.Data {
    public class LSMManager : SingletonLite<LSMManager>, IDataManager {
        static ConfigWrapper ConfigWrapper => ConfigWrapper.instance;
        public bool IsLoading { get; private set; }
        public bool IsLoaded { get; private set; }
        public event Action EventLoaded;

        string skipPath_;
        public string SkipPath {
            get => skipPath_;
            set {
                if (value == skipPath_)
                    return;
                if(!File.Exists(value?.Trim()))
                    value = null;
                skipPath_ = value;
                ConfigWrapper.Dirty = true;
            }
        }

        public bool SkipPrefabs => skipPath_ != null;

        public void Load() {
            try {
                Log.Called();
                IsLoading = true;
                IsLoaded = false;
                if (ConfigWrapper.LSMConfig.skipPrefabs)
                    SkipPath = ConfigWrapper.LSMConfig.skipFile;
                else
                    SkipPath = null;
            } catch (Exception ex) { ex.Log(); }
            try { EventLoaded?.Invoke(); } catch (Exception ex) { ex.Log(); }
        }

        public void Save() {
            try {
                ConfigWrapper.LSMConfig.skipPrefabs = SkipPrefabs;
                if (SkipPrefabs)
                    ConfigWrapper.LSMConfig.skipFile = SkipPath;
            } catch (Exception ex) { ex.Log(); }
        }

        public void LoadFromProfile(LoadOrderProfile profile, bool replace = true) {
            if (replace) {
                SkipPath = profile.SkipFilePath;
            } else if(SkipPath != profile.SkipFilePath) {
                // merge can only take place if skip files are the same.
                // other wise just include everything to be safe.
                SkipPath = null; 
            }
        }

        public void SaveToProfile(LoadOrderProfile profile) {
            profile.SkipFilePath = SkipPath;
        }
    }
}
