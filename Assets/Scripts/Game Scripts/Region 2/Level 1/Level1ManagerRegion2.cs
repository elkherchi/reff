using UnityEngine;

namespace com.just.joystick
{
    public class Level1ManagerRegion2 : LevelManager
    {
        protected override int Region => 2;
        protected override int Level => 1;
        protected override SpriteRenderer LevelCharacter => character.ChildHat;
    }
}