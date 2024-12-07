using stroibot.Core.States;
using stroibot.Match3.States;
using stroibot.Match3.Views;
using UnityEngine;
using VContainer;
using ILogger = stroibot.Core.Logging.ILogger;

namespace stroibot.Match3.Input
{
	public class InputService :
		MonoBehaviour
	{
		private readonly string LogTag = $"{nameof(InputService)}";

		private const float SwipeThreshold = 50f;

		private ILogger _logger;
		private StateMachine<GameContext> _stateMachine;
		private GameContext _gameContext;
		private GameStateFactory _gameStateFactory;
		private AnimationService _animationService;
		private Camera _camera;

		private Vector2 _startPosition;
		private GameBoardView _gameBoardView;
		private Vector2Int _gridPosition;

		[Inject]
		public void Construct(
			ILogger logger,
			StateMachine<GameContext> stateMachine,
			GameContext gameContext,
			GameStateFactory gameStateFactory,
			AnimationService animationService)
		{
			_logger = logger;
			_stateMachine = stateMachine;
			_gameContext = gameContext;
			_gameStateFactory = gameStateFactory;
			_animationService = animationService;
			_gameBoardView = FindObjectOfType<GameBoardView>();
			_camera = Camera.main;
		}

		public void Update()
		{
			if (!_gameContext.IsInputAllowed || _animationService.IsAnimating)
			{
				return;
			}

			if (UnityEngine.Input.GetMouseButtonDown(0))
			{
				_startPosition = UnityEngine.Input.mousePosition;
				_gridPosition = GetGridPosition();
			}

			if (UnityEngine.Input.GetMouseButtonUp(0))
			{
				var swipeVector = (Vector2)UnityEngine.Input.mousePosition - _startPosition;
				var swipeDistance = swipeVector.magnitude;

				if (swipeDistance > SwipeThreshold)
				{
					var swipeDirection = swipeVector.normalized;
					_gameContext.SwipeDirection = Vector2Int.RoundToInt(swipeDirection);
					_gameContext.PiecePosition = _gridPosition;
					var swapGameState = _gameStateFactory.Create<CheckMoveGameState>();
					_stateMachine.SwitchTo(swapGameState);
				}
			}
		}

		private Vector2Int WorldToGridPosition(Vector2 worldPosition)
		{
			var localPosition = worldPosition - (Vector2)_gameBoardView.transform.position;
			int x = Mathf.RoundToInt(localPosition.x);
			int y = Mathf.RoundToInt(localPosition.y);
			return new Vector2Int(x, y);
		}

		private Vector2Int GetGridPosition()
		{
			var worldPosition = _camera.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
			var gridPosition = WorldToGridPosition(worldPosition);

			_logger.LogWarning(LogTag, $"Grid Position {gridPosition}");

			if (gridPosition.x >= 0 && gridPosition.x < _gameBoardView.Width &&
				gridPosition.y >= 0 && gridPosition.y < _gameBoardView.Height)
			{
				return gridPosition;
			}

			return default;
		}
	}
}
