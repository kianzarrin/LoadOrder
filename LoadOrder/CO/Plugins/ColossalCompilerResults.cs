using System;
using System.CodeDom.Compiler;
using System.Collections.Specialized;
using System.Reflection;

namespace ColossalFramework.Plugins
{
	[Serializable]
	internal class ColossalCompilerResults : CompilerResults
	{
		public ColossalCompilerResults(TempFileCollection tempFiles) : base(tempFiles)
		{
		}

		public new StringCollection Output
		{
			get
			{
				return base.Output;
			}
			set
			{
				FieldInfo field = typeof(CompilerResults).GetField("output", BindingFlags.Instance | BindingFlags.NonPublic);
				if (field == null)
				{
					throw new MissingFieldException("output");
				}
				field.SetValue(this, value);
			}
		}
	}
}
