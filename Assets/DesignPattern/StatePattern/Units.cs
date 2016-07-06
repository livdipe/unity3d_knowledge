using UnityEngine;
using System.Collections;

namespace StateDemo
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

	public class CLvBu : CUnitDecorator
	{
		public CLvBu()
		{
			myName = "吕布";
			mySpeed = 110;
			myLife = 220;
			myPower = 110;
			myType = EUnitType.LvBu;
		}
	}

	public class CWeapon2 : CUnit
	{
		public CWeapon2()
		{
			myName = "青龙偃月刀";
			mySpeed = -10;
			myLife = 0;
			myPower = 50;
			myType = EUnitType.Weapon2;
		}
	}

	public class CWeapon4 : CUnit
	{
		public CWeapon4()
		{
			myName = "方天画戟";
			mySpeed = -10;
			myLife = 0;
			myPower = 60; 
			myType = EUnitType.Weapon4;
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