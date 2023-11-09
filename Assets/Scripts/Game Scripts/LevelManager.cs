using System.Collections;
using UnityEngine;
using com.just.joystick;

public abstract class LevelManager : MonoBehaviour
{
    public GameObject theLevelUI;
    public Level level;
    public bool finalRegionLevel;

    [Space(10)]
    [Header("In Game Objective UI")]
    public GameObject objectiveImage;

    protected GameManager gameManager;
    protected MyCharacterController character;
    protected bool canPause;

    protected virtual bool HasCharacter => true;
    protected virtual bool ShowLevelInfo => true;
    protected abstract int Region { get; }
    protected abstract int Level { get; }
    protected abstract SpriteRenderer LevelCharacter { get; }
    protected string LevelInfoKey => $"R{Region}L{Level}Info";
    protected string LevelWinKey => $"R{Region}L{Level}Win";
    protected string LevelLostKey => $"R{Region}L{Level}Lost";
    protected string LevelInfo => LocalizationManager.GetFromDictionary(LevelInfoKey);

    protected virtual void OnEnable()
    {
        SubscribeToEvents();
        SetGameManager();

        if (HasCharacter)
        {
            SetCharacter();
            SetCamera();
        }

        if (objectiveImage != null)
            objectiveImage.SetActive(true);

        // Turn on the level
        theLevelUI.SetActive(true);

        ResetLevel();
        canPause = false;

        if (ShowLevelInfo)
            gameManager.EnableLevelInfo(LevelInfo, LevelInfoKey);
    }

    protected virtual void OnDisable()
    {
        UnsubscribeToEvents();
        // Turn off the level
        theLevelUI.SetActive(false);
        gameManager.ToggleLevelInfo(false);
    }

    protected virtual void SetGameManager()
    {
        gameManager = GameManager.Instance;
        gameManager.TurnOffAllObjectiveUI();
        gameManager.seedsCollected = 0;
        gameManager.SetSeedsText(level.AmountToCollect);

        if (HasCharacter)
            gameManager.EnableCharacter();
    }

    protected virtual void SetCharacter()
    {
        // Set the character
        character = gameManager.Character;
        // Start the character
        character.InitializeCharacter();

        // Disable all animnations
        character.EnableAnimation(LevelCharacter);
    }

    protected virtual void SetCamera()
    {
        // Give the camera the character selected so she can follow him
        SmoothFollow camera = Camera.main.GetComponent<SmoothFollow>();
        camera.enabled = true;
        camera.target = gameManager.Character.transform;
    }

    private void SubscribeToEvents()
    {
        EventManager.OnResetEvent += RestartLevel;
        EventManager.OnWinEvent += Win;
        EventManager.OnLoseEvent += Lose;
        EventManager.OnFallDownEvent += Lose;
        EventManager.OnCharacterHit += PlayerHit;
    }

    private void UnsubscribeToEvents()
    {
        EventManager.OnResetEvent -= RestartLevel;
        EventManager.OnWinEvent -= Win;
        EventManager.OnLoseEvent -= Lose;
        EventManager.OnFallDownEvent -= Lose;
        EventManager.OnCharacterHit -= PlayerHit;
    }

    public virtual void DisableLevelInfo()
    {
        gameManager.ToggleJoystick(true);
        canPause = true;
    }

    public virtual void RestartLevel()
    {
        EditorDebugger.Log("Restarting level.."); 
        StartCoroutine(nameof(RestartCoroutine));
    }

    protected virtual void ResetLevel()
    {
        gameManager.ToggleLoseUI(false);
        gameManager.ToggleWinUI(false);

        gameManager.seedsCollected = 0;
        gameManager.SetSeedsText(level.AmountToCollect);
        if (HasCharacter)
            character.enableSwipeController = true;
    }

    protected virtual IEnumerator RestartCoroutine()
    {
        canPause = false;
        Time.timeScale = 1;
        yield return new WaitUntil(() => FadeScript.Instance.FadedIn);
        ResetLevel();
        canPause = true;
    }

    public void FallDown()
    {
        EndGame();
        gameManager.SetLoseUI(LocalizationManager.GetFromDictionary("FallDownLost"), "", Level);
    }

    // This function is called when the player loses
    public virtual void Lose()
    {
        EndGame();
        gameManager.SetLoseUI(LocalizationManager.GetFromDictionary(LevelLostKey), LevelLostKey, Level);
    }

    // This function is caleld when the player finishes the level
    public virtual void Win()
    {
        canPause = false;
        gameManager.SetWinText(LocalizationManager.GetWonLevel(Level), Region, Level);
        gameManager.ToggleWinUI(true, finalRegionLevel);

        // Save collected seeds
        gameManager.SaveCollectedSeedsToJson();
        // If player ddnt previously win this level, else skip
        if (level.CurrentAmount != level.AmountToCollect)
        {
            // Check if the seeds collected are bigger than the current seeds collected of the level, then save the new current seeds collected
            if (gameManager.seedsCollected >= level.CurrentAmount)
            {
                level.CurrentAmount = gameManager.seedsCollected;
                SaveData.SaveLevelProgress(Region, Level, level.CurrentAmount);
            }
        }

        Time.timeScale = 0;
    }

    public virtual void SkipLevel()
    {
        gameManager.SetWinText(LocalizationManager.GetWonLevel(Level), Region, Level);
        gameManager.ToggleWinUI(true, finalRegionLevel);
        gameManager.ToggleLoseUI(false);
        SaveData.SaveLevelProgress(Region, Level, 1);
    }

    protected virtual void EndGame()
    {
        canPause = false;
        gameManager.ShowRepeatButton(Region == 3);
        gameManager.ToggleLoseUI(true);
        Time.timeScale = 0;

        if (HasCharacter)
            character.enableSwipeController = false;
    }

    public void PlayerHit(string s)
    {
        if (s == "Seed")
        {
            gameManager.seedsCollected += 1;
            gameManager.SetSeedsText(level.AmountToCollect);
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if (canPause && !focus)
            gameManager.ApplicationFocusPause();
    }
}