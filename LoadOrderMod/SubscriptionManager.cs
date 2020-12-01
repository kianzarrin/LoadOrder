using ColossalFramework;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using KianCommons;

namespace LoadOrderMod {
    public class TestComponent : MonoBehaviour {
        void Awake() => Log.Debug("TestComponent.Awake() was called");
        void Start() => Log.Debug("TestComponent.Start() was called");

        void OnGUI() {
            Log.Info("OnGUI called");
            GUILayout.Label("OnGUI called");
        }
    }

    public static class SubscriptionManager {

        public static bool PostBootAction() {
            bool sman = false;
            OptionSet optionSet = new OptionSet();
            optionSet.Add("sman", "Subscription Manager", v => sman = v != null);
            optionSet.Parse(Environment.GetCommandLineArgs());
            if (sman) SMan();
            return sman;
        }

        static GameObject myGO;

        public static void SMan() {
            Log.Info("SMAN called ...", true);
            var scene = SceneManager.GetActiveScene();
            Log.Debug("current scene = " + scene);

            foreach (var t in scene.GetRootGameObjects())
                Log.Debug("scene has root game object:" + t.name);
            RectTransform rectTransform;

            myGO = Singleton<Starter>.instance.gameObject;

            // camera:
            Camera cam = myGO.AddComponent<Camera>();

            // Canvas
            myGO.name = "TestCanvas";
            Canvas myCanvas = myGO.AddComponent<Canvas>();
            myCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            myGO.AddComponent<CanvasScaler>();
            myGO.AddComponent<GraphicRaycaster>();

            // Text
            GameObject myText = new GameObject();
            myText.transform.parent = myGO.transform;
            myText.name = "wibble";

            Text text = myText.AddComponent<Text>();
            text.font = (Font)Resources.Load("MyFont");
            text.text = "wobble";
            text.fontSize = 100;

            // Text position
            rectTransform = text.GetComponent<RectTransform>();
            rectTransform.localPosition = new Vector3(0, 0, 0);
            rectTransform.sizeDelta = new Vector2(400, 200);

            var TestComponent = myGO.AddComponent<TestComponent>();
        }
    }
}
