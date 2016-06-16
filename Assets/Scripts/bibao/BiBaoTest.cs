using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BiBaoTest : MonoBehaviour 
{
	void Start()
	{
		WrongWay();
		RightWay();
		RightWayLikeThis();
	}

	void WrongWay()
	{
		List<Action> lists = new List<Action>();
		for (int i = 0; i < 5; i ++)
		{
			Action t = () =>
			{
				Debug.Log(i.ToString());
			};
			lists.Add(t);
		}

		foreach(Action t in lists)
		{
			t();
		}
	}

	void RightWay()
	{
		List<Action> lists = new List<Action>();
		for (int i = 0; i < 5; i ++)
		{
			int temp = i;
			Action t = ()=>
			{
				Debug.Log(temp.ToString());
			};
			lists.Add(t);
		}
		foreach(Action t in lists)
		{
			t();
		}
	}

	void RightWayLikeThis()
	{
		List<Action> lists = new List<Action>();
		for(int i = 0; i < 5; i ++)
		{
			TempClass tempClass = new TempClass();
			tempClass.i = i;
			Action t = tempClass.TempFuc;
			lists.Add(t);
		}

		foreach(Action t in lists)
		{
			t();
		}
	}

	class TempClass
	{
		public int i;
		public void TempFuc()
		{
			Debug.Log(i.ToString());
		}
	}
}
