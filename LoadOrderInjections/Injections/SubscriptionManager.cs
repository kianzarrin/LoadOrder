using ColossalFramework.PlatformServices;
using KianCommons;
using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace LoadOrderInjections {
    public class TestComponent : MonoBehaviour {
        void Awake() => Log.Debug("TestComponent.Awake() was called");
        void Start() => Log.Debug("TestComponent.Start() was called");

        void OnGUI() {
            if (GUILayout.Button("Check Installed"))
                SteamUtilities.EnsureAll();
            if (GUILayout.Button("Delete UnInstalled"))
                SteamUtilities.DeleteExtra();
        }
    }

    public class Example : MonoBehaviour {
        private enum UpDown { Down = -1, Start = 0, Up = 1 };
        private UpDown textChanged = UpDown.Start;
        private Text TextComponent;

        // Load the Arial font from the Unity Resources folder.
        static Font Arial = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");

        void Awake() {
            Log.Debug("Example.Awake() was called");

            var canvas = AddCanvas();
            TextComponent = AddText(canvas.gameObject, "press any key");
            //var btn1 = AddButton(
            //    canvas.gameObject, 
            //    "RequestItemDetails", 
            //    Vector2.zero,
            //    SteamUtilities.OnRequestItemDetailsClicked);
            //var btn2 = AddButton(
            //    canvas.gameObject, 
            //    "QueryItems", 
            //    new Vector2(0, 250),
            //    SteamUtilities.OnQueryItemsClicked);
        }

        static Canvas AddCanvas() {
            // Create Canvas GameObject.
            GameObject canvasGO = new GameObject();
            canvasGO.name = "Canvas";
            canvasGO.AddComponent<Canvas>();
            canvasGO.AddComponent<CanvasScaler>();
            canvasGO.AddComponent<GraphicRaycaster>();

            // Get canvas from the GameObject.
            Canvas canvas = canvasGO.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            return canvas;
        }

        static Text AddText(GameObject parent, string label) {
            // Create the Text GameObject.
            GameObject textGO = new GameObject();
            textGO.transform.parent = parent.transform;
            textGO.AddComponent<Text>();

            // Set Text component properties.
            Text text = textGO.GetComponent<Text>();
            text.font = Arial;
            text.text = label;
            text.fontSize = 48;
            text.alignment = TextAnchor.MiddleCenter;

            // Provide Text position and size using RectTransform.
            RectTransform rectTransform = text.GetComponent<RectTransform>();
            rectTransform.localPosition = new Vector3(0, 0, 0);
            rectTransform.sizeDelta = new Vector2(600, 200);

            return text;
        }

        //static Button AddButton(GameObject canvasGO, string label, Vector2 position, UnityAction action)
        //{
        //    // Create the Text GameObject.
        //    GameObject buttonGO = new GameObject();
        //    buttonGO.transform.parent = canvasGO.transform;
        //    var button = buttonGO.AddComponent<Button>();

        //    var image = buttonGO.AddComponent<Image>();
        //    image.sprite = new Sprite()

        //    var text = AddText(buttonGO, label);

        //    RectTransform rectTransform = button.GetComponent<RectTransform>();
        //    rectTransform.sizeDelta = new Vector2(600, 200);
        //    rectTransform.localPosition = rectTransform.sizeDelta * 0.5f + new Vector2(50, 50) + position;
        //    button.onClick.AddListener(action);
        //    return button;
        //}

        void Update() {
            // Press the space key to change the Text message.
            if (Input.GetKeyDown(KeyCode.Space)) {
                if (textChanged != UpDown.Down) {
                    TextComponent.text = "Text changed";
                    textChanged = UpDown.Down;
                } else {
                    TextComponent.text = "Text changed back";
                    textChanged = UpDown.Up;
                }
            }
        }
    }

    public static class SubscriptionManager {
        /// <returns>true, to avoid loading intro</returns>
        public static bool PostBootAction() {
            bool sman = Environment.GetCommandLineArgs().Any(_arg => _arg == "-sman");
            if (sman) {
                new GameObject().AddComponent<Camera>();
                //new GameObject("base").AddComponent<Example>();
                new GameObject("test component go").AddComponent<TestComponent>();
            }
            return sman;
        }

        public static void EnsureSubs() {
            DirectoryInfo d = new DirectoryInfo("");
        }
    }

    public static class SteamUtilities {
        // steam manager is already initialized at this point.
        public static void RegisterEvents() {
            Log.Debug(Environment.StackTrace);
            PlatformService.eventSteamControllerInit += OnInitSteamController;

            PlatformService.eventGameOverlayActivated += OnGameOverlayActivated;
            PlatformService.workshop.eventSubmitItemUpdate += OnSubmitItemUpdate;
            PlatformService.workshop.eventWorkshopItemInstalled += OnWorkshopItemInstalled;
            PlatformService.workshop.eventWorkshopSubscriptionChanged += OnWorkshopSubscriptionChanged;
            PlatformService.workshop.eventUGCQueryCompleted += OnUGCQueryCompleted;
            PlatformService.workshop.eventUGCRequestUGCDetailsCompleted += OnUGCRequestUGCDetailsCompleted;
        }

        public static void OnInitSteamController() {
            Log.Debug(Environment.StackTrace);
            bool sman = Environment.GetCommandLineArgs().Any(_arg => _arg == "-sman");
            if (sman)
                EnsureAll();
            else foreach (var id in PlatformService.workshop.GetSubscribedItems())
                EnsureIncludedOrExcluded(id);
        }

        public static void OnRequestItemDetailsClicked() {
            Log.Debug("RequestItemDetails pressed");
            foreach (var id in PlatformService.workshop.GetSubscribedItems())
                PlatformService.workshop.RequestItemDetails(id).LogRet($"RequestItemDetails({id.AsUInt64})");
            //var id = new PublishedFileId(2040656402ul);
            //PlatformService.workshop.RequestItemDetails(id).LogRet($"RequestItemDetails({id.AsUInt64})");
        }
        public static void OnQueryItemsClicked() {
            Log.Debug("QueryItems pressed");
            PlatformService.workshop.QueryItems().LogRet($"QueryItems()"); ;
        }

        private static void OnSubmitItemUpdate(SubmitItemUpdateResult result, bool ioError) {
            Log.Debug($"PlatformService.workshop.eventSubmitItemUpdate(result:{result.result}, {ioError})");
        }


        private static void OnGameOverlayActivated(bool active) {
            Log.Debug($"PlatformService.workshop.eventGameOverlayActivated({active})");
        }

        private static void OnWorkshopItemInstalled(PublishedFileId id) {
            Log.Debug($"PlatformService.workshop.eventWorkshopItemInstalled({id.AsUInt64})");
        }

        private static void OnWorkshopSubscriptionChanged(PublishedFileId id, bool subscribed) {
            Log.Debug($"PlatformService.workshop.eventWorkshopSubscriptionChanged({id.AsUInt64}, {subscribed})");
        }

        static void OnUGCQueryCompleted(UGCDetails result, bool ioError) {
            // called after QueryItems
            Log.Debug($"OnUGCQueryCompleted(" +
                $"result:{result.ToSTR()}, " +
                $"ioError:{ioError})");
        }

        static void OnUGCRequestUGCDetailsCompleted(UGCDetails result, bool ioError) {
            // called after RequestItemDetails
            //Log.Debug($"OnUGCRequestUGCDetailsCompleted(" +
            //    $"result:{result.ToSTR2()}, " +
            //    $"ioError:{ioError})");
            bool good = IsUGCUpToDate(result, out string reason);
            if (!good) {
                Log.Info($"[WARNING!] subscribed item not installed properly:{result.publishedFileId} {result.title} " +
                    $"reason={reason}. " +
                    $"try resintalling the item.",
                    true);
            } else {
                Log.Debug($"subscribed item is good:{result.publishedFileId} {result.title}");
            }
        }

        public static string ToSTR(this ref UGCDetails result)
            => $"UGCDetails({result.result} {result.publishedFileId.AsUInt64})";

        public static string ToSTR2(this ref UGCDetails result) {
#pragma warning disable
            string m =
                $"UGCDetails:\n" +
                $"    publishedFileId:{result.publishedFileId}\n" +
                $"    title:{result.title}\n" +
                $"    creator: {result.creatorID} : {result.creatorID.ToAuthorName()}\n" +
                $"    result:{result.result}\n" +
                $"    timeUpdated:{result.timeUpdated} : {result.timeUpdated.ToLocalTime()}\n" +
                $"    timeCreated:{result.timeCreated} : {result.timeCreated.ToLocalTime()}\n" +
                $"    fileSize:{result.fileSize}\n" +
                $"    tags:{result.tags}\n";

#pragma warning enable
            return m;
        }

        //code copied from package entry
        public static DateTime GetLocalTimeCreated(string modPath) {
            DateTime dateTime = DateTime.MinValue;

            foreach (string path in Directory.GetFiles(modPath)) {
                string ext = Path.GetExtension(path);
                //if (ext == ".dll" || ext == ".crp" || ext == ".png")
                {
                    DateTime creationTimeUtc = File.GetCreationTimeUtc(path);
                    if (creationTimeUtc > dateTime) {
                        dateTime = creationTimeUtc;
                    }
                }
            }
            return dateTime;
        }

        //code copied from package entry
        public static DateTime GetLocalTimeUpdated(string modPath) {
            DateTime dateTime = DateTime.MinValue;
            if (Directory.Exists(modPath)) {
                foreach (string path in Directory.GetFiles(modPath)) {
                    string ext = Path.GetExtension(path);
                    //if (ext == ".dll" || ext == ".crp" || ext == ".png")
                    {
                        DateTime lastWriteTimeUtc = File.GetLastWriteTimeUtc(path);
                        if (lastWriteTimeUtc > dateTime) {
                            dateTime = lastWriteTimeUtc;
                        }
                    }
                }
            }
            return dateTime;
        }

        public static DateTime kEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime ToUTCTime(this uint time) => kEpoch.AddSeconds(time);
        public static DateTime ToLocalTime(this uint time) => kEpoch.AddSeconds(time).ToLocalTime();

        public static string ToAuthorName(this UserID userID) => new Friend(userID).personaName;

        public enum IsUGCUpToDateResult {
            OK,
            OutOfDate,
            NotDownloaded,
            PartiallyDownloaded,
        }

        static string STR(DateTime time) {
            var local = time.ToLocalTime().ToString();
            var utc = time.ToUniversalTime().ToShortTimeString();
            return $"{local} (UTC {utc})";
        }

        public static bool IsUGCUpToDate(UGCDetails det, out string reason) {
            string localPath = GetSubscribedItemFinalPath(det.publishedFileId);
            if (localPath == null) {
                reason = "subscribed item is not downloaded. path does not exits: " +
                    PlatformService.workshop.GetSubscribedItemPath(det.publishedFileId);
                return false;
            }

            var updatedServer = det.timeUpdated.ToUTCTime();
            var updatedLocal = GetLocalTimeUpdated(localPath).ToUniversalTime();
            var sizeServer = det.fileSize;
            var localSize = GetTotalSize(localPath);
            if (updatedLocal < updatedServer) {
                bool serious =
                    localSize < sizeServer ||
                    updatedLocal < updatedServer.AddHours(-24);
                string be = serious ? "is" : "may be";
                reason = $"subscribed item {be} out of date.\n\t" +
                    $"server-time={STR(updatedServer)} |  local-time={STR(updatedLocal)}";
                return false;
            }


            if (localSize < sizeServer ) // could be smaller if user has its own files in there.
            {
                reason = $"subscribed item download is incomplete. server-size={sizeServer}) local-size={localSize})";
                return false;
            }

            reason = null;
            return true;
        }

        public static long GetTotalSize(string path) {
            var files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
            return files.Sum(_f => new FileInfo(_f).Length);
        }

        public static string GetSubscribedItemFinalPath(PublishedFileId id) {
            string path = PlatformService.workshop.GetSubscribedItemPath(id);
            if (!Directory.Exists(path)) {
                path = ToExcludedPath(path);
                if (!Directory.Exists(path))
                    return null;
            }
            return path;
        }
        public static string ToExcludedPath(string includedPath) {
            string p1 = Path.GetDirectoryName(includedPath);
            string p2 = Path.GetFileName(includedPath);
            if (string.IsNullOrEmpty(p1) || string.IsNullOrEmpty(p2)) {
                Log.Error("LoadOrderInjections.ToExcludedPath()\n" +
                    $"includedPath={includedPath}" +
                    $"p1={p1}\n" +
                    $"p2={p2}\n");
            }
            if (p2.StartsWith("_")) {
                Log.Error($"includedPath={includedPath} should not start with _");
                return includedPath;
            }
            p2 = "_" + p2;
            return Path.Combine(p1, p2);
        }

        public static bool IsPathIncluded(string fullPath) {
            return Path.GetFileName(fullPath).StartsWith("_");
        }
        public static string ToIncludedPath(string fullPath) {
            string parent = Path.GetDirectoryName(fullPath);
            string file = Path.GetFullPath(fullPath);
            if (file.StartsWith("_"))
                file = file.Substring(1); //drop _
            return Path.Combine(parent, file);
        }
        public static string ToExcludedPath2(string fullPath) {
            string parent = Path.GetDirectoryName(fullPath);
            string file = Path.GetFullPath(fullPath);
            if (!file.StartsWith("_"))
                file = "_" + file;
            return Path.Combine(parent, file);
        }

        public static void EnsureIncludedOrExcluded(PublishedFileId id) {
            try {
                string path1 = PlatformService.workshop.GetSubscribedItemPath(id);
                string path2 = ToExcludedPath(path1);
                if (Directory.Exists(path1) && Directory.Exists(path2)) {
                    Directory.Delete(path2, true);
                    Directory.Move(path1, path2);
                }
            } catch (Exception ex) {
                Log.Exception(ex, $"EnsureIncludedOrExcluded({id})", showInPanel: false);
            }
        }

        public static void EnsureIncludedOrExcludedFiles(string path) {
            try {
                foreach (string file in Directory.GetFiles(path))
                    EnsureFile(file);
            } catch (Exception ex) {
                Log.Exception(ex);
            }
        }

        public static void EnsureFile(string fullFilePath) {
            if (string.IsNullOrEmpty(fullFilePath)) return;
            string included = ToIncludedPath(fullFilePath);
            string excluded = ToExcludedPath2(fullFilePath);
            if (File.Exists(included) && File.Exists(excluded)) {
                File.Delete(excluded);
                File.Move(included, excluded);
                fullFilePath = excluded;
            }
        }

        public static void DeleteExtra() {
            Log.Info("DeleteExtra called ...");
            var items = PlatformService.workshop.GetSubscribedItems();
            if (items == null || items.Length == 0)
                return;

            var path = PlatformService.workshop.GetSubscribedItemPath(items[0]);
            path = Path.GetDirectoryName(path);

            foreach (var dir in Directory.GetDirectories(path)) {
                ulong id;
                string strID = Path.GetFileName(dir);
                if (strID.StartsWith("_"))
                    strID = strID.Substring(1);
                if (!ulong.TryParse(strID, out id))
                    continue;
                bool deleted = !items.Any(item => item.AsUInt64 == id);
                if (deleted) {
                    Log.Info("[Warning!] unsubbed mod will be deleted: " + dir);
                    Directory.Delete(dir, true);
                }
            }
        }

        public static void EnsureAll() {
            Log.Info("EnsureAll called ...");
            var items = PlatformService.workshop.GetSubscribedItems();
            foreach (var id in items) {
                EnsureIncludedOrExcluded(id);
                PlatformService.workshop.RequestItemDetails(id)
                    .LogRet($"RequestItemDetails({id.AsUInt64})");
            }
        }

    }
}
