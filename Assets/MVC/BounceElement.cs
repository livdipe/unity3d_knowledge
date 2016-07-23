using UnityEngine;
using System.Collections;

public class BounceElement : MonoBehaviour 
{
	public BounceApplication app
	{
		get
		{
			return GameObject.FindObjectOfType<BounceApplication>();
		}
	}
}
