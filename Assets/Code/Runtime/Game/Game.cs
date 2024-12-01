using stroibot.Core.States;
using stroibot.Match3.States;

namespace stroibot.Match3
{
	public class Game
	{
		private readonly StateMachine<GameContext> _stateMachine;
		private readonly GameStateFactory _gameStateFactory;

		protected Game(
			StateMachine<GameContext> stateMachine,
			GameStateFactory gameStateFactory)
		{
			_stateMachine = stateMachine;
			_gameStateFactory = gameStateFactory;
		}

		public virtual void Start()
		{
			var setupBoardGameState = _gameStateFactory.Create<SetupBoardGameState>();
			_stateMachine.SwitchTo(setupBoardGameState);
		}
	}
}
