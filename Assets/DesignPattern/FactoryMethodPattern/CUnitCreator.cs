namespace FactoryMethodDemo
{
	public abstract class CUnitCreator
	{
		public abstract IUnit CreateUnit(EUnitType unitType);
	}
}
