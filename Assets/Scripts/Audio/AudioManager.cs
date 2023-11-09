using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

public class AudioManager : MonoBehaviour
{
    private const string MasterVolume = "MasterVolume";
    private const string MusicVolume = "MusicVolume";
    private const string SFXVolume = "SFXVolume";

    public delegate void ToggleAudioSource(bool toggle);
    public static event ToggleAudioSource OnToggleMusic;
    public static event ToggleAudioSource OnToggleSFX;

    public static AudioManager Instance { get; private set; }

    [Header("Audio Mixer Settings")]
    [SerializeField] private AudioMixer gameMixer;
    [SerializeField] private float minimumVolume;
    [SerializeField] private float maximumVolume;

    [Header("Voice Overs")]
    [Range(0.1f, 1)]
    [SerializeField] private float delayBetweenVoiceOvers;

    [Header("Guardian Voice Overs")]
    [SerializeField] private GuardianAudioClips guardianClips;
    [SerializeField] private OnAudioSourceFinished repeatLevel;
    [SerializeField] private OnAudioSourceFinished skipLevel;
    [SerializeField] private OnAudioSourceFinished tellAdult;
    [SerializeField] private OnAudioSourceFinished levelInfo;

    [Header("Child Voice Overs")]
    [SerializeField] private AudioSource childNO;
    [SerializeField] private OnAudioSourceFinished childWin;
    [SerializeField] private OnAudioSourceFinished childLose;
    [SerializeField] private OnAudioSourceFinished whatIfBullying;
    [SerializeField] private OnAudioSourceFinished R2L3Story1;

    [Header("Adult Voice Overs")]
    [SerializeField] private OnAudioSourceFinished adultWin;
    [SerializeField] private OnAudioSourceFinished adultLose;
    [SerializeField] private OnAudioSourceFinished turned18;
    [SerializeField] private AudioSource enterGreenhouse;
    [SerializeField] private AudioSource R3L2Win;
    [SerializeField] private OnAudioSourceFinished R3L6Flyer;
    [SerializeField] private AudioSource R3L6Win;

    private WaitForSecondsRealtime voiceOverWait;

    [RuntimeInitializeOnLoadMethod()]
    private static void SetUpSceneLoadedEvent()
    {
        SceneManager.sceneLoaded += (scene, loadMode) => GetAudioControllers();
    }

    private static void GetAudioControllers()
    {
        AudioSourceController[] controllers = FindObjectsOfType<AudioSourceController>(true);
        foreach (AudioSourceController controller in controllers)
            controller.Subscribe();
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }

