using UnityEngine;

namespace StrategyDemo
{
	public class CMachineGun : CWeapon
	{
		public override void Attack (IUnit unit, int x, int y)
		{
			Debug.Log(string.Format("{0} 的机枪攻击 {1}, {2}", unit.Name, x, y));
		}
	}
}
