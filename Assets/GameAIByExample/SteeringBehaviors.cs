namespace GameAI
{
	using UnityEngine;
	using System.Collections;

	public class SteeringBehaviors 
	{
		private Vehicle m_pVehicle;
		public Vector2 m_vTarget;

		public SteeringBehaviors(Vehicle vehicle)
		{
			m_pVehicle = vehicle;
		}

		public Vector2 Calculate()
		{
			return Seek(m_vTarget);
		}

		public Vector2 Seek(Vector2 TargetPos)
		{
			Vector2 DesiredVelocity = (TargetPos - m_pVehicle.m_vPos).normalized * m_pVehicle.MaxSpeed();

			return DesiredVelocity - m_pVehicle.Velocity();
		}
	}
}
