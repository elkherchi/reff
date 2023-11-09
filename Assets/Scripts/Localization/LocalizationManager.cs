using ArabicSupport;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

public static class LocalizationManager
{
    public enum Language
    {
        English,
        Arabic
    }

    private static Dictionary<string, string> english;
    private static Dictionary<string, string> arabic;
    private static Language language;
    private const string Key = "Language";
    public delegate void LanguageChanged(bool isArabic);
    public static event LanguageChanged OnLanguageChanged;

    public static bool IsArabic => language == Language.Arabic;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Initialize()
    {
        SceneManager.sceneLoaded += (scene, loadMode) => GetLocalizedText();
        LoadJson();
        language = (Language)PlayerPrefs.GetInt(Key, (int)Language.Arabic);
        GetLocalizedText();
    }

    public static void GetLocalizedText()
    {
        LocalizedText[] texts = Object.FindObjectsOfType<LocalizedText>(true);
        foreach (LocalizedText text in texts)
            text.SubscribeToEvent();

        RaiseLanguageChanged();
    }

    private static void LoadJson()
    {
        string path;

        path = "Text/en";
        TextAsset json = Resources.Load<TextAsset>(path);
        TextValues englishText = JsonUtility.FromJson<TextValues>(json.text);

        path = "Text/ar";
        json = Resources.Load<TextAsset>(path);
        TextValues arabicText = JsonUtility.FromJson<TextValues>(json.text);

        arabic = new Dictionary<string, string>();
        english = new Dictionary<string, string>();

        FieldInfo[] textFields = typeof(TextValues).GetFields();

        foreach (FieldInfo field in textFields)
        {
            string fixedArabic = field.GetValue(arabicText).ToString();
            fixedArabic = ArabicFixer.Fix(fixedArabic, false, true);
            arabic.Add(field.Name, fixedArabic);
            english.Add(field.Name, field.GetValue(englishText).ToString());
        }
    }

    public static void ChangeLanguage(Language selectedLanguage)
    {
        if (language == selectedLanguage)
            return;

        language = selectedLanguage;
        SaveSelectedLanguage();
        RaiseLanguageChanged();
    }

    private static void RaiseLanguageChanged() => OnLanguageChanged?.Invoke(IsArabic);
    public static void SaveSelectedLanguage() => PlayerPrefs.SetInt(Key, (int)language);

    public static string GetFromDictionary(string fieldName)
    {
        return language switch
        {
            Language.English => english[fieldName],
            Language.Arabic => arabic[fieldName],
            _ => throw new System.ArgumentException(),
        };
    }

    public static string GetWonLevel(int level)
    {
        return language switch
        {
            Language.English => $"{english["WonLevel"]} {level}!",
            Language.Arabic => $"!{ArabicFixer.Fix(level.ToString(), true, true)} {arabic["WonLevel"]}",
            _ => throw new System.ArgumentException(),
        };
    }

    public static string GetFallDown()
    {
        return language switch
        {
            Language.English => english["FallDownLost"],
            Language.Arabic => arabic["FallDownLost"],
            _ => throw new System.ArgumentException(),
        };
    }

    public static string ConvertNumber(string text)
    {
        if (!IsArabic)
            return text;

        return ArabicFixer.Fix(text, true, true);
    }

    #region EDITOR FUNCTIONS
#if UNITY_EDITOR

    [UnityEditor.MenuItem("Localization/Create Enum")]
    public static void CreateEnum()
    {
        string path = $"{Application.dataPath}/Scripts/Localization/TextValuesEnum.cs";

        StringBuilder enumBuilder = new StringBuilder();
        enumBuilder.Append("public enum TextValuesEnum \n{\n");

        var info = typeof(TextValues).GetFields().OrderBy(x => x.Name);

        foreach (FieldInfo i in info)
        {
            enumBuilder.Append($"\t{i.Name},");
            enumBuilder.AppendLine();
        }

        enumBuilder.Append("}");

        File.WriteAllText(path, enumBuilder.ToString());
    }

    [UnityEditor.MenuItem("Localization/Create JSON Template")]
    public static void CreateJSON()
    {
        TextValues txt = new TextValues();
        string json = JsonUtility.ToJson(txt);

        string path = $"{Application.dataPath}/Resources/Text/Template.json";

        File.WriteAllText(path, json);
    }

    [UnityEditor.MenuItem("Localization/Order Text Values")]
    public static void OrderTextValues()
    {
        string path = $"{Application.dataPath}/Scripts/Localization/TextValues.cs";
        StringBuilder textValuesBuilder = new StringBuilder();
        textValuesBuilder.Append("[System.Serializable]\npublic class TextValues\n{\n");

        List<FieldInfo> fields = GetFields();

        foreach (FieldInfo field in fields)
            textValuesBuilder.Append($"\tpublic string {field.Name};\n");

        textValuesBuilder.Append("}");

        File.WriteAllText(path, textValuesBuilder.ToString());
    }

    [UnityEditor.MenuItem("Localization/Check Localized Strings")]
    public static void CheckLocalizedStrings()
    {
        LocalizedString[] localizedStrings = Object.FindObjectsOfType<LocalizedString>(true);
        List<string> validKeys = new List<string>();
        bool allKeysValid = true;

        foreach (FieldInfo field in typeof(TextValues).GetFields())
            validKeys.Add(field.Name);

        foreach (LocalizedString localizedString in localizedStrings)
        {
            if (!validKeys.Contains(localizedString.StringKey))
            {
                EditorDebugger.Log($"{localizedString.name} has an invalid key", localizedString.gameObject);
                allKeysValid = false;
            }
        }

        if (allKeysValid)
            EditorDebugger.Log("All keys valid");
    }

    private static List<FieldInfo> GetFields()
    {
        return typeof(TextValues).GetFields().OrderBy(field => field.Name).ToList();
    }

#endif
    #endregion
}