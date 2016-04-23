using UnityEngine;
using System.Collections;

public abstract class Steering : MonoBehaviour 
{
	public float weight = 1;

	void Start()
	{
	}

	void Update()
	{
	}

	public virtual Vector2 Force()
	{
		return Vector2.zero;
	}
}
