using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace stroibot.Core.Factories
{
	public class PrefabFactory<T> :
		IFactory<T>
		where T : Component
	{
		private readonly IObjectResolver _container;
		private readonly GameObject _prefab;

		public PrefabFactory(
			IObjectResolver container,
			GameObject prefab)
		{
			_container = container;
			_prefab = prefab;
		}

		public T Create()
		{
			return _container.Instantiate(_prefab).GetComponent<T>();
		}
	}
}
