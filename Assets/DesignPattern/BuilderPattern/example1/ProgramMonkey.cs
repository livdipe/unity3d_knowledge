using UnityEngine;
using System.Collections;

namespace example1
{
	public class ProgramMonkey
	{
		private bool mIsLiterated;
		private bool mKnowMath;
		private string mLanguage;
		private bool mKnowDesign;

		public bool ismIsLiterated()
		{
			return mIsLiterated;
		}

		public void setmIsLiterated(bool mIsLiterated)
		{
			this.mIsLiterated = mIsLiterated;
		}

		public bool ismKnowMath()
		{
			return mKnowMath;
		}

		public void setmKnowMath(bool mKnowMath)
		{
			this.mKnowMath = mKnowMath;
		}

		public string getmLanguage()
		{
			return mLanguage;
		}

		public void setmLanguage(string mLanguage)
		{
			this.mLanguage = mLanguage;
		}

		public bool ismKnowDesign()
		{
			return mKnowDesign;
		}

		public void setmKnowDesign(bool mKnowDesign)
		{
			this.mKnowDesign = mKnowDesign;
		}

		public void show()
		{
			Debug.Log("\rIsLiterated="+mIsLiterated+"\n"
					   +"KnowMath="+mKnowMath+"\n"
					   +"Language="+mLanguage+"\n"
					   +"KnowDesign="+mKnowDesign+"\n");
		}
	}
}
