using UnityEngine;
using System.Collections;

public class SteeringForSeparation : Steering
{
	//可接受的距离
	public float comfortDistance = 1;

	//当AI角色与邻居之间的距离过近时的惩罚因子
	public float multiplierInsideComfortDistance = 2;

	private Radar radar;

	void Start()
	{
		radar = gameObject.GetComponent<Radar>();
	}

	public override Vector2 Force()
	{
		Vector2 steeringForce = Vector2.zero;

		foreach(GameObject s in radar.neighbors)
		{
			if ( (s != null) && (s != this.gameObject) )
			{
				Vector2 toNeighbor = transform.position - s.transform.position;

				float length = toNeighbor.magnitude;

				steeringForce += toNeighbor.normalized / length;

				if (length < comfortDistance)
					steeringForce *= multiplierInsideComfortDistance;
			}
		}

		return steeringForce;
	}
}
