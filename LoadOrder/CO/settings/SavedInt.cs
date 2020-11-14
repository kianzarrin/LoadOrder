using System;

namespace COSettings
{
	public class SavedInt : SavedValue
	{
		private int m_Value;

		public SavedInt(string name, string fileName) : base(name, fileName, false)
		{
			this.m_Value = 0;
		}

		public SavedInt(string name, string fileName, int def) : base(name, fileName, false)
		{
			this.m_Value = def;
		}

		public SavedInt(string name, string fileName, int def, bool autoUpdate) : base(name, fileName, autoUpdate)
		{
			this.m_Value = def;
		}

		public int value
		{
			get
			{
				if (this.m_AutoUpdate || !this.m_Synced)
				{
					base.Sync();
				}
				return this.m_Value;
			}
			set
			{
				this.m_Value = value;
				if (base.settingsFile != null)
				{
					base.settingsFile.SetValue(this.m_Name, this.m_Value);
				}
			}
		}

		protected override void SyncImpl()
		{
			if (base.settingsFile != null)
			{
				this.m_Exists = base.settingsFile.GetValue(this.m_Name, ref this.m_Value);
			}
		}

		public static implicit operator int(SavedInt s)
		{
			return s.value;
		}

		public override string ToString()
		{
			return this.value.ToString();
		}
	}
}
