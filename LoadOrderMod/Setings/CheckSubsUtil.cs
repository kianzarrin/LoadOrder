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
        Log.Info("EnsureAll called ...");
        var items = PlatformService.workshop.GetSubscribedItems();
        foreach (var id in items) {
            EnsureIncludedOrExcluded(id);
            PlatformService.workshop.RequestItemDetails(id)
                .LogRet($"RequestItemDetails({id.AsUInt64})");
        }
    }

    public static void OnUGCRequestUGCDetailsCompleted(UGCDetails result, bool ioError) {
        // called after RequestItemDetails
        //Log.Debug($"OnUGCRequestUGCDetailsCompleted(" +
        //    $"result:{result.ToSTR2()}, " +
        //    $"ioError:{ioError})");
        bool good = IsUGCUpToDate(result, out string reason);
        if (!good) {
            Log.Info($"[WARNING!] subscribed item not installed properly:{result.publishedFileId} {result.title} " +
                $"reason={reason}. " +
                $"try resintalling the item.");
        } else {
            Log.Debug($"subscribed item is good:{result.publishedFileId} {result.title}");
        }
    }

    public static string ToSTR(this ref UGCDetails result)
        => $"UGCDetails({result.result} {result.publishedFileId.AsUInt64})";

    public static string ToSTR2(this ref UGCDetails result) {
#pragma warning disable
        string m =
            $"UGCDetails:\n" +
            $"    publishedFileId:{result.publishedFileId}\n" +
            $"    title:{result.title}\n" +
            $"    creator: {result.creatorID} : {result.creatorID.ToAuthorName()}\n" +
            $"    result:{result.result}\n" +
            $"    timeUpdated:{result.timeUpdated} : {result.timeUpdated.ToLocalTime()}\n" +
            $"    timeCreated:{result.timeCreated} : {result.timeCreated.ToLocalTime()}\n" +
            $"    fileSize:{result.fileSize}\n" +
            $"    tags:{result.tags}\n";

#pragma warning enable
        return m;
    }

    //code copied from package entry
    public static DateTime GetLocalTimeCreated(string modPath) {
        DateTime dateTime = DateTime.MinValue;

        foreach (string path in Directory.GetFiles(modPath)) {
            string ext = Path.GetExtension(path);
            //if (ext == ".dll" || ext == ".crp" || ext == ".png")
            {
                DateTime creationTimeUtc = File.GetCreationTimeUtc(path);
                if (creationTimeUtc > dateTime) {
                    dateTime = creationTimeUtc;
                }
            }
        }
        return dateTime;
    }

    //code copied from package entry
    public static DateTime GetLocalTimeUpdated(string modPath) {
        DateTime dateTime = DateTime.MinValue;
        if (Directory.Exists(modPath)) {
            foreach (string path in Directory.GetFiles(modPath)) {
                string ext = Path.GetExtension(path);
                //if (ext == ".dll" || ext == ".crp" || ext == ".png")
                {
                    DateTime lastWriteTimeUtc = File.GetLastWriteTimeUtc(path);
                    if (lastWriteTimeUtc > dateTime) {
                        dateTime = lastWriteTimeUtc;
                    }
                }
            }
        }
        return dateTime;
    }

    public static DateTime kEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
    public static DateTime ToLocalTime(this uint time) => kEpoch.AddSeconds(time).ToLocalTime();

    public static string ToAuthorName(this UserID userID) => new Friend(userID).personaName;

    public enum IsUGCUpToDateResult {
        OK,
        OutOfDate,
        NotDownloaded,
        PartiallyDownloaded,
    }
    public static bool IsUGCUpToDate(UGCDetails data, out string reason) {
        string localPath = GetSubscribedItemFinalPath(data.publishedFileId);
        if (localPath == null) {
            reason = "subscribed item is not downloaded. path does not exits: " +
                PlatformService.workshop.GetSubscribedItemPath(data.publishedFileId);
            return false;
        }

        var updatedServer = data.timeUpdated.ToLocalTime();
        var updatedLocal = GetLocalTimeUpdated(localPath);
        if (updatedLocal < updatedServer) {
            reason = $"subscribed item is out of date. server-time={updatedServer}  local-time={updatedLocal}";
            return false;
        }

        var sizeServer = data.fileSize;
        var localSize = GetTotalSize(localPath);
        if (sizeServer > localSize) // could be smaller if user has its own files in there.
        {
            reason = $"subscribed item download is incomplete. server-size={sizeServer}) local-size={localSize})";
            return false;
        }

        reason = null;
        return true;
    }

    public static long GetTotalSize(string path) {
        var files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
        return files.Sum(_f => new FileInfo(_f).Length);
    }

    public static string GetSubscribedItemFinalPath(PublishedFileId id) {
        string path = PlatformService.workshop.GetSubscribedItemPath(id);
        if (!Directory.Exists(path)) {
            path = ToExcludedPath(path);
            if (!Directory.Exists(path))
                return null;
        }
        return path;
    }
    public static string ToExcludedPath(string includedPath) {
        string p1 = Path.GetDirectoryName(includedPath);
        string p2 = Path.GetFileName(includedPath);
        if (string.IsNullOrEmpty(p1) || string.IsNullOrEmpty(p2)) {
            Log.Error("CheckSubsUtil.ToExcludedPath()\n" +
                $"includedPath={includedPath}" +
                $"p1={p1}\n" +
                $"p2={p2}\n");
        }
        if (p2.StartsWith("_")) {
            Log.Error($"includedPath={includedPath} should not start with _");
            return includedPath;
        }
        p2 = "_" + p2;
        return Path.Combine(p1, p2);
    }

    public static void EnsureIncludedOrExcluded(PublishedFileId id) {
        try {
            string path1 = PlatformService.workshop.GetSubscribedItemPath(id);
            string path2 = ToExcludedPath(path1);
            if (Directory.Exists(path1) && Directory.Exists(path2)) {
                Directory.Delete(path2, true);
                Directory.Move(path1, path2);
            }
        } catch (Exception ex) {
            Log.Exception(ex, $"EnsureIncludedOrExcluded({id})" ,showInPanel: false);
        }
    }

    public static void DeleteExtra() {
        Log.Info("DeleteExtra called ...");
        var items = PlatformService.workshop.GetSubscribedItems();
        if (items == null || items.Length == 0)
            return;

        var path = PlatformService.workshop.GetSubscribedItemPath(items[0]);
        path = Path.GetDirectoryName(path);

        foreach (var dir in Directory.GetDirectories(path)) {
            ulong id;
            string strID = Path.GetFileName(dir);
            if (strID.StartsWith("_"))
                strID = strID.Substring(1);
            if (!ulong.TryParse(strID, out id))
                continue;
            bool deleted = !items.Any(item => item.AsUInt64 == id);
            if (deleted) {
                Log.Info("[Warning!] unsubbed mod will be deleted: " + dir);
                Directory.Delete(dir, true);
            }
        }
    }
}