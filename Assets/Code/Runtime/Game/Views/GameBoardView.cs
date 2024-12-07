using DG.Tweening;
using stroibot.Match3.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

namespace stroibot.Match3.Views
{
	public class GameBoardView :
		MonoBehaviour
	{
		[SerializeField] private Transform _tilesTransform;
		[SerializeField] private GameObject _tilePrefab;
		[SerializeField] private Transform _piecesTransform;
		[SerializeField] private GameObject _pieceViewPrefab;
		[SerializeField] private float _delayBeforeBombExplosion = 0.2f;
		[SerializeField] private float _delayAfterBombExplosion = 0.2f;

		public int Width { get; private set; }
		public int Height { get; private set; }

		private PieceView.Pool _piecePool;
		private AnimationService _animationService;

		private GameBoard _gameBoard;
		private PieceView[,] _pieceViews;

		[Inject]
		public void Construct(
			IObjectResolver container,
			AnimationService animationService)
		{
			_piecePool = new PieceView.Pool(
				_piecesTransform,
				new PieceView.Factory(container, _pieceViewPrefab));
			_animationService = animationService;
		}

		public void Initialize(
			GameBoard gameBoard)
		{
			_gameBoard = gameBoard;
			Width = _gameBoard.Width;
			Height = _gameBoard.Height;
			_gameBoard.OnSwap += OnSwap;
			_gameBoard.OnRemoveMultiple += OnRemoveMultiple;
			_gameBoard.OnSet += OnSet;
			_gameBoard.OnCascade += OnCascade;
			FillTiles();
			FillPieces();
		}

		private void FillTiles()
		{
			for (int x = 0; x < Width; x++)
			{
				for (int y = 0; y < Height; y++)
				{
					var tile = Instantiate(_tilePrefab, _tilesTransform);
					tile.transform.localPosition = new Vector3(x, y);
					tile.name = $"BG Tile - {x}, {y}";
				}
			}
		}

		private void FillPieces()
		{
			var tween = DOTween.Sequence().Pause();
			_pieceViews = new PieceView[Width, Height];

			for (int x = 0; x < Width; x++)
			{
				var columnSequence = DOTween.Sequence().Pause();

				for (int y = 0; y < Height; y++)
				{
					var pieceView = CreatePieceView(x, y);
					var spawnTween = pieceView.GetSpawnAnimation(new Vector3(x, y), y * 0.05f);
					columnSequence.Join(spawnTween);
				}

				tween.Join(columnSequence.SetDelay(x * 0.05f));
			}

			_animationService.EnqueueAnimation(tween);
		}

		private void OnSwap(
			Vector2Int firstPosition,
			Vector2Int secondPosition)
		{
			var firstPieceView = _pieceViews[firstPosition.x, firstPosition.y];
			var secondPieceView = _pieceViews[secondPosition.x, secondPosition.y];
			_pieceViews[firstPosition.x, firstPosition.y] = secondPieceView;
			_pieceViews[secondPosition.x, secondPosition.y] = firstPieceView;
			var firstTween = firstPieceView.Move(new Vector3(secondPosition.x, secondPosition.y));
			var secondTween = secondPieceView.Move(new Vector3(firstPosition.x, firstPosition.y));
			var tween = DOTween.Sequence().Join(firstTween).Join(secondTween).Pause();
			_animationService.EnqueueAnimation(tween);
		}

		private void OnRemoveMultiple(
			IReadOnlyCollection<Vector2Int> positionsToRemove)
		{
			var neighborGroupTween = DOTween.Sequence().Pause();
			var bombsTween = DOTween.Sequence().Pause();
			var othersTween = DOTween.Sequence().Pause();

			var bombPositions = positionsToRemove
				.Where(pos => _pieceViews[pos.x, pos.y].Piece is Bomb)
				.ToList();

			var nonBombPositions = positionsToRemove
				.Where(pos => !bombPositions.Contains(pos))
				.ToList();

			var bombExplosionPositions = new HashSet<Vector2Int>();

			foreach (var bombPos in bombPositions)
			{
				foreach (var offset in BombChecker.BombExplosionPattern)
				{
					var targetPos = bombPos + offset;

					if (!_gameBoard.IsValidPosition(targetPos) ||
						bombPositions.Contains(targetPos) ||
						!bombExplosionPositions.Add(targetPos))
					{
						continue;
					}

					var pieceView = _pieceViews[targetPos.x, targetPos.y];

					if (pieceView == null)
					{
						continue;
					}

					_pieceViews[targetPos.x, targetPos.y] = null;
					float delay = Mathf.Abs(offset.x) + Mathf.Abs(offset.y) * 0.05f;
					var pieceTween = pieceView.GetDestroyAnimation();
					pieceTween.onComplete += () => ReturnPieceViewToPool(pieceView);
					neighborGroupTween.Insert(delay, pieceTween);
				}
			}

			if (bombPositions.Count > 0)
			{
				neighborGroupTween.SetDelay(_delayBeforeBombExplosion);
				_animationService.EnqueueAnimation(neighborGroupTween);
			}

			foreach (var bombPos in bombPositions)
			{
				var bombView = _pieceViews[bombPos.x, bombPos.y];

				if (bombView == null)
				{
					continue;
				}

				_pieceViews[bombPos.x, bombPos.y] = null;
				var bombTween = bombView.GetDestroyAnimation();
				bombTween.onComplete += () => ReturnPieceViewToPool(bombView);
				bombsTween.Join(bombTween);
			}

			if (bombPositions.Count > 0)
			{
				bombsTween.SetDelay(_delayAfterBombExplosion);
				_animationService.EnqueueAnimation(bombsTween);
			}

			foreach (var pos in nonBombPositions.Except(bombExplosionPositions))
			{
				var pieceView = _pieceViews[pos.x, pos.y];

				if (pieceView == null)
				{
					continue;
				}

				_pieceViews[pos.x, pos.y] = null;
				var pieceTween = pieceView.GetDestroyAnimation();
				pieceTween.onComplete += () => ReturnPieceViewToPool(pieceView);
				othersTween.Join(pieceTween);
			}

			_animationService.EnqueueAnimation(othersTween);
		}

		private void OnSet(
			Vector2Int position)
		{
			var existingPieceView = _pieceViews[position.x, position.y];

			if (existingPieceView != null)
			{
				_piecePool.Return(existingPieceView);
			}

			var pieceView = CreatePieceView(position.x, position.y);
			var spawnTween = pieceView.GetSpawnAnimation(new Vector3(position.x, position.y), position.y * 0.005f);
			_animationService.EnqueueAnimation(spawnTween);
		}

		private void OnCascade()
		{
			var tween = DOTween.Sequence().Pause();

			for (int x = 0; x < Width; x++)
			{
				var columnSequence = DOTween.Sequence();

				for (int y = 1; y < Height; y++)
				{
					if (_pieceViews[x, y] != null)
					{
						int fallY = y;

						while (fallY > 0 && _pieceViews[x, fallY - 1] == null)
						{
							fallY--;
						}

						if (fallY == y)
						{
							continue;
						}

						var pieceView = _pieceViews[x, y];
						_pieceViews[x, fallY] = pieceView;
						_pieceViews[x, y] = null;
						var fallTween = pieceView.Move(new Vector3(x, fallY), fallY * 0.05f);
						columnSequence.Join(fallTween);
					}
				}

				tween.Join(columnSequence.SetDelay(x * 0.05f));
			}

			_animationService.EnqueueAnimation(tween);
		}

		private PieceView CreatePieceView(
			int x,
			int y)
		{
			var piece = _gameBoard.GetPiece(x, y);
			var pieceView = _piecePool.Request();
			pieceView.Initialize(piece, new Vector2Int(x, y));
			pieceView.transform.localPosition = new Vector3(x, Height, 0);
			_pieceViews[x, y] = pieceView;
			return pieceView;
		}

		private void ReturnPieceViewToPool(
			PieceView pieceView)
		{
			_piecePool.Return(pieceView);
		}
	}
}
