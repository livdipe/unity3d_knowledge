namespace StateDemo
{
	public interface IUnitState
	{
		string Description { get; set; } //状态描述
		int ChangeValue(int val); //改变原值
	}

	public interface IUnit
	{
		string Name { get; set; }
		EUnitType Type { get; set; }
		int Speed { get; set; }
		int Life { get; set; }
		int Power { get; set; }
		void Move(int x, int y);
		void Attack(int x, int y);

		IUnitState State { get; set; } //单位状态
	}
}
