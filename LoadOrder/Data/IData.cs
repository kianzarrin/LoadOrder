namespace LoadOrderTool.Data {
    using CO.PlatformServices;
    using LoadOrderShared;
    using System;

    public interface IData {
        public void ResetCache();

        /// <summary>
        /// discards changes
        /// reloads configs
        /// reloads CS cache.
        /// </summary>
        public void ReloadConfig();
    }

    public interface IDataManager : IData {
        /// <summary>
        /// load or reload data
        /// </summary>
        void Load();
        void LoadFromProfile(LoadOrderProfile profile, bool replace = true);
        bool IsLoading { get; }
        bool IsLoaded { get; }
        event Action EventLoaded;

        /// <summary>
        /// save data to config but do not serialize
        /// </summary>
        void Save();
        void SaveToProfile(LoadOrderProfile profile);

    }
    public interface IWSItem : IData {
        public bool IsWorkshop { get; }
        //public bool IsLocal { get; }
        //public bool IsBuiltIn { get; }

        public PublishedFileId PublishedFileId { get; }

        public string DisplayText { get; }

        public string Description { get; }

        public string IncludedPath { get; }

        public bool IsIncludedPending { get; }

        public string Author { get; }

        public SteamCache.Item ItemCache { get; }
        public CSCache.Item CSItemCache { get; }
        public LoadOrderShared.ItemInfo ItemConfig { get; }
    }
}
