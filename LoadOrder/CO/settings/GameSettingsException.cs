using System;

namespace CO
{
	public class GameSettingsException : Exception
	{
		public GameSettingsException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}
