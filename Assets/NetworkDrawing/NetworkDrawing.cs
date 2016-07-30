namespace NetworkDrawing
{
	using UnityEngine;
	using System.Collections;
	using System.Collections.Generic;
	using protocals;
	using System;
	using System.IO;

	public enum MouseOpType
	{
		Down = 1,
		Up = 2,
	}

	public class NetworkDrawing : MonoBehaviour 
	{
		public static NetworkDrawing _instance;
		SocketClient client;
		public Queue<string> msgQueue = new Queue<string>();
		private Dictionary<int, Player> players = new Dictionary<int, Player>();
		public PlayerView view;
		public Color[] brushColors = new Color[]{Color.blue, 
			Color.cyan, Color.gray, 
			Color.green, Color.grey, 
			Color.magenta, Color.red, 
			Color.yellow};

		void Awake()
		{
			_instance = this;
			UnityEngine.Random.seed = System.DateTime.Now.Millisecond;
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
			ScreenDebuger.Instance.AddMsg("NetworkDrawing Start");
			InitNetwork();
			view = gameObject.AddComponent<PlayerView>();
		}

		void InitNetwork()
		{
			ScreenDebuger.Instance.AddMsg("InitNetwork");
			client = new SocketClient();
			client.ConnectServer("192.168.0.101", 4444);
		}

		void Update () 
		{
			UpdateMessage();
		}

		void UpdateMessage()
		{
			while (msgQueue.Count > 0)
			{
				string msg = msgQueue.Dequeue();
				ProcessData(msg);
			}
		}

		public void Send(string msg)
		{
			byte[] bytes = System.Text.Encoding.UTF8.GetBytes(msg);
			client.SendMessage(bytes);
		}

		void ProcessData(string msg)
		{
//			Debug.Log("msg:" + msg);
//			msg = msg.Substring(0, msg.IndexOf('\r'));
			Player player = null;
			string[] array = msg.Split(new char[]{','});
			switch(array[0])
			{
			case "loginok":
				int playerID = int.Parse(array[1]);
				player = AddPlayer(playerID);
				if (player != null)
				{
					view.controller = player;
				}
				break;
			case "newclient":
				AddPlayer(int.Parse(array[1]));
				break;
			case "color":
				player = GetPlayer(int.Parse(array[6]));
				if (player != null)
				{
					player.SetColor(new Color(float.Parse(array[1]), float.Parse(array[2]),float.Parse(array[3]),float.Parse(array[4])));
				}
				break;
			case "mouseop":
				player = GetPlayer(int.Parse(array[3]));
				if (player != null)
				{
					player.MouseOperate(int.Parse(array[1]));
				}
				break;
			case "point":
				player = GetPlayer(int.Parse(array[4]));
				if (player != null)
				{
					player.AddPoint(float.Parse(array[1]), float.Parse(array[2]));
				}
				break;
			case "clear":
				player = GetPlayer(int.Parse(array[2]));
				if (player != null)
				{
					player.Clear();
				}
				break;
			default:
				break;
			}
		}

		Player AddPlayer(int id)
		{
			if (!players.ContainsKey(id))
			{
				Player player = new Player();
				player.PlayerID = id;
				player.Init();
				players.Add(id, player);
				return player;
			}
			return null;
		}

		public void RemovePlayer(int id)
		{
			if (players.ContainsKey(id))
			{
				players.Remove(id);
			}
		}

		Player GetPlayer(int id)
		{
			if (players.ContainsKey(id))
			{
				return players[id];
			}
			else
			{
				return null;
			}
		}

		public Color RandomColor()
		{
			int idx = UnityEngine.Random.Range(0, brushColors.Length);
			return brushColors[idx];
		}
	}
}