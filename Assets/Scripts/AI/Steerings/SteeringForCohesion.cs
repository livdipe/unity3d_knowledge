using UnityEngine;
using System.Collections;

public class SteeringForCohesion : Steering 
{
	private Vector2 desiredVelocity;
	private Vehicle m_vehicle;
	private float maxSpeed;
	private Radar radar;

	void Start()
	{
		m_vehicle = gameObject.GetComponent<Vehicle>();
		maxSpeed = m_vehicle.maxSpeed;
		radar = gameObject.GetComponent<Radar>();
	}

	public override Vector2 Force()
	{
		Vector2 steeringForce = Vector2.zero;

		Vector3 centerOfMass = Vector3.zero;

		int neighborCount = 0;

		foreach(GameObject s in radar.neighbors)
		{
			if ( (s != null) && (s != this.gameObject) )
			{
				centerOfMass += s.transform.position;
				neighborCount ++;
			}
		}
		if (neighborCount > 0)
		{
			centerOfMass /= (float)neighborCount;
			Vector2 to = centerOfMass - transform.position;
			desiredVelocity = to.normalized * maxSpeed;
			steeringForce = desiredVelocity - m_vehicle.velocity;
		}

		return steeringForce;
	}
}
