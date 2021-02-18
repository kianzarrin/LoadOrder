namespace LoadOrderInjections.Injections {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using JetBrains.Annotations;
    using UnityEngine;
    using KianCommons;

    public static class Utils {
        public static Assembly LoadAssemblyModified(string dllPath) {
            try {
                string mdbPath = dllPath + ".mdb";
                string pdbPath = dllPath.Substring(0, dllPath.Length - 3) + "pdb";

                if (File.Exists(pdbPath)) {
                    try {
                        Pdb2Mdb.Converter.Convert(dllPath);
                        Log.Info($"Created {mdbPath} from {pdbPath} to ...");
                    } catch (Exception ex) {
                        Log.Exception(ex, $"failed to convert {pdbPath} to mdb.", showInPanel: false);
                    }
                }

                Assembly assembly;
                if (File.Exists(mdbPath)) {
                    Log.Info("\nLoading " + dllPath + "\nSymbols " + mdbPath);
                    assembly = Assembly.Load(File.ReadAllBytes(dllPath), File.ReadAllBytes(mdbPath));
                } else {
                    Log.Info("Loading " + dllPath);
                    assembly = Assembly.Load(File.ReadAllBytes(dllPath));
                }
                
                if(assembly is null)
                    Log.Error("Assembly at " + dllPath + " failed to load.");

                return assembly;
            } catch (Exception ex) {
                Log.Exception(ex, "Assembly at " + dllPath + " failed to load.", showInPanel: false);
                return null;
            }
            

        }
    }
}
