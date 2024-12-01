using NUnit.Framework;
using stroibot.Match3.Models;
using System.Collections.Generic;
using UnityEngine;

namespace stroibot.Match3.Tests
{
	[TestFixture]
	public class MatchCheckerTests :
		MonoBehaviour
	{
		[Test]
		public void GivenGrid3x3WithMatches_WhenCallingFindMatches_ThenReturns3MatchedPosition()
		{
			var gameBoard = new GameBoard(3, 3);
			var matchChecker = new MatchChecker(gameBoard);

			gameBoard.SetPiece(0, 0, new Gem(Piece.Color.Blue));
			gameBoard.SetPiece(1, 0, new Gem(Piece.Color.Green));
			gameBoard.SetPiece(2, 0, new Gem(Piece.Color.Green));

			gameBoard.SetPiece(0, 1, new Gem(Piece.Color.Blue));
			gameBoard.SetPiece(1, 1, new Gem(Piece.Color.Blue));
			gameBoard.SetPiece(2, 1, new Gem(Piece.Color.Red));

			gameBoard.SetPiece(0, 2, new Gem(Piece.Color.Blue));
			gameBoard.SetPiece(1, 2, new Gem(Piece.Color.Red));
			gameBoard.SetPiece(2, 2, new Gem(Piece.Color.Green));

			var foundMatches = new List<Vector2Int>(matchChecker.FindMatches());

			Assert.IsNotNull(foundMatches);
			Assert.AreEqual(foundMatches.Count, 3);
			Assert.AreEqual(foundMatches[0], new Vector2Int(0, 0));
			Assert.AreEqual(foundMatches[1], new Vector2Int(0, 1));
			Assert.AreEqual(foundMatches[2], new Vector2Int(0, 2));
		}

		[Test]
		public void GivenGrid4x4WithNoMatches_WhenCallingFindMatches_ThenReturnsNoMatchedPosition()
		{
			var gameBoard = new GameBoard(4, 4);
			var matchChecker = new MatchChecker(gameBoard);

			gameBoard.SetPiece(0, 0, new Gem(Piece.Color.Blue));
			gameBoard.SetPiece(1, 0, new Gem(Piece.Color.Green));
			gameBoard.SetPiece(2, 0, new Gem(Piece.Color.Yellow));
			gameBoard.SetPiece(3, 0, new Gem(Piece.Color.Green));

			gameBoard.SetPiece(0, 1, new Gem(Piece.Color.Purple));
			gameBoard.SetPiece(1, 1, new Gem(Piece.Color.Purple));
			gameBoard.SetPiece(2, 1, new Gem(Piece.Color.Red));
			gameBoard.SetPiece(3, 1, new Gem(Piece.Color.Red));

			gameBoard.SetPiece(0, 2, new Gem(Piece.Color.Blue));
			gameBoard.SetPiece(1, 2, new Gem(Piece.Color.Red));
			gameBoard.SetPiece(2, 2, new Gem(Piece.Color.Yellow));
			gameBoard.SetPiece(3, 2, new Gem(Piece.Color.Green));

			gameBoard.SetPiece(0, 3, new Gem(Piece.Color.Red));
			gameBoard.SetPiece(1, 3, new Gem(Piece.Color.Red));
			gameBoard.SetPiece(2, 3, new Gem(Piece.Color.Purple));
			gameBoard.SetPiece(3, 3, new Gem(Piece.Color.Green));

			var foundMatches = new List<Vector2Int>(matchChecker.FindMatches());

			Assert.IsNotNull(foundMatches);
			Assert.AreEqual(foundMatches.Count, 0);
		}
	}
}
