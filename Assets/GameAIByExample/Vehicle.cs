namespace GameAI
{
	using UnityEngine;
	using System.Collections;

	public class Vehicle : MonoBehaviour 
	{
		public Vector2 m_vPos;
		Vector2 m_vVelocity;
		public Vector2 m_vHeading;
		public Vector2 m_vSide;
		public float m_dMass;
		public float m_dMaxSpeed;
		public float m_dMaxForce;
		public float m_dMaxTurnRate;
		SteeringBehaviors m_pSteering;

		public Transform trTarget;

		void Start () 
		{
			m_pSteering = new SteeringBehaviors(this);
			InvokeRepeating("ChangeTarget", 2, 20);
		}

		void ChangeTarget()
		{
			m_pSteering.m_vTarget = new Vector2(Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f));
			trTarget.position = m_pSteering.m_vTarget;
		}
		
		void Update () 
		{
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

			transform.position = m_vPos;

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
	}
}
