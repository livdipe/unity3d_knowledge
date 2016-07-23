using UnityEngine;
using System.Collections;

public class BallView : BounceElement 
{
	void OnCollisionEnter()
	{
		app.Notify(BounceNotification.BallHitGround, this);
	}
}
