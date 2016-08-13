namespace GameAI
{
	using UnityEngine;
	using System.Collections;
	using System.Collections.Generic;

	public class SteeringBehaviors 
	{
		private Vehicle m_pVehicle;

		public SteeringBehaviors(Vehicle vehicle)
		{
			m_pVehicle = vehicle;
		}

		public Vector2 Calculate()
		{
			Vector2 SteeringForce = Vector2.zero;
			if (m_pVehicle.isSeekOn)
			{
				SteeringForce += Seek(AIManager.Instance.trTarget.position);
			}

			if (m_pVehicle.isSeparation)
			{
				SteeringForce += Separation(m_pVehicle.neighbors);
			}

			return SteeringForce;
		}

		public Vector2 Seek(Vector2 TargetPos)
		{
			Vector2 DesiredVelocity = (TargetPos - m_pVehicle.m_vPos).normalized * m_pVehicle.MaxSpeed();

			return DesiredVelocity - m_pVehicle.Velocity();
		}

		public Vector2 Separation(List<Vehicle> neighbors)
		{
			Vector2 SteeringForce = Vector2.zero;

			for (int i = 0; i < neighbors.Count; i++)
			{
				if (neighbors[i] != m_pVehicle)
				{
					Vector2 ToAgent = m_pVehicle.Pos() - neighbors[i].Pos();

					float magnitude = ToAgent.magnitude;
					if (magnitude == 0)
					{
						SteeringForce += Vector2.zero;
					}
					else
					{
						SteeringForce += ToAgent.normalized / ToAgent.magnitude;
					}
				}
			}

			return SteeringForce;
		}
	}
}
