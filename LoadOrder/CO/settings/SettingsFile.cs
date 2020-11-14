using System;
using System.Collections.Generic;
using System.IO;
using COSettings.IO;
using LoadOrder;

namespace COSettings
{
	public class SettingsFile
	{
		private readonly char[] settingsIdentifier = new char[]
		{
			'C',
			'G',
			'S',
			'F'
		};

		private readonly ushort settingsVersion = 3;

		private ushort m_Version;

		private Dictionary<string, int> m_SettingsIntValues = new Dictionary<string, int>();

		private Dictionary<string, bool> m_SettingsBoolValues = new Dictionary<string, bool>();

		private Dictionary<string, float> m_SettingsFloatValues = new Dictionary<string, float>();

		private Dictionary<string, string> m_SettingsStringValues = new Dictionary<string, string>();

		private Dictionary<string, InputKey> m_SettingsInputKeyValues = new Dictionary<string, InputKey>();

		private string m_PathName;

		//private bool m_UseCloud;

		private bool m_IsSystem;

		private bool m_DontSave;

		private bool m_IsDirty;

		private object m_Saving = new object();

		public ushort version
		{
			get
			{
				return this.m_Version;
			}
		}

		public bool isDirty
		{
			get
			{
				return this.m_IsDirty;
			}
		}

		public bool isSystem
		{
			get
			{
				return this.m_IsSystem;
			}
		}

		public bool dontSave
		{
			get
			{
				return this.m_DontSave;
			}
			set
			{
				this.m_DontSave = true;
			}
		}

		public string fileName
		{
			get
			{
				return Path.GetFileNameWithoutExtension(this.m_PathName);
			}
			set
			{
				this.m_PathName = Path.Combine(DataLocation.localApplicationData, PathUtils.AddExtension(value, GameSettings.extension));
			}
		}

		public string systemFileName
		{
			get
			{
				return Path.GetFileNameWithoutExtension(this.m_PathName);
			}
			set
			{
				this.m_PathName = Path.Combine(DataLocation.applicationBase, PathUtils.AddExtension(value, GameSettings.extension));
				this.m_IsSystem = true;
			}
		}

		public string pathName
		{
			get
			{
				return this.m_PathName;
			}
			set
			{
				this.m_PathName = PathUtils.AddExtension(value, GameSettings.extension);
			}
		}

		//public string cloudName
		//{
		//	get
		//	{
		//		return this.m_PathName;
		//	}
		//	set
		//	{
		//		this.m_PathName = PathUtils.AddExtension(value, GameSettings.extension);
		//		this.m_UseCloud = true;
		//	}
		//}

		internal Stream CreateReadStream()
		{
			//if (this.m_UseCloud)
			//{
			//	return new MemoryStream(PlatformService.cloud.ReadAllBytes(this.pathName));
			//}
			return new FileStream(this.pathName, FileMode.Open, FileAccess.Read);
		}

		internal Stream CreateWriteStream()
		{
			//if (this.m_UseCloud)
			//{
			//	return new CloudStream(this.pathName);
			//}
			return new SafeFileStream(this.pathName);
		}

		public void DeleteEntry(string key)
		{
			this.m_SettingsInputKeyValues.Remove(key);
			this.m_SettingsIntValues.Remove(key);
			this.m_SettingsBoolValues.Remove(key);
			this.m_SettingsFloatValues.Remove(key);
			this.m_SettingsStringValues.Remove(key);
			this.MarkDirty();
		}

		public string[] ListKeys()
		{
			Dictionary<string, InputKey>.KeyCollection keys = this.m_SettingsInputKeyValues.Keys;
			Dictionary<string, int>.KeyCollection keys2 = this.m_SettingsIntValues.Keys;
			Dictionary<string, bool>.KeyCollection keys3 = this.m_SettingsBoolValues.Keys;
			Dictionary<string, float>.KeyCollection keys4 = this.m_SettingsFloatValues.Keys;
			Dictionary<string, string>.KeyCollection keys5 = this.m_SettingsStringValues.Keys;
			string[] array = new string[keys.Count + keys2.Count + keys3.Count + keys4.Count + keys5.Count];
			int num = 0;
			keys.CopyTo(array, num);
			num += keys.Count;
			keys2.CopyTo(array, num);
			num += keys2.Count;
			keys3.CopyTo(array, num);
			num += keys3.Count;
			keys4.CopyTo(array, num);
			num += keys4.Count;
			keys5.CopyTo(array, num);
			num += keys5.Count;
			return array;
		}

		public bool IsValid()
		{
			if (string.IsNullOrEmpty(this.pathName))
			{
				return false;
			}
			//if (!this.m_UseCloud)
			{
				return File.Exists(this.pathName);
			}
			//return PlatformService.cloud.Exists(this.pathName);
		}

		public void Delete()
		{
			if (this.IsValid())
			{
				//if (this.m_UseCloud)
				//{
				//	PlatformService.cloud.Delete(this.pathName);
				//	return;
				//}
				File.Delete(this.pathName);
			}
		}

