using UnityEngine;
using System.Collections;

namespace DecoratorDemo
{
	public class Main : MonoBehaviour 
	{
		void Start () 
		{
			CGuanYu guanYu = new CGuanYu();
			guanYu.ShowInfo();
			//创建装备
			IUnit weapon2 = new CWeapon2();
			IUnit equipper1 = new CEquipper1();
			IUnit chiTuMa = new CChiTuMa();
			//
			Debug.Log("用上青龙偃月刀");
			guanYu.AddUnit(weapon2);
			guanYu.ShowInfo();

			Debug.Log("穿上黄金铠甲");
			guanYu.AddUnit(equipper1);
			guanYu.ShowInfo();

			Debug.Log("骑上赤兔马");
			guanYu.AddUnit(chiTuMa);
			guanYu.ShowInfo();
		}
	}
}
