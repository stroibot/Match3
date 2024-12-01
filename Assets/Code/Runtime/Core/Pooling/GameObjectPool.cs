using stroibot.Core.Factories;
using UnityEngine;

namespace stroibot.Core.Pooling
{
	public class GameObjectPool<T> :
		Pool<T>
		where T : Component
	{
		private readonly Transform _transform;

		public GameObjectPool(
			Transform transform,
			IFactory<T> factory,
			GrowthStrategy growthStrategy = GrowthStrategy.Block,
			int blockSize = 5) :
			base(factory, growthStrategy, blockSize)
		{
			_transform = transform;
		}

		public override T Request()
		{
			var item = base.Request();
			item.gameObject.SetActive(true);
			return item;
		}

		public override void Return(
			T item)
		{
			item.gameObject.SetActive(false);
			base.Return(item);
		}

		protected override T Allocate()
		{
			var item = base.Allocate();
			item.gameObject.transform.SetParent(_transform);
			item.gameObject.SetActive(false);
			return item;
		}
	}
}
