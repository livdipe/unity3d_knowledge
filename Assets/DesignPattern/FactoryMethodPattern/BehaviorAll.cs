using UnityEngine;

namespace FactoryMethodDemo
{
	public class CLandBehavior : CBehavior
	{
		public override void Move (IUnit unit, int x, int y)
		{
			Debug.Log(string.Format("{0} 在陆地上移动到({1}, {2})", unit.Name, x, y));
		}
	}

	public class CSeaBehavior : CBehavior
	{
		public override void Move (IUnit unit, int x, int y)
		{
			Debug.Log(string.Format("{0} 在海里移动到({1}, {2})", unit.Name, x, y));
		}
	}

	public class CAirBehavior : CBehavior
	{
		public override void Move (IUnit unit, int x, int y)
		{
			Debug.Log(string.Format("{0} 在空中移动到({1}, {2})", unit.Name, x, y));
		}
	}
}
