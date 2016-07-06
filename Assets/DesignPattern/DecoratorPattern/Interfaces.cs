using UnityEngine;
using System.Collections;

namespace DecoratorDemo
{
	public interface IUnit
	{
		string Name { get; set; }
		EUnitType Type { get; set; }
		int Speed { get; set; }
		int Life { get; set; }
		int Power { get; set; }
		void Move(int x, int y);
		void Attack(int x, int y);
	}
}