        Instance = this;
        guardianClips.SetLevelInfoDictionary();
        voiceOverWait = new WaitForSecondsRealtime(delayBetweenVoiceOvers);
    }

    private float GetVolume(float alpha) => Mathf.Lerp(minimumVolume, maximumVolume, alpha);
    public void SetMasterVolume(float value) => gameMixer.SetFloat(MasterVolume, GetVolume(value));
    public void SetMusicVolume(float value) => gameMixer.SetFloat(MusicVolume, GetVolume(value));
    public void SetSFXVolume(float value) => gameMixer.SetFloat(SFXVolume, GetVolume(value));
    public void ToggleMusic(bool toggle) => OnToggleMusic?.Invoke(toggle);
    public void ToggleSFX(bool toggle) => OnToggleSFX?.Invoke(toggle);

    public void PlayAudioSource(AudioSource source)
    {
        if (LocalizationManager.IsArabic)
            source.Play();
    }

    public void StartAudioSource(OnAudioSourceFinished source)
    {
        if (LocalizationManager.IsArabic)
            source.StartAudioSource();
    }

    public void StopAudioSource(AudioSource source) => source.Stop();

    public void PlayChildNo() => childNO.Play();
    public void StartR1L7Coroutine() => StartCoroutine(nameof(R1L7Coroutine));
    public void StopR1L7Coroutine()
    {
        StopCoroutine(nameof(R1L7Coroutine));
        whatIfBullying.Stop();
        tellAdult.Stop();
    }

    IEnumerator R1L7Coroutine()
    {
        whatIfBullying.StartAudioSource();
        yield return new WaitForAudioEvent(whatIfBullying);
        yield return voiceOverWait;
        tellAdult.StartAudioSource();
    }

    public void PlayLevelInfo(string level)
    {
        if (!LocalizationManager.IsArabic)
            return;

        AudioClip audioClip;
        try
        {
            audioClip = guardianClips.LevelInfo[level];
        }
        catch (System.Exception)
        {
            EditorDebugger.LogWarning($"{level} has no audio clip");
            return;
        }

        levelInfo.SetAudioSourceClip(audioClip);
        StartCoroutine(PlayLevelInfo());
    }

    public IEnumerator PlayLevelInfo()
    {
        yield return new WaitUntil(() => FadeScript.Instance.FadedOut);
        levelInfo.StartAudioSource();
    }

    public void StopLevelInfoSource()
    {
        levelInfo.Stop();
    }

    public void PlayChildWin(AudioClip clip)
    {
        childWin.SetAudioSourceClip(clip);
        childWin.StartAudioSource();
    }

    public void PlayAdultWin(AudioClip clip)
    {
        adultWin.SetAudioSourceClip(clip);
        adultWin.StartAudioSource();
    }

    public void StopWinSources()
    {
        childWin.Stop();
        adultWin.Stop();
    }

    public void PlayChildLose(AudioClip clip, bool playSkipAudio, bool playButtonsVoiceOvers)
    {
        childLose.SetAudioSourceClip(clip);
        childLose.StartAudioSource();
        StartCoroutine(PlayGuardianLoseVoiceOver(childLose, playSkipAudio, playButtonsVoiceOvers));
    }


    public void PlayAdultLose(AudioClip clip, bool playSkipAudio, bool playButtonsVoiceOvers)
    {
        adultLose.SetAudioSourceClip(clip);
        adultLose.StartAudioSource();
        StartCoroutine(PlayGuardianLoseVoiceOver(adultLose, playSkipAudio, playButtonsVoiceOvers));
    }

    IEnumerator PlayGuardianLoseVoiceOver(OnAudioSourceFinished currentLoseSource, bool playSkipAudio, bool playButtonsVoiceOvers)
    {
        yield return new WaitForAudioEvent(currentLoseSource);
        yield return voiceOverWait;

        if (playButtonsVoiceOvers)
            StartCoroutine(PlayLoseButtonVoiceOvers(playSkipAudio));
        else
            skipLevel.InvokeStopEvent();
    }

    IEnumerator PlayLoseButtonVoiceOvers(bool playSkipAudio)
    {
        repeatLevel.StartAudioSource();
        yield return new WaitForAudioEvent(repeatLevel);
        yield return voiceOverWait;

        if (playSkipAudio)
            skipLevel.StartAudioSource();
        else
            skipLevel.InvokeStopEvent();
    }

    public void StopLoseSources()
    {
        childLose.Stop();
        adultLose.Stop();
    }

    public void PlayR3L1Story1()
    {
        turned18.StartAudioSource();
    }

    public void PlayR2L3Story1()
    {
        R2L3Story1.StartAudioSource();
    }

    public void SetAdultClips(AudioClip turned, AudioClip greenhouse, AudioClip l2win, AudioClip l6win)
    {
        turned18.SetAudioSourceClip(turned);
        enterGreenhouse.clip = greenhouse;
        R3L2Win.clip = l2win;
        R3L6Win.clip = l6win;
    }

    public void PlayR3L6Flyer(AudioClip clip)
    {
        R3L6Flyer.SetAudioSourceClip(clip);
        R3L6Flyer.StartAudioSource();
    }

    public void StopR3L6Flyer()
    {
        R3L6Flyer.Stop();
    }
}