using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using protocals;

public class Drawing2 : MonoBehaviour 
{
	private LineRenderer line;
	private Material material;
	private bool isMousePressed;
	public List<Vector3> pointsList;
	private Vector3 mousePos;
	public static Drawing2 _instance;

	void Awake()
	{
		_instance = this;
		DontDestroyOnLoad(gameObject);
	}

	public static Drawing2 Instance
	{
		get
		{
			return _instance;
		}
	}

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
//			isMousePressed = true;
//			line = NewLine();
//			pointsList.RemoveRange(0, pointsList.Count);

			MouseOp m = new MouseOp();
			m.op = (int)MouseOpType.Down;
			NetworkManager.Instance.SendMouseOp(m);
		}

		if (Input.GetMouseButtonUp(0))
		{
//			isMousePressed = false;
			MouseOp m = new MouseOp();
			m.op = (int)MouseOpType.Up;
			NetworkManager.Instance.SendMouseOp(m);
		}

		if (isMousePressed)
		{
			mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			mousePos.z = 0;
			Point p = new Point();
			p.x = mousePos.x;
			p.y = mousePos.y;
			NetworkManager.Instance.SendPoint(p);
//			if (!pointsList.Contains(mousePos))
//			{
//				pointsList.Add(mousePos);
//				line.SetVertexCount(pointsList.Count);
//				line.SetPosition(pointsList.Count - 1, pointsList[pointsList.Count-1]);
//			}
		}
	}

	enum MouseOpType
	{
		Down = 1,
		Up = 2,
	}

	public void MouseOperate(MouseOp mouse)
	{
		//down
		switch((MouseOpType)mouse.op)
		{
		case MouseOpType.Down:
			isMousePressed = true;
			line = NewLine();
			pointsList.RemoveRange(0, pointsList.Count);
			break;

		case MouseOpType.Up:
			isMousePressed = false;
			break;
		}
	}

	public void AddPoint(Point p)
	{
		Vector3 mousePos = new Vector3(p.x, p.y, 0);
		if (!pointsList.Contains(mousePos))
		{
			pointsList.Add(mousePos);
			line.SetVertexCount(pointsList.Count);
			line.SetPosition(pointsList.Count - 1, pointsList[pointsList.Count-1]);
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
		newLine.SetColors(Color.green, Color.green);
		newLine.useWorldSpace = true;

		return newLine;
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
