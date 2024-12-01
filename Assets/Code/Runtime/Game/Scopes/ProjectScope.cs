using stroibot.Core.Logging;
using stroibot.Core.States;
using stroibot.Match3;
using stroibot.Match3.Input;
using stroibot.Match3.States;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using ILogger = stroibot.Core.Logging.ILogger;

namespace stroibot.TapTapPop.Scopes
{
	public class ProjectScope :
		LifetimeScope
	{
		[SerializeField] private GameSettings _gameSettings;

		protected override void Configure(
			IContainerBuilder builder)
		{
			// Game Settings
			builder.RegisterInstance(_gameSettings);
			// Logging
			builder.Register<ILogger, UnityLogger>(Lifetime.Singleton);
			// Input
			builder.Register<InputService>(Lifetime.Singleton);
			// Game
			builder.Register<GameContext>(Lifetime.Singleton);
			builder.Register<StateMachine<GameContext>>(Lifetime.Singleton);
			builder.Register<GameStateFactory>(Lifetime.Singleton);
			builder.Register<SetupBoardGameState>(Lifetime.Transient);
			builder.Register<IdleGameState>(Lifetime.Transient);
			builder.Register<CheckMoveGameState>(Lifetime.Transient);
			builder.Register<DestroyMatchesGameState>(Lifetime.Transient);
			builder.Register<CascadeGameState>(Lifetime.Transient);
			builder.Register<RefillBoardGameState>(Lifetime.Transient);
			// Game
			builder.Register<Game>(Lifetime.Singleton);
		}
	}
}
