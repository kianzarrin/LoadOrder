namespace GameState {
    using System.IO;
    using System;
    using LoadOrderIPatch.Patches;
    public static class GameStateUtil {
        internal static Patch.API.ILogger logger => Entry.Logger;
        internal static string localAppDataPath => Entry.GamePaths.AppDataPath;

        public static bool? IsModEnabled(string workingPath) {
            string name = Path.GetFileNameWithoutExtension(workingPath);
            string key = name + workingPath.GetHashCode().ToString() + ".enabled";
            SavedBool save = new SavedBool(key, "userGameState");
            return save.m_Exists ? save.value : null;
        }
    }
}
