using UnityEngine;
using System.Collections;

public class ScrollViewTest : MonoBehaviour {
	public Vector2 scrollPosition = Vector2.zero;
	public float scrollVelocity = 0f;
	public float timeTouchPhaseEnded = 0f;
	public float inertiaDuration = 0.5f;
	
	public Vector2 lastDeltaPos;
	
	// Use this for initialization
	void Start () {
		
	}
	
	void OnGUI()
	{
		scrollPosition = GUI.BeginScrollView(new Rect(100, 40, 600, 400), scrollPosition, new Rect(0, 0, 500, 1600), false, true);
		
		
		for (int i = 0; i < 32; i++)
		{
			GUI.Button(new Rect(0, i*50, 400, 50), "Button"+i);
		}
		GUI.EndScrollView();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.touchCount > 0)
		{
			if (Input.GetTouch(0).phase == TouchPhase.Moved)
			{
				scrollPosition.y += Input.GetTouch(0).deltaPosition.y;
				lastDeltaPos = Input.GetTouch(0).deltaPosition;
			}        
			else if (Input.GetTouch(0).phase == TouchPhase.Ended)
			{
				print ("End:"+lastDeltaPos.y+"|"+Input.GetTouch(0).deltaTime);
				if (Mathf.Abs(lastDeltaPos.y)> 20.0f)
				{
					scrollVelocity = (int)(lastDeltaPos.y * 0.5/ Input.GetTouch(0).deltaTime);
					print(scrollVelocity);
				}
				timeTouchPhaseEnded = Time.time;
			}
		}
		else
		{
			if (scrollVelocity != 0.0f)
			{
				// slow down
				float t = (Time.time - timeTouchPhaseEnded)/inertiaDuration;
				float frameVelocity = Mathf.Lerp(scrollVelocity, 0, t);
				scrollPosition.y += frameVelocity * Time.deltaTime;
				
				if (t >= inertiaDuration)
					scrollVelocity = 0;
			}
		}
	}
}
