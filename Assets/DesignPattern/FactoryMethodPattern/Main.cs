using UnityEngine;
using System.Collections;

namespace FactoryMethodDemo
{
	public class Main : MonoBehaviour 
	{
		void Start () 
		{
			CLandUnitCreator landUnitCreator = new CLandUnitCreator();
			IUnit soldier = landUnitCreator.CreateUnit(EUnitType.Soldier);
			soldier.Name = "瑞恩";
			soldier.Move(100, 100);
			soldier.Attack(150, 150);
		}
	}
}
