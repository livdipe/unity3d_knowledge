using UnityEngine;
using System.Collections;

namespace Unity_5x_AI
{
	public class Flee : AgentBehaviour
	{
		public override Steering GetSteering ()
		{
			Steering steering = new Steering();
			steering.linear = transform.position - target.transform.position;
			steering.linear.Normalize();
			steering.linear = steering.linear * agent.maxAccel;
			return steering;
		}
	}
}
