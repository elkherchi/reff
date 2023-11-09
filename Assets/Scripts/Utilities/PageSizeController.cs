using System.Collections;
using UnityEngine;

public class PageSizeController : MonoBehaviour
{
    private const float commonOffset = 100f;
    RectTransform rect;
    RectTransform childRect;
    [SerializeField] float offset;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        childRect = transform.GetChild(0).GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        StartCoroutine(FixText());
    }

    private IEnumerator FixText()
    {
        LocalizedString description = childRect.GetComponent<LocalizedString>();
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => !description.UpdatingText);
        yield return new WaitForEndOfFrame();
        UpdateRectSize();
    }

    private void UpdateRectSize()
    {
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, childRect.sizeDelta.y + commonOffset + offset);
    }
}