using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace com.just.joystick
{
    public class RegionLevelsHandler : MonoBehaviour
    {
        public GameManager gameManager;
        public List<Level> levels = new List<Level>();

        public enum Region { Region1, Region2, Region3 };
        public Region region;

        // Play animation on enable
        void OnEnable()
        {
            GetComponent<Animation>().Play("RegionAnimation");
        }

        // this function will go through each region levels and update their lock/unlock based on the ammount required
        public void UpdateLevels()
        {
            LockAllLevels();

            // Get levels progress from json file and update the current ammount of coins for levels
            if (region == Region.Region1)
                SetLevels(SaveData.GameDataObject.Region1Levels, true);
            else if (region == Region.Region2)
                SetLevels(SaveData.GameDataObject.Region2Levels, false);
            else if (region == Region.Region3)
                SetLevels(SaveData.GameDataObject.Region3Levels, false);
        }

        private void LockAllLevels()
        {
            // Lock all levels first
            for (int i = 0; i < levels.Count; i++)
                levels[i].UpdateLevel(false);
        }

        private void SetLevels(List<LevelData> levelsProgress, bool ignoreTutorial)
        {
            levelsProgress = levelsProgress.OrderBy(level => level.Level).ToList();
            for (int i = 0; i < levels.Count; i++)
            {
                levels[i].CurrentAmount = levelsProgress[i].SeedsCollected;
                bool isFirstLevel = i == 0;
                bool tutorialCompleted = PlayerPrefs.HasKey("TutorialCompleted") || ignoreTutorial;
                bool previousLevelCompleted = !isFirstLevel && levels[i - 1].CurrentAmount > 0;
                bool levelCompleted = levels[i].CurrentAmount > 0;
                levels[i].UpdateLevel((isFirstLevel && tutorialCompleted) || previousLevelCompleted || levelCompleted);
               levels[i].UpdateLevel(true);
            }
        }
    }
}