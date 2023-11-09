using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeScript : MonoBehaviour
{
    public static FadeScript Instance { get; private set; }

    public Image image;
    public bool FadedOut { private set; get; }
    public bool FadedIn { private set; get; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        image.color = Color.black;
        gameObject.SetActive(false);
    }

    public void StartFadeInOut()
    {
        gameObject.SetActive(true);
        StartCoroutine(FadeInOut());
    }

    private IEnumerator FadeInOut()
    {
        FadedOut = false;
        FadedIn = false;
        FadeIn();
        yield return new WaitForSeconds(1.5f);
        FadedIn = true;
        FadeOut();
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
        FadedIn = false;
        FadedOut = true;
    }

    private void FadeIn()
    {
        image.canvasRenderer.SetAlpha(0f);
        image.CrossFadeAlpha(1f, 0.5f, false);
    }

    private void FadeOut()
    {
        image.canvasRenderer.SetAlpha(1f);
        image.CrossFadeAlpha(0f, 0.5f, false);
    }
}