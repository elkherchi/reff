using LitJson;

public class GameDataV1
{
    public string version { get; set; }
    public string deviceid { get; set; }
    public string collected_coins { get; set; }
    public string characterselected { get; set; }
    public string region1Levels { get; set; }
    public string region2Levels { get; set; }
    public string region3Levels { get; set; }
    public string charactername { get; set; }
    public string storiesunlocked { get; set; }

    public GameDataV1(JsonData json)
    {
        collected_coins = GetJSONValue(json, "collected_coins", "0");
        CheckCharacterSelected(json);
        string regionsDefault = "0,0,0,0,0,0,0";
        region1Levels = GetJSONValue(json, "region1Levels", regionsDefault);
        region2Levels = GetJSONValue(json, "region2Levels", regionsDefault);
        region3Levels = GetJSONValue(json, "region3Levels", regionsDefault);
        charactername = GetJSONValue(json, "charactername", "name");
        storiesunlocked = GetJSONValue(json, "storiesunlocked", "");
    }

    private void CheckCharacterSelected(JsonData json)
    {
        if (json.Keys.Contains("characterselected"))
        {
            if (json["characterselected"] == null || json["characterselected"].ToString() == "")
            {
                characterselected = "male";
            }
            else
            {
                characterselected = json.Keys.Contains("characterselected") ? json["characterselected"].ToString() : "male";
            }
        }
    }

    private string GetJSONValue(JsonData json, string key, string defaultValue)
    {
        return json.Keys.Contains(key) ? json[key].ToString() : defaultValue;
    }
}