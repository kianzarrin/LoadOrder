using LoadOrderTool;
using System;

namespace CO {
    public abstract class SingletonLite<T> where T : new() {
        private static T sInstance;

        public static T instance {
            get {
                if (sInstance == null) {
                    sInstance = (default(T) == null) ? Activator.CreateInstance<T>() : default(T);
                    Log.Debug("Creating singleton of type " + typeof(T).Name);
                }
                return sInstance;
            }
        }

        public static bool exists => sInstance != null;

        public static void Ensure() =>_ = instance;
    }
}
