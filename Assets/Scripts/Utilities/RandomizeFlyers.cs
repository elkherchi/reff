using UnityEngine;

public class RandomizeFlyers : MonoBehaviour
{
    private void OnEnable()
    {
        if (Random.Range(0f, 1) > 0.5f)
            Randomize();
    }

    private void Randomize()
    {
        Vector2 firstPosition = transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition;
        transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition;
        transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = firstPosition;
    }
}