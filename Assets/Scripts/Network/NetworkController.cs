using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.IO;
using System;

public class NetworkController : MonoBehaviour {

	void Awake() {
		DontDestroyOnLoad(this);
	}

	// Use this for initialization
	void Start() {
		startServer();
	}

	// Update is called once per frame
	void Update() {
		processMessage();
		if (Input.GetKeyDown(KeyCode.Space))
		{
			byte[] bs = Encoding.UTF8.GetBytes("helloworld");
			Message msg = new Message(bs);
			send(msg);
		}
	}

	static TcpClient client = null;
	static BinaryReader reader = null;
	static BinaryWriter writer = null;
	static Thread networkThread = null;
	private static Queue<Message> messageQueue = new Queue<Message>();

	static void addItemToQueue(Message item) {
		lock(messageQueue) {
			messageQueue.Enqueue(item);
		}
	}

	static Message getItemFromQueue() {
		lock(messageQueue) {
			if (messageQueue.Count > 0) {
				return messageQueue.Dequeue();
			} else {
				return null;
			}
		}
	}

	static void processMessage() {
		Message msg = getItemFromQueue();
		if (msg != null) {
			Debug.Log(msg.content.Length);
			// do some processing here, like update the player state
			Debug.Log(Encoding.UTF8.GetString(msg.content));
		}
	}

	static void startServer() {
		if (networkThread == null) {
			connect();
			networkThread = new Thread(() => {
				while (reader != null) {
					Message msg = Message.ReadFromStream(reader);
					addItemToQueue(msg);
				}
				lock(networkThread) {
					networkThread = null;
				}
			});
			networkThread.Start();
		}
	}

	static void connect() {
		if (client == null) {
			string server = "localhost";
			int port = 2222;
			client = new TcpClient(server, port);
			Stream stream = client.GetStream();
			reader = new BinaryReader(stream);
			writer = new BinaryWriter(stream);
		}
	}

	public static void send(Message msg) {
		msg.WriteToStream(writer);
		writer.Flush();
	}
}

public class Message {
	public ushort length { get; set; }
	public byte[] content { get; set; }

	public static Message ReadFromStream(BinaryReader reader) {
		ushort len;
		byte[] len_buf;
		byte[] buffer;

		len_buf = reader.ReadBytes(2);
		if (BitConverter.IsLittleEndian) {
			Array.Reverse(len_buf);
		}
		len = BitConverter.ToUInt16(len_buf, 0);

		buffer = reader.ReadBytes(len);

		return new Message(buffer);
	}

	public void WriteToStream(BinaryWriter writer) {
		byte[] len_bytes = BitConverter.GetBytes(length);

		if (BitConverter.IsLittleEndian) {
			Array.Reverse(len_bytes);
		}
		writer.Write(len_bytes);

		writer.Write(content);
	}

	public Message(byte[] data) {
		content = data;
	}
}
