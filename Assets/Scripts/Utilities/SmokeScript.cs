using UnityEngine;
using UnityEngine.UI;

public class SmokeScript : MonoBehaviour
{
    public Image image;
    public float FadeInTime;
    public float FadeOutTime;

    public void FadeIn()
    {
        image.canvasRenderer.SetAlpha(0f);
        image.CrossFadeAlpha(1f, FadeInTime, false);
    }

    public void FadeOut()
    {
        image.canvasRenderer.SetAlpha(1f);
        image.CrossFadeAlpha(0f, FadeOutTime, false);
        Invoke(nameof(Disable), FadeOutTime);
    }

    void Disable()
    {
        gameObject.SetActive(false);
    }
}