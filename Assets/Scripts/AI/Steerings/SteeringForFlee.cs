using UnityEngine;
using System.Collections;

public class SteeringForFlee : Steering 
{
	public GameObject target;
	public float fearDistance = 20;
	private Vector2 desiredVelocity;
	private Vehicle m_vehicle;
	private float maxSpeed;

	void Start()
	{
		m_vehicle = GetComponent<Vehicle>();
		maxSpeed = m_vehicle.maxSpeed;
	}

	public override Vector2 Force()
	{
		if (Vector2.Distance(transform.position, target.transform.position) > fearDistance)
			return Vector2.zero;

		desiredVelocity = (transform.position - target.transform.position).normalized * maxSpeed;

		return (desiredVelocity - m_vehicle.velocity);
	}
}
