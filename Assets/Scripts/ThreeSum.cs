using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/*
Given an array S of n integers, are there elements a, b, c in S such that a + b + c = 0? Find all unique triplets in the array which gives the sum of zero.

	Note:
	Elements in a triplet (a,b,c) must be in non-descending order. (ie, a ≤ b ≤ c)
	The solution set must not contain duplicate triplets.
	For example, given array S = {-1 0 1 2 -1 -4},

A solution set is:
(-1, 0, 1)
(-1, -1, 2)
*/

public class ThreeSum : MonoBehaviour 
{
//	int[] S = {-1, 0, 1, 2, -1, -4};
////	List<int[]> triplets = new List<int[]>();
//
//	void Solve()
//	{
//		Array.Sort(S);
//		for (int i = 0; i < S.Length - 2; i++) {
//			for (int j = i + 1; j < S.Length - 1; j++) {
//				for (int k = j + 1; k < S.Length; k++) {
//					if (S[i] + S[j] + S[k] == 0) {
//						if (!IsInTriplets(S[i], S[j], S[k])) {
//							triplets.Add(new int[]{S[i], S[j], S[k]});
//							Debug.Log(string.Format("({0} {1} {2})", S[i], S[j], S[k]));
//						}
//					}
//				}
//			}
//		}
//	}

	IList<IList<int>> TS(int[] nums)
	{
		List<IList<int>> triplets = new List<IList<int>>();

		Array.Sort(nums);
		int len = nums.Length;
		for (int i = 0; i < len - 2; i++) {
			for (int j = i + 1; j < len - 1; j++) {
				for (int k = j + 1; k < len; k++) {
					if (nums[i] + nums[j] + nums[k] == 0) {
						if (!IsInTriplets(triplets, nums[i], nums[j], nums[k])) {
							triplets.Add(new List<int>{nums[i], nums[j], nums[k]});
						}
					}
				}
			}
		}

		return triplets;
	}

	bool IsInTriplets(List<IList<int>> triplets, int a, int b, int c)
	{
		for (int i = 0; i < triplets.Count; i++)
		{
			if (triplets[i][0] == a && 
				triplets[i][1] == b && 
				triplets[i][2] == c) {
				return true;
			}
		}
		return false;
	}

	void Start()
	{
//		Solve();
		var results = TS(new int[]{-1, 0, 1, 2, -1, -4});
		for (int i = 0; i < results.Count; i++)
		{
			Debug.Log(string.Format("({0} {1} {2})", results[i][0], results[i][1], results[i][2]));
		}
	}
}
