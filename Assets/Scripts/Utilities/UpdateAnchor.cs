using UnityEngine;

public class UpdateAnchor : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] private float x;
    [Range(0, 1)]
    [SerializeField] private float y;

    [ContextMenu("Position Element")]
    private void Position()
    {
        GetComponent<RectTransform>().anchorMin = new Vector2(x, y);
        GetComponent<RectTransform>().anchorMax = new Vector2(x, y);
    }
}