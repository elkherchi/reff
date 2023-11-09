using UnityEngine;

public class SunMoveScript : MonoBehaviour {

	public Transform character;
	Vector3 sunPosition;

	void Awake()
	{
		sunPosition = transform.transform.localPosition;
	}

	void OnEnable()
	{

		// Allign the sun to the right position from the start
		transform.position = new Vector3 (character.position.x - 6, transform.position.y, transform.position.z);
	}

	void Update()
	{
		transform.position = new Vector3 (character.position.x - 6, transform.position.y, transform.position.z);
	}

	public void ResetPosition()
	{
		transform.localPosition = sunPosition;
	}
}