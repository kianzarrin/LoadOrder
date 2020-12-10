using KianCommons;
using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static KianCommons.ReflectionHelpers;
using ColossalFramework;
using ColossalFramework.PlatformServices;
using UnityEngine.Events;

namespace LoadOrderInjections {
    public class TestComponent : MonoBehaviour {
        void Awake() => Log.Debug("TestComponent.Awake() was called");
        void Start() => Log.Debug("TestComponent.Start() was called");

        void OnGUI()
        {
            if (GUILayout.Button("RequestItemDetails"))
                SteamUtilities.OnRequestItemDetailsClicked();
            if (GUILayout.Button("QueryItems"))
                SteamUtilities.OnQueryItemsClicked();
        }
    }

    public class Example : MonoBehaviour {
        private enum UpDown { Down = -1, Start = 0, Up = 1 };
        private UpDown textChanged = UpDown.Start;
        private Text TextComponent;

        // Load the Arial font from the Unity Resources folder.
        static Font Arial = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");

        void Awake()
        {
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

        static Canvas AddCanvas()
        {
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

        static Text AddText(GameObject parent, string label)
        {
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

        void Update()
        {
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
        public static bool PostBootAction()
        {
            bool sman = Environment.GetCommandLineArgs().Any(_arg => _arg == "-sman");
            if (sman) {
                new GameObject().AddComponent<Camera>();
                new GameObject("base").AddComponent<Example>();
                new GameObject("test component go").AddComponent<TestComponent>();
            }
            return sman;
        }

        public static void EnsureSubs()
        {
            DirectoryInfo d = new DirectoryInfo("");
        }
    }

    public static class SteamUtilities {
        // steam manager is already initialized at this point.
        public static void RegisterEvents()
        {
            Log.Debug(Environment.StackTrace);
            PlatformService.eventSteamControllerInit += OnInitSteamController;

            PlatformService.eventGameOverlayActivated += OnGameOverlayActivated;
            PlatformService.workshop.eventSubmitItemUpdate += OnSubmitItemUpdate;
            PlatformService.workshop.eventWorkshopItemInstalled += OnWorkshopItemInstalled;
            PlatformService.workshop.eventWorkshopSubscriptionChanged += OnWorkshopSubscriptionChanged;
            PlatformService.workshop.eventUGCQueryCompleted += OnUGCQueryCompleted;
            PlatformService.workshop.eventUGCRequestUGCDetailsCompleted += OnUGCRequestUGCDetailsCompleted;
        }

        public static void OnInitSteamController()
        {
            Log.Debug(Environment.StackTrace);
        }

        public static void OnRequestItemDetailsClicked()
        {
            Log.Debug("RequestItemDetails pressed");
            var id = new PublishedFileId(2040656402ul);
            //foreach (var id in PlatformService.workshop.GetSubscribedItems()) 
                PlatformService.workshop.RequestItemDetails(id).LogRet($"RequestItemDetails({id.AsUInt64})");
        }
        public static void OnQueryItemsClicked()
        {
            Log.Debug("QueryItems pressed");
            PlatformService.workshop.QueryItems().LogRet($"QueryItems()"); ;
        }

        private static void OnSubmitItemUpdate(SubmitItemUpdateResult result, bool ioError)
        {
            Log.Debug($"PlatformService.workshop.eventSubmitItemUpdate(result:{result.result}, {ioError})");
        }


        private static void OnGameOverlayActivated(bool active)
        {
            Log.Debug($"PlatformService.workshop.eventGameOverlayActivated({active})");
        }

        private static void OnWorkshopItemInstalled(PublishedFileId id)
        {
            Log.Debug($"PlatformService.workshop.eventWorkshopItemInstalled({id.AsUInt64})");
        }

        private static void OnWorkshopSubscriptionChanged(PublishedFileId id, bool subscribed)
        {
            Log.Debug($"PlatformService.workshop.eventWorkshopSubscriptionChanged({id.AsUInt64}, {subscribed})");
        }

        static void OnUGCQueryCompleted(UGCDetails result, bool ioError)
        {
            Log.Debug($"OnUGCQueryCompleted(" +
                $"result:{result.ToSTR()}, " +
                $"ioError:{ioError})");
        }
        static void OnUGCRequestUGCDetailsCompleted(UGCDetails result, bool ioError)
        {
            Log.Debug($"OnUGCRequestUGCDetailsCompleted(" +
                $"result:{result.ToSTR()}, " +
                $"ioError:{ioError})");
        }

        public static string ToSTR(this ref UGCDetails result)
            => $"UGCDetails({result.result} {result.publishedFileId.AsUInt64})";





    }
}
