using UnityEngine;
using UnityEngine.UI;
using com.just.joystick;

public class Level : MonoBehaviour
{
    public GameObject LockUI;
    public GameObject finishedEmblem;
    public LevelInfo LevelData;

    public int AmountToCollect => LevelData.AmountToCollect; // ammount required to collect in this level
    public int CurrentAmount { get; set; } // the current progress of this level

    // This function will enable/disable the lockUI based on the given bool
    public void UpdateLevel(bool levelUnlocked)
    {
        LockUI.SetActive(!levelUnlocked);
        GetComponent<Button>().enabled = levelUnlocked;

        // Check for emblem
        finishedEmblem.SetActive(CurrentAmount == AmountToCollect);
    }

    public void LoadLevel()
    {
        GameManager.Instance.LoadLevel(LevelData.Level);
    }
}