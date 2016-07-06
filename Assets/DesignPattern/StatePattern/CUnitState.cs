using UnityEngine;
using System.Collections;

namespace StateDemo
{
	public abstract class CUnitState : IUnitState
	{
		protected string myDescription;

		public string Description
		{
			get { return myDescription; }
			set { myDescription = value; }
		}

		public abstract int ChangeValue(int val);
	}
}
