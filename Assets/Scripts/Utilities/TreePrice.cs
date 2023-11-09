using UnityEngine;

public class TreePrice : MonoBehaviour
{
    public int price;
    public GameObject marker;

    public void TurnOnTree()
    {
        gameObject.SetActive(true);
        marker.SetActive(false);
    }

    public void TurnOffTree()
    {
        gameObject.SetActive(false);
        marker.SetActive(true);
    }
}