		private void Serialize(Stream stream)
		{
			Dictionary<string, int> dictionary;
			lock (this.m_SettingsIntValues)
			{
				dictionary = new Dictionary<string, int>(this.m_SettingsIntValues);
			}
			Dictionary<string, bool> dictionary2;
			lock (this.m_SettingsBoolValues)
			{
				dictionary2 = new Dictionary<string, bool>(this.m_SettingsBoolValues);
			}
			Dictionary<string, float> dictionary3;
			lock (this.m_SettingsFloatValues)
			{
				dictionary3 = new Dictionary<string, float>(this.m_SettingsFloatValues);
			}
			Dictionary<string, string> dictionary4;
			lock (this.m_SettingsStringValues)
			{
				dictionary4 = new Dictionary<string, string>(this.m_SettingsStringValues);
			}
			Dictionary<string, InputKey> dictionary5;
			lock (this.m_SettingsInputKeyValues)
			{
				dictionary5 = new Dictionary<string, InputKey>(this.m_SettingsInputKeyValues);
			}
			using (BinaryWriter binaryWriter = new BinaryWriter(stream))
			{
				binaryWriter.Write(this.settingsIdentifier);
				binaryWriter.Write(this.settingsVersion);
				binaryWriter.Write(dictionary.Count);
				foreach (KeyValuePair<string, int> keyValuePair in dictionary)
				{
					binaryWriter.Write(keyValuePair.Key);
					binaryWriter.Write(keyValuePair.Value);
				}
				binaryWriter.Write(dictionary2.Count);
				foreach (KeyValuePair<string, bool> keyValuePair2 in dictionary2)
				{
					binaryWriter.Write(keyValuePair2.Key);
					binaryWriter.Write(keyValuePair2.Value);
				}
				binaryWriter.Write(dictionary3.Count);
				foreach (KeyValuePair<string, float> keyValuePair3 in dictionary3)
				{
					binaryWriter.Write(keyValuePair3.Key);
					binaryWriter.Write(keyValuePair3.Value);
				}
				binaryWriter.Write(dictionary4.Count);
				foreach (KeyValuePair<string, string> keyValuePair4 in dictionary4)
				{
					binaryWriter.Write(keyValuePair4.Key);
					binaryWriter.Write(keyValuePair4.Value);
				}
				binaryWriter.Write(dictionary5.Count);
				foreach (KeyValuePair<string, InputKey> keyValuePair5 in dictionary5)
				{
					binaryWriter.Write(keyValuePair5.Key);
					binaryWriter.Write(keyValuePair5.Value);
				}
				binaryWriter.Flush();
			}
		}

		private bool ValidateID(char[] id)
		{
			for (int i = 0; i < 4; i++)
			{
				if (id[i] != this.settingsIdentifier[i])
				{
					return false;
				}
			}
			return true;
		}

		private void Deserialize(Stream stream)
		{
			using (BinaryReader binaryReader = new BinaryReader(stream))
			{
				if (this.ValidateID(binaryReader.ReadChars(4)))
				{
					this.m_Version = binaryReader.ReadUInt16();
					if (this.m_Version < 2)
					{
						throw new GameHandledException("Setting file '" + this.fileName + "' version is incompatible. The internal format of settings files has changed and your settings will be reset.");
					}
					lock (this.m_SettingsIntValues)
					{
						this.m_SettingsIntValues.Clear();
						int num = binaryReader.ReadInt32();
						for (int i = 0; i < num; i++)
						{
							string key = binaryReader.ReadString();
							int value = binaryReader.ReadInt32();
							this.m_SettingsIntValues.Add(key, value);
						}
					}
					lock (this.m_SettingsBoolValues)
					{
						this.m_SettingsBoolValues.Clear();
						int num2 = binaryReader.ReadInt32();
						for (int j = 0; j < num2; j++)
						{
							string key2 = binaryReader.ReadString();
							bool value2 = binaryReader.ReadBoolean();
							this.m_SettingsBoolValues.Add(key2, value2);
						}
					}
					lock (this.m_SettingsFloatValues)
					{
						this.m_SettingsFloatValues.Clear();
						int num3 = binaryReader.ReadInt32();
						for (int k = 0; k < num3; k++)
						{
							string key3 = binaryReader.ReadString();
							float value3 = binaryReader.ReadSingle();
							this.m_SettingsFloatValues.Add(key3, value3);
						}
					}
					lock (this.m_SettingsStringValues)
					{
						this.m_SettingsStringValues.Clear();
						int num4 = binaryReader.ReadInt32();
						for (int l = 0; l < num4; l++)
						{
							string key4 = binaryReader.ReadString();
							string value4 = binaryReader.ReadString();
							this.m_SettingsStringValues.Add(key4, value4);
						}
					}
					lock (this.m_SettingsInputKeyValues)
					{
						this.m_SettingsInputKeyValues.Clear();
						int num5 = binaryReader.ReadInt32();
						for (int m = 0; m < num5; m++)
						{
							string key5 = binaryReader.ReadString();
							InputKey value5 = binaryReader.ReadInt32();
							this.m_SettingsInputKeyValues.Add(key5, value5);
						}
					}
					return;
				}
				throw new GameHandledException("Setting file '" + this.fileName + "' header mismatch. The internal format of settings files has changed and your settings will be reset.");
			}
		}

