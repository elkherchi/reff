using System.Collections.Generic;
using UnityEngine;

public class GameDataV2
{
    private const int Region1LevelCount = 7;
    private const int Region2LevelCount = 7;
    private const int Region3LevelCount = 6;
    private const int MinimumCoins = 0;

    [SerializeField] private string version;
    [SerializeField] private string deviceid;
    [SerializeField] private int collected_coins;
    [SerializeField] private string characterselected;
    [SerializeField] private List<LevelData> region1Levels;
    [SerializeField] private List<LevelData> region2Levels;
    [SerializeField] private List<LevelData> region3Levels;
    [SerializeField] private string charactername;
    [SerializeField] private List<string> storiesUnlocked;

    public string Version { get { return version; } set { version = value; } }
    public string DeviceID { get { return deviceid; } set { deviceid = value; } }
    public int CollectedCoins { get { return collected_coins; } set { collected_coins = value; } }
    public string CharacterSelected { get { return characterselected; } set { characterselected = value; } }
    public string CharacterName { get { return charactername; } set { charactername = value; } }
    public List<LevelData> Region1Levels { get { return region1Levels; } set { region1Levels = value; } }
    public List<LevelData> Region2Levels { get { return region2Levels; } set { region2Levels = value; } }
    public List<LevelData> Region3Levels { get { return region3Levels; } set { region3Levels = value; } }
    public List<string> StoriesUnlocked { get { return storiesUnlocked; } set { storiesUnlocked = value; } }

    public bool IsNameSet => charactername != "name";

    public GameDataV2()
    {
        deviceid = SystemInfo.deviceUniqueIdentifier;
        collected_coins = MinimumCoins;
        characterselected = "male";
        charactername = "name";
        storiesUnlocked = new List<string>() { "Default" };
        region1Levels = CreateLevelDataList(Region1LevelCount);
        region2Levels = CreateLevelDataList(Region2LevelCount);
        region3Levels = CreateLevelDataList(Region3LevelCount);
    }

    public GameDataV2(GameDataV1 oldData)
    {
        deviceid = SystemInfo.deviceUniqueIdentifier;
        collected_coins = MinimumCoins;
        int.TryParse(oldData.collected_coins, out collected_coins);
        characterselected = oldData.characterselected;
        charactername = oldData.charactername;
        region1Levels = GetLevelDataList(oldData.region1Levels);
        region2Levels = GetLevelDataList(oldData.region2Levels);
        region3Levels = GetLevelDataList(oldData.region3Levels);
        storiesUnlocked = new List<string>() { "Default" };
    }

    public GameDataV2(string ver) : this()
    {
        version = ver;
    }

    public GameDataV2(GameDataV1 oldData, string ver) : this(oldData)
    {
        version = ver;
        UpdateLevelData();
    }

    private List<LevelData> CreateLevelDataList(int levelCount)
    {
        List<LevelData> levels = new List<LevelData>();
        for (int i = 0; i < levelCount; i++)
            levels.Add(new LevelData(i, 0));

        return levels;
    }

    private List<LevelData> GetLevelDataList(string regionLevels)
    {
        List<LevelData> data = new List<LevelData>();
        string[] levels = regionLevels.Split(',');

        for (int i = 0; i < levels.Length; i++)
        {
            int.TryParse(levels[i], out int seedsCollected);
            data.Add(new LevelData(i, seedsCollected));
        }

        return data;
    }

    private void UpdateLevelData()
    {
        MoveR1L2ToR1L7();
        MoveR2L7ToR2L3();
    }

    private void MoveR1L2ToR1L7()
    {
        LevelData r1l2 = region1Levels[1];
        region1Levels.Remove(r1l2);
        region1Levels.Add(r1l2);
        ReorderRegionLevels(region1Levels);
    }

    private void MoveR2L7ToR2L3()
    {
        LevelData r2l7 = region2Levels[6];
        region2Levels.Remove(r2l7);
        region2Levels.Insert(2, r2l7);
        ReorderRegionLevels(region2Levels);
    }

    private void ReorderRegionLevels(List<LevelData> region)
    {
        for (int i = 0; i < region.Count; i++)
            region[i].Level = i;
    }

    public void SetRegionProgress(int region, int level, int progress)
    {
        switch (region)
        {
            case 1:
                region1Levels[level - 1].SeedsCollected = progress;
                break;
            case 2:
                region2Levels[level - 1].SeedsCollected = progress;
                break;
            case 3:
                region3Levels[level - 1].SeedsCollected = progress;
                break;
        }
    }

    public bool AddStory(string storyID)
    {
        bool wasLocked = false;

        if (!storiesUnlocked.Contains(storyID))
        {
            storiesUnlocked.Add(storyID);
            collected_coins += 10;
            wasLocked = true;
        }

        return wasLocked;
    }

    public bool IsStoryUnlocked(string storyID)
    {
        return storiesUnlocked.Contains(storyID);
    }
}