using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using ColossalFramework.IO;
using UnityEngine;

namespace ColossalFramework.Plugins
{
	internal class ColossalCSharpCodeCompiler : ICodeCompiler
	{
		private static readonly string kMcsPath;

		private static readonly string kMonoPath;

		private static readonly string kEnvMonoPath;

		private Mutex mcsOutMutex;

		private StringCollection mcsOutput;

		static ColossalCSharpCodeCompiler()
		{
			RuntimePlatform platform = Application.platform;
			if (platform == RuntimePlatform.WindowsPlayer || platform == RuntimePlatform.WindowsEditor)
			{
				string path = Path.Combine(DataLocation.applicationBase, "Mono");
				ColossalCSharpCodeCompiler.kMonoPath = Path.Combine(Path.Combine(path, "bin"), "mono.exe");
				ColossalCSharpCodeCompiler.kMcsPath = Path.Combine(Path.Combine(Path.Combine(Path.Combine(path, "lib"), "mono"), "2.0"), "gmcs.exe");
				ColossalCSharpCodeCompiler.kEnvMonoPath = Path.Combine(Path.Combine(Path.Combine(path, "lib"), "mono"), "2.0");
				return;
			}
			if (platform == RuntimePlatform.LinuxPlayer)
			{
				string path2 = Path.Combine(DataLocation.applicationBase, "Mono");
				ColossalCSharpCodeCompiler.kMonoPath = Path.Combine(Path.Combine(path2, "bin-linux32"), "mono");
				ColossalCSharpCodeCompiler.kMcsPath = Path.Combine(Path.Combine(Path.Combine(Path.Combine(path2, "lib"), "mono"), "2.0"), "gmcs.exe");
				ColossalCSharpCodeCompiler.kEnvMonoPath = Path.Combine(Path.Combine(Path.Combine(path2, "lib"), "mono"), "2.0");
				return;
			}
			if (platform == RuntimePlatform.OSXPlayer || platform == RuntimePlatform.OSXEditor)
			{
				string path3 = Path.Combine(Path.Combine(DataLocation.applicationBase, "Frameworks"), "Mono");
				ColossalCSharpCodeCompiler.kMonoPath = Path.Combine(Path.Combine(path3, "bin"), "mono");
				ColossalCSharpCodeCompiler.kMcsPath = Path.Combine(Path.Combine(Path.Combine(Path.Combine(path3, "lib"), "mono"), "2.0"), "gmcs.exe");
				ColossalCSharpCodeCompiler.kEnvMonoPath = Path.Combine(Path.Combine(Path.Combine(path3, "lib"), "mono"), "2.0");
			}
		}

		public CompilerResults CompileAssemblyFromDom(CompilerParameters options, CodeCompileUnit compilationUnit)
		{
			throw new NotImplementedException();
		}

		public CompilerResults CompileAssemblyFromDomBatch(CompilerParameters options, CodeCompileUnit[] batch)
		{
			throw new NotImplementedException();
		}

		public CompilerResults CompileAssemblyFromSource(CompilerParameters options, string source)
		{
			throw new NotImplementedException();
		}

		public CompilerResults CompileAssemblyFromFile(CompilerParameters options, string fileName)
		{
			throw new NotImplementedException();
		}

		public CompilerResults CompileAssemblyFromFileBatch(CompilerParameters options, string[] fileNames)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			CompilerResults result;
			try
			{
				result = this.CompileFromFileBatch(options, fileNames);
			}
			finally
			{
				options.TempFiles.Delete();
			}
			return result;
		}

