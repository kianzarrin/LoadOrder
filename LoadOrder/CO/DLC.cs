namespace CO {
    using System;
    using LoadOrderTool.UI;
    using System.Reflection;

    public static class Extensions {
        public static DLCInfoAttribute GetDLCInfo(this DLC dlc) {
            var member = typeof(DLC).GetMember(dlc.ToString())[0];
            return member.GetCustomAttribute<DLCInfoAttribute>();
        }
    }

    [Flags]
    public enum DLCType {
        [Text("<DLC Type>")] None = 0,
        Main = 1,
        [Text("Content Creator")] ContentCreator = 2,
        Misc = 4,
    }

    public class DLCInfoAttribute : Attribute {
        public string Text;
        public DLCType Type;
        public DLCInfoAttribute(string text, DLCType type) {
            Text = text;
            Type = type;
        }
    }

    public enum DLC {
        None = 0,

        [DLCInfo("After Dark", DLCType.Main)]
        AfterDarkDLC = 369150,

        [DLCInfo("Snow Fall", DLCType.Main)]
        SnowFallDLC = 420610,

        [DLCInfo("Natural Disasters", DLCType.Main)]
        NaturalDisastersDLC = 515191,

        [DLCInfo("Mass Transit", DLCType.Main)]
        InMotionDLC = 547502,

        [DLCInfo("Green Cities", DLCType.Main)]
        GreenCitiesDLC = 614580,

        [DLCInfo("Parklife", DLCType.Main)]
        ParksDLC = 715191,

        [DLCInfo("Industries", DLCType.Main)]
        IndustryDLC = 715194,

        [DLCInfo("Campus", DLCType.Main)]
        CampusDLC = 944071,

        [DLCInfo("Sunset Harbor", DLCType.Main)]
        UrbanDLC = 1146930,

        [DLCInfo("Pearls From the East", DLCType.Misc)]
        OrientalBuildings = 563850,

        [DLCInfo("Match Day (Football)", DLCType.Misc)]
        Football = 456200,

        [DLCInfo("Concerts (music festivals)", DLCType.Misc)]
        MusicFestival = 614581,

        [DLCInfo("Carols, Candles and Candy (Christmas)", DLCType.Misc)]
        Christmas = 715192,

        [DLCInfo("Deluxe Pack", DLCType.Misc)]
        DeluxeDLC = 346791,

        [DLCInfo("Music DLCs", DLCType.Misc)]
        MusicDLCs = 547501,

        [DLCInfo("CCP: Art Deco", DLCType.ContentCreator)]
        ModderPack1 = 515190,

        [DLCInfo("CCP: High-Tech", DLCType.ContentCreator)]
        ModderPack2 = 547500,

        [DLCInfo("CCP: European Suburbia", DLCType.ContentCreator)]
        ModderPack3 = 715190,

        [DLCInfo("CCP: University City", DLCType.ContentCreator)]
        ModderPack4 = 1059820,

        [DLCInfo("CCP: Modern City Center", DLCType.ContentCreator)]
        ModderPack5 = 1148020,

        [DLCInfo("CCP: Modern Japan", DLCType.ContentCreator)]
        ModderPack6 = 1148022,

        [DLCInfo("CCP: Train Stations", DLCType.ContentCreator)]
        ModderPack7 = 1531470,

        [DLCInfo("CCP: Bridges & Piers", DLCType.ContentCreator)]
        ModderPack8,

    }
}
