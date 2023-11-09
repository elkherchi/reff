using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignScale : MonoBehaviour {
	
	public float amplitude = 1;
	public float _speed = 0.35f;
	private float _oscillateRange;
	private float _oscillateOffset;

	private Vector3 tmp;
	private float val;

	public bool signYPos = false;
	public bool signXPos = false;

	void Start () {
		//_oscillateRange = (_endRange - _startRange);
	}

	void Update () {
		RectTransform rect = transform.GetComponent < RectTransform> ();
		rect.localPosition = new Vector3(rect.localPosition.x, rect.localPosition.y + (amplitude * Mathf.Sin (_speed * Time.deltaTime)),rect.localPosition.z);
	}
}
