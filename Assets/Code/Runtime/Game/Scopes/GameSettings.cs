using UnityEngine;

namespace stroibot.TapTapPop.Scopes
{
	[CreateAssetMenu(fileName = "GameSettings", menuName = "Settings/Game")]
	public class GameSettings :
		ScriptableObject
	{
		[Header("Game Rules")]
		[Tooltip("Maximum amount of iterations during initial board setup to avoid matches")]
		[SerializeField, Range(50, 200)] private int _maxIterations = 100;
		[SerializeField, Range(1, 50)] private int _scorePerGem = 10;
		[SerializeField] private int _seed;

		[Header("Game Board")]
		[SerializeField, Range(2, 10)] private int _width = 7;
		[SerializeField, Range(2, 10)] private int _height = 7;

		public int MaxIterations => _maxIterations;
		public int ScorePerGem => _scorePerGem;
		public int Seed => _seed;
		public int Width => _width;
		public int Height => _height;
	}
}
