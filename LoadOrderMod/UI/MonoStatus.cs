namespace LoadOrderMod.UI {
    using KianCommons;
    using UnityEngine;
    using ColossalFramework.UI;
    using System.Collections;
    using KianCommons.IImplict;
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    internal class MonoStatus : MonoBehaviour, IStartingObject {
        #region LifeCycle

        public static MonoStatus Instance => FindObjectOfType<MonoStatus>();
        public static void Ensure() => _ = Instance ?? Create();
        static MonoStatus Create() => UIView.GetAView()?.gameObject.AddComponent<MonoStatus>();

        public static void Release() {
            DecreaseRefCount(GetStatuslabel());
            DestroyImmediate(Instance?.gameObject);
        }

        public void Start() {
            try {
                CreateOrIncreaseRefCount();
            } catch (Exception ex) {
                Log.Exception(ex);
            }
        }

        static UILabel CreateOrIncreaseRefCount() {
            if (GetStatuslabel() is UILabel label) {
                label.objectUserData ??= 1; // recover from failure.
                label.objectUserData = (int)label.objectUserData + 1;
                return label;
            } else {
                return CreateLabel();
            }
        }

        static UILabel CreateLabel() {
            if (IsDebugMono()) {
                Log.Warning("using DEBUG MONO is slow! use Load order tool to launch game in release mode!", true);
            }

            if (Helpers.InStartupMenu)
                return SetupStatusAboveChirper();
            else
                return SetupStatusInGame();
        }


        static void DecreaseRefCount(UILabel label) {
            if (label == null)
                return;

            label.objectUserData ??= 1; // recover from failure.
            label.objectUserData = (int)label.objectUserData - 1;
            if ((int)label.objectUserData <= 0)
                DestroyImmediate(label.gameObject);
        }
        #endregion 

        const string LABEL_NAME = "MonoDebugStatusLabel";

        static UILabel SetupStatusAboveChirper() {
            Log.Info("Setting up status text around the chirper logo");
            var chirperSprite = UIView.GetAView().FindUIComponent<UISprite>("Chirper");
            var LOMStatusLabel = chirperSprite.parent.AddUIComponent<UILabel>();
            LOMStatusLabel.name = LABEL_NAME;
            LOMStatusLabel.text = GetText();
            LOMStatusLabel.textColor = new Color(0.97f, 1f, 0.69f);
            LOMStatusLabel.bottomColor = new Color(1f, 0.2f, 0f);
            LOMStatusLabel.useGradient = true;
            LOMStatusLabel.relativePosition = new Vector3(140, 10);
            LOMStatusLabel.zOrder = 0;
            LOMStatusLabel.tooltip = "controlled by Load Order tool";
            LOMStatusLabel.objectUserData = 1; //refcount
            return LOMStatusLabel;
        }

        static UILabel SetupStatusInGame() {
            Log.Info("Setting up status text around the chirper logo");
            UILabel floatingStatus = UIView.GetAView().AddUIComponent(typeof(FloatingMonoStatus)) as UILabel;
            floatingStatus.name = LABEL_NAME;
            floatingStatus.text = GetText();

            return floatingStatus;
        }

        static UILabel GetStatuslabel() {
            return UIView.GetAView()?.FindUIComponent<UILabel>(LABEL_NAME);
        }

        public void ModLoaded() => ShowText("Mod Loaded");

        public void ModUnloaded() => ShowText("Mod Unloaded");
        

        Coroutine ShowText(string text, float sec = 4) => StartCoroutine(ShowTextCoroutine(text, sec));

        private IEnumerator ShowTextCoroutine(string text, float sec) {
            ShowText(text, true);
            yield return new WaitForSeconds(sec);
            ShowText(text, false);
            yield return null;
        }

        static void ShowText(string text, bool visible) {
            var lbl = GetStatuslabel();
            if (!lbl) return;
            if (visible) {
                if (Helpers.InStartupMenu) {
                    lbl.text = text + "\n" + lbl.text;
                } else {
                    lbl.text = lbl.text + "\n" + text;
                }
            } else {
                int index = lbl.text.IndexOf(text);
                lbl.text = lbl.text.Remove(startIndex: index, count: text.Length);
                lbl.text = lbl.text.RemoveEmptyLines().Trim();
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static bool IsDebugMono() {
            try {
                string file = new StackFrame(true).GetFileName();
                return file?.EndsWith(".cs") ?? false;
            }catch(Exception ex) {
                Log.Exception(ex);
                return false;
            }
        }

        static string GetText() {
            if (IsDebugMono())
                return "Debug Mono (SLOW!)";
            else if (Helpers.InStartupMenu)
                return "Release Mono";
            else
                return "";
        }
    }
}
