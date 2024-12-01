namespace stroibot.Match3.Models
{
	public class Grid<T>
	{
		public int Width { get; }
		public int Height { get; }

		private readonly T[,] _grid;

		public Grid(
			int width,
			int height)
		{
			Width = width;
			Height = height;
			_grid = new T[width, height];
		}

		public T this[
			int x,
			int y]
		{
			get => GetValue(x, y);
			set => SetValue(x, y, value);
		}

		public void SetValue(
			int x,
			int y,
			T value)
		{
			if (x >= 0 && y >= 0 &&
				x <= Width && y <= Height)
			{
				_grid[x, y] = value;
			}
		}

		public T GetValue(
			int x,
			int y)
		{
			if (x >= 0 && y >= 0 &&
				x <= Width && y <= Height)
			{
				return _grid[x, y];
			}

			return default;
		}
	}
}
