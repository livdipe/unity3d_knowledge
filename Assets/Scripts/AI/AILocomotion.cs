using UnityEngine;
using System.Collections;

public class AILocomotion : Vehicle
{
	private Vector2 moveDistance;
	protected override void Start()
	{
		moveDistance = Vector2.zero;
		base.Start();
	}

	//物理相关操作
	void FixedUpdate()
	{
		//速度
		velocity += acceleration * Time.fixedDeltaTime;

		if (velocity.sqrMagnitude > sqrMaxSpeed)
			velocity = velocity.normalized * maxSpeed;

		moveDistance = velocity * Time.fixedDeltaTime;

		transform.position += new Vector3(moveDistance.x, moveDistance.y, 0);

		//如果速度大于一个阈值（为了防止抖动）
		if (velocity.sqrMagnitude > 0.00001)
		{
			float targetAngle = Mathf.Atan2(-velocity.x, velocity.y) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, targetAngle), damping * Time.deltaTime);
		}
	}
}
