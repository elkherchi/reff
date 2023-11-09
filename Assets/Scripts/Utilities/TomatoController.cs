using UnityEngine;

public class TomatoController : MonoBehaviour
{
    private static Camera mainCamera;

    [SerializeField] RectTransform tomatoImage;

    public Vector2 GetAnchors => tomatoImage.anchorMin;

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        tomatoImage.anchoredPosition = Vector3.zero;
    }

    private void LateUpdate()
    {
        Vector3 anchorPosition = mainCamera.WorldToViewportPoint(transform.position);
        tomatoImage.SetAnchors(anchorPosition);
    }
}