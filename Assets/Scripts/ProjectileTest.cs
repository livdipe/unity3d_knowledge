using UnityEngine;
using System.Collections;

public class ProjectileTest : MonoBehaviour 
{
	public GameObject target;
	public float speed = 10;
	private float distanceToTarget;
	private bool move = true;

	//新建两个Sphere，其中一个挂该脚本，另一个做为target,拖拽到以上target变量上

	void Start()
	{
		distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
	}

	void Update()
	{
		if (move)
		{
			transform.LookAt(target.transform.position);
			float currentDist = Vector3.Distance(transform.position, target.transform.position);
			float angle = Mathf.Min(1, currentDist / distanceToTarget) * 30;
			transform.rotation = transform.rotation * Quaternion.Euler(Mathf.Clamp(-angle, -30, 30), 0, 0);
			if (currentDist < 0.1f)
				move = false;
			transform.Translate(Vector3.forward * Mathf.Min(speed * Time.deltaTime, currentDist));
		}
	}
}