		public CompilerResults CompileAssemblyFromSourceBatch(CompilerParameters options, string[] fileNames)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			CompilerResults result;
			try
			{
				result = this.CompileFromSourceBatch(options, fileNames);
			}
			finally
			{
				options.TempFiles.Delete();
			}
			return result;
		}

		private static string GetTempFileNameWithExtension(TempFileCollection temp_files, string extension)
		{
			return temp_files.AddExtension(extension);
		}

		private static string GetTempFileNameWithExtension(TempFileCollection temp_files, string extension, bool keepFile)
		{
			return temp_files.AddExtension(extension, keepFile);
		}

		private CompilerResults CompileFromSourceBatch(CompilerParameters options, string[] sources)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			if (sources == null)
			{
				throw new ArgumentNullException("sources");
			}
			string[] array = new string[sources.Length];
			for (int i = 0; i < sources.Length; i++)
			{
				array[i] = ColossalCSharpCodeCompiler.GetTempFileNameWithExtension(options.TempFiles, i.ToString() + ".cs");
				FileStream fileStream = new FileStream(array[i], FileMode.OpenOrCreate);
				using (StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
				{
					streamWriter.Write(sources[i]);
					streamWriter.Close();
				}
				fileStream.Close();
			}
			return this.CompileFromFileBatch(options, array);
		}

		private CompilerResults CompileFromFileBatch(CompilerParameters options, string[] fileNames)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			if (fileNames == null)
			{
				throw new ArgumentNullException("fileNames");
			}
			ColossalCompilerResults colossalCompilerResults = new ColossalCompilerResults(options.TempFiles);
			Process process = new Process();
			process.StartInfo.FileName = ColossalCSharpCodeCompiler.kMonoPath;
			process.StartInfo.Arguments = "\"" + ColossalCSharpCodeCompiler.kMcsPath + "\" " + ColossalCSharpCodeCompiler.BuildArgs(options, fileNames);
			FileUtils.SetExecutablePermissions(ColossalCSharpCodeCompiler.kMonoPath);
			this.mcsOutput = new StringCollection();
			this.mcsOutMutex = new Mutex();
			string environmentVariable = Environment.GetEnvironmentVariable("MONO_PATH");
			string text = string.IsNullOrEmpty(environmentVariable) ? ColossalCSharpCodeCompiler.kEnvMonoPath : environmentVariable;
			string privateBinPath = AppDomain.CurrentDomain.SetupInformation.PrivateBinPath;
			if (!string.IsNullOrEmpty(privateBinPath))
			{
				text = string.Format("{0}:{1}", privateBinPath, text);
			}
			if (text.Length > 0)
			{
				StringDictionary environmentVariables = process.StartInfo.EnvironmentVariables;
				if (environmentVariables.ContainsKey("MONO_PATH"))
				{ 
					environmentVariables["MONO_PATH"] = text;
				}
				else
				{
					environmentVariables.Add("MONO_PATH", text);
				}
			}
			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.RedirectStandardError = true;
			process.ErrorDataReceived += this.McsStderrDataReceived;
			try
			{
				process.Start();
			}
			catch (Exception ex)
			{
				Win32Exception ex2 = ex as Win32Exception;
				MethodInfo method = typeof(Win32Exception).GetMethod("W32ErrorMessage", BindingFlags.Static | BindingFlags.NonPublic);
				if (ex2 != null)
				{
					throw new SystemException(string.Format("Error running {0}: {1} ({2})", process.StartInfo.FileName, ex2.Message, (method != null) ? method.Invoke(null, new object[]
					{
						ex2.NativeErrorCode
					}) : ex2.NativeErrorCode.ToString()));
				}
				throw;
			}
			try
			{
				process.BeginOutputReadLine();
				process.BeginErrorReadLine();
				process.WaitForExit();
				colossalCompilerResults.NativeCompilerReturnValue = process.ExitCode;
			}
			finally
			{
				process.CancelErrorRead();
				process.CancelOutputRead();
				process.Close();
			}
			StringCollection stringCollection = this.mcsOutput;
			bool flag = true;
			foreach (string error_string in this.mcsOutput)
			{
				CompilerError compilerError = ColossalCSharpCodeCompiler.CreateErrorFromString(error_string);
				if (compilerError != null)
				{
					colossalCompilerResults.Errors.Add(compilerError);
					if (!compilerError.IsWarning)
					{
						flag = false;
					}
				}
			}
			if (stringCollection.Count > 0)
			{
				stringCollection.Insert(0, process.StartInfo.FileName + " " + process.StartInfo.Arguments + Environment.NewLine);
				colossalCompilerResults.Output = stringCollection;
			}
			if (flag)
			{
				if (!File.Exists(options.OutputAssembly))
				{
					StringBuilder stringBuilder = new StringBuilder();
					foreach (string str in stringCollection)
					{
						stringBuilder.Append(str + Environment.NewLine);
					}
					throw new Exception("Compiler failed to produce the assembly. Output: '" + stringBuilder.ToString() + "'");
				}
				if (options.GenerateInMemory)
				{
					using (FileStream fileStream = File.OpenRead(options.OutputAssembly))
					{
						byte[] array = new byte[fileStream.Length];
						fileStream.Read(array, 0, array.Length);
						colossalCompilerResults.CompiledAssembly = Assembly.Load(array, null, options.Evidence);
						fileStream.Close();
						return colossalCompilerResults;
					}
				}
				colossalCompilerResults.PathToAssembly = options.OutputAssembly;
			}
			else
			{
				colossalCompilerResults.CompiledAssembly = null;
			}
			return colossalCompilerResults;
		}

		private static string BuildArgs(CompilerParameters options, string[] fileNames)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (options.GenerateExecutable)
			{
				throw new NotSupportedException();
			}
			stringBuilder.Append("/target:library ");
			string privateBinPath = AppDomain.CurrentDomain.SetupInformation.PrivateBinPath;
			if (!string.IsNullOrEmpty(privateBinPath))
			{
				stringBuilder.AppendFormat("/lib:\"{0}\" ", privateBinPath);
			}
			if (options.IncludeDebugInformation)
			{
				stringBuilder.Append("/debug+ /optimize- ");
			}
			else
			{
				stringBuilder.Append("/debug- /optimize+ ");
			}
			if (options.TreatWarningsAsErrors)
			{
				stringBuilder.Append("/warnaserror ");
			}
			if (options.WarningLevel >= 0)
			{
				stringBuilder.AppendFormat("/warn:{0} ", options.WarningLevel);
			}
			if (string.IsNullOrEmpty(options.OutputAssembly))
			{
				options.OutputAssembly = ColossalCSharpCodeCompiler.GetTempFileNameWithExtension(options.TempFiles, "dll", !options.GenerateInMemory);
			}
			stringBuilder.AppendFormat("/out:\"{0}\" ", options.OutputAssembly);
			foreach (string text in options.ReferencedAssemblies)
			{
				if (text != null && text.Length != 0)
				{
					stringBuilder.AppendFormat("/r:\"{0}\" ", text);
				}
			}
			if (options.CompilerOptions != null)
			{
				stringBuilder.Append(options.CompilerOptions);
				stringBuilder.Append(" ");
			}
			foreach (string arg in options.EmbeddedResources)
			{
				stringBuilder.AppendFormat("/resource:\"{0}\" ", arg);
			}
			foreach (string arg2 in options.LinkedResources)
			{
				stringBuilder.AppendFormat("/linkresource:\"{0}\" ", arg2);
			}
			stringBuilder.Append(" -- ");
			foreach (string arg3 in fileNames)
			{
				stringBuilder.AppendFormat("\"{0}\" ", arg3);
			}
			return stringBuilder.ToString();
		}

		private void McsStderrDataReceived(object sender, DataReceivedEventArgs args)
		{
			if (args.Data == null)
			{
				return;
			}
			this.mcsOutMutex.WaitOne();
			this.mcsOutput.Add(args.Data);
			this.mcsOutMutex.ReleaseMutex();
		}

		private static CompilerError CreateErrorFromString(string error_string)
		{
			if (error_string.StartsWith("BETA"))
			{
				return null;
			}
			if (error_string == null || error_string == string.Empty)
			{
				return null;
			}
			CompilerError compilerError = new CompilerError();
			Match match = new Regex("^(\\s*(?<file>.*)\\((?<line>\\d*)(,(?<column>\\d*))?\\)(:)?\\s+)*(?<level>\\w+)\\s*(?<number>.*):\\s(?<message>.*)", RegexOptions.ExplicitCapture | RegexOptions.Compiled).Match(error_string);
			if (!match.Success)
			{
				compilerError.ErrorText = error_string;
				compilerError.IsWarning = false;
				compilerError.ErrorNumber = string.Empty;
				return compilerError;
			}
			if (string.Empty != match.Result("${file}"))
			{
				compilerError.FileName = match.Result("${file}");
			}
			if (string.Empty != match.Result("${line}"))
			{
				compilerError.Line = int.Parse(match.Result("${line}"));
			}
			if (string.Empty != match.Result("${column}"))
			{
				compilerError.Column = int.Parse(match.Result("${column}"));
			}
			string a = match.Result("${level}");
			if (a == "warning")
			{
				compilerError.IsWarning = true;
			}
			else if (a != "error")
			{
				return null;
			}
			compilerError.ErrorNumber = match.Result("${number}");
			compilerError.ErrorText = match.Result("${message}");
			return compilerError;
		}
	}
}
