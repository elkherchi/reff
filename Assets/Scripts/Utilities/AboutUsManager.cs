using UnityEngine;

public class AboutUsManager : MonoBehaviour {

	public GameObject introductionButton;
	public GameObject introductionText;
	[Space(5)]
	public GameObject TheFieldButton;
	public GameObject TheFieldText;
	[Space(5)]
	public GameObject TheOrchardButton;
	public GameObject TheOrchardText;
	[Space(5)]
	public GameObject TheGreenhouseButton;
	public GameObject TheGreenhouseText;

	void OnEnable()
	{
		EnableIntroduction ();
	}

	void DisableAll()
	{
		introductionText.SetActive (false);
		TheFieldText.SetActive (false);
		TheOrchardText.SetActive (false);
		TheGreenhouseText.SetActive (false);
	}

	void DisableAllButtons()
	{
		introductionButton.SetActive (false);
		TheFieldButton.SetActive (false);
		TheOrchardButton.SetActive (false);
		TheGreenhouseButton.SetActive (false);
	}

	public void EnableIntroduction()
	{
		DisableAll ();
		DisableAllButtons ();
		introductionButton.SetActive (true);
		introductionText.SetActive (true);
	}
	public void EnableTheField()
	{
		DisableAll ();
		DisableAllButtons ();
		TheFieldButton.SetActive (true);
		TheFieldText.SetActive (true);
	}
	public void EnableTheOrchard()
	{
		DisableAll ();
		DisableAllButtons ();
		TheOrchardButton.SetActive (true);
		TheOrchardText.SetActive (true);
	}
	public void EnableTheGreenhouse()
	{
		DisableAll ();
		DisableAllButtons ();
		TheGreenhouseButton.SetActive (true);
		TheGreenhouseText.SetActive (true);
	}

	public void DisableAboutUs()
	{
		this.gameObject.SetActive (false);
	}
}