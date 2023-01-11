namespace LoadOrderTool {
    using CO.IO;
    using CO.Plugins;
    using LoadOrderTool.Util;
    using System;
    using System.IO;
    using System.Reflection;
    using System.Windows.Forms;
    using System.Threading;
    using System.Diagnostics;
    using System.Security.Principal;
    using System.Linq;
    using System.Runtime.InteropServices;
    using LoadOrderTool.CommandLine;
    using System.Collections.Generic;

    static class Program {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        public static bool IsMain { get; private set; }

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            try {

                Console.WriteLine("Hello!");
                IsMain = true;
                Application.SetHighDpiMode(HighDpiMode.SystemAware);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                AppDomain.CurrentDomain.TypeResolve += BuildConfig.CurrentDomain_AssemblyResolve;
                AppDomain.CurrentDomain.AssemblyResolve += BuildConfig.CurrentDomain_AssemblyResolve;
                AppDomain.CurrentDomain.AssemblyResolve += ResolveInterface;
                AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;
                Application.ThreadException += UnhandledThreadExceptionHandler;

                if (IsAdministrator) {
                    string m = "Running this application as administrator can cause problems. Please quite and run this normally";
                    Console.WriteLine(m);
                    MessageBox.Show(
                        text: m,
                        caption: "Warning: Admin mode",
                        buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Warning);
                }


                var handle = GetConsoleWindow();
                Console.WriteLine("command line args = " + Environment.GetCommandLineArgs());
                bool commandLine = Parse.CommandLine;
                if (commandLine) {
                    ShowWindow(handle, SW_SHOW);// Show
                    Console.WriteLine("showing terminal");
                } else {
                    ShowWindow(handle, SW_HIDE);// Hide
                    Console.WriteLine("hiding terminal");
                    //new UI.ProgressWindow().Show();
                }


                _ = DataLocation.GamePath; // run DataLocation static constructor
                _ = Log.LogFilePath; // run Log static constructor

                Log.Info("command line args = " + Environment.GetCommandLineArgs());

                CacheDLLs();

                if (commandLine) {
                    Entry.Start();
                } else {
                    Application.Run(new UI.LoadOrderWindow());
                }
            } catch (Exception ex) {
                Log.Exception(ex);
            }
        }

        static void CacheDLLs() {
            var timer = Stopwatch.StartNew();
            var res = Directory.GetFiles(DataLocation.WorkshopContentPath, "*.dll", searchOption: SearchOption.AllDirectories)
                .AsParallel()
                .Select(File.ReadAllBytes)
                .ToList();
            string m = $"caching {res.Count} dlls took {timer.ElapsedMilliseconds}ms";
            Log.Info(m, true);
            //MessageBox.Show(m);
            //Process.GetCurrentProcess().Kill();

        }

        public static bool IsAdministrator =>
           new WindowsPrincipal(WindowsIdentity.GetCurrent())
               .IsInRole(WindowsBuiltInRole.Administrator);

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

        private static void UnhandledThreadExceptionHandler(object sender, ThreadExceptionEventArgs args) {
            Exception ex = (Exception)args.Exception;
            Log.Exception(ex, "Unhanded Exception Occurred.");
        }

        private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args) {
            Exception ex = (Exception)args.ExceptionObject;
            Log.Exception(ex, "Unhanded Exception Occurred.");
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
