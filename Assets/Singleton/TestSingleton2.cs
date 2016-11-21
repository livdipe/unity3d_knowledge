using UnityEngine;
using System.Collections;

public class TestSingleton2 : MonoBehaviour 
{
	void Start () 
	{
		Debug.LogError(TestSingleton.Instance.number);
	}
}
