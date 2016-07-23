using UnityEngine;
using System.Collections;

public class BounceApplication : MonoBehaviour 
{
	public BounceModel model;
	public BounceView view;
	public BounceController controller;

	public void Notify(string p_event_path, Object p_target, params object[] p_data)
	{
		BounceController[] controller_list = GetAllControllers();
		for (int i = 0; i < controller_list.Length; i++)
		{
			controller_list[i].OnNotification(p_event_path, p_target, p_data);
		}
	}

	public BounceController[] GetAllControllers()
	{
		return new BounceController[]{ controller };
	}
}
