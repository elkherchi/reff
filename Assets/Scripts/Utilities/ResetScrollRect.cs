using UnityEngine;
using UnityEngine.UI;

public class ResetScrollRect : MonoBehaviour
{
    [SerializeField] ScrollRect ScrollRect;

    private void OnDisable()
    {
        ScrollRect.normalizedPosition = Vector2.up;
    }
}
