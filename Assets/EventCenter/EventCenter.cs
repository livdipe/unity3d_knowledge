namespace EventCenterDemo
{
	using System;
	using System.Collections.Generic;
	using UnityEngine;

	static internal class EventCenter 
	{
		public delegate void Callback();
		public delegate void Callback<T>(T arg1);

		static public Dictionary<string, Delegate> mEventTable = new Dictionary<string, Delegate>();

		static public void OnListenerAdding(string eventType, Delegate listenerBeingAdded)
		{
			if (!mEventTable.ContainsKey(eventType))
			{
				mEventTable.Add(eventType, null);
			}

			Delegate d = mEventTable[eventType];
			if (d != null && d.GetType() != listenerBeingAdded.GetType())
			{
				Debug.LogError("委托方法类型不同!");
			}
		}

		static public void OnListenerRemoved(string eventType)
		{
			if (mEventTable[eventType] == null)
			{
				mEventTable.Remove(eventType);
			}
		}

		// 没有参数
		static public void AddListener(string eventType, Callback handler)
		{
			OnListenerAdding(eventType, handler);
			mEventTable[eventType] = (Callback)mEventTable[eventType] + handler;
		}

		// Single parameter
		static public void AddListener<T>(string eventType, Callback<T> handler)
		{
			OnListenerAdding(eventType, handler);
			mEventTable[eventType] = (Callback<T>)mEventTable[eventType] + handler;
		}

		static public void RemoveListener(string eventType, Callback handler)
		{
			mEventTable[eventType] = (Callback)mEventTable[eventType] - handler;
			OnListenerRemoved(eventType);
		}

		static public void RemoveListener<T>(string eventType, Callback<T> handler)
		{
			mEventTable[eventType] = (Callback<T>)mEventTable[eventType] - handler;
			OnListenerRemoved(eventType);
		}

		static public void Broadcast(string eventType)
		{
			Delegate d;
			if (mEventTable.TryGetValue(eventType, out d))
			{
				Callback callback = d as Callback;

				if (callback != null)
				{
					callback();
				}
			}
		}

		static public void Broadcast<T>(string eventType, T arg1)
		{
			Delegate d;
			if (mEventTable.TryGetValue(eventType, out d))
			{
				Callback<T> callback = d as Callback<T>;

				if (callback != null)
				{
					callback(arg1);
				}
			}
		}

		static public void Cleanup()
		{
			mEventTable.Clear();
		}
	}

}
