using System;

namespace CO
{
	public class SavedFloat : SavedValue
	{
		private float m_Value;

		public SavedFloat(string name, string fileName) : base(name, fileName, false)
		{
			this.m_Value = 0f;
		}

		public SavedFloat(string name, string fileName, float def) : base(name, fileName, false)
		{
			this.m_Value = def;
		}

		public SavedFloat(string name, string fileName, float def, bool autoUpdate) : base(name, fileName, autoUpdate)
		{
			this.m_Value = def;
		}

		public float value
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

		public static implicit operator float(SavedFloat s)
		{
			return s.value;
		}

		public override string ToString()
		{
			return this.value.ToString();
		}
	}
}
