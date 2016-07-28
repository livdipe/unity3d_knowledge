namespace NetworkDrawing
{
	using UnityEngine;
	using System.Collections;
	using System.Collections.Generic;
	using protocals;
	using System;
	using System.IO;

	public class NetworkDrawing : MonoBehaviour 
	{
		private LineRenderer line;
		private Material material;
		private bool isMousePressed;
		public List<Vector3> pointsList;
		private Vector3 mousePos;
		public static NetworkDrawing _instance;
		SocketClient client;
		public Queue<string> msgQueue = new Queue<string>();

		void Awake()
		{
			_instance = this;
			DontDestroyOnLoad(gameObject);
		}

		public static NetworkDrawing Instance
		{
			get
			{
				return _instance;
			}
		}

		void Start () 
		{
			InitDrawTool();
			InitNetwork();
		}

		void InitDrawTool()
		{
			material = new Material(Shader.Find("Particles/Additive"));
			isMousePressed = false;
			pointsList = new List<Vector3>();
		}

		void InitNetwork()
		{
			client = new SocketClient();
			client.ConnectServer("127.0.0.1", 4444);
		}

		void Send(string msg)
		{
			byte[] bytes = System.Text.Encoding.UTF8.GetBytes(msg);
			client.SendMessage(bytes);
		}

		void UpdateMessage()
		{
			while (msgQueue.Count > 0)
			{
				string msg = msgQueue.Dequeue();
				ProcessData(msg);
			}
		}

		void ProcessData(string msg)
		{
//			Debug.Log("msg:" + msg);
			string[] array = msg.Split(new char[]{','});
			switch(array[0])
			{
			case "id":
				break;
			case "mouseop":
				MouseOperate(int.Parse(array[1]));
				break;
			case "point":
				AddPoint(float.Parse(array[1]), float.Parse(array[2]));
				break;
			case "clear":
				Clear();
				break;
			default:
				break;
			}
		}
		
		void Update () 
		{
			UpdateMessage();

			if (Input.GetMouseButtonDown(0))
			{
				Send(string.Format("mouseop,{0}", (int)MouseOpType.Down));
			}

			if (Input.GetMouseButtonUp(0))
			{
				Send(string.Format("mouseop,{0}", (int)MouseOpType.Up));
			}

			if (isMousePressed)
			{
				mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				mousePos.z = 0;
				Send(string.Format("point,{0},{1}", mousePos.x, mousePos.y));
			}
		}

		enum MouseOpType
		{
			Down = 1,
			Up = 2,
		}

		void MouseOperate(int op)
		{
			//down
			switch((MouseOpType)op)
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

		void AddPoint(float x, float y)
		{
			Vector3 mousePos = new Vector3(x, y, 0);
			if (!pointsList.Contains(mousePos))
			{
				pointsList.Add(mousePos);
				line.SetVertexCount(pointsList.Count);
				line.SetPosition(pointsList.Count - 1, pointsList[pointsList.Count-1]);
			}
		}

		void Clear()
		{
			for (int i = 0;i < lineObjects.Count; i ++)
			{
				Destroy(lineObjects[i]);
			}
			lineObjects.Clear();
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
				Send("clear");
			}
		}
	}
}