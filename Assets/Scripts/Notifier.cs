using UnityEngine;
using System;

namespace PureMVC.interfaces
{
	public class Notifier : INotifier
	{
		public virtual void SendNotification(string notificationName)
		{
			Debug.Log("Notifier SendNotification 1");
		}

		public virtual void SendNotification(string notificationName, object body)
		{
			Debug.Log("Notifier SendNotification 2");
		}

		public virtual void SendNotification(string notificationName, object body, string type)
		{
			Debug.Log("Notifier SendNotification 3");
		}
	}

}