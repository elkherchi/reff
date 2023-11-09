using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComicsManager : MonoBehaviour
{
    public static ComicsManager Instance { get; private set; }

    [SerializeField] Button next;
    [SerializeField] Button back;
    [SerializeField] GameObject comicsGallery;
    [SerializeField] GameObject comicsPage;
    [SerializeField] List<ComicController> comicsButtons;
    [SerializeField] List<ComicController> comics;
    [SerializeField] GameObject touchBlocker;
    [SerializeField] RectTransform animatedComic;
    [SerializeField] Image animatedComicImage;
    [SerializeField] float comicAnimationDuration;
    [SerializeField] float comicAnimationDelay;
    private int currentComic;
    private int levelComic;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ResetComicsUI()
    {
        comicsGallery.SetActive(true);
        comicsPage.SetActive(false);
    }

    public void ShowAnimatedComic(bool useLevelComic = false) => StartCoroutine(nameof(AnimateComic), useLevelComic);

    private IEnumerator AnimateComic(bool useLevelComic)
    {
        if (useLevelComic)
            currentComic = levelComic;

        //Prevent user from pressing buttons
        touchBlocker.SetActive(true);
        yield return new WaitForSecondsRealtime(comicAnimationDelay);

        //Set up comic target and initial position
        ComicController controller = comicsButtons[currentComic];
        controller.Setup();
        Vector2 target = animatedComic.anchoredPosition;
        Vector2 initial = controller.Position;

        //Update animated comic image and position
        animatedComicImage.sprite = controller.ComicSprite;
        animatedComic.anchoredPosition = initial;
        animatedComic.gameObject.SetActive(true);

        //Disable current comic outline
        controller.DisableOutline();

        //Start comic animation
        animatedComic.DOScale(4.5f, comicAnimationDuration).SetUpdate(true);
        animatedComic.DOAnchorPos(target, comicAnimationDuration).SetUpdate(true).OnComplete(ShowComics);
    }

    //Set comic the user is currently looking at
    public void SetCurrentComic(int comic)
    {
        currentComic = comic;
    }

    //Set comic of the current level
    public void SetLevelComic(int comic)
    {
        levelComic = comic;
    }

    public void ShowComics()
    {
        //Hide animated comic and enable pressing buttons
        animatedComic.localScale = Vector3.one;
        animatedComic.gameObject.SetActive(false);
        touchBlocker.SetActive(false);

        //Show comics page ui
        comicsGallery.SetActive(false);
        comicsPage.SetActive(true);

        CheckButtonInteraction();

        //Show selected comic
        for (int i = 0; i < comics.Count; i++)
            comics[i].gameObject.SetActive(i == currentComic);
    }

    public void NextComic()
    {
        comics[currentComic].gameObject.SetActive(false);
        currentComic += 1;
        comics[currentComic].gameObject.SetActive(true);
        comics[currentComic].DisableOutline();
        CheckButtonInteraction();
    }

    public void PreviousComic()
    {
        comics[currentComic].gameObject.SetActive(false);
        currentComic -= 1;
        comics[currentComic].gameObject.SetActive(true);
        comics[currentComic].DisableOutline();
        CheckButtonInteraction();
    }

    private void CheckButtonInteraction()
    {
        back.interactable = currentComic > 0;
        next.interactable = currentComic + 1 < comics.Count;
    }
}