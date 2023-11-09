using UnityEngine;

namespace com.just.joystick
{
    public class Level5Manager : ComicsLevel
    {
        protected override int Region => 1;
        protected override int Level => 5;
        protected override SpriteRenderer LevelCharacter => character.ChildHat;
        protected override string ComicsID => "3";
        protected override int ComicPage => 2;
    }
}