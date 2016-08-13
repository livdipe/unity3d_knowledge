namespace GameAI
{
	using UnityEngine;
	using System.Collections;
	using System.Collections.Generic;

	public class AIManager : MonoBehaviour 
	{
		public Transform trTarget;
		public Vehicle[] players;
		public static AIManager Instance;

		void Awake()
		{
			Instance = this;
		}

		void Start () 
		{
			players = GameObject.FindObjectsOfType<Vehicle>() as Vehicle[];
//			for (int i = 0; i < players.Length; i++)
//			{
//				players[i].trTarget = trTarget;
//			}
		}

		void TouchSetTargetPoint()
		{
			if (Input.GetMouseButtonDown(0))
			{
				Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				mousePosition.z = 0;
				trTarget.position = mousePosition;
			}
		}

		void Update () 
		{
			TouchSetTargetPoint();
		}
	}
}
