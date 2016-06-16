using UnityEngine;
using System.Collections;

namespace BuilderPatternExample2
{
	public interface ICarBuilder 
	{
		void SetColour(string colour);

		void SetWheels(int count);

		Car GetResult();
	}
}
