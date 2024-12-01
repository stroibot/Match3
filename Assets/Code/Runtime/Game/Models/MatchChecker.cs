using System.Collections.Generic;
using UnityEngine;

namespace stroibot.Match3.Models
{
	public class MatchChecker
	{
		private const int NumberOfGemsToMatch = 3;

		private readonly GameBoard _gameBoard;

		public MatchChecker(
			GameBoard gameBoard)
		{
			_gameBoard = gameBoard;
		}

		public HashSet<Vector2Int> FindMatches()
		{
			var matches = new HashSet<Vector2Int>();
			// Check horizontal matches
			matches.UnionWith(FindDirectionalMatches(isHorizontal: true));
			// Check vertical matches
			matches.UnionWith(FindDirectionalMatches(isHorizontal: false));
			return matches;
		}

		private HashSet<Vector2Int> FindDirectionalMatches(
			bool isHorizontal)
		{
			var matches = new HashSet<Vector2Int>();

			int outerLimit = isHorizontal ? _gameBoard.Height : _gameBoard.Width;
			int innerLimit = isHorizontal ? _gameBoard.Width : _gameBoard.Height;

			for (int outer = 0; outer < outerLimit; outer++)
			{
				var currentMatch = new List<Vector2Int>();

				for (int inner = 0; inner < innerLimit; inner++)
				{
					int x = isHorizontal ? inner : outer;
					int y = isHorizontal ? outer : inner;

					var piece = _gameBoard.GetPiece(x, y);

					if (piece is Gem &&
						(currentMatch.Count == 0 || piece.Type == _gameBoard.GetPiece(currentMatch[0].x, currentMatch[0].y).Type))
					{
						currentMatch.Add(new Vector2Int(x, y));
					}
					else
					{
						if (currentMatch.Count >= NumberOfGemsToMatch)
						{
							foreach (var position in currentMatch)
							{
								matches.Add(position);
							}
						}

						currentMatch.Clear();

						if (piece != null)
						{
							currentMatch.Add(new Vector2Int(x, y));
						}
					}
				}

				if (currentMatch.Count >= NumberOfGemsToMatch)
				{
					matches.UnionWith(currentMatch);
				}
			}

			return matches;
		}

		public bool HasMatchesAt(
			Vector2Int position,
			Piece.Color type)
		{
			if (position.x > 1)
			{
				if (_gameBoard.GetPiece(position.x - 1, position.y).Type == type &&
					_gameBoard.GetPiece(position.x - 2, position.y).Type == type)
				{
					return true;
				}
			}

			if (position.y > 1)
			{
				if (_gameBoard.GetPiece(position.x, position.y - 1).Type == type &&
					_gameBoard.GetPiece(position.x, position.y - 2).Type == type)
				{
					return true;
				}
			}

			return false;
		}
	}
}
