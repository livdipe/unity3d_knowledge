using UnityEngine;
using System.Collections;

namespace Unity_5x_AI
{
	public class Steering 
	{
		public float angular;		//角
		public Vector3 linear;
		public Steering()
		{
			angular = 0.0f;
			linear = new Vector3();
		}
	}
}
