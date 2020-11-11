using System;

namespace ColossalFramework
{
	public class GameSettingsException : Exception
	{
		public GameSettingsException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}
