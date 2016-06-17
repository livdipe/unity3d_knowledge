using UnityEngine;
using System.Collections;

public class SteeringForEvade : Steering 
{
	public GameObject target;

	private Vector2 desireVelocity;

	private Vehicle m_vehicle;

	void Start()
	{
		m_vehicle = GetComponent<Vehicle>();
	}

	public override Vector2 Force()
	{
		Vector2 toTarget = target.transform.position - transform.position;
		Vehicle targetVehicle = target.GetComponent<Vehicle>();
		float lookaheadTime = toTarget.magnitude / (m_vehicle.maxSpeed + targetVehicle.velocity.magnitude);
		toTarget = transform.position - target.transform.position;
		toTarget += targetVehicle.velocity * lookaheadTime;
		desireVelocity = toTarget.normalized * m_vehicle.maxSpeed;

		return (desireVelocity - m_vehicle.velocity);
	}
}
