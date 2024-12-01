using VContainer;

namespace stroibot.Match3.States
{
	public class GameStateFactory
	{
		protected readonly IObjectResolver _container;

		public GameStateFactory(
			IObjectResolver container)
		{
			_container = container;
		}

		public TGameState Create<TGameState>()
			where TGameState : GameState
		{
			return _container.Resolve<TGameState>();
		}
	}
}
