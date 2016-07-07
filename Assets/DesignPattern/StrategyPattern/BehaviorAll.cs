using UnityEngine;

namespace StrategyDemo
{
	public class CLandBehavior : CBehavior
	{
		public override void Move (IUnit unit, int x, int y)
		{
			Debug.Log(string.Format("{0} 移动到 {1}, {2}", unit.Name, x, y));
		}
	}
}
