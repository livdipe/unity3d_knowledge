using UnityEngine;
using System.Collections;

namespace DecoratorDemo
{
	public class CGuanYu : CUnitDecorator
	{
		public CGuanYu()
		{
			myName = "关羽";
			mySpeed = 100;
			myLife = 200;
			myPower = 100;
			myType = EUnitType.GuanYu;
		}
	}

	public class CWeapon2 : CUnit
	{
		public CWeapon2()
		{
			myName = "青龙偃月刀";
			mySpeed = 0;
			myLife = 0;
			myPower = 50;
			myType = EUnitType.Weapon2;
		}
	}

	public class CEquipper1: CUnit
	{
		public CEquipper1()
		{
			myName = "黄金铠甲";
			mySpeed = -10;
			myLife = 60;
			myPower = 0;
			myType = EUnitType.Equipper1;
		}
	}

	public class CChiTuMa : CUnit
	{
		public CChiTuMa()
		{
			myName = "赤兔马";
			mySpeed = 30;
			myLife = 0;
			myPower = 0;
			myType = EUnitType.ChiTuMa;
		}
	}
}