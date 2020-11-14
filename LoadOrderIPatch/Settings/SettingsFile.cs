using System;
using System.Collections.Generic;
using System.IO;
using COSettings.IO;

namespace COSettings
{
	public class SettingsFile
	{
		public static readonly string extension = ".cgs";

		private readonly char[] settingsIdentifier = new char[]
		{
			'C',
			'G',
			'S',
			'F'
		};

		private readonly ushort settingsVersion = 3;

		public ushort m_Version { get; private set; }

		private Dictionary<string, int> m_SettingsIntValues = new Dictionary<string, int>();

		private Dictionary<string, bool> m_SettingsBoolValues = new Dictionary<string, bool>();

		private Dictionary<string, float> m_SettingsFloatValues = new Dictionary<string, float>();

		private Dictionary<string, string> m_SettingsStringValues = new Dictionary<string, string>();


		private string m_PathName;

		public string fileName
		{
			get
			{
				return Path.GetFileNameWithoutExtension(this.m_PathName);
			}
			set
			{
				this.m_PathName = Path.Combine(DataLocation.localApplicationData, value + extension);
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
			return new FileStream(this.m_PathName, FileMode.Open, FileAccess.Read);
		}


		public bool IsValid() => !string.IsNullOrEmpty(m_PathName) && File.Exists(m_PathName);

		private bool ValidateID(char[] id)
		{
			for (int i = 0; i < 4; i++)
				if (id[i] != settingsIdentifier[i])
					return false;
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
						throw new Exception("Setting file '" + this.fileName + "' version is incompatible. The internal format of settings files has changed and your settings will be reset.");
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
					return;
				}
				throw new Exception("Setting file '" + this.fileName + "' header mismatch. The internal format of settings files has changed and your settings will be reset.");
			}
		}

		internal void Load()
		{
			if (this.IsValid())
			{
				//Log.Info("Loading " + this.m_PathName);
				using (Stream stream = this.CreateReadStream())
				{
					this.Deserialize(stream);
				}
			}
		}

		internal bool GetValue(string name, out object v)
		{
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

	}
}
