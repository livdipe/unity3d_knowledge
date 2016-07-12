using UnityEngine;
using System.Collections;

// 手机上可以安装音乐、游戏等app，也可以不安装，不管安不安装这些app
// 都不会影响手机的使用，由此可见手机和app之间的关系是一种聚合关系
// 把手机和app分离开
// 尽量使用组合/聚合，而不要使用类继承
namespace BridgePatternDemo
{
	public abstract class MobilePhoneApp
	{
		public abstract void Run();
	}

	public class Music : MobilePhoneApp
	{
		public override void Run()
		{
			Debug.Log("运行手机音乐");
		}
	}

	public class Game : MobilePhoneApp
	{
		public override void Run()
		{
			Debug.Log("运行手机游戏");
		}
	}

	public abstract class MobilePhone
	{
		protected MobilePhoneApp app;

		public MobilePhone(MobilePhoneApp app)
		{
			this.app = app;
		}

		public abstract void Run();
	}

	public class MEIZU : MobilePhone
	{
		public MEIZU(MobilePhoneApp app) : base(app)
		{
		}

		public override void Run()
		{
			this.app.Run();
		}
	}

	public class IPhone : MobilePhone
	{
		public IPhone(MobilePhoneApp app) : base(app)
		{
		}

		public override void Run()
		{
			this.app.Run();
		}
	}
}