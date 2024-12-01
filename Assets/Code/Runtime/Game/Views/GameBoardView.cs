using DG.Tweening;
using stroibot.Match3.Models;
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

		public int Width { get; private set; }
		public int Height { get; private set; }

		private PieceView.Pool _piecePool;
		private GameBoard _gameBoard;

		private PieceView[,] _pieceViews;

		[Inject]
		public void Construct(
			IObjectResolver container)
		{
			_piecePool = new PieceView.Pool(
				_piecesTransform,
				new PieceView.Factory(container, _pieceViewPrefab));
		}

		public void Initialize(
			GameBoard gameBoard)
		{
			_gameBoard = gameBoard;
			Width = _gameBoard.Width;
			Height = _gameBoard.Height;
			_gameBoard.OnSwap += OnSwap;
			_gameBoard.OnRemove += OnRemove;
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
			_pieceViews = new PieceView[Width, Height];

			for (int x = 0; x < Width; x++)
			{
				for (int y = 0; y < Height; y++)
				{
					var piece = _gameBoard.GetPiece(x, y);
					var pieceView = _piecePool.Request();
					pieceView.Initialize(piece, new Vector2Int(x, y));
					pieceView.transform.localPosition = new Vector3(x, Height, 0);
					_pieceViews[x, y] = pieceView;
					pieceView.transform
						.DOLocalMove(new Vector3(x, y, 0), 0.2f)
						.SetDelay((x + y * Width) * 0.05f)
						.SetEase(Ease.OutBounce);
				}
			}
		}

		private void OnSwap(
			Vector2Int firstPosition,
			Vector2Int secondPosition)
		{
			var firstPieceView = _pieceViews[firstPosition.x, firstPosition.y];
			var secondPieceView = _pieceViews[secondPosition.x, secondPosition.y];
			_pieceViews[firstPosition.x, firstPosition.y] = secondPieceView;
			_pieceViews[secondPosition.x, secondPosition.y] = firstPieceView;
			firstPieceView.transform.localPosition = new Vector3(secondPosition.x, secondPosition.y);
			secondPieceView.transform.localPosition = new Vector3(firstPosition.x, firstPosition.y);
		}

		private void OnRemove(
			Vector2Int position)
		{
			var pieceView = _pieceViews[position.x, position.y];
			_pieceViews[position.x, position.y] = null;
			_piecePool.Return(pieceView);
		}

		private void OnSet(
			Vector2Int position)
		{
			var piece = _gameBoard.GetPiece(position.x, position.y);
			var pieceView = _piecePool.Request();
			pieceView.Initialize(piece, position);
			pieceView.transform.localPosition = new Vector3(position.x, position.y);
			_pieceViews[position.x, position.y] = pieceView;
		}

		private void OnCascade()
		{
			int nullCounter = 0;

			for (int x = 0; x < Width; x++)
			{
				for (int y = 0; y < Height; y++)
				{
					var pieceView = _pieceViews[x, y];

					if (pieceView == null)
					{
						nullCounter++;
					}
					else if (nullCounter > 0)
					{
						_pieceViews[x, y - nullCounter] = pieceView;
						_pieceViews[x, y] = null;
						pieceView.transform.localPosition = new Vector3(x, y - nullCounter);
					}
				}

				nullCounter = 0;
			}
		}
	}
}
