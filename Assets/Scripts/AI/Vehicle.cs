using UnityEngine;
using System.Collections;

public class Vehicle : MonoBehaviour 
{
	//操控行为列表
	private Steering[] steerings;
	//最大速度
	public float maxSpeed = 10;
	//能施加的最大力
	public float maxForce = 100;

	protected float sqrMaxSpeed;

	public float mass = 1;

	public Vector2 velocity;

	public Vector2 heading;

	public float damping = 0.9f;

	public float computeInterval = 0.2f;

	private Vector2 steeringForce;

	protected Vector2 acceleration;

	private float timer;

	protected virtual void Start()
	{
		steeringForce = Vector2.zero;
		sqrMaxSpeed = maxSpeed * maxSpeed;
		timer = 0;
		steerings = GetComponents<Steering>();
	}

	void Update()
	{
		timer += Time.deltaTime;
		steeringForce = Vector2.zero;
		if (timer > computeInterval)
		{
			for (int i = 0; i < steerings.Length; i++)
			{
				if (steerings[i].enabled)
				{
					steeringForce += steerings[i].Force() * steerings[i].weight;
				}
			}
			steeringForce = Vector2.ClampMagnitude(steeringForce, maxForce);
			//力除以质量，求出加速度
			acceleration = steeringForce / mass;
			timer = 0;
		}
	}

}
