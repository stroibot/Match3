using UnityEngine;

namespace stroibot.Core.Logging
{
	public class UnityLogger :
		ILogger
	{
		private readonly Logger _logger = new(Debug.unityLogger.logHandler);

		public void Log(
			string tag,
			object message)
		{
			_logger.Log(tag, message);
		}

		public void LogWarning(
			string tag,
			object message)
		{
			_logger.LogWarning(tag, message);
		}

		public void LogError(
			string tag,
			object message)
		{
			_logger.LogError(tag, message);
		}
	}
}
