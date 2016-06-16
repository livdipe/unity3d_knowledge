using UnityEngine;
using System.Collections;

namespace BuilderPatternExample2
{
	public class CarBuildDirector 
	{

		public Car BuildCar1()
		{
			CarBuilder builder = new CarBuilder();
			builder.SetColour("Red");
			builder.SetWheels(4);

			return builder.GetResult();
		}

		public Car BuildCar2()
		{
			CarBuilder builder = new CarBuilder();
			builder.SetColour("Blue");
			builder.SetWheels(3);

			return builder.GetResult();
		}
	}
}
