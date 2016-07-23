using UnityEngine;
using System.Collections;

public class TestVirtualB : TestVirtualA 
{
	protected override void AwakeUnityMsg()
	{
		base.AwakeUnityMsg();

		Debug.Log("TestVirtualB AwakeUnityMsg");
	}
}
