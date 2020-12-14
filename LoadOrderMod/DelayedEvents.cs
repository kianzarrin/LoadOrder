using System;
using ColossalFramework.Packaging;
using ColossalFramework.Plugins;
using KianCommons;
using static KianCommons.ReflectionHelpers;
using ColossalFramework.Threading;

namespace LoadOrderMod {
    //public interface IEventInvoker {
    //    public bool IsEnabled { get; }
    //    public bool IsMarked { get; set; }

    //    public void Disable();
    //    public void Enable();

    //    public void Trigger();
    //    public void Force();
    //    public void Flush();
    //}

    public static class AssetStateChangedInvoker {
        public static void Disable() => PackageManager.DisableEvents();
        public static void Enable() => PackageManager.EnabledEvents();
        public static bool IsEnabled =>
            (int)GetFieldValue<PackageManager>("m_EventsEnabled") == 0;
        
        public static bool IsMarked { get; set; }

        public static void Trigger() {
            if (IsEnabled) {
                IsMarked = false;
                if (Dispatcher.currentSafe == ThreadHelper.dispatcher) {
                    PackageManager.ForceAssetStateChanged();
                } else {
                    ThreadHelper.dispatcher.Dispatch(delegate () {
                        PackageManager.ForceAssetStateChanged();
                    });
                }
            } else {
                IsMarked = true;
            }
        }

        public static void Force() {
            IsMarked = false;
            PackageManager.ForceAssetStateChanged();
        }

        public static void Flush() {
            if (IsMarked)
                Trigger();
        }
    }

    public static class PackagesChangedInvoker {
        public static void Disable() => PackageManager.DisableEvents();
        public static void Enable() => PackageManager.EnabledEvents();
        public static bool IsEnabled =>
            (int)GetFieldValue<PackageManager>("m_EventsEnabled") == 0;
        public static bool IsMarked { get; set; }

        public static void Trigger() {
            if (IsEnabled) {
                // includes AssetStateChanged
                IsMarked = AssetStateChangedInvoker.IsMarked = false;
                InvokeMethod(typeof(PackageManager), "TriggerEvents");
            } else IsMarked = true;
        }

        public static void Force() {
            IsMarked = AssetStateChangedInvoker.IsMarked = false;
            PackageManager.ForcePackagesChanged();
        }

        public static void Flush() {
            if (IsMarked)
                Trigger();
        }
    }

    public static class PluginsChangedInvoker {
        public static void Disable() => PluginManager.DisableEvents();
        public static void Enable() => PluginManager.EnabledEvents();
        public static bool IsEnabled =>
            (int)GetFieldValue<PluginManager>("m_EventsEnabled") == 0;
        public static bool IsMarked { get; set; }

        public static void Trigger() {
            if (IsEnabled) {
                // includes PluginsStateChanged
                IsMarked = PluginsStateChangedInvoker.IsMarked = false;
                //TODO use Dispatcher
                PluginManager.instance.ForcePluginsChanged();
                // ForcePluginsChanged() calls TriggerEventPluginsChanged()
                // InvokeMethod(PluginManager.instance, "TriggerEventPluginsChanged");
            } else IsMarked = true;
        }

        public static void Force() {
            IsMarked = PluginsStateChangedInvoker.IsMarked = false;
            //todo: force (this does not actually force).
            PluginManager.instance.ForcePluginsChanged();
        }

        public static void Flush() {
            if (IsMarked)
                Trigger();
        }
    }

    public static class PluginsStateChangedInvoker {
        public static void Disable() => PluginManager.DisableEvents();
        public static void Enable() => PluginManager.EnabledEvents();
        public static bool IsEnabled =>
            (int)GetFieldValue<PluginManager>("m_EventsEnabled") == 0;
        public static bool IsMarked { get; set; }

        public static void Trigger() {
            if (IsEnabled) {
                IsMarked = false;
                // TODO use Dispatcher
                InvokeMethod(PluginManager.instance, "TriggerEventPluginsStateChanged");
            } else IsMarked = true;
        }

        public static void Force() {
            IsMarked = false;
            // todo: force 
            InvokeMethod(PluginManager.instance, "TriggerEventPluginsStateChanged");
        }

        public static void Flush() {
            if (IsMarked)
                Trigger();
        }
    }

    public static class DelayedEventInvoker {
        public static void Enable() {
            PackageManager.EnabledEvents();
            PluginManager.EnabledEvents();
        }
        public static void Disable() {
            PackageManager.DisableEvents();
            PluginManager.DisableEvents();
        }
        public static void Flush() {
            PackagesChangedInvoker.Flush();
            AssetStateChangedInvoker.Flush();
            PluginsChangedInvoker.Flush();
            PluginsStateChangedInvoker.Flush();
        }
    }
}
