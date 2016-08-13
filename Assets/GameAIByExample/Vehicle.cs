namespace GameAI
{
	using UnityEngine;
	using System.Collections;
	using System.Collections.Generic;

	public class Vehicle : MonoBehaviour 
	{
		public Vector2 m_vPos;
		Vector2 m_vVelocity;
		public Vector2 m_vHeading;
		public Vector2 m_vSide;
		public float m_dMass;
		public float m_dMaxSpeed;
		public float m_dMaxForce;
		float m_dMaxTurnRate = 20;
		public float radius = 2;
		SteeringBehaviors m_pSteering;


//		public Transform trTarget;
		public bool isSeekOn = false;
		public bool isSeparation = true;
		float worldRadius = 5f;
		public List<Vehicle> neighbors = new List<Vehicle>();

		void Start () 
		{
			m_pSteering = new SteeringBehaviors(this);
			m_vPos = transform.position;
			Debug.Log(new Vector2(3,4).magnitude);
			Debug.Log(transform.TransformPoint(transform.position));
		}

		void TagNeighbors()
		{
			neighbors.Clear();
			for (int i = 0; i < AIManager.Instance.players.Length; i++)
			{
				Vector2 to = AIManager.Instance.players[i].Pos() - Pos();

				float range = radius * AIManager.Instance.players[i].radius;

				if (AIManager.Instance.players[i] != this && (to.sqrMagnitude < range * range))
				{
					neighbors.Add(AIManager.Instance.players[i]);
				}
			}
		}


		void Update () 
		{
//			if (trTarget == null)
//			{
//				return ;
//			}
			TagNeighbors();
//			Vector2 SteeringForce = m_pSteering.Seek(trTarget.position);
			Vector2 SteeringForce = m_pSteering.Calculate();

			//加速度 ＝ 力 / 质量
			Vector2 acceleration = SteeringForce / m_dMass;

			//更新速度
			m_vVelocity += acceleration * Time.deltaTime;

			if (m_vVelocity.magnitude > m_dMaxSpeed)
			{
				m_vVelocity = m_vVelocity.normalized * m_dMaxSpeed;
			}

			//更新位置
			m_vPos += m_vVelocity * Time.deltaTime;

			if (m_vVelocity.sqrMagnitude > 0.0000001f)
			{
				m_vHeading = m_vVelocity.normalized;

				m_vSide = Perp(m_vHeading); 
			}

			if (m_vPos.x > worldRadius) 
				m_vPos.x = -worldRadius;

			if (m_vPos.y > worldRadius)
				m_vPos.y = -worldRadius;

			if (m_vPos.x < -worldRadius)
				m_vPos.x = worldRadius;

			if (m_vPos.y < -worldRadius)
				m_vPos.y = worldRadius;

			transform.position = m_vPos;

			float targetAngle = Mathf.Atan2(-m_vSide.y, m_vSide.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, targetAngle), m_dMaxTurnRate * Time.deltaTime);
		}

		Vector2 Perp(Vector2 v)
		{
			return new Vector2(-v.y, v.x);
		}

		public float MaxSpeed()
		{
			return m_dMaxSpeed;
		}

		public Vector2 Velocity()
		{
			return m_vVelocity;
		}

		public Vector2 Pos()
		{
			return new Vector2(transform.position.x, transform.position.y);
		}
	}
}
