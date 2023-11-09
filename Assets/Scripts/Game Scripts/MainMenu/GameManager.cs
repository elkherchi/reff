using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace com.just.joystick
{

    public class GameManager : MonoBehaviour
    {
        [Header("User Interface References")]
        public MyCharacterController Character;
        public GameObject[] Levels;

        [Space(5)]
        public GameObject SettingsUI;
        public GameObject LanguageUI;
        public GameObject AboutUsUI;
        public GameObject PauseUI;

        [Header("Starting UI")]
        public GameObject LanguageSelection;
        public GameObject TermsOfUse;
        public GameObject CharacterSelection;
        public GameObject GuadrianSelection;
        public GameObject RegionsSelectionUI;
        public GameObject GatherSeeds;
        public GameObject UserIDUI;

        [Header("Game UI")]
        public GameObject loseUI;
        public GameObject winUI;
        public GameObject comicWinUI;
        public Button comicNextLevel;
        public Button comicHomeButton;
        public AudioSource comicAudioSource;
        public GameObject lastLevelWinUI;
        [SerializeField] private Text seedText;
        public GameObject inGameUI;
        public GameObject Region1UI;
        public GameObject Region2UI;
        public GameObject Region3UI;
        public GameObject region3Notifier;
        public GameObject joystick;
        public GameObject jumpButton;
        public BoxCollider2D loseTrigger;
        public List<GameObject> LifesUI = new List<GameObject>();
        public Button loseRestartButton;
        public Button pauseRestartButton;
        public int lifes = 3;
        public int age = 16;
        bool isNameSet = false;
        [HideInInspector]
        public int seedsCollected = 0;

        [Header("Characters Sprites")]
        public CharacterData Boy;
        public CharacterData Girl;
        public CharacterData CharacterX;
        public CharacterData CharacterF;
        public Image characterGuardian;
        public Image characterSeeds;

        [Space(10)]
        [Header("User ID References")]
        public Image characterID;
        public Text characterNameText;

        [Space(10)]
        [Header("Lose Repeat Button")]
        public Image characterRepeat;

        [Space(10)]
        [Header("Region 1 Level 2")]
        public Image characterWinImage;

        [Space(10)]
        [Header("Region 2 Level 3")]
        public Image Story1;
        public Image Story2;
        public Image Story3;

        [Space(10)]
        [Header("Region 3")]
        public Image characterR3L2;
        public Image characterR3L2Info;
        public Image characterWinR3L2;
        public Image characterR3L6;
        public Image characterR3L6Bed;

        [Space(10)]
        [Header("Game UI")]
        public Text winText;
        public Text loseText;
        public Button skipLevel;
        public AudioSource childLost;
        public AudioSource adultLost;
        public GameObject LevelInfo;
        public Text LevelInfoText;
        public int SkipLevelCost = 50;

        [Space(10)]
        [Header("Levels Handler References")]
        public RegionLevelsHandler region1LevelsHandler;
        public RegionLevelsHandler region2LevelsHandler;
        public RegionLevelsHandler region3LevelsHandler;

        [Space(10)]
        [Header("In Game Objective UI")]
        public List<GameObject> objectivesUI = new List<GameObject>();

        [Space(10)]
        [Header("World 3 notice references")]
        public GameObject World3Notice;
        public Image childNotice;
        public Image adultNotice;

        [Space(10)]
        [Header("Comics")]
        public GameObject comicsPage;

        [Space(10)]
        [Header("Garden references")]
        public GardenLevel garden;

        [Header("Fonts")]
        [SerializeField] Font english;
        [SerializeField] Font arabic;

        [HideInInspector]
        // Private variables
        public string characterSelected;
        // Pause game
        private bool gamePaused = false;
        private int levelBeingPlayed = 0;
        private bool forceComicsPage;

        [HideInInspector]
        public GameObject sticker;

        [HideInInspector]
        public GameObject window;
        [HideInInspector]
        public BoxCollider2D smokeDetector;
        [HideInInspector]
        public string oldWindowName = "oldwindow";

        private CharacterData selectedCharacterData;

        public static GameManager Instance { get; private set; }


        #region   S T A R T  /  A W A K  E  /  U P D A T E
        // Use this for initialization
        void Start()
        {
            // Start the game
            StartGame();
            AddButtonListeners();
        }

        private void AddButtonListeners()
        {
            loseRestartButton.onClick.AddListener(ButtonRestart);
            pauseRestartButton.onClick.AddListener(ButtonRestart);
        }

        private void ButtonRestart()
        {
            FadeScript.Instance.StartFadeInOut();
            EventManager.RaiseResetEvent();
        }

        void Awake()
        {
            Application.targetFrameRate = 60;

            // Check if the player has played this game before, if not delete the json file
            if (!PlayerPrefs.HasKey("PlayedBefore"))
            {
                SaveData.DeleteJSONFile();
                EditorDebugger.Log("Player has never played this game before, deleting JsonSave File.");
                PlayerPrefs.SetInt("PlayedBefore", 1);
            }

            if (Instance == null)
                Instance = this;
            else
            {
                Destroy(Instance.gameObject);
                Instance = this;
            }

#if UNITY_EDITOR
            bool isArabic = PlayerPrefs.GetInt("Language", 1) == 1;
            LevelInfoText.font = isArabic ? arabic : english;
            winText.font = isArabic ? arabic : english;
            loseText.font = isArabic ? arabic : english;
#else
            LevelInfoText.font = LocalizationManager.IsArabic ? arabic : english;
            winText.font = LocalizationManager.IsArabic ? arabic : english;
            loseText.font = LocalizationManager.IsArabic ? arabic : english;
#endif
        }

        public void ToggleJoystick(bool toggle) => joystick.SetActive(toggle);

        #endregion

        #region     U S E R     I N T E R F A C E

        public void ChangeToEnglish()
        {
            LocalizationManager.ChangeLanguage(LocalizationManager.Language.English);
            LevelInfoText.font = english;
            winText.font = english;
            loseText.font = english;
        }

        public void ChangeToArabic()
        {
            LocalizationManager.ChangeLanguage(LocalizationManager.Language.Arabic);
            LevelInfoText.font = arabic;
            winText.font = arabic;
            loseText.font = arabic;
        }

        public void SetSeedsText(int totalAmount)
        {
            string text = $"{seedsCollected}/{totalAmount}";
            seedText.text = LocalizationManager.ConvertNumber(text);
        }

        public void ShowGuardianUI()
        {
            TurnOffAllUIs();
            GuadrianSelection.SetActive(true);
            characterGuardian.sprite = selectedCharacterData.Character;
        }

        public void ShowUserIdUI()
        {
            TurnOffAllUIs();
            UserIDUI.SetActive(true);
            characterID.sprite = selectedCharacterData.ID;

            // Set character name
            characterNameText.text = SaveData.GameDataObject.CharacterName;

            if (LocalizationManager.IsArabic)
                characterNameText.alignment = TextAnchor.MiddleRight;
            else
                characterNameText.alignment = TextAnchor.MiddleLeft;
        }

        public void ShowGatherSeeds()
        {
            TurnOffAllUIs();
            GatherSeeds.SetActive(true);
            characterSeeds.sprite = selectedCharacterData.GatherSeedsCharacter;
        }

        public void ToggleSettingsUI()
        {
            SettingsUI.SetActive(!SettingsUI.activeSelf);
        }

        public void ToggleLanguageUI()
        {
            //this.ToggleSettingsUI ();
            LanguageUI.SetActive(!LanguageUI.activeSelf);
        }

        public void QuitApplication()
        {
            Application.Quit();
        }

        public void GotoAboutUs()
        {
            AboutUsUI.SetActive(true);
        }

        public void GotoRegionSelection()
        {
            TurnOffAllUIs();
            RegionsSelectionUI.SetActive(true);
        }

        public void ShowRepeatButton(bool isAdult)
        {
            characterRepeat.sprite = isAdult ? selectedCharacterData.AdultRepeat : selectedCharacterData.Repeat;
        }

        public void SetCharacterBullyMessage()
        {
            characterWinImage.sprite = selectedCharacterData.BullyMessage;
        }

        public void SetCharacterR3L2Info()
        {
            characterR3L2Info.sprite = selectedCharacterData.R3L2CharacterInfo;
        }

        public void SetCharacterR3L2Searching()
        {
            characterR3L2.sprite = selectedCharacterData.R3L2CharacterSearching;
        }

        public void SetCharacterR3L2Happy()
        {
            characterR3L2.sprite = selectedCharacterData.R3L2CharacterHappy;
        }

        public void SetCharacterR3L2Win()
        {
            characterWinR3L2.sprite = selectedCharacterData.R3L2CharacterWin;
        }

        public void SetCharacterR3L6Dirty()
        {
            characterR3L6.sprite = selectedCharacterData.R3L6Dirty;
        }

        public void SetCharacterR3L6Clean()
        {
            characterR3L6.sprite = selectedCharacterData.R3L6Clean;
        }

        public void SetCharacterR3L6Bed()
        {
            characterR3L6Bed.sprite = selectedCharacterData.R3L6Bed;
        }

        public void SetR2L3Story()
        {
            Story1.sprite = selectedCharacterData.R2L3Story1;
            Story2.sprite = selectedCharacterData.R2L3Story2;
            Story3.sprite = selectedCharacterData.R2L3Story3;
        }

        public void ToggleR2L3Story(bool toggle)
        {
            Story1.transform.parent.parent.gameObject.SetActive(toggle);
        }

        public void ResetR2L3Story()
        {
            Story1.transform.parent.gameObject.SetActive(true);
            Story2.transform.parent.gameObject.SetActive(false);
        }

        public void ToggleWinUI(bool active, bool showRegionWin = false)
        {
            winUI.SetActive(active);
            lastLevelWinUI.SetActive(showRegionWin);

            if (showRegionWin)
                AudioManager.Instance.StopWinSources();
        }

        public void ShowComicWinUI(string comicID, bool finalRegion)
        {
            comicWinUI.SetActive(true);
            forceComicsPage = SaveData.UnlockComic(comicID);
            comicNextLevel.interactable = !forceComicsPage && !comicAudioSource.isPlaying;
            comicHomeButton.interactable = comicNextLevel.interactable;
            comicNextLevel.gameObject.SetActive(!finalRegion);
            comicHomeButton.gameObject.SetActive(finalRegion);
        }

        public void SetComicNextLevelInteraction()
        {
            comicNextLevel.interactable = !forceComicsPage;
            comicHomeButton.interactable = comicNextLevel.interactable;
        }

        public void GoToComicPage()
        {
            comicsPage.SetActive(true);
            ComicsManager.Instance.ShowAnimatedComic(true);
            comicNextLevel.interactable = true;
            comicHomeButton.interactable = true;
        }

        public void ToggleLoseUI(bool active)
        {
            loseUI.SetActive(active);

            if (active)
            {
                SetSkipLevelButton();
            }
            else
            {
                AudioManager.Instance.StopLoseSources();
            }
        }

        public void SetSkipLevelButton()
        {
            LevelManager level = Levels[levelBeingPlayed].GetComponent<LevelManager>();
            skipLevel.gameObject.SetActive(!level.finalRegionLevel && level.level.CurrentAmount == 0);
            skipLevel.interactable = SaveData.GameDataObject.CollectedCoins >= SkipLevelCost && !childLost.isPlaying && !adultLost.isPlaying;
        }

        public void SetWinText(string text, int region, int level)
        {
            winText.text = text;
            if (LocalizationManager.IsArabic)
            {
                if (region != 3)
                {
                    try
                    {
                        AudioClip clip = selectedCharacterData.ChildAudio.LevelWin[level];
                        AudioManager.Instance.PlayChildWin(clip);
                    }
                    catch (System.Exception)
                    {
                        EditorDebugger.LogWarning($"{level} has no win voice over");
                    }
                }
                else
                {
                    try
                    {
                        AudioClip clip = selectedCharacterData.AdultAudio.LevelWin[level];
                        AudioManager.Instance.PlayAdultWin(clip);
                    }
                    catch (System.Exception)
                    {
                        EditorDebugger.LogWarning($"{level} has no win voice over");
                    }
                }
            }
        }

        public void SetLoseUI(string text, string leveLostKey, int level)
        {
            loseText.text = text;
            if (LocalizationManager.IsArabic)
            {
                StartCoroutine(FixArabicText.FixText(loseText, loseText.text));
                if (!leveLostKey.StartsWith("R3"))
                {
                    try
                    {
                        AudioClip clip = selectedCharacterData.ChildAudio.LevelLost[leveLostKey];
                        AudioManager.Instance.PlayChildLose(clip, skipLevel.gameObject.activeSelf, level == 1);
                    }
                    catch (System.Exception)
                    {
                        EditorDebugger.LogWarning($"{leveLostKey} has no lose voice over");
                    }
                }
                else
                {
                    try
                    {
                        AudioClip clip = selectedCharacterData.AdultAudio.LevelLost[leveLostKey];
                        AudioManager.Instance.PlayAdultLose(clip, skipLevel.gameObject.activeSelf, level == 1);
                    }
                    catch (System.Exception)
                    {
                        EditorDebugger.LogWarning($"{leveLostKey} has no lose voice over");
                    }
                }
            }
        }

        public void SkipLevel()
        {
            SaveData.GameDataObject.CollectedCoins -= SkipLevelCost;
            SaveData.SaveJSONFile();

            LevelManager level = Levels[levelBeingPlayed].GetComponent<LevelManager>();
            level.SkipLevel();
            //NextLevel();
        }

        #region D E L A Y S
        // This function adds a global wait for the button animation to play
        public void GotoSettingsUIDelayed()
        {
            Invoke(nameof(ToggleSettingsUI), 0.15f);
        }
        public void GoToTermsOfUseDelayed()
        {
            Invoke(nameof(GoToTermsOfUse), 0.15f);
        }
        public void GotoCharacterScreenDelayed()
        {
            Invoke(nameof(GotoCharacterScreen), 0.15f);
        }
        public void GotoRegionSelectionDelayed()
        {
            Invoke(nameof(GotoRegionSelection), 0.15f);
        }

        IEnumerator DisableGameObjectDelayed(GameObject go)
        {
            yield return new WaitForSeconds(0.15f);
            go.SetActive(false);
            yield return null;
        }

        IEnumerator LoadLevelDelayed(GameObject level)
        {
            yield return new WaitForSeconds(0.15f);
            FadeScript.Instance.StartFadeInOut();
            StartCoroutine(nameof(LateLoad), level);

            // Check which level is being loaded and save the integer
            for (int i = 0; i < Levels.Length; i++)
            {
                if (level.name == Levels[i].name)
                {
                    levelBeingPlayed = i;
                    EditorDebugger.Log("Level being played is:" + i);
                    break;
                }
            }

            yield return null;
        }

        #endregion

        public void GotoMainMenu()
        {
            EventManager.RaiseResetEvent();
            AudioManager.Instance.StopLevelInfoSource();

            Character.StopAllCoroutines();

            TurnOffAllUIs();
            TurnOffLevels();
            Character.gameObject.SetActive(false);
            Time.timeScale = 1;

            // Reset camera
            Camera.main.GetComponent<SmoothFollow>().enabled = false;
            Camera.main.transform.position = Vector3.zero;

            RegionsSelectionUI.SetActive(true);

            // Turn off all objectives UI
            TurnOffAllObjectiveUI();

            // disable joystick
            joystick.SetActive(false);

            // enable the jump button
            jumpButton.SetActive(true);

            // turn off the regions
            Region1UI.SetActive(false);
            Region2UI.SetActive(false);
            Region3UI.SetActive(false);

            // Update the locks
            UpdateLevelLocks();
        }

        private void TurnOffAllUIs()
        {
            LanguageSelection.SetActive(false);
            TermsOfUse.SetActive(false);
            CharacterSelection.SetActive(false);
            GuadrianSelection.SetActive(false);
            GatherSeeds.SetActive(false);
            UserIDUI.SetActive(false);
            RegionsSelectionUI.SetActive(false);
            inGameUI.SetActive(false);
            loseUI.SetActive(false);
            winUI.SetActive(false);
            PauseUI.SetActive(false);
            SettingsUI.SetActive(false);
            AboutUsUI.SetActive(false);
            LanguageUI.SetActive(false);
        }

        // This function turns off all objectives ui so that one level manager is capable of choosing his desired one
        public void TurnOffAllObjectiveUI()
        {
            for (int i = 0; i < objectivesUI.Count; i++)
            {
                objectivesUI[i].SetActive(false);
            }
        }

        // This function will turn on a specific region ui
        //public void SelectRegion(GameObject region)
        //{

        //    if (region.name == "Region 3 Levels" && !PlayerPrefs.HasKey("EnteredGreenhouse"))
        //        ShowRegionNotifier();
        //    else
        //        region.SetActive(true);
        //}

        private void ShowRegionNotifier()
        {
            region3Notifier.SetActive(true);
            PlayerPrefs.SetInt("EnteredGreenhouse", 1);
        }

        // This function will take us back to character selection screen
        public void GotoLanguageSelection()
        {
            TurnOffAllUIs();
            LanguageSelection.SetActive(true);
        }

        public void GoToTermsOfUse()
        {
            TurnOffAllUIs();
            TermsOfUse.SetActive(true);
        }

        // This function will take us back to character selection screen
        public void GotoCharacterScreen()
        {
            TurnOffAllUIs();
            CharacterSelection.SetActive(true);
        }

        // This function will disable all regions ui
        public void DisableAllRegions()
        {
            Region1UI.SetActive(false);
            Region2UI.SetActive(false);
            Region3UI.SetActive(false);
        }

        // This function will disable a gameobject, used for region notifier
        public void DisableGameObject(GameObject go)
        {
            StartCoroutine(nameof(DisableGameObjectDelayed), go);
        }

        public void PauseGame()
        {
            gamePaused = !gamePaused;
            if (gamePaused)
            {
                Time.timeScale = 0;
                PauseUI.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
                PauseUI.SetActive(false);
            }
        }

        public void ApplicationFocusPause()
        {
            gamePaused = false;
            Time.timeScale = 0;
            PauseUI.SetActive(true);
        }

        public void ToggleLevelInfo(bool active)
        {
            LevelInfo.SetActive(active);

            if (!active)
                AudioManager.Instance.StopLevelInfoSource();
        }

        public void EnableLevelInfo(string info, string levelInfoKey)
        {
            ToggleLevelInfo(true);
            ToggleJoystick(false);

            if (LocalizationManager.IsArabic)
            {
                StartCoroutine(FixArabicText.FixText(LevelInfoText, info));
                AudioManager.Instance.PlayLevelInfo(levelInfoKey);
            }
            else
                LevelInfoText.text = info;
        }

        public void DisableLevelInfo()
        {
            ToggleLevelInfo(false);
            Levels[levelBeingPlayed].GetComponent<LevelManager>().DisableLevelInfo();
        }

        public void PlayR3L6FlyerAudio(string flyerKey)
        {
            try
            {
                AudioClip clip = selectedCharacterData.AdultAudio.R3L6Flyers[flyerKey];
                AudioManager.Instance.PlayR3L6Flyer(clip);
            }
            catch (System.Exception)
            {
                EditorDebugger.LogWarning($"{flyerKey} has no voice over");
                return;
            }
        }

        #endregion

        #region W O R L D   3   N O T I C E
        // This function handles the world 3 notices
        public void HandleWorld3Notice()
        {
            World3Notice.SetActive(true);
            childNotice.transform.parent.gameObject.SetActive(true);
            adultNotice.transform.parent.gameObject.SetActive(false);
            childNotice.sprite = selectedCharacterData.ChildGreenHouseNotice;
            adultNotice.sprite = selectedCharacterData.AdultGreenHouseNotice;
        }

        #endregion

        #region      M A I N     G A M E    F U N C T I O N S

        // This function is called at Start
        void StartGame()
        {
            // Checking user data file before anything else.
            SaveData.CheckMigration();

            // Check if user has set name
            isNameSet = SaveData.GameDataObject.IsNameSet;
            characterSelected = SaveData.GameDataObject.CharacterSelected;
            SetCharacterData();
            Character.SetAnimatorController(selectedCharacterData.Controller);
            Character.gameObject.SetActive(false);
            // Turn off all uis except main menu
            TurnOffAllUIs();
            TurnOffLevels();

            if (isNameSet)
                GotoRegionSelection();
            else
                GotoLanguageSelection();

            // Update the lock/unlock of levels
            UpdateLevelLocks();

            // Disable all region UIS at game start
            Region1UI.SetActive(false);
            Region2UI.SetActive(false);
            Region3UI.SetActive(false);
        }

        // This function will reset the game
        public void ResetWholeGame()
        {
            SaveData.OverwriteJSON();
            PlayerPrefs.DeleteAll();
            LocalizationManager.SaveSelectedLanguage();
            SceneManager.LoadScene("main");
        }

        // This function updates the locks of the levels
        public void UpdateLevelLocks()
        {
            region1LevelsHandler.UpdateLevels();
            region2LevelsHandler.UpdateLevels();
            region3LevelsHandler.UpdateLevels();
        }

        public void LoadLevel(int level)
        {
            LoadLevel(Levels[level]);
        }

        public void LoadLevel(GameObject level)
        {
            StartCoroutine(nameof(LoadLevelDelayed), level);
        }

        IEnumerator LateLoad(GameObject level)
        {
            yield return new WaitForSeconds(0.5f);

            // Turn off the main menu UI's
            CharacterSelection.SetActive(false);
            RegionsSelectionUI.SetActive(false);
            loseUI.SetActive(false);
            winUI.SetActive(false);
            comicWinUI.SetActive(false);
            inGameUI.SetActive(true);
            TurnOffLevels();

            // Enable the level
            level.SetActive(true);
            yield return null;
        }

        // This function will trigger when the player takes full damage from sun
        public void RemoveLifeUIOnly()
        {
            lifes--;  // remove 1 life
            LifesUI[lifes].SetActive(false); // Turn off last life UI from the UI

            if (lifes <= 0)
            {
                // Raise lose event
                EventManager.RaiseLoseEvent();
                return;
            }
        }

        // This function will reset the UI lifes
        public void ResetLifesUI()
        {
            lifes = 3;
            // turn on all lifes
            for (int i = 0; i < LifesUI.Count; i++)
            {
                LifesUI[i].SetActive(true);
            }
        }

        // This function will enable sticker on house
        public void EnableSticker()
        {
            if (sticker != null && !sticker.transform.GetChild(0).gameObject.activeSelf)
            {
                sticker.transform.GetChild(0).gameObject.SetActive(true);
                // Raise seed event as if its a seed
                EventManager.RaiseCharacterHitEvent("Seed");
            }
        }

        // This function will open the window
        public void OpenWindow()
        {
            if (window != null && oldWindowName != window.name)
            {
                // Disable smoke detector
                oldWindowName = window.transform.GetChild(0).transform.name;
                window.GetComponent<SpriteRenderer>().enabled = false;
                window.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
                smokeDetector.enabled = false;
                // Raise seed event as if its a seed
                EventManager.RaiseCharacterHitEvent("Seed");
                EventManager.RaiseSmokeStopEvent();
            }
        }

        // This function goes to the next level
        public void NextLevel()
        {
            if (levelBeingPlayed + 1 >= Levels.Length)
                return;

            EventManager.RaiseResetEvent();
            winUI.SetActive(true);
            joystick.SetActive(false);
            jumpButton.SetActive(true);
            TurnOffLevels();
            Character.gameObject.SetActive(false);
            Camera.main.GetComponent<SmoothFollow>().enabled = false;
            Camera.main.transform.position = Vector3.zero;
            levelBeingPlayed += 1;
            LoadLevel(Levels[levelBeingPlayed]);
        }

        #endregion

        #region      U T I L    F U N C T I O N S

        public void SetCharacterSelected(string sexe)
        {
            characterSelected = sexe;
            SetCharacterData();
            Character.SetAnimatorController(selectedCharacterData.Controller);
            SaveData.GameDataObject.CharacterSelected = sexe;
            SaveData.SaveJSONFile();
        }

        // This function saves the seeds collected within a level to json file
        public void SaveCollectedSeedsToJson()
        {
            SaveData.GameDataObject.CollectedCoins += seedsCollected;
            SaveData.SaveJSONFile();
        }

        public void EnableCharacter()
        {
            Character.gameObject.SetActive(true);
        }

        void TurnOffLevels()
        {
            for (int i = 0; i < Levels.Length; i++)
                Levels[i].SetActive(false);
        }

        // This function will save the character name set
        public void SaveCharacterName(InputField input)
        {
            SaveData.GameDataObject.CharacterName = ArabicSupport.ArabicFixer.Fix(input.text, true, true);
            SaveData.SaveJSONFile();
            EditorDebugger.Log("Character name saved.");
        }

        private void SetCharacterData()
        {
            if (characterSelected == "characterX")
                selectedCharacterData = CharacterX;
            else if (characterSelected == "male")
                selectedCharacterData = Boy;
            else if (characterSelected == "characterf")
                selectedCharacterData = CharacterF;
            else
                selectedCharacterData = Girl;

            selectedCharacterData.SetAudioDictionaries();
        }

        #endregion
    }
}