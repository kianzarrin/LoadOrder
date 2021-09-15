namespace LoadOrderTool.Data {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using CO.PlatformServices;

    public interface IWSItem {
        public bool IsWorkshop { get; }
        //public bool IsLocal { get; }
        //public bool IsBuiltIn { get; }

        public PublishedFileId PublishedFileId { get; }

        public string DisplayText { get; }

        //public string Author { get; }

        public string IncludedPath { get; }

        public bool IsIncludedPending { get; }

        public string Author { get; }

        public LoadOrderCache.Item ItemCache { get; }
        public LoadOrderShared.ItemInfo ItemConfig { get; }

        public void ResetCache();
    }
}
