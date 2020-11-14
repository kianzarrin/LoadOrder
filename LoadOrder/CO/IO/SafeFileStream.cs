using LoadOrderTool;
using System;
using System.IO;
using System.Threading;

namespace COSettings.IO
{
	public class SafeFileStream : Stream
	{
		private static readonly string sTempExtension = ".tmp";

		private static readonly string sBackupExtension = ".bck";

		private string m_RealPath;

		private FileStream m_BaseStream;

		private bool m_UseTempFolder;

		private bool m_UseMoveBackup;

		public SafeFileStream(string path) : this(path, FileMode.Create, true, true)
		{
		}

		public SafeFileStream(string path, FileMode filemode) : this(path, filemode, true, true)
		{
		}

		public SafeFileStream(string path, FileMode filemode, bool useTempFolder, bool useMoveBackup)
		{
			this.m_UseTempFolder = useTempFolder;
			this.m_UseMoveBackup = useMoveBackup;
			this.m_RealPath = path;
			this.AttemptToCreateUnderlyingStream(filemode);
		}

		public void Cancel()
		{
			if (this.m_BaseStream == null)
			{
				return;
			}
			string name = this.m_BaseStream.Name;
			this.m_BaseStream.Dispose();
			this.m_BaseStream = null;
			File.Delete(name);
		}

		private void AttemptToCreateUnderlyingStream(FileMode filemode)
		{
			string path;
			if (this.m_UseTempFolder)
			{
				path = Path.Combine(DataLocation.tempFolder, Path.GetFileName(this.m_RealPath) + SafeFileStream.sTempExtension);
			}
			else
			{
				path = this.m_RealPath + SafeFileStream.sTempExtension;
			}
			try
			{
				this.m_BaseStream = new FileStream(path, FileMode.Create, FileAccess.Write);
			}
			catch (Exception ex)
			{
				Log.Exception(ex);
			}
			if (filemode == FileMode.Append)
			{
				byte[] array = File.ReadAllBytes(this.m_RealPath);
				this.m_BaseStream.Write(array, 0, array.Length);
			}
		}

		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		public override bool CanWrite
		{
			get
			{
				return this.m_BaseStream != null && this.m_BaseStream.CanWrite;
			}
		}

		public override bool CanSeek
		{
			get
			{
				return this.m_BaseStream != null && this.m_BaseStream.CanSeek;
			}
		}

		public override long Length
		{
			get
			{
				if (this.m_BaseStream == null)
				{
					throw new IOException("Base stream is not open");
				}
				return this.m_BaseStream.Length;
			}
		}

		public override long Position
		{
			get
			{
				if (this.m_BaseStream == null)
				{
					throw new IOException("Base stream is not open");
				}
				return this.m_BaseStream.Position;
			}
			set
			{
				if (this.m_BaseStream == null)
				{
					throw new IOException("Base stream is not open");
				}
				this.m_BaseStream.Position = value;
			}
		}

		public override void Flush()
		{
			if (this.m_BaseStream == null)
			{
				throw new IOException("Base stream is not open");
			}
			this.m_BaseStream.Flush();
		}

		public override void SetLength(long value)
		{
			if (this.m_BaseStream == null)
			{
				throw new IOException("Base stream is not open");
			}
			this.m_BaseStream.SetLength(value);
		}

		public override int Read(byte[] array, int offset, int count)
		{
			throw new NotSupportedException("Stream does not support reading");
		}

		public override void Write(byte[] array, int offset, int count)
		{
			if (this.m_BaseStream == null)
			{
				throw new IOException("Base stream is not open");
			}
			this.m_BaseStream.Write(array, offset, count);
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			if (this.m_BaseStream == null)
			{
				throw new IOException("Base stream is not open");
			}
			return this.m_BaseStream.Seek(offset, origin);
		}

		protected override void Dispose(bool disposing)
		{
			if (this.m_BaseStream == null)
			{
				return;
			}
			string name = this.m_BaseStream.Name;
			this.m_BaseStream.Dispose();
			string text = null;
			if (this.m_UseMoveBackup && File.Exists(this.m_RealPath))
			{
				text = this.m_RealPath + SafeFileStream.sBackupExtension;
				File.Delete(text);
				File.Move(this.m_RealPath, text);
			}
			try
			{
				File.Move(name, this.m_RealPath);
			}
			catch
			{
				Thread.Sleep(500);
				File.Move(name, this.m_RealPath);
			}
			if (this.m_UseMoveBackup && text != null)
			{
				File.Delete(text);
			}
			this.m_BaseStream = null;
		}
	}
}
