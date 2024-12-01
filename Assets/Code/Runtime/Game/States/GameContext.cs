using stroibot.Match3.Models;
using System.Collections.Generic;
using UnityEngine;

namespace stroibot.Match3.States
{
	public class GameContext
	{
		public GameBoard GameBoard { get; set; }
		public MatchChecker MatchChecker { get; set; }
		public BombChecker BombChecker { get; set; }
		public Vector2Int SwipeDirection { get; set; }
		public Vector2Int PiecePosition { get; set; }
		public Piece.Color MatchColor { get; set; }
		public IReadOnlyCollection<Vector2Int> CurrentMatches { get; set; }
		public bool IsInputAllowed { get; set; }
		public int Score { get; set; }
	}
}