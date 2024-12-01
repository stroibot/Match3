using stroibot.Core.States;
using stroibot.TapTapPop.Scopes;
using ILogger = stroibot.Core.Logging.ILogger;

namespace stroibot.Match3.States
{
	public class CascadeGameState :
		GameState
	{
		public CascadeGameState(
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
			var gameBoardModel = Context.GameBoard;
			gameBoardModel.Cascade();

			var refillBoardGameState = GameStateFactory.Create<RefillBoardGameState>();
			StateMachine.SwitchTo(refillBoardGameState);
		}
	}
}
