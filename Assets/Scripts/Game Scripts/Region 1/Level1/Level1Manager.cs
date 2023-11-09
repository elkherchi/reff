using System.Collections;
using UnityEngine;

namespace com.just.joystick
{
    public class Level1Manager : ComicsLevel
    {
        public GameObject TutorialUI;
        public GameObject MoveTutorial;
        public AudioSource MoveSource;
        public GameObject JumpTutorial;
        public AudioSource JumpSource;
        public GameObject HatTutorial;
        public AudioSource HatSource;
        public GameObject HatCircle;
        public GameObject JumpButton;

        protected override int Region => 1;
        protected override int Level => 1;
        protected override bool ShowLevelInfo => false;
        protected override SpriteRenderer LevelCharacter => character.ChildNoHat;
        protected override string ComicsID => "1";
        protected override int ComicPage => 0;

        // This function applies the hat on the character
        protected override void OnEnable()
        {
            base.OnEnable();
            if (!PlayerPrefs.HasKey("TutorialCompleted"))
                StartCoroutine(StartTutorial());
            else
                gameManager.EnableLevelInfo(LevelInfo, LevelInfoKey);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            ResetCharacter();
        }

        public void ToggleHat()
        {
            character.ToggleHat();
        }

        public override void DisableLevelInfo()
        {
            base.DisableLevelInfo();
            ResetCharacter();
            character.StartLifeCoroutine();
        }

        private void ResetCharacter()
        {
            character.ResetLifes();
            character.DisableAllAnimations();
            character.EnableAnimation(LevelCharacter);
            character.StopAllCoroutines();
        }

        IEnumerator StartTutorial()
        {
            //Wait for fade out
            yield return new WaitUntil(() => FadeScript.Instance.FadedOut);

            //Wait for player move audio
            TutorialUI.transform.SetAsLastSibling();
            Vector3 startPosition = character.transform.position;
            TutorialUI.SetActive(true);
            MoveTutorial.SetActive(true);
            JumpTutorial.SetActive(false);
            HatTutorial.SetActive(false);
            HatCircle.SetActive(false);
            JumpButton.SetActive(false);
            gameManager.ToggleJoystick(false);
            yield return new WaitForAudio(MoveSource);

            //Wait for player to move 2 units
            gameManager.ToggleJoystick(true);
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            TutorialUI.SetActive(false);
            MoveTutorial.SetActive(false);
            yield return new WaitUntil(() => (character.transform.position - startPosition).sqrMagnitude > 4);

            //Wait for jump audio
            gameManager.ToggleJoystick(false);
            TutorialUI.SetActive(true);
            JumpTutorial.SetActive(true);
            yield return new WaitForAudio(JumpSource);

            //Wait for player to jump and land
            JumpButton.SetActive(true);
            yield return new WaitUntil(() => !character.grounded);
            TutorialUI.SetActive(false);
            JumpTutorial.SetActive(false);
            yield return new WaitForSeconds(0.2f);
            yield return new WaitUntil(() => character.grounded);

            //Wait for hat audio source
            TutorialUI.SetActive(true);
            JumpButton.SetActive(false);
            HatTutorial.SetActive(true);
            HatCircle.SetActive(true);
            yield return new WaitForAudio(HatSource);

            //Wait for player to wear hat
            TutorialUI.transform.SetAsFirstSibling();
            yield return new WaitUntil(() => character.IsHatOn);
            HatCircle.SetActive(false);
            TutorialUI.SetActive(false);
            JumpButton.SetActive(true);
            yield return new WaitUntil(() => !character.IsHatOn);

            //Reset player then wait to start level
            character.StopAllCoroutines();
            character.healthBar.SetActive(false);
            yield return new WaitForSeconds(0.25f);
            ResetCharacter();
            gameManager.EnableLevelInfo(LevelInfo, LevelInfoKey);
            PlayerPrefs.SetInt("TutorialCompleted", 1);
        }

        protected override IEnumerator RestartCoroutine()
        {
            yield return base.RestartCoroutine();
            yield return new WaitUntil(() => FadeScript.Instance.FadedOut);
            ResetCharacter();
            character.StartLifeCoroutine();
        }
    }
}