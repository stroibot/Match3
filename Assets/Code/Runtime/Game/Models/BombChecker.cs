using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace stroibot.Match3.Models
{
	public class BombChecker
	{
		private const int BlasSize = 1;

		private readonly GameBoard _gameBoard;

		public BombChecker(
			GameBoard gameBoard)
		{
			_gameBoard = gameBoard;
		}

		public HashSet<Vector2Int> CheckForBombs(
			IReadOnlyCollection<Vector2Int> matches)
		{
			var newMatches = new HashSet<Vector2Int>();

			foreach (var matchPosition in matches)
			{
				var piece = _gameBoard.GetPiece(matchPosition.x, matchPosition.y);

				switch (piece)
				{
					case Bomb:
					{
						newMatches.UnionWith(MarkBombArea(matchPosition, BlasSize));
						break;
					}
					case Gem gem:
					{
						newMatches.UnionWith(CheckNeighborsForBombs(matchPosition, gem.Type));
						break;
					}
				}
			}

			return newMatches;
		}

		private HashSet<Vector2Int> CheckNeighborsForBombs(
			Vector2Int position,
			Piece.Color color)
		{
			var newMatches = new HashSet<Vector2Int>();
			var neighbors = GetNeighbors(position);

			foreach (var neighbor in neighbors)
			{
				var piece = _gameBoard.GetPiece(neighbor.x, neighbor.y);

				switch (piece)
				{
					case Bomb:
					{
						newMatches.Add(neighbor);
						newMatches.UnionWith(MarkBombArea(neighbor, BlasSize));
						break;
					}
					case Gem gem when gem.Type == color:
					{
						if (HasValidMatch(neighbor, color))
						{
							newMatches.Add(neighbor);
						}
						break;
					}
				}
			}

			return newMatches;
		}

		private List<Vector2Int> GetNeighbors(Vector2Int position)
		{
			var neighbors = new List<Vector2Int>
			{
				new(position.x - 1, position.y),
				new(position.x + 1, position.y),
				new(position.x, position.y - 1),
				new(position.x, position.y + 1)
			};

			return neighbors.Where(_gameBoard.IsValidPosition).ToList();
		}

		private HashSet<Vector2Int> MarkBombArea(
			Vector2Int position,
			int blastSize)
		{
			var matches = new HashSet<Vector2Int>();

			for (int x = position.x - blastSize; x <= position.x + blastSize; x++)
			{
				for (int y = position.y - blastSize; y <= position.y + blastSize; y++)
				{
					if (_gameBoard.IsValidPosition(new Vector2Int(x, y)))
					{
						var gem = _gameBoard.GetPiece(x, y);

						if (gem == null)
						{
							continue;
						}

						matches.Add(new Vector2Int(x, y));
					}
				}
			}

			return matches;
		}

		private bool HasValidMatch(
			Vector2Int position,
			Piece.Color color)
		{
			int horizontalMatch = CountMatchInDirection(position, color, Vector2Int.left) +
								  CountMatchInDirection(position, color, Vector2Int.right) + 1;

			int verticalMatch = CountMatchInDirection(position, color, Vector2Int.up) +
								CountMatchInDirection(position, color, Vector2Int.down) + 1;

			return horizontalMatch >= 3 || verticalMatch >= 3;
		}

		private int CountMatchInDirection(
			Vector2Int position,
			Piece.Color color,
			Vector2Int direction)
		{
			int count = 0;

			while (true)
			{
				position += direction;

				if (!_gameBoard.IsValidPosition(position))
				{
					break;
				}

				var piece = _gameBoard.GetPiece(position.x, position.y);

				if (piece is Gem gem && gem.Type == color)
				{
					count++;
				}
				else
				{
					break;
				}
			}

			return count;
		}
	}
}
