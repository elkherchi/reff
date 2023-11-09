using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class ButtonAnimation : MonoBehaviour
{
	public float time = 0.1f;
	public float scaleValue = 0.1f;
	Button btn;
	Vector3 initScale;


	void Start ()
	{
		initScale = transform.localScale;
		gameObject.AddComponent<BoxCollider2D> ();
		GetComponent<BoxCollider2D> ().size = new Vector2 (GetComponent<RectTransform> ().rect.width, GetComponent<RectTransform> ().rect.height);
	}

	void OnMouseDown ()
	{
		AnimateButton ();
	}

	void OnMouseUp ()
	{
		transform.localScale = initScale;
	}

	void OnMouseExit ()
	{
		AnimationBackwards ();
	}

	public void AnimateButton ()
	{
		transform.DOScale (transform.localScale - Vector3.one * scaleValue, time/2);
	}

	public void AnimationBackwards ()
	{
		// animate to initial scale again
		transform.DOScale (initScale, time/2);
	}




}
