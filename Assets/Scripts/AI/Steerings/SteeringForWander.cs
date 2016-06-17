using UnityEngine;
using System.Collections;

public class SteeringForWander : Steering 
{
	//徘徊半径，即Wander圈的半径
	public float wanderRadius = 2;
	//徘徊距离, 即wander圈凸出在AI角色前面的距离
	public float wanderDistance;
	//每秒加到目标的随机位移的最大值
	public float wanderJitter;

	private Vector2 desireVelocity;

	private Vehicle m_vehicle;

	private Vector2 circleTarget;
	private Vector2 wanderTarget;

	void Start()
	{
		m_vehicle = GetComponent<Vehicle>();
		circleTarget = new Vector2(wanderRadius * 0.707f, wanderRadius * 0.707f);
	}

	public override Vector2 Force()
	{
		Vector2 randomDisplacement = new Vector2((Random.value - 0.5f) * 2 * wanderJitter,
			(Random.value - 0.5f) * 2 * wanderJitter);

		circleTarget += randomDisplacement;

		circleTarget = wanderRadius * circleTarget.normalized;

		wanderTarget = m_vehicle.velocity.normalized * wanderDistance + circleTarget;

		desireVelocity = wanderTarget.normalized * m_vehicle.maxSpeed;

		return (desireVelocity - m_vehicle.velocity);

	}
}
