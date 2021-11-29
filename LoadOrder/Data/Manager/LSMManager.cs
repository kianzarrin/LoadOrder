using System;
using System.IO;
using System.Linq;
using CO;

namespace LoadOrderTool.Data {
    public class LSMManager : SingletonLite<LSMManager>, IDataManager {
        static ConfigWrapper ConfigWrapper => ConfigWrapper.instance;
        static LoadingScreenMod.Settings LSMConfig => ConfigWrapper.instance.LSMConfig;
        public bool IsLoading { get; private set; }
        public bool IsLoaded { get; private set; }
        public event Action EventLoaded;

        string skipPath_;
        public string SkipPath {
            get => skipPath_;
            set {
                Log.Called(value);
                if (value == skipPath_)
                    return;
                if (!File.Exists(value?.Trim())) {
                    value = null;
                    Log.Warning($"{value} does not exist");
                }
                skipPath_ = value;
                ConfigWrapper.Dirty = true;
            }
        }

        public bool SkipPrefabs => skipPath_ != null;

        private bool loadEnabled_ = true;
        public bool LoadEnabled {
            get => loadEnabled_;
            set {
                loadEnabled_ = value;
                ConfigWrapper.Dirty = true;
            }
        }

        private bool LoadUsed_ = true;
        public bool LoadUsed {
            get => LoadUsed_;
            set {
                LoadUsed_ = value;
                ConfigWrapper.Dirty = true;
            }
        }



        public void Load() {
            try {
                Log.Called();
                IsLoading = true;
                IsLoaded = false;
                if (LSMConfig.skipPrefabs)
                    SkipPath = LSMConfig.skipFile;
                else
                    SkipPath = null;
                loadEnabled_ = LSMConfig.loadEnabled;
                LoadUsed_ = LSMConfig.loadUsed;
            } catch (Exception ex) { ex.Log(); }
            try { EventLoaded?.Invoke(); } catch (Exception ex) { ex.Log(); }
        }

        public void Save() {
            try {
                LSMConfig.skipPrefabs = SkipPrefabs;
                if (SkipPrefabs)
                    LSMConfig.skipFile = SkipPath;
                LSMConfig.loadEnabled = LoadEnabled;
                LSMConfig.loadUsed = LoadUsed;
            } catch (Exception ex) { ex.Log(); }
        }

        public void LoadFromProfile(LoadOrderProfile profile, bool replace = true) {
            Log.Called("replaced" + replace);
            if (replace) {
                SkipPath = profile.SkipFilePathFinal;
            } else if(SkipPath != profile.SkipFilePathFinal) {
                // merge can only take place if skip files are the same.
                // other wise just include everything to be safe.
                SkipPath = null;
            }
            LoadEnabled = profile.LoadEnabled;
            LoadUsed = profile.LoadUsed;
        }

        public void SaveToProfile(LoadOrderProfile profile) {
            profile.SkipFilePathFinal = SkipPath;
            profile.LoadEnabled = LoadEnabled;
            profile.LoadUsed = LoadUsed;
        }

        public void ResetCache() { }

        public void ReloadConfig() {
            if(LSMConfig.skipPrefabs)
                SkipPath = LSMConfig.skipFile;
            else
                SkipPath = null;
            loadEnabled_ = LSMConfig.loadEnabled;
            LoadUsed_ = LSMConfig.loadUsed;
        }
    }
}
