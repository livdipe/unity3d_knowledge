using UnityEngine;
using System.Collections;

namespace StateDemo
{
	public class Main : MonoBehaviour 
	{
		void Start () 
		{
			Debug.Log("吕布出场");
			CLvBu lvBu = new CLvBu();
			lvBu.ShowInfo();

			Debug.Log("添加装备");
			lvBu.AddUnit(new CWeapon4());
			lvBu.AddUnit(new CEquipper1());
			lvBu.AddUnit(new CChiTuMa());
			lvBu.ShowInfo();

			Debug.Log("吕布被关羽'水淹七军'击中");
			lvBu.State = new CSlowlyState();
			lvBu.ShowInfo();

			Debug.Log("吕布被刘备'冰封'击中");
			lvBu.State = new CLockedState();
			lvBu.ShowInfo();

			Debug.Log("吕布状态恢复");
			lvBu.State = new CNormalState();
			lvBu.ShowInfo();
		}
	}
}
