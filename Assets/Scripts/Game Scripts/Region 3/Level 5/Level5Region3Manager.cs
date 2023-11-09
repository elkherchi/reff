using UnityEngine;

namespace com.just.joystick
{
    public class Level5Region3Manager : ComicsLevel
    {
        protected override int Region => 3;
        protected override int Level => 5;
        protected override SpriteRenderer LevelCharacter => character.AdultHat;
        protected override string ComicsID => "11";
        protected override int ComicPage => 10;
    }
}