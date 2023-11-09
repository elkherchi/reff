using UnityEngine;
using UnityEngine.UI;

public class GardenLevel : MonoBehaviour
{
    public GameObject GardenUILevel;
    public Text seedsText;
    public GameObject[] Trees;

    [Header("Tree references")]
    public int treesToSeed;
    private int seededTrees;

    #region       M A I N         F U N C T I O N S

    public void TurnOnGardenLevel()
    {
        LoadSavedTrees();
        GardenUILevel.SetActive(true);
        seedsText.text = LocalizationManager.ConvertNumber(SaveData.GameDataObject.CollectedCoins.ToString());
    }

    public void TurnOnGardenLevelDelayed()
    {
        Invoke(nameof(TurnOnGardenLevel), 0.15f);
    }

    // This function creates a tree at a random location
    public void InsertTree(GameObject go)
    {
        int collected_coins = SaveData.GameDataObject.CollectedCoins;
        TreePrice tree = go.GetComponent<TreePrice>();
        int treePrice = tree.price;
        // Check if we have enough money
        if (collected_coins >= treePrice && !go.activeSelf)
        {
            tree.TurnOnTree();
            int collected_coins_new = collected_coins - treePrice;
            SaveData.GameDataObject.CollectedCoins = collected_coins_new;
            SaveData.SaveJSONFile();
            seededTrees++;
            PlayerPrefs.SetString(go.name, "1");
            seedsText.text = LocalizationManager.ConvertNumber(collected_coins_new.ToString());
        }
    }

    public void TurnOffGardenLevel()
    {
        GardenUILevel.SetActive(false);
    }

    public void TurnOffGardenLevelDelayed()
    {
        Invoke(nameof(TurnOffGardenLevel), 0.15f);
    }

    void LoadSavedTrees()
    {
        for (int i = 0; i < Trees.Length; i++)
        {
            if (PlayerPrefs.HasKey(Trees[i].name))
            {
                Trees[i].GetComponent<TreePrice>().TurnOnTree();
            }
            else
            {
                Trees[i].SetActive(false);
            }
        }
    }

    #endregion
}