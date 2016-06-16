using UnityEngine;
using System.Collections;
	
namespace BuilderPatternExample2
{
	public class Car 
	{
		public Car()
		{
		}

		public int Wheels { get; set; }

		public string Colour { get; set; }

		public void Show()
		{
			Debug.Log("Wheels:" + Wheels + " Colour:" + Colour);
		}
	}
}
