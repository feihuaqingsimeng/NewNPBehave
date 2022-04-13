using System;

namespace NPBehave
{
	public abstract class NPValue<T> : NPValueBase, INPValue<T>
	{
		public T value;
		public T Get()
		{
			return value;
		}

		public void Set(INPValue<T> npValue)
		{
			value = npValue.Get();
		}

		public void Set(T value)
		{
			this.value = value;
		}
		
		public override string ToString()
		{
			return value.ToString();
		}
	}
}
