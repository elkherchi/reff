using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.just.joystick
{
    public class Level6Region3Manager : LevelManager
    {
        [SerializeField] private List<GameObject> flyers;
        [SerializeField] private List<FlyerButtons> flyersButtons;
        [SerializeField] private List<string> stringKeys;
        [SerializeField] private Text flyerText;
        [SerializeField] private GameObject levelInfoUI;
        [SerializeField] private GameObject winScreen;
        [SerializeField] private OnAudioSourceFinished congratulations;
        [SerializeField] private Font english;
        [SerializeField] private Font arabic;
        private int currentFlyer;

        protected override bool ShowLevelInfo => false;
        protected override int Region => 3;
        protected override int Level => 6;
        protected override SpriteRenderer LevelCharacter => throw new System.NotImplementedException();
        protected override bool HasCharacter => false;


        protected override void OnEnable()
        {
            base.OnEnable();
            ResetFlyers();
            gameManager.SetCharacterR3L6Dirty();
            winScreen.SetActive(false);
            levelInfoUI.SetActive(true);
            flyerText.font = LocalizationManager.IsArabic ? arabic : english;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            AudioManager.Instance.StopR3L6Flyer();
        }

        public void RaiseWinEvent()
        {
            EventManager.RaiseWinEvent();

            if (LocalizationManager.IsArabic)
                congratulations.StartAudioSource();
        }

        public void RaiseLoseEvent()
        {
            EventManager.RaiseLoseEvent();
            AudioManager.Instance.StopR3L6Flyer();
        }

        protected override IEnumerator RestartCoroutine()
        {
            gameManager.SetCharacterR3L6Dirty();
            winScreen.SetActive(false);
            yield return base.RestartCoroutine();
            ResetFlyers();
        }

        public void GoToNextFlyer()
        {
            PlayerHit("Seed");
            flyers[currentFlyer].SetActive(false);
            currentFlyer += 1;
            flyers[currentFlyer].SetActive(true);
            flyerText.text = LocalizationManager.GetFromDictionary(stringKeys[currentFlyer]);

            if (LocalizationManager.IsArabic)
            {
                StartCoroutine(FixArabicText.FixText(flyerText, flyerText.text));
                PlayFlyerAudio();
            }
        }

        private void ResetFlyers()
        {
            for (int i = 0; i < flyers.Count; i++)
                flyers[i].SetActive(i == 0);

            currentFlyer = 0;
            flyerText.text = LocalizationManager.GetFromDictionary(stringKeys[currentFlyer]);

            if (LocalizationManager.IsArabic)
                StartCoroutine(FixArabicText.FixText(flyerText, flyerText.text));
        }

        public void ShowWinScreen()
        {
            AudioManager.Instance.StopR3L6Flyer();
            PlayerHit("Seed");
            gameManager.SetCharacterR3L6Bed();
            winScreen.SetActive(true);
        }

        public void HideLevelInfo()
        {
            levelInfoUI.SetActive(false);

            if (LocalizationManager.IsArabic)
                PlayFlyerAudio();
        }

        private void PlayFlyerAudio()
        {
            AudioManager.Instance.StopR3L6Flyer();
            gameManager.PlayR3L6FlyerAudio($"R3L6Flyer{currentFlyer + 1}");
        }

        public void DisableButtons()
        {
            flyersButtons[currentFlyer].SetButtonState(false);
        }

        public void EnableButtons()
        {
            flyersButtons[currentFlyer].SetButtonState(true);
        }
    }
}

[System.Serializable]
public class FlyerButtons
{
    public Button wrong;
    public Button correct;

    public void SetButtonState(bool state)
    {
        wrong.interactable = state;
        correct.interactable = state;
    }
}