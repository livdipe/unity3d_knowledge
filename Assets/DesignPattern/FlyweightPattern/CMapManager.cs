using UnityEngine;
using System.Collections;
using System;

namespace FlyweightDemo
{
	public class CMapManager
	{
		static System.Random r = new System.Random();
		const int SCREEN_WIDTH = 800;
		const int SCREEN_HEIGHT = 600;
		const int DEBRIS_NUMBER = 6;
		const int DEBRIS_TYPE_MIN = 9001;
		const int DEBRIS_TYPE_MAX = 9004 + 1;

		protected SDebris[] arrDebris = new SDebris[DEBRIS_NUMBER];

		public void ShowDebris()
		{
			for (int i = 0; i < DEBRIS_NUMBER; i++)
			{
				arrDebris[i].X = r.Next(0, SCREEN_WIDTH);
				arrDebris[i].Y = r.Next(0, SCREEN_HEIGHT);
				arrDebris[i].Type = (EDebrisType)r.Next(DEBRIS_TYPE_MIN, DEBRIS_TYPE_MAX);
				arrDebris[i].Name = "Debris_" + i.ToString();
				arrDebris[i].ShowInfo();
			}
		}
	}
}
