using UnityEngine;
using DG.Tweening;
using UnityAction = UnityEngine.Events.UnityAction;

public class ItemController : MonoBehaviour
{
    [SerializeField] private RectTransform rect;
    [SerializeField] private Transform targetPosition;
    private Vector3 initialPosition;

    private void Start()
    {
        Vector2 position = rect.anchoredPosition;
        if (CanvasScaleMatcher.AspectMultiplier > 1)
            position.x *= CanvasScaleMatcher.AspectMultiplier;
        else
            position.y /= CanvasScaleMatcher.AspectMultiplier;

        rect.anchoredPosition = position;
        initialPosition = transform.position;
    }

    private void OnEnable() => EventManager.OnResetEvent += ResetPosition;
    private void OnDisable() => EventManager.OnResetEvent -= ResetPosition;
    public void GoToTarget(UnityAction onComplete, float duration) => transform.DOMove(targetPosition.position, duration).OnComplete(() => onComplete());
    private void ResetPosition() => transform.position = initialPosition;
}