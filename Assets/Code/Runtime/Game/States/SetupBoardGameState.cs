using stroibot.Core.States;
using stroibot.Match3.Models;
using stroibot.Match3.Views;
using stroibot.TapTapPop.Scopes;
using UnityEngine;
using ILogger = stroibot.Core.Logging.ILogger;

namespace stroibot.Match3.States
{
	public class SetupBoardGameState :
		GameState
	{
		private readonly string LogTag = $"{nameof(SetupBoardGameState)}";

		public SetupBoardGameState(
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
			var gameBoard = new GameBoard(GameSettings.Width, GameSettings.Height);
			Context.GameBoard = gameBoard;
			Context.MatchChecker = new MatchChecker(gameBoard);
			Context.BombChecker = new BombChecker(gameBoard);

			for (int x = 0; x < gameBoard.Width; x++)
			{
				for (int y = 0; y < gameBoard.Height; y++)
				{
					var gemTypeToUse = Piece.GetRandomType();
					int attempts = 0;

					while (Context.MatchChecker.HasMatchesAt(new Vector2Int(x, y), gemTypeToUse) &&
						   attempts < GameSettings.MaxIterations)
					{
						gemTypeToUse = Piece.GetRandomType();
						attempts++;
					}

					Logger.Log(LogTag, $"Resolved game board setup in {attempts} attempts");
					var gemModel = new Gem(gemTypeToUse);
					gameBoard.SetPiece(x, y, gemModel);
				}
			}

			var gameBoardView = Object.FindObjectOfType<GameBoardView>();
			gameBoardView.Initialize(gameBoard);

			var loadKingdomState = GameStateFactory.Create<IdleGameState>();
			StateMachine.SwitchTo(loadKingdomState);
		}
	}
}
