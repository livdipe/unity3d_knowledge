using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

public class SocketClient
{
	private TcpClient client = null;
	private NetworkStream outStream = null;
	private MemoryStream memStream;
	private BinaryReader reader;

	private const int MAX_READ = 8192;
	private byte[] byteBuffer = new byte[MAX_READ];

	public SocketClient()
	{
	}

	public void OnRegister()
	{
		memStream = new MemoryStream();
		reader = new BinaryReader(memStream);
	}

	public void OnRemove()
	{
		this.Close();
		reader.Close();
		memStream.Close();
	}

	public void Close()
	{
		if (client != null)
		{
			if (client.Connected)
			{
				client.Close();
			}
			client = null;
		}
	}

	public void SendConnect()
	{
		ConnectServer("127.0.0.1", 2222);
	}

	public void SendMessage(byte[] bytes)
	{
		WriteMessage(bytes);
	}

	void ConnectServer(string host, int port)
	{
		client = null;
		client = new TcpClient();
		client.SendTimeout = 1000;
		client.ReceiveTimeout = 1000;
		client.NoDelay = true;
		try
		{
			client.BeginConnect(host, port, new AsyncCallback(OnConnect), null);
		}
		catch(Exception e)
		{
			Close();
			Debug.LogError(e.Message);
		}
	}

	void OnConnect(IAsyncResult asr)
	{
		outStream = client.GetStream();
		client.GetStream().BeginRead(byteBuffer, 0, MAX_READ, new AsyncCallback(OnRead), null);
	}

	void OnRead(IAsyncResult  asr)
	{
		int bytesRead = 0;
		try
		{
			lock(client.GetStream())
			{
				bytesRead = client.GetStream().EndRead(asr);
			}

			if (bytesRead < 1)
			{
				OnDisconnected("bytesRead < 1");
				return ;
			}
			//分析数据包内容，抛给逻辑层
			OnReceive(byteBuffer, bytesRead);
			lock(client.GetStream())
			{
				Array.Clear(byteBuffer, 0, byteBuffer.Length);
				client.GetStream().BeginRead(byteBuffer, 0, MAX_READ, new AsyncCallback(OnRead), null);
			}
		}
		catch(Exception e)
		{
			OnDisconnected(e.Message);
		}
	}

	void OnReceive(byte[] bytes, int length)
	{
		memStream.Seek(0, SeekOrigin.End);
		memStream.Write(bytes, 0, length);
		memStream.Seek(0, SeekOrigin.Begin);
		while (RemainingBytes() > 2)
		{
			ushort messageLen = reader.ReadUInt16();
			if (RemainingBytes() >= messageLen)
			{
//				MemoryStream ms = new MemoryStream();
//				BinaryWriter writer = new BinaryWriter(ms);
//				writer.Write(reader.ReadBytes(messageLen));
//				ms.Seek(0, SeekOrigin.Begin);
//				OnReceivedMessage(ms);
				NetworkManager.Instance.msgQueue.Enqueue(reader.ReadBytes(messageLen));
			}
			else
			{
				memStream.Position = memStream.Position - 2;
				break;
			}
		}
		byte[] leftover = reader.ReadBytes((int)RemainingBytes());
		memStream.SetLength(0);
		memStream.Write(leftover, 0, leftover.Length);
	}

//	void OnReceivedMessage(MemoryStream ms)
//	{
//		BinaryReader r = new BinaryReader(ms);
//		byte[] message = r.ReadBytes((int)(ms.Length - ms.Position));
//		//处理接收到的 bytes
//		NetworkManager.Instance.msgQueue.Enqueue(message);
//	}


	long RemainingBytes()
	{
		return memStream.Length - memStream.Position;
	}


	void WriteMessage(byte[] message)
	{
		MemoryStream ms = null;
		using(ms = new MemoryStream())
		{
			ms.Position = 0;
			BinaryWriter writer = new BinaryWriter(ms);
			ushort msgLen = (ushort)message.Length;
			writer.Write(msgLen);
			writer.Write(message);
			writer.Flush();
			if (client != null && client.Connected)
			{
				byte[] payload = ms.ToArray();
				outStream.BeginWrite(payload, 0, payload.Length, new AsyncCallback(OnWrite), null);
			}
			else
			{
				Debug.LogError("client.connected------>>false!");
			}
		}
	}

	void OnWrite(IAsyncResult asr)
	{
		try
		{
			outStream.EndWrite(asr);
		}
		catch(Exception e)
		{
			Debug.LogError("OnWrite---->>" + e.Message);
		}
	}

	void OnDisconnected(string msg)
	{
		Close();
//		Debug.LogError("Connection was closed by the server:>" + msg);
	}
}
