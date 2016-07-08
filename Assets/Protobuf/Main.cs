using UnityEngine;
using System.Collections;
using test;

namespace ProtobufDemo
{
	public class Main : MonoBehaviour 
	{
		void Start () 
		{
			Person person = new Person();
			person.age = 28;
			person.name = "lcl";

			byte[] bytes = Serialization.Serialize<Person>(person);
			Debug.Log("序列化bytes length:" + bytes.Length);
			Person person2 = Serialization.Deserialize<Person>(bytes);
			Debug.Log("反列化 age:" + person2.age);
		}
	}
}
