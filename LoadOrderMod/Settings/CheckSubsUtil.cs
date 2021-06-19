namespace LoadOrderMod.Settings {
    extern alias Injections;
    using Injections.LoadOrderInjections;
    using SteamUtilities = Injections.LoadOrderInjections.SteamUtilities;

    using ColossalFramework.PlatformServices;
    using KianCommons;
    using UnityEngine;
    using System.Collections;
    using System.IO;
    using System.Linq;

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
            Log.Called();
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

        public Coroutine DeleteUnsubbed() => StartCoroutine(DeleteUnsubbedCoroutine());

        public IEnumerator DeleteUnsubbedCoroutine() {
            Log.Info("DeleteUnsubbed called ...");
            var items = PlatformService.workshop.GetSubscribedItems();
            if (items == null || items.Length == 0)
                yield break;

            var path = PlatformService.workshop.GetSubscribedItemPath(items[0]);
            path = Path.GetDirectoryName(path);

            int counter = 0;
            foreach (var dir in Directory.GetDirectories(path)) {
                ulong id;
                string strID = Path.GetFileName(dir);
                if (strID.StartsWith("_"))
                    strID = strID.Substring(1);
                if (!ulong.TryParse(strID, out id))
                    continue;
                bool deleted = !items.Any(item => item.AsUInt64 == id);
                if (deleted) {
                    Log.Warning("unsubbed item will be deleted: " + dir);
                    Directory.Delete(dir, true);
                }

                if (counter >= 100) {
                    counter = 0;
                    yield return 0;
                }
            }
        }

        public Coroutine Resubscribe(PublishedFileId id) => StartCoroutine(ResubscribeCoroutine(id));
        public IEnumerator ResubscribeCoroutine(PublishedFileId id) {
            Log.Called(id);
            if (id != PublishedFileId.invalid) {
                PlatformService.workshop.Unsubscribe(id);
                yield return new WaitForSeconds(5);
                PlatformService.workshop.Subscribe(id);
            }
        }
    }
}