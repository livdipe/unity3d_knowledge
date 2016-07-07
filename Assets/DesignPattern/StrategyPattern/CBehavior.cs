namespace StrategyDemo
{
	public abstract class CBehavior : IBehavior
	{
		public abstract void Move(IUnit unit, int x, int y);
	}
}
