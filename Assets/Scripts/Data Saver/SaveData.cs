using AdvancedEncryption;
using LitJson;
using System.IO;
using UnityEngine;

public static class SaveData
{
    // Current version of game
    public static string version = "2.3";
    public static string filename = "GameData.json";

    // Data to return
    public static string dataLoaded;
    public static GameDataV2 GameDataObject;

    private static string FilePath => $"{Application.persistentDataPath}/{filename}";

    public static void CheckMigration()
    {
        // Check if file exists
        if (File.Exists(FilePath))
        {
            if (!IsDataDecrypted())
            {
                CreateJSONFile();
                return;
            }

            JsonData jsonData = MapJSONData();

            if (jsonData == null)
            {
                OverwriteJSON();
                return;
            }

            if (IsJSONCorrupted(jsonData))
            {
                EditorDebugger.Log("Cannot find some json variables! DELETING THE FILE AND CREATING NEW ONE..");
                OverwriteJSON();
                return;
            }

            if (DeviceNotMatching(jsonData))
            {
                EditorDebugger.Log("Device ID HAS BEEN MESSED WITH! DELETED ALL PROGRESS.");
                OverwriteJSON();
                return;
            }

            if (IsJSONOutdated(jsonData))
            {
                EditorDebugger.Log("Some variables are not up to date! Migrating up..");
                MigrateUp(jsonData);
                return;
            }

            EditorDebugger.Log("All JSON Variables are up to date.");
            GameDataObject = JsonUtility.FromJson<GameDataV2>(jsonData.ToJson());
            EditorDebugger.Log(jsonData.ToJson());

            return; // Nope. All variables are up to date. No need for migration.
        }
        else
        {
            EditorDebugger.Log("You're trying to Check Migration and the file does not exist!");
            CreateJSONFile();
            return;
        }
    }

    private static bool IsDataDecrypted()
    {
        string text = File.ReadAllText(FilePath);
        try
        {
            dataLoaded = RijndaelWixel.AES_decrypt(text);
            return true;
        }
        catch
        {
            EditorDebugger.Log("Cannot decrypt the file! UserData has been messed with! Deleting it..");
            return false;
        }
    }

    private static JsonData MapJSONData()
    {
        JsonData jsonData;
        try
        {
            jsonData = JsonMapper.ToObject(dataLoaded);
            return jsonData;
        }
        catch
        {
            EditorDebugger.Log("File is there but cannot map/read the json! UserData FILE HAS BEEN MESSED WITH! DELETED ALL PROGRESS.");
            return null;
        }
    }

    private static bool IsJSONCorrupted(JsonData jsonData)
    {
        return !jsonData.Keys.Contains("version") || !jsonData.Keys.Contains("deviceid");
    }

    private static bool IsJSONOutdated(JsonData jsonData)
    {
        string ver = jsonData["version"].ToString();
        return ver != version;
    }

    private static bool DeviceNotMatching(JsonData jsonData)
    {
        string deviceID = jsonData["deviceid"].ToString();
        return deviceID != SystemInfo.deviceUniqueIdentifier || deviceID == null;
    }

    public static void MigrateUp(JsonData json)
    {
        GameDataV1 oldData = new GameDataV1(json);
        EditorDebugger.Log("OLD Data: " + json.ToJson());
        GameDataObject = new GameDataV2(oldData, version);
        EditorDebugger.Log("New Data: " + JsonUtility.ToJson(GameDataObject));

        DeleteJSONFile();
        SaveJSONFile();
    }

    public static void SaveJSONFile()
    {
        string playerJson = JsonUtility.ToJson(GameDataObject);
        File.WriteAllText(FilePath, RijndaelWixel.AES_encrypt(playerJson));
        EditorDebugger.Log("Saving JSON GameData:" + playerJson);
        EditorDebugger.Log("JSON GameData saved.");
    }

    public static void OverwriteJSON()
    {
        DeleteJSONFile();
        CreateJSONFile();
    }

    public static void CreateJSONFile()
    {
        GameDataObject = new GameDataV2(version);
        SaveJSONFile();
        EditorDebugger.Log("Created a new JSON file.");
    }

    public static void DeleteJSONFile()
    {
        // Handle error
        if (File.Exists(FilePath))
            File.Delete(FilePath);
    }

    public static void SaveLevelProgress(int region, int level, int levelProgress)
    {
        GameDataObject.SetRegionProgress(region, level, levelProgress);
        SaveJSONFile();
    }

    public static bool UnlockComic(string comic)
    {
        bool wasLocked = GameDataObject.AddStory(comic);

        if (wasLocked)
            SaveJSONFile();

        return wasLocked;
    }

    public static bool IsComicUnlocked(string story)
    {
        return GameDataObject.IsStoryUnlocked(story);
    }
}