using stroibot.Core.Logging;
using System;

namespace stroibot.Core.States
{
	public class StateMachine<TStateContext>
		where TStateContext : class, new()
	{
		private readonly string LogTag = $"{nameof(StateMachine<TStateContext>)}";

		private readonly ILogger _logger;

		private IState _currentState;

		public StateMachine(
			ILogger logger)
		{
			_logger = logger;
		}

		public void SwitchTo(
			IState newState)
		{
			if (newState == null)
			{
				throw new ArgumentNullException(nameof(newState));
			}

			if (newState == _currentState)
			{
				_logger.Log(LogTag, $"Already in {newState}");
				return;
			}

			var currentState = _currentState;

			if (currentState != null)
			{
				_logger.Log(LogTag, $"Exiting {currentState}");
				currentState.OnExit();
			}

			_logger.Log(LogTag, $"Entering {newState}");
			_currentState = newState;
			newState.OnEnter();
		}
	}
}
