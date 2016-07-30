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
				ScreenDebuger.Instance.AddMsg(e.Message);
			}
		}

		void OnConnect(IAsyncResult asr)
		{
			ScreenDebuger.Instance.AddMsg("Connected Server");
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

//		void OnReceive(byte[] bytes, int length)
//		{
//			string msg = System.Text.Encoding.UTF8.GetString(bytes, 0, length);
//			NetworkDrawing.Instance.msgQueue.Enqueue(msg);
//		}

		void OnReceive(byte[] bytes, int length)
		{
//			Debug.Log("OnReceive:"+length);
			memStream.Seek(0, SeekOrigin.End);
			memStream.Write(bytes, 0, length);
			memStream.Seek(0, SeekOrigin.Begin);
			while (RemainingBytes() > 2)
			{
				byte[] len_buf = reader.ReadBytes(2);
				if (BitConverter.IsLittleEndian) {
					Array.Reverse(len_buf);
				}
				ushort messageLen = BitConverter.ToUInt16(len_buf, 0);
				if (RemainingBytes() >= messageLen)
				{
					string msg = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(messageLen), 0, messageLen);
					NetworkDrawing.Instance.msgQueue.Enqueue(msg);
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
				byte[] len_bytes = BitConverter.GetBytes(message.Length);
				if (BitConverter.IsLittleEndian)
				{
					Array.Reverse(len_bytes);
				}
				writer.Write(len_bytes);
				writer.Write(message);
				writer.Flush();
				if (client != null && client.Connected)
				{
					byte[] payload = ms.ToArray();
					outStream.BeginWrite(payload, 0, payload.Length, new AsyncCallback(OnWrite), null);
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
		}
	}
}