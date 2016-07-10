using UnityEngine;

namespace FactoryMethodDemo
{
	public class CMachineGun : CWeapon
	{
		public override void Attack (IUnit unit, int x, int y)
		{
			Debug.Log(string.Format("{0} 用机枪攻击({1}, {2})", unit.Name, x, y));
		}
	}

	public class CCannon : CWeapon
	{
		public override void Attack (IUnit unit, int x, int y)
		{
			Debug.Log(string.Format("{0} 用大炮攻击({1}, {2})", unit.Name, x, y));
		}
	}
}
