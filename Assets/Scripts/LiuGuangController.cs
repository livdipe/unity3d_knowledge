using UnityEngine;
using System.Collections;

public class LiuGuangController : MonoBehaviour 
{
	public float flowLightUVStartPos = 0.5f;
	public float flowLightUVEndPos = 1.5f;
	public float flowLightMovingSpeed = 0.1f;
	public string flowLightPosNameFromShader = "_UvPos";
	public float flowLightDisplayDelayTime = 2f;
	private float _uvPos;
	Renderer rend;

	void Start ()
	{
		rend = GetComponent<Renderer>();
		_uvPos = flowLightUVStartPos;
		InvokeRepeating("ControlFlowLight", flowLightDisplayDelayTime, 0.1f);
	}

	void ControlFlowLight ()
	{
		if (_uvPos <= flowLightUVEndPos)
		{
			rend.material.SetFloat(flowLightPosNameFromShader, _uvPos);
			_uvPos = _uvPos + flowLightMovingSpeed;
		}
		else
		{
			CancelInvoke("ControlFlowLight");
		}
	}
}
