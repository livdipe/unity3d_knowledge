using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class RotateImage : MonoBehaviour 
{
	public Image image;
	float rotate = 0;
	public AnimationCurve curve;

	void Start()
	{
//		Rotate ();
		Rotate2();
	}

	void Rotate()
	{
		rotate -= 90;
		if (rotate <= -360) rotate = 0;
		image.transform.DOLocalRotate(new Vector3(0, 0, rotate), 1).SetDelay(0.2f).SetEase(curve).OnComplete(() => Rotate());
	}

	void Rotate2()
	{
		Sequence s = DOTween.Sequence();
		s.Append(image.transform.DOLocalRotate(new Vector3(0, 0, -90), 1).SetDelay(0.2f).SetEase(curve));
		s.Append(image.transform.DOLocalRotate(new Vector3(0, 0, -180), 1).SetDelay(0.2f).SetEase(curve));
		s.Append(image.transform.DOLocalRotate(new Vector3(0, 0, -270), 1).SetDelay(0.2f).SetEase(curve));
		s.Append(image.transform.DOLocalRotate(new Vector3(0, 0, -360), 1).SetDelay(0.2f).SetEase(curve));
		s.SetLoops(-1);
	}
}
