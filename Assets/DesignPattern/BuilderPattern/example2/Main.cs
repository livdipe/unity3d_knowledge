using UnityEngine;
using System.Collections;

namespace BuilderPatternExample2
{
	public class Main : MonoBehaviour 
	{
		void Start()
		{
			CarBuildDirector director = new CarBuildDirector();
			Car car1 = director.BuildCar1();
			Car car2 = director.BuildCar2();
			car1.Show();
			car2.Show();
		}
	}
}
