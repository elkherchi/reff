using System.Collections;
using UnityEngine;

namespace com.just.joystick
{
    public class Level2Manager : LevelManager
    {
        protected override int Region => 1;
        protected override int Level => 2;
        protected override SpriteRenderer LevelCharacter => character.ChildHat;

        // When the level starts
        protected override void OnEnable()
        {
            base.OnEnable();

            character.healthBar.SetActive(true);
            DrinkWater();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            ResetCharacter();
        }

        //Fill character healthbar
        public void DrinkWater()
        {
            character.redHealthBar.transform.localScale = new Vector3(1, 1, 1);
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

        protected override IEnumerator RestartCoroutine()
        {
            yield return base.RestartCoroutine();
            yield return new WaitUntil(() => FadeScript.Instance.FadedOut);
            ResetCharacter();
            character.StartLifeCoroutine();
        }
    }
}