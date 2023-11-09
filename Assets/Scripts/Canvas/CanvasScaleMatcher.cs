using UnityEngine;

public class CanvasScaleMatcher : MonoBehaviour
{
    private const float referenceAspectRatio = 1137f / 640;
    public static CanvasScaleMatcher Instance { get; private set; }
    public static int MatchWidthHeight { get; private set; }
    public static float AspectMultiplier { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        float screenAspectRatio = (float)Screen.width / Screen.height;
        AspectMultiplier = screenAspectRatio / referenceAspectRatio;
        MatchWidthHeight = referenceAspectRatio < screenAspectRatio ? 1 : 0;
    }
}