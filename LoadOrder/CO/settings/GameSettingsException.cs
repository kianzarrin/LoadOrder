using System;

namespace COSettings
{
	public class GameSettingsException : Exception
	{
		public GameSettingsException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}
