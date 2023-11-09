using System.Collections;
using UnityEngine;

namespace com.just.joystick
{
    public class Level1Region3Manager : ComicsLevel
    {
        protected override int Region => 3;
        protected override int Level => 1;
        protected override SpriteRenderer LevelCharacter => character.PecheNotHat;
        protected override bool ShowLevelInfo => true;
      //  protected override string ComicsID => throw new System.NotImplementedException();

     //   protected override int ComicPage => throw new System.NotImplementedException();
         protected override string ComicsID => "0";
          protected override int ComicPage => 0;

        protected override void OnEnable()
        {
            base.OnEnable();
            gameManager.HandleWorld3Notice();
            if (LocalizationManager.IsArabic)
                StartCoroutine(PlayR3StoryAudio());
        }

        public void EnableLevelInfo()
        {
            gameManager.EnableLevelInfo(LevelInfo, LevelInfoKey);
        }

        private IEnumerator PlayR3StoryAudio()
        {
            yield return new WaitUntil(() => FadeScript.Instance.FadedOut);
            AudioManager.Instance.PlayR3L1Story1();
        }
    }
}