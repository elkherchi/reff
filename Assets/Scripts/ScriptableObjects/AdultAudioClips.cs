using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Adult Audio Clips")]
public class AdultAudioClips : ScriptableObject
{
    [Header("World 3 Unlock")]
    public AudioClip Turned18;
    public AudioClip EnterGreenhouse;
    [Header("Win Voice Overs")]
    public AudioClip L1Win;
    public AudioClip L2Win;
    public AudioClip L3Win;
    public AudioClip L4Win;
    public AudioClip L5Win;
    public AudioClip L6Win;
    public AudioClip L7Win;
    [Header("Lose Voice Overs")]
    public AudioClip R3L1Lost;
    public AudioClip R3L2Lost;
    public AudioClip R3L3Lost;
    public AudioClip R3L4Lost;
    public AudioClip R3L5Lost;
    public AudioClip R3L6Lost;
    [Header("R3L6 Flyers")]
    public AudioClip R3L6Flyer1;
    public AudioClip R3L6Flyer2;
    public AudioClip R3L6Flyer3;
    public AudioClip R3L6Flyer4;
    [Header("Win Messages")]
    public AudioClip R3L2WinMessage;
    public AudioClip R3L6WinMessage;

    public Dictionary<int, AudioClip> LevelWin;
    public Dictionary<string, AudioClip> LevelLost;
    public Dictionary<string, AudioClip> R3L6Flyers;

    public void SetLevelDictionaries()
    {
        LevelWin = new Dictionary<int, AudioClip>()
        {
            {1, L1Win},
            {2, L2Win},
            {3, L3Win},
            {4, L4Win},
            {5, L5Win},
            {6, L6Win},
            {7, L7Win},
        };

        LevelLost = new Dictionary<string, AudioClip>()
        {
            {nameof(R3L1Lost), R3L1Lost },
            {nameof(R3L2Lost), R3L2Lost },
            {nameof(R3L3Lost), R3L3Lost },
            {nameof(R3L4Lost), R3L4Lost },
            {nameof(R3L5Lost), R3L5Lost },
            {nameof(R3L6Lost), R3L6Lost },
        };

        R3L6Flyers = new Dictionary<string, AudioClip>()
        {
            {nameof(R3L6Flyer1), R3L6Flyer1 },
            {nameof(R3L6Flyer2), R3L6Flyer2 },
            {nameof(R3L6Flyer3), R3L6Flyer3 },
            {nameof(R3L6Flyer4), R3L6Flyer4 },
        };

        AudioManager.Instance.SetAdultClips(Turned18, EnterGreenhouse, R3L2WinMessage, R3L6WinMessage);
    }
}