using UnityEngine;
using System.Collections;

namespace example1
{
	public class ConcreteProgramMonkey : Builder
	{
		private ProgramMonkey mMonkey = new ProgramMonkey();

		public override void buildIsLiterated(bool arg)
		{
			mMonkey.setmIsLiterated(arg);
		}

		public override void buildKnowMath(bool arg)
		{
			mMonkey.setmKnowMath(arg);
		}

		public override void buildLanguage(string arg)
		{
			mMonkey.setmLanguage(arg);
		}

		public override void buildKnowDesign(bool arg)
		{
			mMonkey.setmKnowDesign(arg);
		}

		public override ProgramMonkey getMonkey()
		{
			return mMonkey;
		}
	}
}
