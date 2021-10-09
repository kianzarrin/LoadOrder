using System;

namespace CO.PlatformServices
{
	public struct PublishedFileId : IEquatable<PublishedFileId>
	{
                public bool IsValid => this != invalid && AsUInt64 != 0;

                public static readonly PublishedFileId invalid = new PublishedFileId(ulong.MaxValue);

		private ulong m_Handle;

		public PublishedFileId(ulong value)
		{
			this.m_Handle = value;
		}

		public ulong AsUInt64
		{
			get
			{
				return this.m_Handle;
			}
		}

		public static bool operator ==(PublishedFileId x, PublishedFileId y)
		{
			return x.m_Handle == y.m_Handle;
		}

		public static bool operator !=(PublishedFileId x, PublishedFileId y)
		{
			return x.m_Handle != y.m_Handle;
		}

		public bool Equals(PublishedFileId other)
		{
			return this == other;
		}

		public override bool Equals(object obj)
		{
			return obj is PublishedFileId && this == (PublishedFileId)obj;
		}

		public override int GetHashCode()
		{
			return this.m_Handle.GetHashCode();
		}

		public override string ToString()
		{
			return this.m_Handle.ToString();
		}

		// Note: this type is marked as 'beforefieldinit'.
		static PublishedFileId()
		{
		}
	}
}
