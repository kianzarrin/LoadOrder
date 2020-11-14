using LoadOrder;
using System;

namespace COSettings
{
	public abstract class SingletonLite<T> where T : new()
	{
		private static T sInstance;

		public static T instance
		{
			get
			{
				if (SingletonLite<T>.sInstance == null)
				{
					SingletonLite<T>.sInstance = ((default(T) == null) ? Activator.CreateInstance<T>() : default(T));
					Log.Debug("Creating singleton of type " + typeof(T).Name);
				}
				return SingletonLite<T>.sInstance;
			}
		}

		public static bool exists
		{
			get
			{
				return SingletonLite<T>.sInstance != null;
			}
		}

		public static void Ensure()
		{
			T instance = SingletonLite<T>.instance;
		}
	}
}
