using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AnswersScript : MonoBehaviour {

	public GameObject Answer1,Answer2;

	Vector3 answer1InitPos,answer2InitPos;

	void Awake()
	{
		answer1InitPos = Answer1.transform.position;
		answer2InitPos = Answer2.transform.position;
	}

	void ResetPositions()
	{
		int rnd = Random.Range (0, 2);
		if (rnd == 0) {
			Answer1.transform.position = answer1InitPos;
			Answer2.transform.position = answer2InitPos;
		} else if (rnd == 1) {
			Answer1.transform.position = answer2InitPos;
			Answer2.transform.position = answer1InitPos;
		}
	}

	void OnDisable()
	{
		Answer1.transform.DOKill ();
		Answer2.transform.DOKill ();
		EditorDebugger.Log ("KILLED");
		StopCoroutine (nameof(MoveCoroutine));
	}

	void OnEnable()
	{
		ResetPositions ();
		StartCoroutine (nameof(MoveCoroutine));
	}

	
	void Answer1MovetoAnswer2()
	{
		Answer1.transform.DOMoveX (Answer2.transform.position.x, 0.5f, false);
	}

	void Answer2MovetoAnswer1()
	{
		Answer2.transform.DOMoveX (Answer1.transform.position.x, 0.5f, false);
	}


	IEnumerator MoveCoroutine()
	{
		while (true) {
			Answer1MovetoAnswer2 ();
			Answer2MovetoAnswer1 ();
			yield return new WaitForSeconds (1.5f);
			yield return null;
		}
	}
}