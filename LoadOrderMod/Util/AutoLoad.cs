namespace LoadOrderMod.Util {
    using ColossalFramework;
    using ColossalFramework.IO;
    using ColossalFramework.Packaging;
    using ColossalFramework.PlatformServices;
    using ColossalFramework.UI;
    using KianCommons;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using static KianCommons.ReflectionHelpers;
    using static SimulationMetaData;

    public class AutoLoad : MonoBehaviour {
        void Start() {
            LogCalled();
            Invoke("Do", 2.5f);
        }
        void Do() {
            LogCalled();
            bool ready = !SavePanel.isSaving && Singleton<LoadingManager>.exists && !Singleton<LoadingManager>.instance.m_currentlyLoading;
            if (!ready) {
                //Invoke("Do", 3f);
                return;
            }
            OptionSet optionSet = new OptionSet();
            string loadSave = null; // null=>nothing empty=>continueLastSave else=>Load input save
            string newGame = null;// null=>nothing empty=>Net Game From a Map else=>Load Game from input Map
            bool asset = false;

            optionSet.Add("game|newGame", "Load New Game", v => newGame = v ?? "");
            optionSet.Add("save|loadSave", "Load Save", v => loadSave = v ?? "");
            optionSet.Add("asset|loadAsset", "Load Asset", v => asset = true);
            try {
                optionSet.Parse(Environment.GetCommandLineArgs());
            } catch (Exception ex) {
                Log.Exception(ex);
            }
            bool lht = Environment.GetCommandLineArgs().Contains("-LHT");

            var mainMenu = GameObject.FindObjectOfType<MainMenu>();

            if (loadSave != null) {
                if (loadSave == "")
                    InvokeMethod<MainMenu>("AutoContinue");
                else
                    LoadSave(loadSave);
            } else if (newGame != null) {
                LoadMap(newGame);
            } else if (asset) {
                Asset(lht: lht);
            }
        }

        static bool IsCrp(string str) => str.ToLower().EndsWith(".crp");

        public void LoadSave(string fileName) {
            var savedGame = IsCrp(fileName)
                ? GetAssetByPath(fileName)
                : GetSaveByName(fileName);
            var metaData = savedGame?.Instantiate<SaveGameMetaData>();
            PrintModsInfo(metaData.mods);

            SimulationMetaData ngs = new SimulationMetaData {
                m_CityName = metaData.cityName,
                m_updateMode = SimulationManager.UpdateMode.LoadGame,
                m_disableAchievements = MetaBool.True,
            };

            UpdateTheme(metaData.mapThemeRef, ngs);
            LoadGame(savedGame, ngs);
        }

        public void LoadMap(string str, bool lht = false) {
            Package.Asset map;
            if (str == "")
                map = GetAMap();
            else if (IsCrp(str))
                map = GetAssetByPath(str);
            else
                map = GetMapByName(str);
            var metaData = map?.Instantiate<MapMetaData>();
            PrintModsInfo(metaData.mods);

            SimulationMetaData ngs = new SimulationMetaData {
                m_CityName = metaData.mapName,
                m_gameInstanceIdentifier = Guid.NewGuid().ToString(),
                m_invertTraffic = lht ? MetaBool.True : MetaBool.False,
                m_disableAchievements = MetaBool.True,
                m_startingDateTime = DateTime.Now,
                m_currentDateTime = DateTime.Now,
                m_newGameAppVersion = DataLocation.productVersion,
                m_updateMode = SimulationManager.UpdateMode.NewGameFromMap,
            };

            UpdateTheme(metaData.mapThemeRef, ngs);
            LoadGame(map, ngs);
        }

        public void Asset(bool load = true, bool lht = false) {
            var mode = load ?
                SimulationManager.UpdateMode.LoadAsset :
                SimulationManager.UpdateMode.NewAsset;
            SimulationMetaData ngs = new SimulationMetaData {
                m_gameInstanceIdentifier = Guid.NewGuid().ToString(),
                m_WorkshopPublishedFileId = PublishedFileId.invalid,
                m_updateMode = mode,
                m_MapThemeMetaData = GetATheme(),
                m_invertTraffic = lht ? MetaBool.True : MetaBool.False,
            };
            Package.Asset asset = PackageManager.FindAssetByName("System.BuiltinTerrainMap-" + ngs.m_MapThemeMetaData.environment);
            SystemMapMetaData systemMapMetaData = asset.Instantiate<SystemMapMetaData>();
            Singleton<LoadingManager>.instance.LoadLevel(systemMapMetaData.assetRef, "AssetEditor", "InAssetEditor", ngs, false);
        }


        static LoadingManager loadingMan_ => Singleton<LoadingManager>.instance;
        static void LoadGame(Package.Asset asset, SimulationMetaData ngs) =>
            loadingMan_.LoadLevel(asset, "Game", "InGame", ngs);


        static void UpdateTheme(string mapThemeRef, SimulationMetaData ngs) {
            if (mapThemeRef != null) {
                Package.Asset asset = PackageManager.FindAssetByName(mapThemeRef, UserAssetType.MapThemeMetaData);
                if (asset != null) {
                    ngs.m_MapThemeMetaData = asset.Instantiate<MapThemeMetaData>();
                    ngs.m_MapThemeMetaData.SetSelfRef(asset);
                }
            }
        }

        static Package.Asset GetAssetByPath(string path) =>
            new Package.Asset(string.Empty, path, Package.AssetType.Data, false);

        static Package.Asset GetSaveByName(string name) =>
            PackageManager.FindAssetByName(name, UserAssetType.SaveGameMetaData);

        static Package.Asset GetMapByName(string name) =>
            PackageManager.FindAssetByName(name, UserAssetType.MapMetaData);

        static Package.Asset GetAMap() => Maps.FirstOrDefault();
        static IEnumerable<Package.Asset> Maps => FilterAssets(UserAssetType.MapMetaData);

        static MapThemeMetaData GetATheme() =>
            FilterAssets(UserAssetType.MapThemeMetaData).FirstOrDefault()?.Instantiate<MapThemeMetaData>();

        static IEnumerable<Package.Asset> FilterAssets(Package.AssetType type) =>
            PackageManager.FilterAssets(new[] { type })
            .Where(asset => asset != null);


        static void PrintModsInfo(ModInfo[] mods) {
            if (mods == null)
                Log.Info("Asset version is too old and does not contain mods info", true);
            else if (mods.Length == 0)
                Log.Info("No mods were used when this asset was created", true);
            else {
                Log.Info(
                    "The following mods were used when this asset was created:\n" +
                    mods.Select(_m => "\t" + _m.modName).JoinLines()
                    , true);
            }

        }

    }

}
