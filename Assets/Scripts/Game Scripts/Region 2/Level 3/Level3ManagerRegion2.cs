using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.just.joystick
{
    public class Level3ManagerRegion2 : LevelManager
    {
        [SerializeField] private List<GameObject> flyers;
        private int currentFlyer;

        protected override int Region => 2;
        protected override int Level => 3;
        protected override SpriteRenderer LevelCharacter => throw new System.NotImplementedException();
        protected override bool HasCharacter => false;
        protected override bool ShowLevelInfo => false;

        protected override void OnEnable()
        {
            base.OnEnable();

            //Set r2l3 story character reset order then show the story
            gameManager.SetR2L3Story();
            gameManager.ResetR2L3Story();
            gameManager.ToggleR2L3Story(true);
            ResetFlyers();

            if (LocalizationManager.IsArabic)
                StartCoroutine(PlayR3StoryAudio());
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            gameManager.ToggleR2L3Story(false);
        }

        public void EnableLevelInfo()
        {
            gameManager.EnableLevelInfo(LevelInfo, LevelInfoKey);
        }

        public void ShowEndMessage()
        {
            flyers[currentFlyer].SetActive(false);
            PlayerHit("Seed");
            RaiseWinEvent();
        }

        public void RaiseWinEvent()
        {
            EventManager.RaiseWinEvent();
        }

        public void RaiseLoseEvent()
        {
            EventManager.RaiseLoseEvent();
        }

        protected override IEnumerator RestartCoroutine()
        {
            ResetFlyers();
            yield return base.RestartCoroutine();
            gameManager.ToggleJoystick(false);
        }

        private IEnumerator PlayR3StoryAudio()
        {
            yield return new WaitUntil(() => FadeScript.Instance.FadedOut);
            AudioManager.Instance.PlayR2L3Story1();
        }

        public void GoToNextFlyer()
        {
            PlayerHit("Seed");
            flyers[currentFlyer].SetActive(false);
            currentFlyer += 1;
            flyers[currentFlyer].SetActive(true);
        }

        private void ResetFlyers()
        {
            for (int i = 0; i < flyers.Count; i++)
                flyers[i].SetActive(i == 0);

            currentFlyer = 0;
        }
    }
}