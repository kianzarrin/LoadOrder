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
        public static Assembly LoadAssemblyModified(string dllPath, Assembly pdb2mdb) {
            try {
                string mdbPath = dllPath + ".mdb";
                string pdbPath = dllPath.Substring(0, dllPath.Length - 3) + "pdb";

                if (File.Exists(pdbPath)) {
                    Log.Debug($"trying to convert {pdbPath} to mdb ...");
                    try {
                        Type tConvert = pdb2mdb.GetExportedTypes().Single(
                            _t => _t.FullName == "Pdb2Mdb.Converter");
                        MethodInfo mConvert = tConvert.GetMethod("Convert");

                        mConvert.Invoke(null, new object[] { dllPath });

                        Log.Info($"Created {mdbPath} from {pdbPath}");
                    } catch (Exception ex) {
                        Log.Exception(ex, $"failed to convert {pdbPath} to mdb.", showInPanel: false);
                    }
                }

                Assembly assembly;
                if (File.Exists(mdbPath)) {
                    Log.Info("\nLoading " + dllPath + "\nSymbols " + mdbPath,true);
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
