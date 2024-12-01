using stroibot.Core.Factories;
using System.Collections.Generic;

namespace stroibot.Core.Pooling
{
	public class Pool<T> :
		IPool<T>
	{
		private int TotalCount => _activeCount + _available.Count;

		private readonly IFactory<T> _factory;
		private readonly GrowthStrategy _growthStrategy;
		private readonly int _blockSize;
		private readonly Stack<T> _available;

		private bool _isInitialized;
		private int _activeCount;

		public Pool(
			IFactory<T> factory,
			GrowthStrategy growthStrategy = GrowthStrategy.Block,
			int blockSize = 5)
		{
			_factory = factory;
			_growthStrategy = growthStrategy;
			_blockSize = blockSize;
			_available = new Stack<T>();
		}

		public void Initialize(
			int initialSize)
		{
			if (_isInitialized)
			{
				return;
			}

			ExpandBy(initialSize);
			_isInitialized = true;
		}

		public virtual T Request()
		{
			if (_available.Count == 0)
			{
				Grow();
			}

			if (_available.Count == 0)
			{
				return default;
			}

			_activeCount++;
			return _available.Pop();
		}

		public virtual void Return(
			T item)
		{
			_activeCount--;
			_available.Push(item);
		}

		protected virtual T Allocate()
		{
			var item = _factory.Create();
			return item;
		}

		private void Grow()
		{
			switch (_growthStrategy)
			{
				case GrowthStrategy.OnDemand:
				{
					ExpandBy(1);
					break;
				}
				case GrowthStrategy.Double:
				{
					ExpandBy(TotalCount == 0 ? 1 : TotalCount);
					break;
				}
				case GrowthStrategy.Block:
					ExpandBy(_blockSize);
					break;
			}
		}

		private void ExpandBy(
			int size)
		{
			for (int i = 0; i < size; i++)
			{
				var item = Allocate();
				_available.Push(item);
			}
		}
	}
}