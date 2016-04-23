using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CanvasTester : MonoBehaviour 
{
	public Canvas canvas;

	void Start()
	{
		Debug.Log(canvas.scaleFactor);
	}
}
