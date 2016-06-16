using UnityEngine;
using System.Collections;

namespace example1
{
	public abstract class Builder 
	{
		public abstract void buildIsLiterated(bool arg);
		public abstract void buildKnowMath(bool arg);
		public abstract void buildLanguage(string arg);
		public abstract void buildKnowDesign(bool arg);

		public abstract ProgramMonkey getMonkey();
	}
}
