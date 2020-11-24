namespace LoadOrderMod.Util {
    using System.Collections.Generic;
    using System.Threading;
    using System;
    public static class AuxiluryThread {
        static Thread thread_;
        private static Queue<Action> tasks_;
        public static void EnqueueAction(Action act) {
            lock (tasks_)
                tasks_.Enqueue(act);
        }
        private static Action DequeueAction() {
            lock (tasks_) {
                if (tasks_.Count == 0)
                    return null;
                return tasks_.Dequeue();
            }
        }

        public static void Start() {
            thread_ = new Thread(ThreadTask);
            thread_.IsBackground = false;
            thread_.Start();
        }

        public static void End() {
            thread_.Abort();
        }


        static void ThreadTask() {
            while (true) {
                Action act = DequeueAction();
                if (act != null) {
                    act();
                } else {
                    Thread.Sleep(100);
                }
            }
        }

        




    }
}
