using UnityEngine;

namespace com.just.joystick
{
    public class Level2ManagerRegion2 : ComicsLevel
    {
        protected override int Region => 2;
        protected override int Level => 2;
        protected override SpriteRenderer LevelCharacter => character.ChildHat;
        protected override string ComicsID => "5";
        protected override int ComicPage => 4;
    }
}