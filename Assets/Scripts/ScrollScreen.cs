using UnityEngine;
using System.Collections;

public class ScrollScreen : MonoBehaviour 
{
	void Start () 
	{
		canMove = false;
		isDraging = false;
	}

	bool canMove;
	Vector3 startPos;
	Vector3 endPos;
	float startTime;
	float endTime;
	float offset;
	float time1 = 0.3f;
	float timer;
	Vector3 lastPos;
	bool isDraging;
	public Transform trMover;
	float[] nodePos = new float[]{0, -500, -1000, -1500, -2000};
	int curNode = 0;
	int destNode;
	float lastFramePosX;
	void Update () 
	{
		if (Input.GetMouseButtonDown(0))
		{
			startPos = Input.mousePosition;
			startTime = Time.time;
			lastPos = trMover.localPosition;
			isDraging = true;
		}
		if (Input.GetMouseButtonUp(0))
		{
			endPos = Input.mousePosition;
			endTime = Time.time;

			float len = Mathf.Abs(endPos.x - startPos.x);
			float speed = len / (endTime - startTime);
			destNode = curNode;
			if (speed > 200)
			{
				if (endPos.x > startPos.x && curNode > 0)
					destNode = curNode - 1;
				if (endPos.x < startPos.x && curNode < nodePos.Length - 1)
					destNode = curNode + 1;
			}
			if (destNode == curNode)
			{
				time1 = 0.3f;
			}

			lastPos = trMover.localPosition;
			offset = nodePos[destNode] - lastPos.x;
			canMove = true;
			isDraging = false;
		}

		if (isDraging)
		{
			trMover.localPosition = lastPos + new Vector3((Input.mousePosition.x - startPos.x) * 0.6f, 0, 0);
			float dis = (Input.mousePosition.x - lastFramePosX) / Time.deltaTime;
			lastFramePosX = Input.mousePosition.x;
			Debug.Log("dis:" + dis);
		}

		if (canMove)
		{
			timer += Time.deltaTime;
			float per = (1 - (time1 - timer) / time1) * offset;
			trMover.localPosition = lastPos + new Vector3(per, 0, 0);
			if (timer >= time1) 
			{
				trMover.localPosition = lastPos + new Vector3(offset, 0, 0);
				canMove = false;
				timer = 0;
				curNode = destNode;
				time1 = 0.3f;
			}
		}
	}
}
