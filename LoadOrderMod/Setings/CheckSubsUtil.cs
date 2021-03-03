using ColossalFramework.PlatformServices;
using KianCommons;
using System;
using System.IO;
using System.Linq;

public static class CheckSubsUtil {
    static void RegisterEvent() {
        PlatformService.workshop.eventUGCRequestUGCDetailsCompleted -= OnUGCRequestUGCDetailsCompleted;
        PlatformService.workshop.eventUGCRequestUGCDetailsCompleted += OnUGCRequestUGCDetailsCompleted;
    }

    public static void EnsureAll() {
        RegisterEvent();
        Log.Info("EnsureAll called ...",true);
        LoadOrderInjections.SteamUtilities.EnsureAll();
    }

    public static void OnUGCRequestUGCDetailsCompleted(UGCDetails result, bool ioError) {
        // called after RequestItemDetails
        //Log.Debug($"OnUGCRequestUGCDetailsCompleted(" +
        //    $"result:{result.ToSTR2()}, " +
        //    $"ioError:{ioError})");
        bool good = LoadOrderInjections.SteamUtilities.IsUGCUpToDate(result, out string reason);
        if (!good) {
            Log.Info($"[WARNING!] subscribed item not installed properly:" +
                $"{result.publishedFileId} {result.title}\n" +
                $"reason={reason}. " +
                $"try resintalling the item.", true);
        } else {
            Log.Debug($"subscribed item is good:{result.publishedFileId} {result.title}", false);
        }
    }
}