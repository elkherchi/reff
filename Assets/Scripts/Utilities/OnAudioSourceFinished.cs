using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class OnAudioSourceFinished : MonoBehaviour
{
    [SerializeField] private UnityEvent AudioSourceStartedPlaying;
    [SerializeField] private UnityEvent AudioSourceFinishedPlaying;
    [SerializeField] private AudioSource source;
    [SerializeField, Min(0)] private float delayedInvoke;

    public bool IsPlaying => source.isPlaying;

    public void StartAudioSource()
    {
        source.Play();
        AudioSourceStartedPlaying.Invoke();
        StartCoroutine(WaitForAudioSource());
    }

    private IEnumerator WaitForAudioSource()
    {
        yield return new WaitForAudio(source);

        if (delayedInvoke > 0.05f)
            yield return new WaitForSecondsRealtime(delayedInvoke);

        AudioSourceFinishedPlaying.Invoke();
    }

    public void SetAudioSourceClip(AudioClip clip)
    {
        source.clip = clip;
    }

    public void InvokeStopEvent() => AudioSourceFinishedPlaying.Invoke();

    public void Stop()
    {
        source.Stop();
    }
}