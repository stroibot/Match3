using stroibot.Core.Logging;
using stroibot.Core.States;
using stroibot.TapTapPop.Scopes;

namespace stroibot.Match3.States
{
	public abstract class GameState :
		IState
	{
		protected ILogger Logger { get; }
		protected GameContext Context { get; }
		protected StateMachine<GameContext> StateMachine { get; }
		protected GameStateFactory GameStateFactory { get; }
		protected GameSettings GameSettings { get; }

		protected GameState(
			ILogger logger,
			GameContext context,
			StateMachine<GameContext> stateMachine,
			GameStateFactory gameStateFactory,
			GameSettings gameSettings)
		{
			Logger = logger;
			Context = context;
			StateMachine = stateMachine;
			GameStateFactory = gameStateFactory;
			GameSettings = gameSettings;
		}

		public virtual void OnEnter()
		{
		}

		public virtual void OnExit()
		{
		}
	}
}
