using UnityEngine;
using System.Collections;

namespace EventCenterDemo
{
	public class Main : MonoBehaviour 
	{
		void Start () 
		{
			EventCenter.AddListener("MouseHit", MouseHit);
			EventCenter.AddListener<int>("MouseHit1", MouseHit1);
		}

		void MouseHit()
		{
			Debug.Log("MouseHit");
		}

		void MouseHit1(int arg1)
		{
			Debug.Log("MouseHit1");
		}

		void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				EventCenter.Broadcast("MouseHit");
				EventCenter.Broadcast<int>("MouseHit1", 563);
			}
		}
	}
}