		internal void Load()
		{
			if (this.IsValid())
			{
				Log.Info("Loading " + this.m_PathName);
				using (Stream stream = this.CreateReadStream())
				{
					this.Deserialize(stream);
				}
			}
		}

		internal void Save()
		{
			if (!this.dontSave && !string.IsNullOrEmpty(this.pathName))
			{
				lock (this.m_Saving)
				{
					using (Stream stream = this.CreateWriteStream())
					{
						this.Serialize(stream);
					}
				}
				this.m_IsDirty = false;
				Log.Info("Saving " + this.m_PathName);
			}
		}

		public void MarkDirty()
		{
			this.m_IsDirty = true;
		}

		internal bool GetValue(string name, out object v)
		{
			InputKey inputKey;
			if (this.m_SettingsInputKeyValues.TryGetValue(name, out inputKey))
			{
				v = inputKey;
				return true;
			}
			int num;
			if (this.m_SettingsIntValues.TryGetValue(name, out num))
			{
				v = num;
				return true;
			}
			bool flag;
			if (this.m_SettingsBoolValues.TryGetValue(name, out flag))
			{
				v = flag;
				return true;
			}
			string text;
			if (this.m_SettingsStringValues.TryGetValue(name, out text))
			{
				v = text;
				return true;
			}
			float num2;
			if (this.m_SettingsFloatValues.TryGetValue(name, out num2))
			{
				v = num2;
				return true;
			}
			v = null;
			return false;
		}

		internal bool GetValue(string name, ref string val)
		{
			string text;
			if (this.m_SettingsStringValues.TryGetValue(name, out text))
			{
				val = text;
				return true;
			}
			return false;
		}

		internal void SetValue(string name, string val)
		{
			string a;
			if (!this.m_SettingsStringValues.TryGetValue(name, out a) || a != val)
			{
				Log.Debug("Setting " + name + " updated to " + val);
				this.m_SettingsStringValues[name] = val;
				this.MarkDirty();
			}
		}

		internal bool GetValue(string name, ref bool val)
		{
			bool flag;
			if (this.m_SettingsBoolValues.TryGetValue(name, out flag))
			{
				val = flag;
				return true;
			}
			return false;
		}

		internal void SetValue(string name, bool val)
		{
			bool flag;
			if (!this.m_SettingsBoolValues.TryGetValue(name, out flag) || flag != val)
			{
				Log.Debug(string.Concat(new object[]
				{
					"Setting ",
					name,
					" updated to ",
					val
				}));
				this.m_SettingsBoolValues[name] = val;
				this.MarkDirty();
			}
		}

		internal bool GetValue(string name, ref int val)
		{
			int num;
			if (this.m_SettingsIntValues.TryGetValue(name, out num))
			{
				val = num;
				return true;
			}
			return false;
		}

		internal void SetValue(string name, int val)
		{
			int num;
			if (!this.m_SettingsIntValues.TryGetValue(name, out num) || num != val)
			{
				Log.Debug(string.Concat(new object[]
				{
					"Setting ",
					name,
					" updated to ",
					val
				}));
				this.m_SettingsIntValues[name] = val;
				this.MarkDirty();
			}
		}

		internal bool GetValue(string name, ref InputKey val)
		{
			InputKey inputKey;
			if (this.m_SettingsInputKeyValues.TryGetValue(name, out inputKey))
			{
				val = inputKey;
				return true;
			}
			return false;
		}

		internal void SetValue(string name, InputKey val)
		{
			InputKey value;
			if (!this.m_SettingsInputKeyValues.TryGetValue(name, out value) || value != val)
			{
				Log.Debug(string.Concat(new object[]
				{
					"Setting ",
					name,
					" updated to ",
					val
				}));
				this.m_SettingsInputKeyValues[name] = val;
				this.MarkDirty();
			}
		}

		internal bool GetValue(string name, ref float val)
		{
			float num;
			if (this.m_SettingsFloatValues.TryGetValue(name, out num))
			{
				val = num;
				return true;
			}
			return false;
		}

		internal void SetValue(string name, float val)
		{
			if (!this.m_SettingsFloatValues.TryGetValue(name, out _) ||
				Math.Abs(m_SettingsFloatValues[name] - val) > float.Epsilon)
			{
				Log.Debug(string.Concat(new object[]
				{
					"Setting ",
					name,
					" updated to ",
					val
				}));
				this.m_SettingsFloatValues[name] = val;
				this.MarkDirty();
			}
		}
	}
}
