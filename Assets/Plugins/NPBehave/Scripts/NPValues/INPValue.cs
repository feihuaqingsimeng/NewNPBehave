namespace NPBehave
{
	public interface INPValue<T>
	{
		T Get();
		void Set(INPValue<T> npValue);
		void Set(T value);
	}
}
