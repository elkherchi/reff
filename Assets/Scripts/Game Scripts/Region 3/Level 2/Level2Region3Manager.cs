using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace com.just.joystick
{
    public class Level2Region3Manager : LevelManager
    {
        public GameObject jumpButton;
        public GameObject levelInfoBackground;
        public GameObject winMessage;
        public Text Timer;

        public float levelDuration;
        public float moveDuration;
        public int CorrectAnswers;
        private float currentTime;
        private int currentAnswers = 0;

        protected override int Region => 3;
        protected override int Level => 2;
        protected override bool HasCharacter => false;
        protected override SpriteRenderer LevelCharacter => throw new System.NotImplementedException();

        #region  E V E N T S

        // When the level starts
        protected override void OnEnable()
        {
            base.OnEnable();

            currentAnswers = 0;
            gameManager.SetCharacterR3L2Info();
            winMessage.SetActive(false);
            levelInfoBackground.SetActive(true);

            // disable the jump button
            jumpButton.SetActive(false);
        }

        public override void DisableLevelInfo()
        {
            base.DisableLevelInfo();
            SetVariables();
            levelInfoBackground.SetActive(false);
            StartCoroutine(nameof(TimerCountDown));
        }

        public override void Win()
        {
            if (winMessage.activeSelf)
                base.Win();

            canPause = false;
            winMessage.SetActive(!winMessage.activeSelf);
            gameManager.SetCharacterR3L2Win();
        }

        #endregion

        #region       M A I N         F U N C T I O N S 

        private void SetVariables()
        {
            //Set character image
            gameManager.SetCharacterR3L2Searching();

            //Reset level score
            currentAnswers = 0;
            gameManager.seedsCollected = 0;
            gameManager.SetSeedsText(level.AmountToCollect);

            //Reset timer text
            currentTime = levelDuration;
            Timer.text = LocalizationManager.ConvertNumber(currentTime.ToString("f2"));

            // reset lifes
            gameManager.ResetLifesUI();
        }

        protected override IEnumerator RestartCoroutine()
        {
            //Reset level
            StopCoroutine(nameof(TimerCountDown));
            StopCoroutine(nameof(ShowHappyCharacter));
            yield return base.RestartCoroutine();

            //Update level variables
            SetVariables();

            //Wait for fade out then start timer
            yield return new WaitUntil(() => FadeScript.Instance.FadedOut);
            StartCoroutine(nameof(TimerCountDown));
        }

        IEnumerator TimerCountDown()
        {
            while (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
                Timer.text = LocalizationManager.ConvertNumber(currentTime.ToString("f2"));
                yield return null;
            }

            currentTime = 0;
            Timer.text = currentTime.ToString("f2");
            EventManager.RaiseLoseEvent();
        }

        public void CorrectAnswer(ItemController item)
        {
            //Tween item to box
            item.GoToTarget(IncrementScore, moveDuration);

            //Show happy character
            StopCoroutine(nameof(ShowHappyCharacter));
            StartCoroutine(nameof(ShowHappyCharacter));

            //Stop timer once all elements are found
            if (currentAnswers == CorrectAnswers)
                StopCoroutine(nameof(TimerCountDown));
        }

        private void IncrementScore()
        {
            currentAnswers++;
            PlayerHit("Seed");

            if (currentAnswers == CorrectAnswers)
                StopCoroutine(nameof(TimerCountDown));
        }

        IEnumerator ShowHappyCharacter()
        {
            gameManager.SetCharacterR3L2Happy();
            yield return new WaitForSeconds(moveDuration * 2);
            gameManager.SetCharacterR3L2Searching();
            if (currentAnswers == CorrectAnswers)
                EventManager.RaiseWinEvent();
        }

        #endregion
    }
}