using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ColossalFramework.IO;
using System.IO;
using System.Reflection;
using ColossalFramework.Plugins;

namespace LoadOrder
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Console.WriteLine("Hello world ...");

            LoadAssemblies();
            //Application.Run(new LoadOrder());
        }


        public static void LoadAssemblies()
        {
            Console.WriteLine("LoadAssemblies() called ...");
            AppDomain.CurrentDomain.TypeResolve += BuildConfig.CurrentDomain_AssemblyResolve;
            AppDomain.CurrentDomain.AssemblyResolve += BuildConfig.CurrentDomain_AssemblyResolve;

            
            var dlls = Directory.GetFiles(DataLocation.ManagedDLL, "*.dll");
            foreach (var dll in dlls)
                Assembly.Load(dll);
            
            PluginManager.instance.LoadPlugins();
        }

    }



}
