using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Drawing1 : MonoBehaviour 
{
	private LineRenderer line;
	private Material material;
	private bool isMousePressed;
	public List<Vector3> pointsList;
	private Vector3 mousePos;

	void Start () 
	{
		material = new Material(Shader.Find("Particles/Additive"));
		line = gameObject.AddComponent<LineRenderer>();
		line.material = material;
		line.SetVertexCount(1);
		line.SetWidth(0.1f, 0.1f);
		line.SetColors(Color.green, Color.green);
		line.useWorldSpace = true;
		isMousePressed = false;
		pointsList = new List<Vector3>();
	}
	
	void Update () 
	{
		if (Input.GetMouseButtonDown(0))
		{
			isMousePressed = true;
			line.SetVertexCount(0);
			pointsList.RemoveRange(0, pointsList.Count);
		}

		if (Input.GetMouseButtonUp(0))
		{
			isMousePressed = false;
		}

		if (isMousePressed)
		{
			mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			mousePos.z = 0;
			if (!pointsList.Contains(mousePos))
			{
				pointsList.Add(mousePos);
				line.SetVertexCount(pointsList.Count);
				line.SetPosition(pointsList.Count - 1, pointsList[pointsList.Count-1]);
			}
		}
	}
}
