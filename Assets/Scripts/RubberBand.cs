using UnityEngine;
using System.Collections;
using DG.Tweening;

public class RubberBand : MonoBehaviour 
{
	public RectTransform target;
	public AnimationCurve rubberBandX;
	public AnimationCurve rubberBandY;

	void Start () 
	{
		target.DOScaleX(1, 1).SetEase(rubberBandX);
		target.DOScaleY(1, 1).SetEase(rubberBandY);
	}
}
