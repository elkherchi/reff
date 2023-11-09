using UnityEngine;

public class WaitForAudioEvent : CustomYieldInstruction
{
    private readonly OnAudioSourceFinished audioSource;
    public override bool keepWaiting => audioSource.IsPlaying || !Application.isFocused;
    public WaitForAudioEvent(OnAudioSourceFinished source) => audioSource = source;
}