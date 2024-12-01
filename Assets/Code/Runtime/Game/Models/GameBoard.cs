using System;
using UnityEngine;

namespace stroibot.Match3.Models
{
	public class GameBoard
	{
		public event Action<Vector2Int, Vector2Int> OnSwap;
		public event Action<Vector2Int> OnRemove;
		public event Action<Vector2Int> OnSet;
		public event Action OnCascade;

		public int Width { get; }
		public int Height { get; }

		private readonly Grid<Piece> _pieces;

		public GameBoard(
			int width,
			int height)
		{
			Width = width;
			Height = height;
			_pieces = new Grid<Piece>(width, height);
		}

		public Piece GetPiece(
			int x,
			int y)
		{
			return _pieces[x, y];
		}

		public void SetPiece(
			int x,
			int y,
			Piece piece)
		{
			_pieces[x, y] = piece;
			OnSet?.Invoke(new Vector2Int(x, y));
		}

		public void RemoveAt(int x, int y)
		{
			_pieces[x, y] = null;
			OnRemove?.Invoke(new Vector2Int(x, y));
		}

		public void Cascade()
		{
			int nullCounter = 0;

			for (int x = 0; x < Width; x++)
			{
				for (int y = 0; y < Height; y++)
				{
					var p = GetPiece(x, y);

					if (p == null)
					{
						nullCounter++;
					}
					else if (nullCounter > 0)
					{
						_pieces[x, y - nullCounter] = p;
						_pieces[x, y] = null;
					}
				}

				nullCounter = 0;
			}

			OnCascade?.Invoke();
		}

		public void Swap(
			Vector2Int firstPosition,
			Vector2Int secondPosition)
		{
			var firstPiece = _pieces[firstPosition.x, firstPosition.y];
			var secondPiece = _pieces[secondPosition.x, secondPosition.y];
			_pieces[secondPosition.x, secondPosition.y] = firstPiece;
			_pieces[firstPosition.x, firstPosition.y] = secondPiece;
			OnSwap?.Invoke(firstPosition, secondPosition);
		}

		public bool IsValidPosition(
			Vector2Int position)
		{
			return position is { x: >= 0, y: >= 0 } && position.x < Width && position.y < Height;
		}
	}
}
