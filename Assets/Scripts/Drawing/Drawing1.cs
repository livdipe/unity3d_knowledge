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
		isMousePressed = false;
		pointsList = new List<Vector3>();
	}
	
	void Update () 
	{
		if (Input.GetMouseButtonDown(0))
		{
			isMousePressed = true;
			line = NewLine();
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

	LineRenderer NewLine()
	{
		GameObject objLine = new GameObject("Line");
		lineObjects.Add(objLine);
		LineRenderer newLine = objLine.AddComponent<LineRenderer>();
		newLine.material = material;
		newLine.SetVertexCount(0);
		newLine.SetWidth(0.1f, 0.1f);
//		newLine.SetColors(Color.green, Color.green);
		Color c = RandomColor();
		newLine.SetColors(c, c);
		newLine.useWorldSpace = true;

		return newLine;
	}

	Color RandomColor()
	{
		return new Color(Random.Range(0, 1.0f), Random.Range(0, 1.0f), Random.Range(0, 1.0f), 1);
	}

	List<GameObject> lineObjects = new List<GameObject>();
	void OnGUI()
	{
		if (GUI.Button(new Rect(0, 0, 60, 40), "Clear"))
		{
			for (int i = 0;i < lineObjects.Count; i ++)
			{
				Destroy(lineObjects[i]);
			}
			lineObjects.Clear();
		}
	}
}
