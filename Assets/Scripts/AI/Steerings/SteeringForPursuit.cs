using UnityEngine;
using System.Collections;

public class SteeringForPursuit : Steering 
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

		float relativeDirection = Vector2.Dot(transform.forward, target.transform.forward);

		if ((Vector2.Dot(toTarget, transform.forward) > 0) &&
			(relativeDirection < -0.95f))
		{
			desireVelocity = toTarget.normalized * m_vehicle.maxSpeed;
			return (desireVelocity - m_vehicle.velocity);
		}

		Vehicle targetVehicle = target.GetComponent<Vehicle>();
		float lookaheadTime = toTarget.magnitude / (m_vehicle.maxSpeed + targetVehicle.velocity.magnitude);
		toTarget += targetVehicle.velocity * lookaheadTime;
		desireVelocity = toTarget.normalized * m_vehicle.maxSpeed;

		return (desireVelocity - m_vehicle.velocity);
	}
}
