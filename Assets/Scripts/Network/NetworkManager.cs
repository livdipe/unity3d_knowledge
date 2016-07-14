using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using protocals;
using System;
using System.IO;

public class NetworkManager : MonoBehaviour 
{
	static NetworkManager _instance;
	SocketClient client;
	public Queue<byte[]> msgQueue = new Queue<byte[]>();

	void Awake()
	{
		_instance = this;
		DontDestroyOnLoad(_instance.gameObject);
	}

	public static NetworkManager Instance
	{
		get
		{
			return _instance;
		}
	}

	void Start()
	{
		client = new SocketClient();
		client.OnRegister();
		client.SendConnect();
	}

	void Update()
	{
		while (msgQueue.Count > 0)
		{
			byte[] bytes = msgQueue.Dequeue();
			ProcessData(bytes);
		}
	}

	public void SendPoint(Point msg)
	{
		byte[] bytes = Serialization.Serialize<Point>(msg);
		MemoryStream ms = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(ms);
		writer.Write((int)ProtocalType.Point);
		writer.Write(bytes);
		client.SendMessage(ms.ToArray());
		writer.Close();
		ms.Close();
	}

	public void SendMouseOp(MouseOp msg)
	{
		byte[] bytes = Serialization.Serialize<MouseOp>(msg);
		MemoryStream ms = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(ms);
		writer.Write((int)ProtocalType.MouseOp);
		writer.Write(bytes);
		client.SendMessage(ms.ToArray());
		writer.Close();
		ms.Close();
	}

	void ProcessData(byte[] bytes)
	{
		byte[] bytes1 = new byte[4];
		byte[] bytes2 = new byte[bytes.Length - 4];
		Array.Copy(bytes, 0, bytes1, 0, 4);
		Array.Copy(bytes, 4, bytes2, 0, bytes.Length - 4);
		int id = BitConverter.ToInt32(bytes1, 0);
		switch((ProtocalType)id)
		{
		case ProtocalType.Point:
			Point p = Serialization.Deserialize<Point>(bytes2);
			Drawing2.Instance.AddPoint(p);
			break;
		case ProtocalType.MouseOp:
			MouseOp m = Serialization.Deserialize<MouseOp>(bytes2);
			Drawing2.Instance.MouseOperate(m);
			break;
		}
	}

	void OnDestroy()
	{
		client.OnRemove();
	}

	enum ProtocalType
	{
		Point = 1,
		MouseOp = 2,
	}
}
