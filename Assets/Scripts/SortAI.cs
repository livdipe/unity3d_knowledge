using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class SortAI : MonoBehaviour 
{
//	void InsertSort(ref int[] data)
//	{
//		int i, j;
//		var count = data.Length;
//
//		for (i = 1; i < count; i++)
//		{
//			var t = data[i];
//			for (j = i - 1; j >= 0 && data[j] > t; j--)
//				data[j + 1] = data[j];
//			data[j + 1] = t;
//		}
//	}

	void InsertSort(ref int[] data)
	{
		int i, j;
		for (i = 1; i < data.Length; i ++)
		{
			int temp = data[i];
			j = i - 1;
			while (j >= 0 && temp < data[j])
			{
				data[j + 1] = data[j];
				j -= 1;
			}
			data[j + 1] = temp;
		}
	}

	void Start()
	{
		int[] data = new int[100];
		List<int> list = new List<int>();
		StringBuilder sb = new StringBuilder();
		Random.seed = 5;

		for (int i = 0; i < data.Length; i++)
		{
			int rdm = Random.Range(1, 101);
			while(list.Contains(rdm))
			{
				rdm = Random.Range(1, 101);
			}
			sb.Append(rdm).Append(" ");

			data[i] = rdm;
			list.Add(rdm);
		}
		print (sb.ToString());

		sb.Remove(0, sb.Length);
		InsertSort(ref data);
		for (int i = 0; i < data.Length; i++)
		{
			sb.Append(data[i]).Append(" ");
//			print (data[i]);
		}
		print (sb.ToString());
	}
}
