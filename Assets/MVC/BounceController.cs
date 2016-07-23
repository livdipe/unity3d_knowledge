using UnityEngine;
using System.Collections;

public class BounceController : BounceElement 
{
	public void OnNotification(string p_event_path, Object p_target, params object[] p_data)
	{
		switch(p_event_path)
		{
		case BounceNotification.BallHitGround:
			OnBallGroundHit();
			break;

		case BounceNotification.GameComplete:
			OnGameComplete();
			break;

		default:
			break;
		}
	}

	void OnBallGroundHit()
	{
		app.model.bounces ++;
		Debug.Log("Bounce " + app.model.bounces);
		if (app.model.bounces >= app.model.winCondition)
		{
			app.view.ball.enabled = false;
			app.view.ball.GetComponent<Rigidbody>().isKinematic = true;
			app.Notify(BounceNotification.GameComplete, this);
		}
	}

	void OnGameComplete()
	{
		Debug.Log("Victory!!");
	}
}
