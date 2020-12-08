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

        void OnGUI()
        {
            GUILayout.Label("OnGUI called");
        }
    }

    public class Example : MonoBehaviour {
        private enum UpDown { Down = -1, Start = 0, Up = 1 };
        private Text text;
        private UpDown textChanged = UpDown.Start;

        void Awake()
        {
            Log.Debug("Example.Awake() was called");

            // Load the Arial font from the Unity Resources folder.
            Font arial;
            arial = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");

            // Create Canvas GameObject.
            GameObject canvasGO = new GameObject();
            canvasGO.name = "Canvas";
            canvasGO.AddComponent<Canvas>();
            canvasGO.AddComponent<CanvasScaler>();
            canvasGO.AddComponent<GraphicRaycaster>();

            // Get canvas from the GameObject.
            Canvas canvas;
            canvas = canvasGO.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            // Create the Text GameObject.
            GameObject textGO = new GameObject();
            textGO.transform.parent = canvasGO.transform;
            textGO.AddComponent<Text>();

            // Set Text component properties.
            text = textGO.GetComponent<Text>();
            text.font = arial;
            text.text = "Press space key";
            text.fontSize = 48;
            text.alignment = TextAnchor.MiddleCenter;

            // Provide Text position and size using RectTransform.
            RectTransform rectTransform;
            rectTransform = text.GetComponent<RectTransform>();
            rectTransform.localPosition = new Vector3(0, 0, 0);
            rectTransform.sizeDelta = new Vector2(600, 200);
        }

        void Update()
        {
            // Press the space key to change the Text message.
            if (Input.GetKeyDown(KeyCode.Space)) {
                if (textChanged != UpDown.Down) {
                    text.text = "Text changed";
                    textChanged = UpDown.Down;
                } else {
                    text.text = "Text changed back";
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
            }
            return sman;
        }

        public static void EnsureSubs()
        {
            DirectoryInfo d = new DirectoryInfo("");
        }
    }
}
