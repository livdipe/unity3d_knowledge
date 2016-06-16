using UnityEngine;
using System.Collections;

namespace example1
{
	public class Director 
	{
		private Builder builder = new ConcreteProgramMonkey();

		public ProgramMonkey getMonkeyLow()
		{
			builder.buildIsLiterated(true);
			builder.buildKnowMath(true);
			builder.buildLanguage("Android");
			builder.buildKnowDesign(false);

			return builder.getMonkey();
		}

		public ProgramMonkey getMonkeyHigh()
		{
			builder.buildIsLiterated(true);
			builder.buildKnowMath(true);
			builder.buildLanguage("Android/Java/Designer");
			builder.buildKnowDesign(true);

			return builder.getMonkey();
		}
	}
}
