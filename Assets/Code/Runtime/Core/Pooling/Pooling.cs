namespace stroibot.Core.Pooling
{
	public interface IPool<T>
	{
		public T Request();
		public void Return(T item);
	}
}
