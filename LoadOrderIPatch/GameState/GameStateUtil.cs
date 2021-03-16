namespace GameState {
    using System.IO;
    public static class GameStateUtil {
        //TODO: remove
        internal static Patch.API.ILogger logger;

        public static bool? IsModEnabled(string workingPath) {
            string name = Path.GetFileNameWithoutExtension(workingPath);
            string key = name + workingPath.GetHashCode().ToString() + ".enabled";
            SavedBool save = new SavedBool(key, "userGameState");
            return save.m_Exists ? save.value : null;
        }
    }
}
