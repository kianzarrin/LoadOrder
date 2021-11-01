namespace LoadOrderInjections {
    using UnityEngine;

    public class DoNothingComponent : MonoBehaviour {
        void Awake() => Debug.Log("TestComponent.Awake() was called");
        void Start() => Debug.Log("TestComponent.Start() was called");
        public static void DoNothing() {
            new GameObject().AddComponent<Camera>();
            new GameObject("nop go").AddComponent<DoNothingComponent>();
        }
    }
}
