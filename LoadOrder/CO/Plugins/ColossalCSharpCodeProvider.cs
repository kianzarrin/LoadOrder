using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using Microsoft.CSharp;

namespace ColossalFramework.Plugins
{
	[DesignerCategory("")]
	internal class ColossalCSharpCodeProvider : CSharpCodeProvider
	{
		[Obsolete("Use CodeDomProvider class")]
		public override ICodeCompiler CreateCompiler()
		{
			return new ColossalCSharpCodeCompiler();
		}
	}
}
