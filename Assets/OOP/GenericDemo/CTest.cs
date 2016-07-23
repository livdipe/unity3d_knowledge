using UnityEngine;
using System.Collections;

namespace GenericDemo
{
	public class CTest : MonoBehaviour 
	{
		//泛型方法
		void Print<T>(T val)
		{
			Debug.Log(val);
		}

		void Start ()
		{
			Print(123);

			int x = 123;
			CTestA<int> test = new CTestA<int>(x);
			Debug.Log(test.GetVal());
		}
	}

	class CTestA<T>
	{
		private T myVal;

		public CTestA(T val)
		{
			myVal = val;
		}

		public T GetVal()
		{
			return myVal;
		}
	}
}
