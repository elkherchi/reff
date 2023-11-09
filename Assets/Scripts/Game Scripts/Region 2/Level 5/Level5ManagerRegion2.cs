using System.Collections;
using UnityEngine;

namespace com.just.joystick
{

    public class Level5ManagerRegion2 : LevelManager
    {
        protected override int Region => 2;
        protected override int Level => 5;
        protected override SpriteRenderer LevelCharacter => character.ChildNoHat;

        // This function applies the hat on the character
        public void ToggleCoat()
        {
            character.ToggleCoat();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            ResetCharacter();
        }

        private void ResetCharacter()
        {
            character.ResetLifes();
            character.DisableAllAnimations();
            character.EnableAnimation(LevelCharacter);
            character.StopAllCoroutines();
        }

        public override void DisableLevelInfo()
        {
            base.DisableLevelInfo();
            ResetCharacter();
            character.StartLifeCoroutine();
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