using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class TestDoTween : MonoBehaviour 
{
	public Image image;
	public Transform trButton;
	public AnimationCurve curve;
	public AnimationCurve curve1;
	public AnimationCurve curve2;
	float x = 0;
	void Start () 
	{
		DOTween.To (value => x = value, x, x + 0.1f, 0.4f).SetEase(curve1).SetDelay(1.5f).OnUpdate(OnUpdate).OnComplete(Start);
//		DOTween.To (value => x = value, x, x + 0.1f, 0.4f).SetEase(EaseFunction).SetDelay(1).OnUpdate(OnUpdate).OnComplete(Start);
		Jello(trButton, 0.5f);
	}

	float EaseFunction (float time, float duration, float overshootOrAmplitude, float period)
	{
		Debug.LogError("time:" + time + "duration:" + duration + " overshootOrAmplitude:" + overshootOrAmplitude + " period:" + period);
		return Mathf.Sin(time / duration);
	}

	void OnUpdate()
	{
//		image.fillAmount = x;
		image.transform.localScale = new Vector3(x, 1, 1);
	}

	void Jello(Transform tr, float duration)
	{
		float dua = duration * 0.111f;
		Sequence s = DOTween.Sequence();
		s.Append(tr.DOLocalRotate(new Vector3(-12.5f, -12.5f, 0), dua));
		s.Append(tr.DOLocalRotate(new Vector3(6.25f, 6.25f, 0), dua));
		s.Append(tr.DOLocalRotate(new Vector3(-3.25f, -3.25f, 0), dua));
		s.Append(tr.DOLocalRotate(new Vector3(1.5625f, 1.5625f, 0), dua));
		s.Append(tr.DOLocalRotate(new Vector3(-0.78125f, 0, -0.78125f), dua));
		s.Append(tr.DOLocalRotate(new Vector3(0.390625f, 0, 0.390625f), dua));
		s.Append(tr.DOLocalRotate(new Vector3(-0.1953f, 0, -0.1953f), dua));
		s.Append(tr.DOLocalRotate(Vector3.zero, dua));
		s.PrependInterval(dua);
	}

}
