
namespace NPBehave
{
	public static class Log
	{
		public static void Info(string content)
		{
#if UNITY_EDITOR
			UnityEngine.Debug.Log(content);
#else
            Console.WriteLine("信息：" + content);
#endif
		}

		public static void Error(string content)
		{
#if UNITY_EDITOR
			UnityEngine.Debug.LogError(content);
#else
            Console.WriteLine("错误：" + content);
#endif
		}
	}
}
