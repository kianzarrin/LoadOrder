namespace LoadOrderMod.Util {
    extern alias Injections;
    using SteamUtilities = Injections.LoadOrderInjections.SteamUtilities;
    using ColossalFramework.PlatformServices;
    using ColossalFramework.Plugins;
    using KianCommons;
    using UnityEngine;
    using System.Collections;
    using System.IO;
    using System.Linq;
    using System;
    using Injections.LoadOrderShared;
    using Settings;
    using ColossalFramework.IO;

    public class CheckSubsUtil : MonoBehaviour {
        static GameObject go_;
        public static CheckSubsUtil Ensure() => Instance;

        public static CheckSubsUtil Instance {
            get {
                if (!go_) {
                    go_ = new GameObject(nameof(CheckSubsUtil), typeof(CheckSubsUtil));
                }
                return go_.GetComponent<CheckSubsUtil>();
            }
        }

        public static void EnsureAll() {
            Instance.EnsureIncludedOrExcluded();
            Instance.RequestItemDetails();
        }

        public Coroutine EnsureIncludedOrExcluded() => StartCoroutine(EnsureIncludedOrExcludedCoroutine());
        public IEnumerator EnsureIncludedOrExcludedCoroutine() {
            Log.Called();
            var items = PlatformService.workshop.GetSubscribedItems();
            int counter = 0;
            foreach (var id in items) {
                SteamUtilities.EnsureIncludedOrExcluded(id);
                if (counter >= 100) {
                    counter = 0;
                    yield return 0;
                }
            }
        }

        public Coroutine RequestItemDetails() => StartCoroutine(RequiestItemDetailsCoroutine());
        public IEnumerator RequiestItemDetailsCoroutine() {
            Log.DisplayMesage($"Checking all items ...");
            var items = PlatformService.workshop.GetSubscribedItems();
            int counter = 0;
            foreach (var id in items) {
                PlatformService.workshop.RequestItemDetails(id)
                    .LogRet($"RequestItemDetails({id.AsUInt64})");
                if (counter >= 100) {
                    counter = 0;
                    yield return 0;
                }
            }
        }

        public Coroutine UnsubDepricated() => StartCoroutine(UnsubDepricatedCoroutine());

        public IEnumerator UnsubDepricatedCoroutine() {
            Log.DisplayMesage($"Unsubscribing from depricated items ...");

            var items = PlatformService.workshop.GetSubscribedItems();
            if (items == null || items.Length == 0)
                yield break;

            int counter = 0;
            int nUnsubbed = 0;
            foreach(var item in items) {
                string path = PlatformService.workshop.GetSubscribedItemPath(item);
                if(path == null) {
                    Log.DisplayWarning($"Depricated item will be unsubbed: {item}");
                    PlatformService.workshop.Unsubscribe(item);
                    nUnsubbed++;
                }

                if (counter >= 100) {
                    counter = 0;
                    yield return 0;
                }
            }

            Log.DisplayMesage($"Unsubscribing from {nUnsubbed} depricated items.");
        }


        public Coroutine DeleteUnsubbed() => StartCoroutine(DeleteUnsubbedCoroutine());

        public IEnumerator DeleteUnsubbedCoroutine() {
            Log.DisplayMesage($"Deleting unsubscribed items.");
            var items = PlatformService.workshop.GetSubscribedItems();
            if (items == null || items.Length == 0)
                yield break;

            var path = PlatformService.workshop.GetSubscribedItemPath(items[0]);
            path = Path.GetDirectoryName(path);

            int counter = 0;
            int n = 0;
            foreach (var dir in Directory.GetDirectories(path)) {
                ulong id;
                string strID = Path.GetFileName(dir);
                if (strID.StartsWith("_"))
                    strID = strID.Substring(1);
                if (!ulong.TryParse(strID, out id))
                    continue;
                bool deleted = !items.Any(item => item.AsUInt64 == id);
                if (deleted) {
                    Log.DisplayWarning("unsubbed item will be deleted: " + dir);
                    Directory.Delete(dir, true);
                    n++;
                }

                if (counter >= 100) {
                    counter = 0;
                    yield return 0;
                }
            }

            Log.DisplayMesage($"Deleted {n} unsubscribed items.");
        }

        public Coroutine Resubscribe(PublishedFileId id) => StartCoroutine(ResubscribeCoroutine(id));
        public IEnumerator ResubscribeCoroutine(PublishedFileId id) {
            Log.Called(id);
            if (id != PublishedFileId.invalid) {
                try { PlatformService.workshop.Unsubscribe(id); } catch(Exception ex) { ex.Log(); }

                yield return new WaitForSeconds(3);

                try {
                    //string path = PlatformService.workshop.GetSubscribedItemPath(id);
                    //if (Directory.Exists(path))
                    //    Directory.Delete(path, true);
                } catch(Exception ex) { ex.Log(); }

                try { PlatformService.workshop.Subscribe(id); } catch (Exception ex) { ex.Log(); }
            }
        }

        public static void RemoveEvents() {
            PlatformService.workshop.eventUGCRequestUGCDetailsCompleted -= OnUGCRequestUGCDetailsCompleted;
            PlatformService.workshop.eventUGCRequestUGCDetailsCompleted -= SteamUtilities.OnUGCRequestUGCDetailsCompleted;
        }
        public static void RegisterEvents() {
            RemoveEvents();
            PlatformService.workshop.eventUGCRequestUGCDetailsCompleted += OnUGCRequestUGCDetailsCompleted;
        }

        private static void OnUGCRequestUGCDetailsCompleted(UGCDetails ugc, bool ioError) {
            try {
                var status = SteamUtilities.IsUGCUpToDate(ugc, out string reason);
                if (status != DownloadStatus.DownloadOK) {
                    string m =
                        "subscribed item not installed properly:" +
                        $"{ugc.publishedFileId} {ugc.title}\n" +
                        $"reason={reason}. " +
                        $"try reinstalling the item.";
                    Log.DisplayWarning(m);
                }
                var mod = ConfigUtil.Config.Mods.FirstOrDefault(_mod => GetID1(_mod.Path) == ugc.publishedFileId);
                if (mod != null) {
                    mod.Status = (LoadOrderShared.DownloadStatus)(int)status;
                    mod.DownloadFailureReason = reason;
                    ConfigUtil.SaveConfig();
                } else {
                    // TODO: this might take long. only check this when ready. (maybe in a parallel thread?)
                    if (false) {
                        foreach (var assetInfo in ConfigUtil.Config.Assets.Where(_asset => GetID2(_asset.Path) == ugc.publishedFileId)) {
                            assetInfo.Status = (LoadOrderShared.DownloadStatus)(int)status;
                            assetInfo.DownloadFailureReason = reason;
                            ConfigUtil.SaveConfig();
                        }
                    }
                }
            } catch (Exception ex) { ex.Log(); }
        }

        public static bool IsWorkshop(string path) => path != null && path.Contains(ConfigUtil.Config.WorkShopContentPath);

        public static PublishedFileId GetID1(string dir) {
            if (dir != null && dir.Contains(ConfigUtil.Config.WorkShopContentPath)) {
                string dirName = new DirectoryInfo(dir).Name;
                if (ulong.TryParse(dirName, out ulong id)){
                    return new PublishedFileId(id);
                }
            }
            return PublishedFileId.invalid;
        }
        public static PublishedFileId GetID2(string file) =>
            GetID1(new FileInfo(file).DirectoryName);

    }
}