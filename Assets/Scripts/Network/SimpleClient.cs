using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Text;

public class SimpleClient : MonoBehaviour 
{
	TcpClient clientSocket = new TcpClient();

	void Start () 
	{
		clientSocket.Connect("127.0.0.1", 2222);
	}
	
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			NetworkStream stream = clientSocket.GetStream();
			byte[] outStream = Encoding.UTF8.GetBytes("helloworld");
			stream.Write(outStream, 0, outStream.Length);

			byte[] inStream = new byte[1024];
			Debug.Log(clientSocket.ReceiveBufferSize);
			stream.Read(inStream, 0, 1024);
			string returndata = Encoding.UTF8.GetString(inStream);
			Debug.Log(returndata);
		}
	}
}
