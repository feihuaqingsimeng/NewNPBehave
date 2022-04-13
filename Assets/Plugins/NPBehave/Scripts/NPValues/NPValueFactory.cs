using UnityEngine;

namespace NPBehave
{
	public class NPValueFactory
	{
		public static NPValueBase Create<T>(T value)
		{
			NPValueBase npValue = null;
			if (value is bool)
				npValue = new NPBool();
			else if (value is int)
				npValue = new NPInt();
			else if (value is float)
				npValue = new NPFloat();
			else if (value is long)
				npValue = new NPLong();
			else if (value is Vector3)
				npValue = new NPVector3();
			else if (value is string)
				npValue = new NPString();
			if(npValue == null)
			{
				Log.Error($"未注册的类型:{typeof(T)},无法创建NPValueBase");
				return null;
			}
			var i = npValue as INPValue<T>;
			i.Set(value);
			return npValue;
		}
	}
}
