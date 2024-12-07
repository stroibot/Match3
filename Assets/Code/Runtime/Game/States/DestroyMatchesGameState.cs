using stroibot.Core.States;
using stroibot.Match3.Models;
using stroibot.TapTapPop.Scopes;
using System.Collections.Generic;
using UnityEngine;
using ILogger = stroibot.Core.Logging.ILogger;

namespace stroibot.Match3.States
{
	public class DestroyMatchesGameState :
		GameState
	{
		public DestroyMatchesGameState(
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
			var currentMatches = Context.CurrentMatches;
			var matchGroups = GroupMatchesByColor(currentMatches);

			Context.Score += GameSettings.ScorePerGem * currentMatches.Count;

			gameBoard.RemoveAt(currentMatches);

			foreach (var matchGroup in matchGroups)
			{
				if (matchGroup.Value.Count < 4)
				{
					continue;
				}

				var piecePosition = Context.PiecePosition;
				var swipeDirection = Context.SwipeDirection;
				var targetPosition = piecePosition + swipeDirection;
				var bomb = new Bomb(matchGroup.Key);
				gameBoard.SetPiece(targetPosition.x, targetPosition.y, bomb);
				break;
			}

			var cascadeGameState = GameStateFactory.Create<CascadeGameState>();
			StateMachine.SwitchTo(cascadeGameState);
		}

		private Dictionary<Piece.Color, List<Vector2Int>> GroupMatchesByColor(
			IReadOnlyCollection<Vector2Int> matches)
		{
			var groups = new Dictionary<Piece.Color, List<Vector2Int>>();

			foreach (var position in matches)
			{
				var piece = Context.GameBoard.GetPiece(position.x, position.y);

				if (piece == null)
				{
					continue;
				}

				if (!groups.ContainsKey(piece.Type))
				{
					groups[piece.Type] = new List<Vector2Int>();
				}

				groups[piece.Type].Add(position);
			}

			return groups;
		}

	}
}
