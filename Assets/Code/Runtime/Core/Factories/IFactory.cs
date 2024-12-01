namespace stroibot.Core.Factories
{
	public interface IFactory
	{
	}

	public interface IFactory<out TValue> :
		IFactory
	{
		public TValue Create();
	}

	public interface IFactory<in TParam1, out TValue> :
		IFactory
	{
		public TValue Create(TParam1 param);
	}
}
