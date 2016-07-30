namespace NetworkDrawing
{
	using UnityEngine;
	using System.Collections;
	using System.Collections.Generic;

	public class PlayerView : MonoBehaviour 
	{
		public Player controller;
		private Vector3 mousePos;
		Vector3 lastPos = Vector3.zero;

		void Update()
		{
			if (controller == null)
			{
				return;
			}

			if (Input.GetMouseButtonDown(0))
			{
				NetworkDrawing.Instance.Send(string.Format("mouseop,{0}", (int)MouseOpType.Down));
			}

			if (Input.GetMouseButtonUp(0))
			{
				controller.isMousePressed = false;
				NetworkDrawing.Instance.Send(string.Format("mouseop,{0}", (int)MouseOpType.Up));
			}

			if (controller.isMousePressed)
			{
				mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				mousePos.x = (float)System.Math.Round(mousePos.x, 2);
				mousePos.y = (float)System.Math.Round(mousePos.y, 2);
				mousePos.z = 0;
				if (mousePos != lastPos)
				{
					NetworkDrawing.Instance.Send(string.Format("point,{0},{1}", mousePos.x, mousePos.y));

					lastPos = mousePos;
				}
			}
		}

		void OnGUI()
		{
			if (GUI.Button(new Rect(Screen.width - 60, 0, 60, 40), "Clear"))
			{
				NetworkDrawing.Instance.Send("clear");
			}
		}

		void OnDestroy()
		{
//			NetworkDrawing.Instance.RemovePlayer(PlayerID);
		}
	}

	public class Player
	{
		private LineRenderer line;
		private Material material;
		public bool isMousePressed;
		public List<Vector3> pointsList;
		List<GameObject> lineObjects = new List<GameObject>();
		public Color color;
		private int playerID;
		public int PlayerID
		{
			get
			{
				return playerID;
			}
			set
			{
				playerID = value;
			}
		}

		public void Init()
		{
			material = new Material(Shader.Find("Particles/Additive"));
			isMousePressed = false;
			pointsList = new List<Vector3>();
			color = NetworkDrawing.Instance.RandomColor();
			NetworkDrawing.Instance.Send(string.Format("color,{0},{1},{2},{3}", color.r, color.g, color.b,color.a));
		}

		LineRenderer NewLine()
		{
			GameObject objLine = new GameObject("Line");
			lineObjects.Add(objLine);
			LineRenderer newLine = objLine.AddComponent<LineRenderer>();
			newLine.material = material;
			newLine.SetVertexCount(0);
			newLine.SetWidth(0.1f, 0.1f);
			newLine.SetColors(color, color);
			newLine.useWorldSpace = false;

			return newLine;
		}

		public void SetColor(Color c)
		{
			color = c;
		}

		public void MouseOperate(int op)
		{
			switch((MouseOpType)op)
			{
			//down
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

		public void AddPoint(float x, float y)
		{
			Vector3 mousePos = new Vector3(x, y, 0);
			if (!pointsList.Contains(mousePos))
			{
				pointsList.Add(mousePos);
				line.SetVertexCount(pointsList.Count);
				line.SetPosition(pointsList.Count - 1, pointsList[pointsList.Count-1]);
			}
		}

		public void Clear()
		{
			for (int i = 0;i < lineObjects.Count; i ++)
			{
				GameObject.Destroy(lineObjects[i]);
			}
			lineObjects.Clear();
		}
	}
}
