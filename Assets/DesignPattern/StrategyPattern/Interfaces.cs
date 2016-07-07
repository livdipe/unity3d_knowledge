namespace StrategyDemo
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
		int Type { get; set; }
		string Name { get; set; }
		int Speed { get; set; }
		IWeapon Weapon { get; set; }
		IBehavior Behavior { get; set; }
		int CurrentX { get; set; }
		int CurrentY { get; set; }
		void Move(int x, int y);
		void Attack(int x, int y);
	}
}