namespace NetworkDrawing
{
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

		public void SendMessage(byte[] bytes)
		{
			WriteMessage(bytes);
		}

		public void ConnectServer(string host, int port)
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
			string msg = System.Text.Encoding.UTF8.GetString(bytes, 0, length);
			NetworkDrawing.Instance.msgQueue.Enqueue(msg);
		}

		long RemainingBytes()
		{
			return memStream.Length - memStream.Position;
		}

		void WriteMessage(byte[] message)
		{
			if (client != null && client.Connected)
			{
				outStream.BeginWrite(message, 0, message.Length, new AsyncCallback(OnWrite), null);
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
		}
	}
}