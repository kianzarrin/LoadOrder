using ColossalFramework.Packaging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using static KianCommons.Patches.TranspilerUtils;
using KianCommons;
using static KianCommons.ReflectionHelpers;

namespace LoadOrderMod.Patches {
    [HarmonyPatch]
    public static class PrintLoadInfoPatch {
        static IEnumerable<MethodBase> TargetMethods() {
            yield return GetMethod(typeof(LoadingManager), "LoadLevelCoroutine");
            var tLevelLoader = Type.GetType("LoadingScreenMod.LevelLoader, LoadingScreenMod", throwOnError: false);
            if (tLevelLoader != null) {
                yield return GetMethod(tLevelLoader, "LoadLevelCoroutine");
            }
        }
        static void Prefix(Package.Asset asset, string playerScene, string uiScene, SimulationMetaData ngs, bool forceEnvironmentReload) {
            if (ngs == null)
                return;
            Log.Info("LoadLevelCoroutine called with arguments: " +
                $"asset={asset.name} playerScene={playerScene} uiScene={uiScene} forceEnvironmentReload={forceEnvironmentReload}\n" +
                $"ngs=[ " +
                $"map:{ngs.m_CityName} " +
                $"theme:{(ngs.m_MapThemeMetaData?.name).ToSTR()} " +
                $"environment:{ngs.m_environment.ToSTR()} " +
                $"LHT:{ngs.m_invertTraffic} " +
                $"disableAchievements:{ngs.m_disableAchievements} " +
                $"updateMode={ngs.m_updateMode}\n" +
                $"filePath:{asset.package?.packagePath}) " +
                "]\n" + Environment.StackTrace);
        }
    }
}
