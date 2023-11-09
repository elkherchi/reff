using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace com.just.joystick
{
    public class Level3Manager : ComicsLevel
    {
        public Text TimerText;

        [Space(5)]
        [Header("Timer")]
        public float timer = 0f;
        private float timerTemp = 0f;

        protected override int Region => 1;
        protected override int Level => 3;
        protected override SpriteRenderer LevelCharacter => character.ChildHat;
        protected override string ComicsID => "2";
        protected override int ComicPage => 1;

        // When the level starts
        protected override void OnEnable()
        {
            timerTemp = timer;
            TimerText.text = LocalizationManager.ConvertNumber(timer.ToString("f2"));
            base.OnEnable();
        }

        protected override void EndGame()
        {
            base.EndGame();
            StopCoroutine(nameof(TimerCoroutine));
        }

        // This function disables the level info
        public override void DisableLevelInfo()
        {
            base.DisableLevelInfo();
            TimerText.text = LocalizationManager.ConvertNumber(timer.ToString("f2"));
            StopCoroutine(nameof(TimerCoroutine));
            StartCoroutine(nameof(TimerCoroutine));
        }

        // Timer system
        IEnumerator TimerCoroutine()
        {
            // Reset timer
            timer = timerTemp;

            while (timer > 0)
            {
                //yield return new WaitForSeconds (1f);
                timer -= Time.deltaTime;

                // Update timer text
                TimerText.text = LocalizationManager.ConvertNumber(timer.ToString("f2"));

                if (timer <= 0)
                {
                    timer = 0f;
                    TimerText.text = LocalizationManager.ConvertNumber(timer.ToString("f2"));
                    Lose();
                    timer = timerTemp;
                    yield break;
                }

                yield return null;
            }
        }

        protected override IEnumerator RestartCoroutine()
        {
            StopCoroutine(nameof(TimerCoroutine));
            timer = timerTemp;

            yield return base.RestartCoroutine();

            // Update timer text
            TimerText.text = LocalizationManager.ConvertNumber(timer.ToString("f2"));
            yield return new WaitUntil(() => FadeScript.Instance.FadedOut);
            StartCoroutine(nameof(TimerCoroutine));
        }
    }
}