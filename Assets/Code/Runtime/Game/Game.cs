using stroibot.Core.States;
using stroibot.Match3.States;
using stroibot.Match3.Views;

namespace stroibot.Match3
{
	public class Game
	{
		private readonly StateMachine<GameContext> _stateMachine;
		private readonly GameStateFactory _gameStateFactory;
		private readonly AnimationService _animationService;

		public Game(
			StateMachine<GameContext> stateMachine,
			GameStateFactory gameStateFactory,
			AnimationService animationService)
		{
			_stateMachine = stateMachine;
			_gameStateFactory = gameStateFactory;
			_animationService = animationService;
		}

		public void Start()
		{
			var setupBoardGameState = _gameStateFactory.Create<SetupBoardGameState>();
			_stateMachine.SwitchTo(setupBoardGameState);
		}

		public void Tick()
		{
			_animationService.UpdateAnimations();
		}
	}
}
