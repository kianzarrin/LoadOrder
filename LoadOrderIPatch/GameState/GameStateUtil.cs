namespace GameState {
    using System.IO;
    using System;
    public static class GameStateUtil {
        //TODO: remove
        internal static Patch.API.ILogger logger;
        internal static string localAppDataPath = Path.Combine(Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Colossal Order"),
            "Cities_Skylines");

        public static bool? IsModEnabled(string workingPath) {
            string name = Path.GetFileNameWithoutExtension(workingPath);
            string key = name + workingPath.GetHashCode().ToString() + ".enabled";
            SavedBool save = new SavedBool(key, "userGameState");
            return save.m_Exists ? save.value : null;
        }
    }
}
