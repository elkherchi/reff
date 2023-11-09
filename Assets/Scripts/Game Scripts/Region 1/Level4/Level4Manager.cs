using System.Collections;
using UnityEngine;

namespace com.just.joystick
{
    public class Level4Manager : LevelManager
    {
        public GameObject jumpButton;

        protected override int Region => 1;
        protected override int Level => 4;
        protected override SpriteRenderer LevelCharacter => character.ChildHat;

        protected override void OnEnable()
        {
            base.OnEnable();
            character.OnCollectAnimationEnd();
        }

        protected override IEnumerator RestartCoroutine()
        {
            character.OnCollectAnimationEnd();
            yield return base.RestartCoroutine();
            character.OnCollectAnimationEnd();
            gameManager.ToggleJoystick(true);
        }

        public void WrongAnswer()
        {
            if (!character.CollectAnimationEnded)
                return;

            // remove life
            character.RemoveLife();
            character.TriggerBend();
        }

        public void CorrectAnswer(GameObject go)
        {
            if (!character.CollectAnimationEnded)
                return;

            // disable the chilren of this crop, which are images
            for (int i = 0; i < go.transform.childCount; i++)
            {
                go.transform.GetChild(i).gameObject.SetActive(false);
            }

            character.TriggerSquat();
            StartCoroutine(nameof(LateCorrectAnswer), go);
        }

        IEnumerator LateCorrectAnswer(GameObject go)
        {
            yield return new WaitForSeconds(0.2f);
            go.GetComponent<SpriteRenderer>().enabled = false;
            go.GetComponent<BoxCollider2D>().enabled = false;

            yield return new WaitUntil(() => character.CollectAnimationEnded);

            gameManager.ToggleJoystick(true);
            character.enableSwipeController = true;

            //Re enable the jump button
            jumpButton.SetActive(true);
            yield return null;
        }
    }
}