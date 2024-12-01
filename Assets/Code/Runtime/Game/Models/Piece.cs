using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace stroibot.Match3.Models
{
	public abstract class Piece
	{
		public Color Type { get; protected set; }

		protected Piece(
			Color type)
		{
			Type = type;
		}

		public static Color GetRandomType(
			HashSet<Color> excludedColors = null)
		{
			var colorTypes = Enum.GetValues(typeof(Color)).Cast<Color>().ToList();

			if (excludedColors != null)
			{
				colorTypes = colorTypes.Where(color => !excludedColors.Contains(color)).ToList();
			}

			if (colorTypes.Count == 0)
			{
				colorTypes = Enum.GetValues(typeof(Color)).Cast<Color>().ToList();
			}

			int randomIndex = Random.Range(0, colorTypes.Count);
			return colorTypes[randomIndex];
		}


		public enum Color
		{
			Blue,
			Green,
			Red,
			Yellow,
			Purple
		}
	}
}
