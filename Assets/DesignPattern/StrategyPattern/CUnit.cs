namespace StrategyDemo
{
	public class CUnit : IUnit
	{
		private int myType;
		private string myName;
		private int mySpeed, myCurrentX, myCurrentY;
		private IBehavior myBehavior;
		private IWeapon myWeapon;

		public int Type
		{
			get { return myType; }
			set { myType = value; }
		}

		public string Name
		{
			get { return myName; }
			set { myName = value; }
		}

		public int Speed
		{
			get { return mySpeed; }
			set { mySpeed = value; }
		}

		public int CurrentX
		{
			get { return myCurrentX; }
			set { myCurrentX = value; }
		}

		public int CurrentY
		{
			get { return myCurrentY; }
			set { myCurrentY = value; }
		}

		public IBehavior Behavior
		{
			get { return myBehavior; }
			set { myBehavior = value; }
		}

		public IWeapon Weapon
		{
			get { return myWeapon; }
			set { myWeapon = value; }
		}

		//方法
		public void Move(int x, int y)
		{
			myBehavior.Move(this, x, y);
		}

		public void Attack(int x, int y)
		{
			myWeapon.Attack(this, x, y);
		}
	}
}
