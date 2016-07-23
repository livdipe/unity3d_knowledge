using UnityEngine;
using System.Collections;

public class BounceController : BounceElement 
{
	public void OnBallGroundHit()
	{
		app.model.bounces ++;
		Debug.Log("Bounce " + app.model.bounces);
		if (app.model.bounces >= app.model.winCondition)
		{
			app.view.ball.enabled = false;
			app.view.ball.GetComponent<Rigidbody>().isKinematic = true;
			OnGameComplete();
		}
	}

	public void OnGameComplete()
	{
		Debug.Log("Victory");
	}
}
