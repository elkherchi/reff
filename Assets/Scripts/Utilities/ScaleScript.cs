using UnityEngine;
using DG.Tweening;

public class ScaleScript : MonoBehaviour
{
    public float localScale;
    private Vector3 initialScale;

    private void Awake()
    {
        initialScale = transform.localScale;
    }

    private void OnEnable()
    {
        transform.DOScale(transform.localScale.x + localScale, 0.5f).SetLoops(-1, LoopType.Yoyo).SetUpdate(true);
    }

    private void OnDisable()
    {
        transform.DOKill();
        transform.localScale = initialScale;
    }
}