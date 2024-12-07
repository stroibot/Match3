using stroibot.Core.States;
using stroibot.Match3.Models;
using stroibot.TapTapPop.Scopes;
using System.Collections.Generic;
using UnityEngine;
using ILogger = stroibot.Core.Logging.ILogger;

namespace stroibot.Match3.States
{
	public class RefillBoardGameState :
		GameState
	{
		private readonly string LogTag = $"{nameof(RefillBoardGameState)}";

		public RefillBoardGameState(
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

			for (int x = 0; x < gameBoard.Width; x++)
			{
				for (int y = 0; y < gameBoard.Height; y++)
				{
					if (gameBoard.GetPiece(x, y) != null)
					{
						continue;
					}

					var gemToUse = Piece.GetRandomType();
					int attempts = 0;

					if (Context.MatchChecker.HasMatchesAt(new Vector2Int(x, y), gemToUse))
					{
						var excludedColors = new HashSet<Piece.Color>();

						while (Context.MatchChecker.HasMatchesAt(new Vector2Int(x, y), gemToUse) &&
							   attempts < GameSettings.MaxIterations)
						{
							excludedColors.Add(gemToUse);
							gemToUse = Piece.GetRandomType(excludedColors);
							attempts++;
						}
					}

					Logger.Log(LogTag, $"Placed refill gem in {attempts} attempts");
					var gemModel = new Gem(gemToUse);
					gameBoard.SetPiece(x, y, gemModel);
				}
			}

			var matches = Context.MatchChecker.FindMatches();
			var bombMatches = Context.BombChecker.CheckForBombs();
			matches.UnionWith(bombMatches);

			if (matches.Count > 0)
			{
				Context.CurrentMatches = matches;
				var destroyMatchesGameState = GameStateFactory.Create<DestroyMatchesGameState>();
				StateMachine.SwitchTo(destroyMatchesGameState);
			}
			else
			{
				var idleGameState = GameStateFactory.Create<IdleGameState>();
				StateMachine.SwitchTo(idleGameState);
			}
		}
	}
}
