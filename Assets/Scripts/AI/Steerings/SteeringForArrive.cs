using UnityEngine;
using System.Collections;

public class SteeringForArrive : Steering 
{
	public GameObject target;
	private Vector2 desiredVelocity;
	private Vehicle m_vehicle;

	enum Deceleration
	{
		slow = 3, 
		normal = 2,
		fast = 1,
	}
//	Deceleration deceleration = Deceleration.fast;

	void Start()
	{
		m_vehicle = GetComponent<Vehicle>();
	}

//	public override Vector2 Force()
//	{
//		Vector2 toTarget = target.transform.position - transform.position;
//
//		float dist = toTarget.magnitude;
//
//		if (dist > 0)
//		{
//			const float decelerationTweaker = 0.3f;
//
//			float speed = dist / decelerationTweaker * (float)deceleration;
//
//			speed = Mathf.Min(speed, m_vehicle.maxSpeed);
//
//			desiredVelocity = toTarget * speed / dist;
//
//			return desiredVelocity - m_vehicle.velocity;
//		}
//
//		return Vector2.zero;
//	}

	public float slowDownDistance = 1;
	public override Vector2 Force()
	{
		Vector2 toTarget = target.transform.position - transform.position;

		Vector2 returnForce;

		float distance = toTarget.magnitude;

		if (distance > slowDownDistance)
		{
			desiredVelocity = toTarget.normalized * m_vehicle.maxSpeed;
			returnForce = desiredVelocity - m_vehicle.velocity;
		}
		else
		{
			desiredVelocity = toTarget - m_vehicle.velocity;
			returnForce = desiredVelocity - m_vehicle.velocity;
		}
		return returnForce;
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere (target.transform.position, slowDownDistance);
	}
}
