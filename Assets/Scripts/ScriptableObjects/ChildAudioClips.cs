using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Child Audio Clips")]
public class ChildAudioClips : ScriptableObject
{
    [Header("Win Voice Overs")]
    public AudioClip L1Win;
    public AudioClip L2Win;
    public AudioClip L3Win;
    public AudioClip L4Win;
    public AudioClip L5Win;
    public AudioClip L6Win;
    public AudioClip L7Win;
    [Header("Lost Voice Overs")]
    [Header("R1")]
    public AudioClip R1L1Lost;
    public AudioClip R1L2Lost;
    public AudioClip R1L3Lost;
    public AudioClip R1L4Lost;
    public AudioClip R1L5Lost;
    public AudioClip R1L6Lost;
    //public AudioClip R1L7Lost;
    [Header("R2")]
    public AudioClip R2L1Lost;
    public AudioClip R2L2Lost;
    public AudioClip R2L3Lost;
    public AudioClip R2L4Lost;
    public AudioClip R2L5Lost;
    public AudioClip R2L6Lost;
    public AudioClip R2L7Lost;

    public Dictionary<string, AudioClip> LevelLost;
    public Dictionary<int, AudioClip> LevelWin;

    public void SetLevelDictionaries()
    {
        LevelLost = new Dictionary<string, AudioClip>()
        {
            {nameof(R1L1Lost), R1L1Lost },
            {nameof(R1L2Lost), R1L2Lost },
            {nameof(R1L3Lost), R1L3Lost },
            {nameof(R1L4Lost), R1L4Lost },
            {nameof(R1L5Lost), R1L5Lost },
            {nameof(R1L6Lost), R1L6Lost },
            //{nameof(R1L7Lost), R1L7Lost },
            {nameof(R2L1Lost), R2L1Lost },
            {nameof(R2L2Lost), R2L2Lost },
            {nameof(R2L3Lost), R2L3Lost },
            {nameof(R2L4Lost), R2L4Lost },
            {nameof(R2L5Lost), R2L5Lost },
            {nameof(R2L6Lost), R2L6Lost },
            {nameof(R2L7Lost), R2L7Lost },
        };

        LevelWin = new Dictionary<int, AudioClip>()
        {
            {1,L1Win },
            {2,L2Win },
            {3,L3Win },
            {4,L4Win },
            {5,L5Win },
            {6,L6Win },
            {7,L7Win },
        };
    }
}