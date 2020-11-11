using LoadOrder;
using System;
using System.Collections.Generic;
using System.Threading;
//using ColossalFramework.UI;
//using UnityEngine;

namespace ColossalFramework
{
	public class GameSettings : SingletonLite<GameSettings>
	{
		public static readonly string extension = ".cgs";

		private Dictionary<string, SettingsFile> m_SettingsFiles = new Dictionary<string, SettingsFile>();

		private static bool m_Run;

		private static object m_LockObject = new object();

		public static void AddSettingsFile(params SettingsFile[] settingsFiles)
		{
			instance.InternalAddSettingsFile(settingsFiles);
		}

		private void InternalAddSettingsFile(params SettingsFile[] settingsFiles)
		{
			lock (this.m_SettingsFiles)
			{
				for (int i = 0; i < settingsFiles.Length; i++)
				{
					try
					{
						settingsFiles[i].Load();
						this.m_SettingsFiles.Add(settingsFiles[i].fileName, settingsFiles[i]);
					}
					catch (GameHandledException ex)
					{
						Log.Exception(ex);
						settingsFiles[i].Delete();
						this.m_SettingsFiles.Add(settingsFiles[i].fileName, settingsFiles[i]);
					}
					catch (Exception ex2)
					{
						Log.Error(string.Concat(new object[]
						{
							"An exception occurred (",
							ex2.GetType(),
							": ",
							ex2.Message,
							") trying to load ",
							settingsFiles[i].fileName,
							". Deleting..."
						}));
						settingsFiles[i].Delete();
						throw new GameSettingsException(ex2.GetType() + " " + ex2.Message, ex2);
					}
				}
			}
		}

		public static SettingsFile FindSettingsFileByName(string name)
		{
			return SingletonLite<GameSettings>.instance.InternalFindSettingsFileByName(name);
		}

		internal SettingsFile InternalFindSettingsFileByName(string name)
		{
			SettingsFile result;
			if (this.m_SettingsFiles.TryGetValue(name, out result))
			{
				return result;
			}
			Log.Info($"WARNING: GameSettings: '{name}' is not found or cannot be loaded. Make sure to call GameSettings.AddSettingsFile() to register a settings file before using SavedValue().");
			return null;
		}

		public static void SaveAll()
		{
			instance.InternalSaveAll();
		}

		public void InternalSaveAll()
		{
			lock (this.m_SettingsFiles)
			{
				foreach (SettingsFile settingsFile in this.m_SettingsFiles.Values)
				{
					if (settingsFile.isDirty)
					{
						settingsFile.Save();
					}
				}
			}
		}

		public static void ClearAll()
		{
			ClearAll(false);
		}

		public static void ClearAll(bool systemToo)
		{
			instance.InternalClearAll(systemToo);
		}

		private void InternalClearAll(bool systemToo)
		{
			foreach (SettingsFile settingsFile in this.m_SettingsFiles.Values)
			{
				if (!settingsFile.isSystem || systemToo)
				{
					settingsFile.Delete();
				}
			}
		}

		private static void MonitorSave()
		{
			try
			{
				while (GameSettings.m_Run)
				{
					GameSettings.SaveAll();
					lock (GameSettings.m_LockObject)
					{
						Monitor.Wait(GameSettings.m_LockObject, 1000);
					}
				}
				GameSettings.SaveAll();
				Log.Info("GameSettings Monitor Exiting...");
			}
			catch (Exception ex)
			{
				Log.Exception(ex);
			}
		}

		private void Awake()
		{
			Log.Info("GameSettings Monitor Started...");
			GameSettings.m_Run = true;
		}

		private void OnDestroy()
		{
			GameSettings.m_Run = false;
			lock (GameSettings.m_LockObject)
			{
				Monitor.Pulse(GameSettings.m_LockObject);
			}
			Log.Info("GameSettings terminated");
		}
	}
}
