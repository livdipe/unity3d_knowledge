using System;

namespace FactoryMethodDemo
{
	public class CLandUnitCreator : CUnitCreator
	{
		public override IUnit CreateUnit (EUnitType unitType)
		{
			switch (unitType)
			{
			case EUnitType.Soldier: 
				return new CSoldier(); 
			default:
				throw new Exception("指定了错误的陆地单位类型");
			}
		}
	}

	public class CAirUnitCreator : CUnitCreator
	{
		public override IUnit CreateUnit (EUnitType unitType)
		{
			switch (unitType)
			{
			case EUnitType.Fighter: 
				return new CFighter(); 
			default:
				throw new Exception("指定了错误的空中单位类型");
			}
		}
	}

	public class CSeaUnitCreator : CUnitCreator
	{
		public override IUnit CreateUnit (EUnitType unitType)
		{
			switch (unitType)
			{
			case EUnitType.Distroyer: 
				return new CDistroyer(); 
			default:
				throw new Exception("指定了错误的海中单位类型");
			}
		}
	}
}
