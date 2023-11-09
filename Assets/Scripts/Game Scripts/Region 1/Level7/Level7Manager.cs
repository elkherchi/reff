using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace com.just.joystick
{
    public class Level7Manager : ComicsLevel
    {
        public float MaxTime;
        public GameObject jumpButton;
        public Button reportButton;
        public GameObject endLevelMessage;
        private GameObject currentBully;
        private UnityAction endMessageDisabledCallback;
        public delegate void StopCharacter(GameObject bully);
        public static StopCharacter stop;

        protected override int Region => 1;
        protected override int Level => 7;
        protected override SpriteRenderer LevelCharacter => character.ChildHat;
        protected override string ComicsID => "4";
        protected override int ComicPage => 3;

        protected override void OnEnable()
        {
            base.OnEnable();
            jumpButton.SetActive(true);
            endLevelMessage.SetActive(false);

            //Function that will be called once the player enter a bully trigger
            stop = StopPlayerMovement;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            character.ResetSadBool();
        }

        public void EndLevelMessageDisabled()
        {
            HideEndLevelMessage();
            endMessageDisabledCallback();
        }

        public override void Win()
        {
            ShowEndLevelMessage();
            canPause = false;

            //Call win function when hiding the end level message
            endMessageDisabledCallback = base.Win;
        }

        public override void Lose()
        {
            ShowEndLevelMessage();
            canPause = false;

            //Call lose function when hiding the end level message
            endMessageDisabledCallback = base.Lose;
        }

        private void HideEndLevelMessage()
        {
            endLevelMessage.SetActive(false);
            reportButton.interactable = true;
        }

        private void ShowEndLevelMessage()
        {
            //Play audio
            if (LocalizationManager.IsArabic)
                AudioManager.Instance.StartR1L7Coroutine();

            endLevelMessage.SetActive(true);
            gameManager.SetCharacterBullyMessage();
            character.enableSwipeController = false;
            character.StopCharacterMovement();
            gameManager.ToggleJoystick(false);
        }

        private void StopPlayerMovement(GameObject bully)
        {
            currentBully = bully;
            gameManager.ToggleJoystick(false);
            jumpButton.SetActive(false);
            character.StopCharacterMovement();
            character.MakeCharacterSad();
        }

        public void StopBully()
        {
            if (currentBully != null)
            {
                character.PlayNoParticleSystem();
                StartCoroutine(EnableMovement(currentBully));
                currentBully = null;

                if (LocalizationManager.IsArabic)
                    AudioManager.Instance.PlayChildNo();
            }
        }

        private IEnumerator EnableMovement(GameObject bully)
        {
            yield return new WaitForSeconds(0.5f);
            bully.GetComponent<BullyController>().TurnOff();
            ResetMovement();
        }

        protected override IEnumerator RestartCoroutine()
        {
            character.ResetSadBool();
            yield return base.RestartCoroutine();
            currentBully = null;
            ResetMovement();
        }

        private void ResetMovement()
        {
            gameManager.ToggleJoystick(true);
            jumpButton.SetActive(true);
            character.enableSwipeController = true;
        }
    }
}