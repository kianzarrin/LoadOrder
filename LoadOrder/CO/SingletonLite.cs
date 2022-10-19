using LoadOrderTool;
using System;

namespace CO {
    public abstract class SingletonLite<T> where T : new() {
        protected static T sInstance;
        private static object lockObject = new object();
        public static T instance {
            get {
                if (sInstance == null) {
                    lock (lockObject) {
                        sInstance = new T();
                        Log.Debug("Creating singleton of type " + typeof(T).Name);
                    }
                }
                return sInstance;
            }
        }

        public static bool exists => sInstance != null;

        public static void Ensure() => _ = instance;
    }
}
