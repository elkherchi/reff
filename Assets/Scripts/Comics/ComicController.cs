using UnityEngine;
using UnityEngine.UI;

public class ComicController : MonoBehaviour
{
    private const string ComicUnlocked = "ComicUnlocked";
    [SerializeField] GameObject outline;
    [SerializeField] GameObject lockedUI;
    [SerializeField] Image comicImage;
    [SerializeField] string comicID;
    [SerializeField] bool showOutline;
    bool comicUnlocked;

    public Vector2 Position { get; private set; }
    public Sprite ComicSprite { get; private set; }

    public void Setup()
    {
        if (ComicSprite != null)
            return;

        Position = GetComponent<RectTransform>().anchoredPosition;
        ComicSprite = comicImage.sprite;
    }

    private void OnEnable()
    {
        comicUnlocked = SaveData.IsComicUnlocked(comicID);
        outline.SetActive(showOutline && comicUnlocked && !PlayerPrefs.HasKey($"{ComicUnlocked}-{comicID}"));
        lockedUI.SetActive(!comicUnlocked);
    }

    public void ShowComic()
    {
        DisableOutline();
        ComicsManager.Instance.SetCurrentComic(transform.GetSiblingIndex());
        ComicsManager.Instance.ShowAnimatedComic();
    }

    public void DisableOutline()
    {
        PlayerPrefs.SetInt($"{ComicUnlocked}-{comicID}", 1);
    }
}