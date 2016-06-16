using UnityEngine;
using System.Collections;

namespace example1
{
	public class Main : MonoBehaviour 
	{
		void Start () 
		{
			Director director = new Director();
			ProgramMonkey monkey = director.getMonkeyLow();
			monkey.show();
			monkey = director.getMonkeyHigh();
			monkey.show();
		}
	}
}
