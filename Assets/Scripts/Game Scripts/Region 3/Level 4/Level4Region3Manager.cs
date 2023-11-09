using System.Collections;
using UnityEngine;

namespace com.just.joystick
{

    public class Level4Region3Manager : ComicsLevel
    {
        public GameObject jumpButton;
        public ThermometerScript thermometerLine;

        [Space(10)]
        [Header("Smoke references")]
        public SmokeScript smoke1;
        public SmokeScript smoke2;

        protected override int Region => 3;
        protected override int Level => 4;
        protected override SpriteRenderer LevelCharacter => character.AdultMask;
        protected override string ComicsID => "10";
        protected override int ComicPage => 9;

        // When the level starts
        protected override void OnEnable()
        {
            base.OnEnable();

            EventManager.OnSmokeDetectEvent += Smoke;
            EventManager.OnSmokeStopEvent += DisableSmoke;

            // Reset thermometer
            thermometerLine.ResetThermometer();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            EventManager.OnSmokeDetectEvent -= Smoke;
            EventManager.OnSmokeStopEvent -= DisableSmoke;
        }

        IEnumerator ThermometerCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(1.5f);
                yield return new WaitUntil(() => thermometerLine.transform.localScale.y >= 1);
                character.RemoveLife();
            }
        }

        // This function is called when the player enters smoke area
        void Smoke()
        {
            // Play smoke 1 anim
            smoke1.gameObject.SetActive(true);
            smoke1.FadeIn();
            // Play smoke2 anim
            smoke2.gameObject.SetActive(true);
            smoke2.FadeIn();
            // Play thermometer line scale
            thermometerLine.IncreaseThermometer();
            EditorDebugger.Log("YES IN HERE");
        }

        void DisableSmoke()
        {
            // Stop smokes
            smoke1.FadeOut();
            smoke2.FadeOut();
            // Reset thermometer line
            thermometerLine.DecreaseThermometer();
        }

        protected override IEnumerator RestartCoroutine()
        {
            yield return base.RestartCoroutine();

            // Reset thermometer
            thermometerLine.ResetThermometer();

            DisableSmoke();

            StopCoroutine(nameof(ThermometerCoroutine));
            StartCoroutine(nameof(ThermometerCoroutine));
        }
    }
}