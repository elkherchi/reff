using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    [Header("Sound Control")]
    public Slider MusicVolume;
    public Toggle MuteMusic;
    public Slider SFXVolume;
    public Toggle MuteSFX;

    [Header("Toggle Icons")]
    public Sprite ToggleOn;
    public Sprite ToggleOff;

    //Player Prefs Keys
    readonly private string MusicMutedKey = "MusicMuted";
    readonly private string MusicVolumeKey = "MusicVolume";
    readonly private string SFXMutedKey = "SFXMuted";
    readonly private string SFXVolumeKey = "SFXVolume";

    private Dictionary<bool, Sprite> toggleIcon;
    private Image MusicToggleImage;
    private Image SFXToggleImage;

    #region  I N I T I A L I Z E

    private void Awake()
    {
        toggleIcon = new Dictionary<bool, Sprite>
        {
            { true, ToggleOff },
            { false, ToggleOn }
        };

        GetToggleImages();
        SetDefault();
        UpdateAudioManager();
        AddListeners();
        UpdateToggleImages();
    }

    private void GetToggleImages()
    {
        MusicToggleImage = MuteMusic.GetComponent<Image>();
        SFXToggleImage = MuteSFX.GetComponent<Image>();
    }

    private void SetDefault()
    {
        MuteMusic.isOn = PlayerPrefs.GetInt(MusicMutedKey, 0) == 1;
        MusicVolume.value = PlayerPrefs.GetFloat(MusicVolumeKey, 1);
        MuteSFX.isOn = PlayerPrefs.GetInt(SFXMutedKey, 0) == 1;
        SFXVolume.value = PlayerPrefs.GetFloat(SFXVolumeKey, 1);
    }

    private void UpdateAudioManager()
    {
        AudioManager.Instance.SetMusicVolume(MusicVolume.value);
        AudioManager.Instance.SetSFXVolume(SFXVolume.value);
        AudioManager.Instance.ToggleMusic(MuteMusic.isOn);
        AudioManager.Instance.ToggleSFX(MuteSFX.isOn);
    }

    private void AddListeners()
    {
        MusicVolume.onValueChanged.AddListener(ControlMusic);
        SFXVolume.onValueChanged.AddListener(ControlSFX);
        MuteMusic.onValueChanged.AddListener(ToggleMusic);
        MuteSFX.onValueChanged.AddListener(ToggleSFX);
    }

    private void UpdateToggleImages()
    {
        MusicToggleImage.sprite = toggleIcon[MuteMusic.isOn];
        SFXToggleImage.sprite = toggleIcon[MuteSFX.isOn];
    }

    #endregion

    #region F U N C T I O N S

    public void ControlMusic(float musicVolume)
    {
        PlayerPrefs.SetFloat(MusicVolumeKey, musicVolume);
        PlayerPrefs.Save();
        AudioManager.Instance.SetMusicVolume(musicVolume);

        bool musicMuted = musicVolume == 0;
        MuteMusic.isOn = musicMuted;
        PlayerPrefs.SetInt(MusicMutedKey, musicMuted ? 1 : 0);
        MusicToggleImage.sprite = toggleIcon[musicMuted];
    }

    public void ControlSFX(float sfxVolume)
    {
        PlayerPrefs.SetFloat(SFXVolumeKey, sfxVolume);
        PlayerPrefs.Save();
        AudioManager.Instance.SetSFXVolume(sfxVolume);

        bool sfxMuted = sfxVolume == 0;
        MuteSFX.isOn = sfxMuted;
        PlayerPrefs.SetInt(SFXMutedKey, sfxMuted ? 1 : 0);
        SFXToggleImage.sprite = toggleIcon[sfxMuted];
    }

    public void ToggleMusic(bool musicMuted)
    {
        int value = musicMuted ? 1 : 0;
        PlayerPrefs.SetInt(MusicMutedKey, value);
        PlayerPrefs.Save();
        AudioManager.Instance.ToggleMusic(musicMuted);
        MusicToggleImage.sprite = toggleIcon[musicMuted];
    }

    public void ToggleSFX(bool sfxMuted)
    {
        int value = sfxMuted ? 1 : 0;
        PlayerPrefs.SetInt(SFXMutedKey, value);
        PlayerPrefs.Save();
        AudioManager.Instance.ToggleSFX(sfxMuted);
        SFXToggleImage.sprite = toggleIcon[sfxMuted];
    }

    #endregion
}