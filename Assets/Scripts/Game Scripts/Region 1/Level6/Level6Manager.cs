using UnityEngine;

namespace com.just.joystick
{
    public class Level6Manager : LevelManager
    {
        protected override int Region => 1;
        protected override int Level => 6;
        protected override SpriteRenderer LevelCharacter => character.ChildHat;
    }
}