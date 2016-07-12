using UnityEngine;
using System.Collections;

public class SteeringForAlignment : Steering 
{
	private Radar radar;

	void Start()
	{
		radar = gameObject.GetComponent<Radar>();
	}

	public override Vector2 Force()
	{
		Vector2 averageDirection = Vector2.zero;

		int neighborCount = 0;

		foreach (GameObject s in radar.neighbors)
		{
			if ( (s != null) && (s != this.gameObject) )
			{
				averageDirection += s.GetComponent<Vehicle>().heading;
				neighborCount ++;
			}
		}

		if (neighborCount > 0)
		{
			averageDirection /= (float)neighborCount;
			averageDirection -= gameObject.GetComponent<Vehicle>().heading;
		}

		return averageDirection;
	}
}
