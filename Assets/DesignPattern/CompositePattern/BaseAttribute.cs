using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CompositeDemo
{
	// 属性系统
	// 加值 加百分比
	// 先计算， 后计算
	// 装备加属性 RawBonuse
	// 技能加属性 FinalBonus
	// 技能有时间限制
	// 属性依赖，比如攻击速度属性要依赖敏捷属性

	public class BaseAttribute 
	{
		private int baseValue;
		private float baseMultiplier;

		public BaseAttribute(int value, float multiplier = 0)
		{
			baseValue = value;
			baseMultiplier = multiplier;
		}

		public int BaseValue()
		{
			return baseValue;
		}

		public float BaseMultiplier()
		{
			return baseMultiplier;
		}
	}

	//----------------------------------------------------

	public class RawBonus : BaseAttribute
	{
		public RawBonus(int value, float multiplier) : base(value, multiplier)
		{
		}
	}

	//----------------------------------------------------

	public class FinalBonus : BaseAttribute
	{
		private Attribute parent;

		public FinalBonus(int value, float multiplier) : base(value, multiplier)
		{
		}

		public void StartTimer(Attribute _parent)
		{
			parent = _parent;
		}

		public void OnTimerEnd()
		{
			parent.RemoveFinalBonus(this);
		}
	}

	//----------------------------------------------------

	public class Attribute : BaseAttribute
	{
		private List<RawBonus> rawBonuses;
		private List<FinalBonus> finalBonuses;
		protected int finalValue;

		public Attribute(int startingValue) : base(startingValue)
		{
			rawBonuses = new List<RawBonus>();
			finalBonuses = new List<FinalBonus>();

			finalValue = BaseValue();
		}

		public void AddRawBonus(RawBonus bonus)
		{
			rawBonuses.Add(bonus);
		}

		public void AddFinalBonus(FinalBonus bonus)
		{
			finalBonuses.Add(bonus);
		}

		public void RemoveRawBonus(RawBonus bonus)
		{
			if (rawBonuses.Contains(bonus))
			{
				rawBonuses.Remove(bonus);
			}
		}

		public void RemoveFinalBonus(FinalBonus bonus)
		{
			if (finalBonuses.Contains(bonus))
			{
				finalBonuses.Remove(bonus);
			}
		}

		public void ApplyRawBonuses()
		{
			int rawBonusValue = 0;
			float rawBonusMultiplier = 0;

			for (int i = 0; i < rawBonuses.Count; i++)
			{
				rawBonusValue += rawBonuses[i].BaseValue();
				rawBonusMultiplier += rawBonuses[i].BaseMultiplier();
			}

			finalValue += rawBonusValue;
			finalValue = (int)(finalValue * (1 + rawBonusMultiplier));
		}

		public void ApplyFinalBonuses()
		{
			int finalBonusValue = 0;
			float finalBonusMultiplier = 0;

			for (int i = 0; i < finalBonuses.Count; i++)
			{
				finalBonusValue += finalBonuses[i].BaseValue();
				finalBonusMultiplier += finalBonuses[i].BaseMultiplier();
			}

			finalValue += finalBonusValue;
			finalValue = (int)(finalValue * (1 + finalBonusMultiplier));
		}

		public virtual int CalculateValue()
		{
			finalValue = BaseValue();

			ApplyRawBonuses();

			ApplyFinalBonuses();

			return finalValue;
		}

		public int FinalValue()
		{
			return CalculateValue();
		}
	}

	//----------------------------------------------------

	public class DependantAttribute : Attribute
	{
		protected List<Attribute> otherAttributes;

		public DependantAttribute(int startingValue) : base(startingValue)
		{
			otherAttributes = new List<Attribute>();
		}

		public void AddAttribute(Attribute attr)
		{
			otherAttributes.Add(attr);
		}

		public void RemoveAttribute(Attribute attr)
		{
			if (otherAttributes.Contains(attr))
			{
				otherAttributes.Remove(attr);
			}
		}

		public override int CalculateValue ()
		{
			finalValue = BaseValue();

			ApplyRawBonuses();

			ApplyFinalBonuses();

			return finalValue;
		}
	}

	//----------------------------------------------------

	public class AttackSpeed : DependantAttribute
	{
		public AttackSpeed(int startingValue) : base(startingValue)
		{
		}

		public override int CalculateValue ()
		{
			finalValue = BaseValue();

			int dexterity = otherAttributes[0].CalculateValue();

			finalValue += (int)(dexterity / 5);

			ApplyRawBonuses();

			ApplyFinalBonuses();

			return finalValue;
		}
	}
}