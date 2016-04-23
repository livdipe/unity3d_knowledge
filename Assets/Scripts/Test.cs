using UnityEngine;
using System.Collections;
using PureMVC.interfaces;

public class Test : MonoBehaviour 
{
	void Awake ()
	{
		print ("Awake");
	}

	void OnEnable ()
	{
		print ("OnEnable");
	}

	void Start () 
	{
		print ("Start");
//		MacroCommand mc = new MacroCommand();
//		mc.Show();
//		mc.SendNotification("abc");

		print ("1");
		StartCoroutine(Fun1());
		print ("2");
	}

	IEnumerator Fun1()
	{
		print ("3");
		yield return new WaitForEndOfFrame();
		print ("4");
	}
}
