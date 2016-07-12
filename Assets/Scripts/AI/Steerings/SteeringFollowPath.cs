using UnityEngine;
using System.Collections;

public class SteeringFollowPath : Steering 
{

	public GameObject[] waypoints = new GameObject[4];

	private Transform target;

	//当前的路点
	public int currentNode;

	//与路点的距离小于这个值时，认为已经到达，可以向下一个路点出发
	private float arriveDistance;
	private float sqrArriveDistance;

	//路点的数量
	private int numberOfNodes;

	//操控力
	private Vector3 force;

	//预期速度
	private Vector2 desireVelocity;

	private Vehicle m_vehicle;

	private float maxSpeed;
	//当与目标小于这个距离时，开始减速
	public float slowDownDistance;

	void Start()
	{
		numberOfNodes = waypoints.Length;
		m_vehicle = GetComponent<Vehicle>();
		maxSpeed = m_vehicle.maxSpeed;
		currentNode = 0;
		target = waypoints[currentNode].transform;
		arriveDistance = 1.0f;
		sqrArriveDistance = arriveDistance * arriveDistance;
	}

	public override Vector2 Force()
	{
		force = Vector3.zero;

		Vector2 dist = target.transform.position - transform.position;

		if (currentNode == numberOfNodes - 1)
		{
			if (dist.magnitude > slowDownDistance)
			{
				desireVelocity = dist.normalized * maxSpeed;
				force = desireVelocity - m_vehicle.velocity;
			}
			else
			{
				desireVelocity = dist - m_vehicle.velocity;
				force = desireVelocity - m_vehicle.velocity;
			}
		}
		else
		{
			if (dist.sqrMagnitude < sqrArriveDistance)
			{
				currentNode ++;
				target = waypoints[currentNode].transform;
			}
			desireVelocity = dist.normalized * maxSpeed;
			force = desireVelocity - m_vehicle.velocity;
		}

		return force;
	}
}
