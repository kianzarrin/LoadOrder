namespace LoadOrderTool.Data {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using CO.PlatformServices;
    using LoadOrderShared;

    public interface IWSItem {
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

        public void ResetCache();
    }
}
