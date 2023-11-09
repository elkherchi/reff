using UnityEngine;

public class WaitForAudio : CustomYieldInstruction
{
    private readonly AudioSource audioSource;
    public override bool keepWaiting => audioSource.isPlaying || !Application.isFocused;
    public WaitForAudio(AudioSource source) => audioSource = source;
}