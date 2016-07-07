using UnityEngine;
using System.Collections;

namespace StrategyDemo
{
	public class Main : MonoBehaviour 
	{
		void Start () 
		{
			CUnit ryan = new CUnit();
			ryan.Name = "Ryan";
			ryan.Behavior = new CLandBehavior();
			ryan.Weapon = new CMachineGun();
			ryan.Move(100, 100);
			ryan.Attack(150, 150);
		}
	}
}
