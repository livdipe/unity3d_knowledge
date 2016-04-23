using UnityEngine;
using System;

namespace PureMVC.interfaces
{
	public class MacroCommand : Notifier, INotifier
	{
		public MacroCommand()
		{
			Debug.Log("MacroCommand Constructor");
		}

		public void Show()
		{
			Debug.Log ("MacroCommand show");
		}
	}
}
