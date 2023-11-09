using UnityEngine;
using UnityEngine.UI;

public class FixCanvasScale : MonoBehaviour
{
    private void Start()
    {
        CanvasScaler canvasScaler = GetComponent<CanvasScaler>();
        canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        canvasScaler.matchWidthOrHeight = CanvasScaleMatcher.MatchWidthHeight;
    }
}