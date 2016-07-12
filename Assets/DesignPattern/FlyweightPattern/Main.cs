using UnityEngine;
using System.Collections;

namespace FlyweightDemo
{
	public class Main : MonoBehaviour 
	{
		void Start () 
		{
			CMapManager map = new CMapManager();
			map.ShowDebris();
		}
	}
}