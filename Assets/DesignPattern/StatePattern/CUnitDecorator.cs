using UnityEngine;
using System.Collections;

namespace StateDemo
{
	public abstract class CUnitDecorator : CUnit
	{
		public ArrayList arrUnits;

		public CUnitDecorator()
		{
			arrUnits = new ArrayList();
		}

		//给出装备列表
		public string GetUnitName()
		{
			if (arrUnits.Count == 0)
			{
				return "No Equip";
			}
			else
			{
				string result = (arrUnits[0] as IUnit).Name;
				for (int i = 1; i < arrUnits.Count; i ++)
				{
					result += ", " + (arrUnits[i] as IUnit).Name;
				}
				return result;
			}
		}

		//添加装备
		public void AddUnit(IUnit unit)
		{
			if (arrUnits.Contains(unit) == false)
			{
				arrUnits.Add(unit);

				mySpeed += unit.Speed;
				myLife += unit.Life;
				myPower += unit.Power;
			}
		}

		//移除装备
		public void RemoveUnit(IUnit unit)
		{
			if (arrUnits.Contains(unit))
			{
				arrUnits.Remove(unit);

				mySpeed -= unit.Speed;
				myLife -= unit.Life;
				myPower -= unit.Power;
			}
		}

		public void ShowInfo()
		{
			Debug.Log(string.Format("姓名: {0}", this.Name));
			Debug.Log(string.Format("装备: {0}", this.GetUnitName()));
			Debug.Log(string.Format("速度: {0}", this.Speed));
			Debug.Log(string.Format("生命值: {0}", this.Life));
			Debug.Log(string.Format("攻击力: {0}", this.Power));
			Debug.Log(string.Format("类型: {0}", this.Type));
		}
	}
}
