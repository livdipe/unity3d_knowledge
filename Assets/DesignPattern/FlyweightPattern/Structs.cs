using UnityEngine;

namespace FlyweightDemo
{
	public struct SDebris
	{
		public int X;
		public int Y;
		public string Name;
		public EDebrisType Type;

		public void ShowInfo()
		{
			Debug.Log(string.Format("{0} 类型: {1} 位置: ({2}, {3})", Name, Type, X, Y));
		}
	}
}
