using UnityEngine;
using System.Collections;

namespace DelegateDemo
{
	using Random = System.Random;

	delegate int DRandomBuilder(int min, int max);

	public class Main : MonoBehaviour 
	{
		void Start () 
		{
			CRandomBuilder rndBuilder = new CRandomBuilder();

			DRandomBuilder rb1 = new DRandomBuilder(rndBuilder.GetRandomNumber1);
			DRandomBuilder rb2 = new DRandomBuilder(CRandomBuilder.GetRandomNumber2);

			// 委托并不真正的去产生随机数，而是要将这个工作委托给相同格式的方法去完成
			Debug.Log(rb1(1, 9));
			Debug.Log(rb2.Invoke(100, 999));
		}
	}

	public class CRandomBuilder
	{
		private Random rnd;

		public CRandomBuilder()
		{
			rnd = new Random();
		}

		public int GetRandomNumber1(int min, int max)
		{
			return rnd.Next(min, max + 1);
		}

		public static int GetRandomNumber2(int min, int max)
		{
			Random r = new Random(System.DateTime.Now.Millisecond);
			return r.Next(min, max + 1);
		}
	}


}
