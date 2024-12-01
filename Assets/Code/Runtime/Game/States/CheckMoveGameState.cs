using stroibot.Core.States;
using stroibot.TapTapPop.Scopes;
using ILogger = stroibot.Core.Logging.ILogger;

namespace stroibot.Match3.States
{
	public class CheckMoveGameState :
		GameState
	{
		private readonly string LogTag = $"{nameof(CheckMoveGameState)}";

		public CheckMoveGameState(
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
			var gameBoard = Context.GameBoard;
			var piecePosition = Context.PiecePosition;
			var swipeDirection = Context.SwipeDirection;
			var targetPosition = piecePosition + swipeDirection;

			if (!gameBoard.IsValidPosition(targetPosition))
			{
				var idleGameState = GameStateFactory.Create<IdleGameState>();
				StateMachine.SwitchTo(idleGameState);
				return;
			}

			gameBoard.Swap(piecePosition, targetPosition);

			var matches = Context.MatchChecker.FindMatches();
			matches.UnionWith(Context.BombChecker.CheckForBombs(matches));

			if (matches.Count > 0)
			{
				Context.CurrentMatches = matches;
				Context.MatchColor = gameBoard.GetPiece(targetPosition.x, targetPosition.y).Type;
				var destroyMatchesGameState = GameStateFactory.Create<DestroyMatchesGameState>();
				StateMachine.SwitchTo(destroyMatchesGameState);
			}
			else
			{
				gameBoard.Swap(piecePosition, targetPosition);
				Logger.LogError(LogTag, "No matches!");
				var idleGameState = GameStateFactory.Create<IdleGameState>();
				StateMachine.SwitchTo(idleGameState);
			}
		}
	}
}
