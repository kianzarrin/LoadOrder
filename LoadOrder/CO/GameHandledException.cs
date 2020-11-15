using System;

namespace CO
{
	public class GameHandledException : Exception
	{
		public GameHandledException()
		{
		}

		public GameHandledException(string message) : base(message)
		{
		}

		public GameHandledException(Exception inner) : base(null, inner)
		{
		}

		public GameHandledException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}
