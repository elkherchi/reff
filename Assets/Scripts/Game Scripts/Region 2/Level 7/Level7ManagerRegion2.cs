using System.Collections;
using UnityEngine;

namespace com.just.joystick
{
    public class Level7ManagerRegion2 : LevelManager
    {
        protected override int Region => 2;
        protected override int Level => 7;
        protected override SpriteRenderer LevelCharacter => character.ChildHat;

        protected override IEnumerator RestartCoroutine()
        {
            yield return base.RestartCoroutine();
            // Disable all animnations
            character.DisableAllAnimations();
            character.EnableAnimation(LevelCharacter);
        }
    }
}