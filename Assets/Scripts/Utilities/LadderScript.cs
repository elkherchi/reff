using UnityEngine;

public class LadderScript : MonoBehaviour {

	public EdgeCollider2D ladderCollider;
	public BoxCollider2D ladderOff;
	public BoxCollider2D ladderOn;

	public void TurnOffLadder()
	{
		ladderCollider.enabled = false;
		ladderOn.enabled = false;
		ladderOff.enabled = false;
	}

	public void TurnOnLadderOff()
	{
		ladderOff.enabled = true;
	}
}