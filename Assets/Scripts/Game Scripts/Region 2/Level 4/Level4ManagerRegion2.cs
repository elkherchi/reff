using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace com.just.joystick
{
    public class Level4ManagerRegion2 : ComicsLevel
    {
        [SerializeField] private GameObject tutorialUI;
        [SerializeField] private GameObject circle;
        [SerializeField] private AudioSource tutorialAudio;
        [SerializeField] private EventSystem eventSystem;
        [SerializeField] private GameObject stickerButton;
        [SerializeField] private GameObject preGameMessage;
        [SerializeField] private OnAudioSourceFinished preGameMessageAudio;
        private bool showTutorial;

        protected override int Region => 2;
        protected override int Level => 4;
        protected override SpriteRenderer LevelCharacter => character.ChildHat;
        protected override bool ShowLevelInfo => false;
        protected override string ComicsID => "6";
        protected override int ComicPage => 5;

        protected override void OnEnable()
        {
            base.OnEnable();
            showTutorial = true;
            EventManager.OnR2L4Tutorial += StartTutorial;
            preGameMessage.SetActive(true);
            if (LocalizationManager.IsArabic)
                StartCoroutine(PlayPreGameMessageAudio());
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            EventManager.OnR2L4Tutorial -= StartTutorial;
            preGameMessage.SetActive(false);
        }

        public void EnableLevelInfo()
        {
            preGameMessage.SetActive(false);
            gameManager.EnableLevelInfo(LevelInfo, LevelInfoKey);
        }

        private void StartTutorial()
        {
            if (showTutorial)
            {
                StartCoroutine(ShowTutorial());
                showTutorial = false;
            }
        }

        private IEnumerator ShowTutorial()
        {
            gameManager.ToggleJoystick(false);
            tutorialUI.SetActive(true);
            tutorialUI.transform.SetAsLastSibling();

            if (LocalizationManager.IsArabic)
            {
                tutorialAudio.Play();
                yield return new WaitForAudio(tutorialAudio);
            }

            tutorialUI.transform.SetAsFirstSibling();
            circle.SetActive(true);
            yield return new WaitUntil(() => eventSystem.currentSelectedGameObject == stickerButton);
            circle.SetActive(false);
            tutorialUI.SetActive(false);
            gameManager.ToggleJoystick(true);
            PlayerPrefs.SetInt("HouseTutorial", 1);
        }

        private IEnumerator PlayPreGameMessageAudio()
        {
            yield return new WaitUntil(() => FadeScript.Instance.FadedOut);
            AudioManager.Instance.StartAudioSource(preGameMessageAudio);
        }
    }
}