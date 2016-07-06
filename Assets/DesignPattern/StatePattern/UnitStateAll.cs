namespace StateDemo
{
	//正常状态
	public class CNormalState : CUnitState
	{
		public CNormalState()
		{
			myDescription = "正常状态";
		}

		public override int ChangeValue (int val)
		{
			return val;
		}
	}

	//缓慢状态
	public class CSlowlyState : CUnitState
	{
		public CSlowlyState()
		{
			myDescription = "缓慢状态";
		}

		public override int ChangeValue (int val)
		{
			return val / 2;
		}
	}

	//锁定状态
	public class CLockedState : CUnitState
	{
		public CLockedState()
		{
			myDescription = "锁定状态";
		}

		public override int ChangeValue (int val)
		{
			return 0;
		}
	}
}
