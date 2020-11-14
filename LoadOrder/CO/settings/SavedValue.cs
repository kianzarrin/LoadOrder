using System;

namespace COSettings
{
	public abstract class SavedValue
	{
		protected string m_Name;

		protected string m_FileName;

		protected bool m_Synced;

		protected bool m_Exists;

		protected bool m_AutoUpdate;

		private SettingsFile m_SettingsFile;

		public bool exists
		{
			get
			{
				return this.m_Exists;
			}
		}

		public string name
		{
			get
			{
				return this.m_Name;
			}
		}

		public ushort version
		{
			get
			{
				return this.settingsFile.version;
			}
		}

		protected SettingsFile settingsFile
		{
			get
			{
				if (this.m_SettingsFile == null && !this.m_Synced)
				{
					this.m_SettingsFile = GameSettings.FindSettingsFileByName(this.m_FileName);
				}
				return this.m_SettingsFile;
			}
		}

		protected void Sync()
		{
			this.SyncImpl();
			this.m_Synced = true;
		}

		public SavedValue(string name, string fileName, bool autoUpdate)
		{
			this.m_Name = name;
			this.m_FileName = fileName;
			this.m_AutoUpdate = autoUpdate;
		}

		public void Delete()
		{
			this.settingsFile.DeleteEntry(this.m_Name);
			this.m_Synced = false;
			this.m_Exists = false;
		}

		public static bool operator ==(SavedValue x, SavedValue y)
		{
			if (object.ReferenceEquals(x, null))
			{
				return object.ReferenceEquals(y, null);
			}
			return x.Equals(y);
		}

		public static bool operator !=(SavedValue x, SavedValue y)
		{
			return !(x == y);
		}

		public override bool Equals(object obj)
		{
			return obj != null && ((SavedValue)obj).m_Name == this.m_Name && ((SavedValue)obj).m_FileName == this.m_FileName;
		}

		public bool Equals(SavedValue obj)
		{
			return !(obj == null) && obj.m_Name == this.m_Name && obj.m_FileName == this.m_FileName;
		}

		public override int GetHashCode()
		{
			return this.m_Name.GetHashCode() ^ this.m_FileName.GetHashCode();
		}

		protected abstract void SyncImpl();
	}
}
