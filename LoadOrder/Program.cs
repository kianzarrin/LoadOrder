namespace LoadOrderTool {
    using CO.IO;
    using CO.Plugins;
    using LoadOrderTool.Util;
    using System;
    using System.IO;
    using System.Reflection;
    using System.Windows.Forms;

    static class Program {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            try {
                Application.SetHighDpiMode(HighDpiMode.SystemAware);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Console.WriteLine("Hello!");
                //LoadAssemblies();

                PluginManager.instance.LoadPlugins();

                Application.Run(new LoadOrderWindow());
            } catch (Exception ex) {
                Log.Exception(ex);
            }
        }

        public static void LoadAssemblies() {
            Log.Info("LoadAssemblies() called ...");
            AppDomain.CurrentDomain.TypeResolve += BuildConfig.CurrentDomain_AssemblyResolve;
            AppDomain.CurrentDomain.AssemblyResolve += BuildConfig.CurrentDomain_AssemblyResolve;
            AppDomain.CurrentDomain.AssemblyResolve += ResolveInterface;
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;

            LoadManagedDLLs();

            //PluginManager.instance.LoadPlugins();

            Log.Info("LoadAssemblies() was successful");
        }

        static void LoadManagedDLLs() {
            var dlls = Directory.GetFiles(DataLocation.ManagedDLL, "*.dll");
            foreach (var dll in dlls) {
                try {
                    var asm = AssemblyUtil.LoadDLL(dll);
                    Log.Info($"Assembly loaded: {asm}");
                } catch (Exception ex) {
                    Log.Exception(new Exception($"the dll {dll} failed to load", ex));
                }
            }
        }

        private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args) {
            Exception ex = (Exception)args.ExceptionObject;
            Log.Exception(new Exception("Unhandled Exception Occured.", ex));
        }

        private static Assembly ResolveInterface(object sender, ResolveEventArgs args) {
            Log.Info("Resolving Assembly " + args.Name);
            string file = Path.Combine(
                DataLocation.DataPath,
                "Managed",
                new AssemblyName(args.Name).Name + ".dll"); // parse name
            if (!File.Exists(file))
                return null;
            return AssemblyUtil.LoadDLL(file);
        }
    }
}
