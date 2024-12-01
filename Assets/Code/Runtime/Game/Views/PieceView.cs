using stroibot.Core.Factories;
using stroibot.Core.Pooling;
using stroibot.Match3.Models;
using System;
using System.Linq;
using System.Text;
using UnityEngine;
using VContainer;

namespace stroibot.Match3.Views
{
	public class PieceView :
		MonoBehaviour
	{
		[SerializeField] private PieceTypeMapping[] _pieceTypeMappings;

		public Piece Piece { get; private set; }

		private SpriteRenderer _spriteRenderer;

		public void Awake()
		{
			_spriteRenderer = GetComponent<SpriteRenderer>();
		}

		public void Initialize(
			Piece piece,
			Vector2Int position)
		{
			Piece = piece;
			PieceTypeMapping pieceTypeMapping;
			switch (piece)
			{
				case Bomb:
				{
					pieceTypeMapping = _pieceTypeMappings.Single(mapping => mapping.Color == piece.Type && mapping.IsBomb);
					_spriteRenderer.color = pieceTypeMapping.BombColor;
					break;
				}
				case Gem:
				{
					pieceTypeMapping = _pieceTypeMappings.Single(mapping => mapping.Color == piece.Type && !mapping.IsBomb);
					_spriteRenderer.color = Color.white;
					break;
				}
				default:
				{
					pieceTypeMapping = default;
					break;
				}
			}

			_spriteRenderer.sprite = pieceTypeMapping.Sprite;
			name = GeName(piece, position);
		}

		private static string GeName(
			Piece piece,
			Vector2Int position)
		{
			var builder = new StringBuilder();
			builder.Append(piece.Type);

			switch (piece)
			{
				case Bomb:
				{
					builder.Append(" Bomb ");
					break;
				}
				case Gem:
				{
					builder.Append(" Gem ");
					break;
				}
			}

			builder.Append($"[{position}]");

			return builder.ToString();
		}

		[Serializable]
		public struct PieceTypeMapping
		{
			public Piece.Color Color;
			public Sprite Sprite;
			public bool IsBomb;
			public Color BombColor;
		}

		public class Pool :
			GameObjectPool<PieceView>
		{
			public Pool(
				Transform transform,
				IFactory<PieceView> factory,
				GrowthStrategy growthStrategy = GrowthStrategy.Block,
				int blockSize = 5) :
				base(transform, factory, growthStrategy, blockSize)
			{
			}
		}

		public class Factory :
			PrefabFactory<PieceView>
		{
			public Factory(
				IObjectResolver container,
				GameObject prefab) :
				base(container, prefab)
			{
			}
		}
	}
}
