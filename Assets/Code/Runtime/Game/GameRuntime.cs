using UnityEngine;
using VContainer;

namespace stroibot.Match3
{
	public class GameRuntime :
		MonoBehaviour
	{
		private Game _game;

		[Inject]
		public void Construct(
			Game game)
		{
			_game = game;
		}

		public void Start()
		{
			_game.Start();
		}

		public void Update()
		{
			_game.Tick();
		}
	}
}
