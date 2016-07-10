namespace FactoryMethodDemo
{
	public abstract class CWeapon : IWeapon
	{
		public abstract void Attack(IUnit unit, int x, int y);
	}
}
