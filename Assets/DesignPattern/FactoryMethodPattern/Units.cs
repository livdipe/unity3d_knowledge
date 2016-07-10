namespace FactoryMethodDemo
{
	public class CSoldier : CUnit
	{
		public CSoldier()
		{
			Type = EUnitType.Soldier;
			Speed = 15;
			myBehavior = new CLandBehavior();
			myWeapon = new CMachineGun();
		}
	}

	public class CDistroyer : CUnit
	{
		public CDistroyer()
		{
			Type = EUnitType.Distroyer;
			Speed = 40;
			myBehavior = new CSeaBehavior();
			myWeapon = new CCannon();
		}
	}

	public class CFighter : CUnit
	{
		public CFighter()
		{
			Type = EUnitType.Fighter;
			Speed = 100;
			myBehavior = new CAirBehavior();
			myWeapon = new CMachineGun();
		}
	}
}