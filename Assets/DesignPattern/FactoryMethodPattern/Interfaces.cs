namespace FactoryMethodDemo
{
	public interface IWeapon
	{
		void Attack(IUnit unit, int x, int y);
	}

	public interface IBehavior
	{
		void Move(IUnit unit, int x, int y);
	}

	public interface IUnit
	{
		EUnitType Type { get; set; }
		string Name { get; set; }
		int Speed { get; set; }
		IWeapon Weapon { get; }
		IBehavior Behavior { get; }
		int CurrentX { get; set; }
		int CurrentY { get; set; }
		void Move(int x, int y);
		void Attack(int x, int y);
	}
}
