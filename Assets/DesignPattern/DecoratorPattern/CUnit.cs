using UnityEngine;
using System.Collections;

namespace DecoratorDemo
{
	public abstract class CUnit : IUnit
	{
		protected string myName;
		protected EUnitType myType;
		protected int mySpeed;
		protected int myLife;
		protected int myPower;

		public string Name
		{
			get { return myName; }
			set { myName = value; }
		}

		public EUnitType Type
		{
			get { return myType; }
			set { myType = value; }
		}

		public int Speed
		{
			get { return mySpeed; }
			set { mySpeed = value; }
		}

		public int Life
		{
			get { return myLife; }
			set { myLife = value; }
		}

		public int Power
		{
			get { return myPower; }
			set { myPower = value; }
		}

		public void Move(int x, int y)
		{
			Debug.Log(string.Format("{0} Move to ({1}, {2}), Speed: {3}", this.Name, x, y, this.Speed));
		}

		public void Attack(int x, int y)
		{
			Debug.Log(string.Format("{0} Attack ({1}, {2}), Power: {3}", this.Name, x, y, this.Power));
		}
	}
}
