using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Guardian Audio Clips")]
public class GuardianAudioClips : ScriptableObject
{
    public AudioClip AmI18;
    public AudioClip GuardianIntro;
    public AudioClip GuardianLegalAge;
    public AudioClip GuardianTurned14;
    public AudioClip HatTutorial;
    public AudioClip MoveTutorial;
    public AudioClip R1L1Info;
    public AudioClip R1L2Info;
    public AudioClip R1L3Info;
    public AudioClip R1L4Info;
    public AudioClip R1L5Info;
    public AudioClip R1L6Info;
    public AudioClip R1L7Info;
    public AudioClip R2L1Info;
    public AudioClip R2L2Info;
    public AudioClip R2L3Info;
    public AudioClip R2L4Info;
    public AudioClip R2L5Info;
    public AudioClip R2L6Info;
    public AudioClip R2L7Info;
    public AudioClip R3L1Info;
    public AudioClip R3L2Info;
    public AudioClip R3L3Info;
    public AudioClip R3L4Info;
    public AudioClip R3L5Info;
    public AudioClip R3L6Info;
    public AudioClip TellAdult;

    public Dictionary<string, AudioClip> LevelInfo;

    public void SetLevelInfoDictionary()
    {
        LevelInfo = new Dictionary<string, AudioClip>()
        {
            {nameof(R1L1Info), R1L1Info },
            {nameof(R1L2Info), R1L2Info },
            {nameof(R1L3Info), R1L3Info},
            {nameof(R1L4Info), R1L4Info },
            {nameof(R1L5Info), R1L5Info },
            {nameof(R1L6Info), R1L6Info },
            {nameof(R1L7Info), R1L7Info },
            {nameof(R2L1Info), R2L1Info },
            {nameof(R2L2Info), R2L2Info },
            {nameof(R2L3Info), R2L3Info },
            {nameof(R2L4Info), R2L4Info },
            {nameof(R2L5Info), R2L5Info },
            {nameof(R2L6Info), R2L6Info },
            {nameof(R2L7Info), R2L7Info },
            {nameof(R3L1Info), R3L1Info },
            {nameof(R3L2Info), R3L2Info },
            {nameof(R3L3Info), R3L3Info },
            {nameof(R3L4Info), R3L4Info },
            {nameof(R3L5Info), R3L5Info },
            {nameof(R3L6Info), R3L6Info },
        };
    }
}