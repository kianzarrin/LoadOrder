namespace LoadOrderMod.Util {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using static ColossalFramework.Plugins.PluginManager;


    public static class LSMUtil {
        public const string LSM_TEST = "LoadingScreenModTest";
        public const string LSM = "LoadingScreenMod";
        internal static bool IsLSM(this PluginInfo p) =>
            p != null && p.name == "667342976" || p.name == "833779378" || p.name == LSM || p.name == LSM_TEST;

        internal static Assembly GetLSMAssembly() =>
            AppDomain.CurrentDomain.GetAssemblies()
            .FirstOrDefault(_asm => _asm.GetName().Name == LSM || _asm.GetName().Name == LSM_TEST);

        internal static IEnumerable<Assembly> GetBothLSMAssembly() =>
            AppDomain.CurrentDomain.GetAssemblies()
            .Where(_asm => _asm.GetName().Name == LSM || _asm.GetName().Name == LSM_TEST);


        /// <param name="type">full type name minus assembly name and root name space</param>
        /// <returns>corresponding types from LSM or LSMTest or both</returns>
        public static IEnumerable<Type> GetTypeFromBothLSMs(string type) {
            var type1 = Type.GetType($"{LSM}.{type}, {LSM}", false);
            var type2 = Type.GetType($"{LSM_TEST}.{type}, {LSM_TEST}", false);
            if(type1 != null) yield return type1;
            if(type2 != null) yield return type2;
        }
    }
}
