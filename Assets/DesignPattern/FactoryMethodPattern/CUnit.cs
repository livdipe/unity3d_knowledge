namespace FactoryMethodDemo
{
	public abstract class CUnit : IUnit
	{
		private EUnitType myType;
		private string myName;
		private int mySpeed;
		protected IWeapon myWeapon;
		protected IBehavior myBehavior;
		private int myCurrentX;
		private int myCurrentY;

		public EUnitType Type
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

		public IWeapon Weapon
		{
			get { return myWeapon; }
		}

		public IBehavior Behavior
		{
			get { return myBehavior; }
		}

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
