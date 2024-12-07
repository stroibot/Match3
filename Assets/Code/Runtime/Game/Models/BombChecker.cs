using System.Collections.Generic;
using UnityEngine;

namespace stroibot.Match3.Models
{
	public class BombChecker
	{
		public static readonly Vector2Int[] BombExplosionPattern =
		{
			new(-2, 0), new(-1, 0), new(1, 0), new(2, 0),
			new(0, -2), new(0, -1), new(0, 1), new(0, 2),
			new(-1, -1), new(1, -1), new(-1, 1), new(1, 1)
		};

		private static readonly Vector2Int[] AdjacentOffsets =
		{
			new(0, 1), new(0, -1), new(1, 0), new(-1, 0)
		};

		private const int NumberOfBombsToMatch = 1;
		private const int NumberOfGemsToMatch = 3;

		private readonly GameBoard _gameBoard;

		public BombChecker(GameBoard gameBoard)
		{
			_gameBoard = gameBoard;
		}

		public HashSet<Vector2Int> CheckForBombs()
		{
			var bombMatches = new HashSet<Vector2Int>();

			for (int x = 0; x < _gameBoard.Width; x++)
			{
				for (int y = 0; y < _gameBoard.Height; y++)
				{
					var piece = _gameBoard.GetPiece(x, y);

					if (piece is not Bomb bomb)
					{
						continue;
					}

					var adjacentBombs = GetAdjacentBombs(new Vector2Int(x, y));

					if (adjacentBombs.Count >= NumberOfBombsToMatch)
					{
						AddBombExplosion(bombMatches, new Vector2Int(x, y));

						foreach (var adjacentBomb in adjacentBombs)
						{
							AddBombExplosion(bombMatches, adjacentBomb);
						}
					}

					var matchingGems = GetMatchingGemsForBomb(new Vector2Int(x, y), bomb.Type);

					if (matchingGems.Count >= NumberOfGemsToMatch)
					{
						AddBombExplosion(bombMatches, new Vector2Int(x, y));
					}
				}
			}

			return bombMatches;
		}

		private List<Vector2Int> GetAdjacentBombs(
			Vector2Int position)
		{
			var adjacentBombs = new List<Vector2Int>();

			foreach (var offset in AdjacentOffsets)
			{
				var adjacentPosition = position + offset;

				if (!_gameBoard.IsValidPosition(adjacentPosition))
				{
					continue;
				}

				var adjacentPiece = _gameBoard.GetPiece(adjacentPosition.x, adjacentPosition.y);

				if (adjacentPiece is Bomb)
				{
					adjacentBombs.Add(adjacentPosition);
				}
			}

			return adjacentBombs;
		}

		private List<Vector2Int> GetMatchingGemsForBomb(
			Vector2Int bombPosition,
			Piece.Color bombColor)
		{
			var matchingGems = new List<Vector2Int>();
			var directions = new[] { Vector2Int.right, Vector2Int.up };

			foreach (var dir in directions)
			{
				var matchingPositions = CheckDirection(bombPosition, dir, bombColor);

				if (matchingPositions.Count >= NumberOfGemsToMatch)
				{
					matchingGems.AddRange(matchingPositions);
				}
			}

			return matchingGems;
		}

		private List<Vector2Int> CheckDirection(
			Vector2Int bombPosition,
			Vector2Int direction,
			Piece.Color bombColor)
		{
			var positions = new List<Vector2Int>();

			for (int i = -2; i <= 2; i++)
			{
				var offset = direction * i;
				var checkPosition = bombPosition + offset;

				if (!_gameBoard.IsValidPosition(checkPosition))
				{
					continue;
				}

				var piece = _gameBoard.GetPiece(checkPosition.x, checkPosition.y);

				if (piece is Gem gem && gem.Type == bombColor)
				{
					positions.Add(checkPosition);
				}
			}

			return positions;
		}

		private void AddBombExplosion(
			HashSet<Vector2Int> bombMatches,
			Vector2Int bombPosition)
		{
			bombMatches.Add(bombPosition);

			foreach (var offset in BombExplosionPattern)
			{
				var explosionPosition = bombPosition + offset;

				if (_gameBoard.IsValidPosition(explosionPosition))
				{
					bombMatches.Add(explosionPosition);
				}
			}
		}
	}
}
