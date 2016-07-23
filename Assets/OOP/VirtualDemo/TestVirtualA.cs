using UnityEngine;
using System.Collections;

public class TestVirtualA : MonoBehaviour 
{
	void Awake()
	{
		AwakeUnityMsg();
	}

	protected virtual void AwakeUnityMsg()
	{
		Debug.Log("TestVirtualA AwakeUnityMsg");
	}
}
