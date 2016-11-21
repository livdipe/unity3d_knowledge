using UnityEngine;
using System.Collections;

public class TestSingleton : GenericSingleton<TestSingleton>
{
	public int number = 123456;
}
