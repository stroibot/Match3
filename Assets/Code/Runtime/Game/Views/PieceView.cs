using DG.Tweening;
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

			transform.localScale = Vector3.one;
			_spriteRenderer.sprite = pieceTypeMapping.Sprite;
			name = GetName(piece, position);
		}

		public Tween GetSpawnAnimation(
			Vector3 position,
			float delay = 0f)
		{
			return transform
				.DOLocalMove(position, 0.2f)
				.SetDelay(delay)
				.SetEase(Ease.OutSine)
				.Pause();
		}

		public Tween Move(
			Vector3 newPosition,
			float delay = 0f)
		{
			UpdateName(newPosition);
			return transform
				.DOLocalMove(newPosition, 0.2f)
				.SetDelay(delay)
				.SetEase(Ease.OutSine)
				.Pause();
		}

		public Tween GetDestroyAnimation(
			float delay = 0f)
		{
			return transform
				.DOScale(Vector3.zero, 0.2f)
				.SetDelay(delay)
				.SetEase(Ease.InBack)
				.Pause();
		}

		private void UpdateName(
			Vector3 newPosition)
		{
			name = GetName(Piece, new Vector2Int(Mathf.RoundToInt(newPosition.x), Mathf.RoundToInt(newPosition.y)));
		}

		private static string GetName(
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
