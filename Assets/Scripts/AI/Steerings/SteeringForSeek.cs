using UnityEngine;
using System.Collections;

public class SteeringForSeek : Steering 
{
	public GameObject target;

	private Vector2 desireVelocity;

	private Vehicle m_vehicle;

	private float maxSpeed;

	void Start()
	{
		m_vehicle = GetComponent<Vehicle>();
		maxSpeed = m_vehicle.maxSpeed;
	}

	public override Vector2 Force()
	{
		desireVelocity = (target.transform.position - transform.position).normalized * maxSpeed;
		return (desireVelocity - m_vehicle.velocity);
	}
}
