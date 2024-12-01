using stroibot.Core.Logging;
using stroibot.Core.States;
using stroibot.TapTapPop.Scopes;

namespace stroibot.Match3.States
{
	public class IdleGameState :
		GameState
	{
		public IdleGameState(
			ILogger logger,
			GameContext context,
			StateMachine<GameContext> stateMachine,
			GameStateFactory gameStateFactory,
			GameSettings gameSettings) :
			base(logger, context, stateMachine, gameStateFactory, gameSettings)
		{
		}

		public override void OnEnter()
		{
			Context.IsInputAllowed = true;
		}

		public override void OnExit()
		{
			Context.IsInputAllowed = false;
		}
	}
}
