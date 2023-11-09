using UnityEngine;

public class EventManager : MonoBehaviour
{

    // Reset Game Event
    public delegate void ResetEvent();
    public static event ResetEvent OnResetEvent;

    // Fruit Cut Event
    public delegate void CutEvent(string s);
    public static event CutEvent OnCutEvent;

    // Character hit event
    public delegate void CharacterHit(string s);
    public static event CharacterHit OnCharacterHit;

    // Win event
    public delegate void WinEvent();
    public static event WinEvent OnWinEvent;

    // Lose event
    public delegate void LoseEvent();
    public static event LoseEvent OnLoseEvent;

    // Lose event snake
    public delegate void LoseSnakeEvent();
    public static event LoseSnakeEvent OnLoseSnakeEvent;

    // Fall down event
    public delegate void FallDownEvent();
    public static event FallDownEvent OnFallDownEvent;

    // Smoke detection event
    public delegate void SmokeDetectEvent();
    public static event SmokeDetectEvent OnSmokeDetectEvent;

    // Smoke stop event
    public delegate void SmokeStopEvent();
    public static event SmokeStopEvent OnSmokeStopEvent;

    public delegate void StartR2L4Tutorial();
    public static event StartR2L4Tutorial OnR2L4Tutorial;

    public static void RaiseR2L4TutorialEvent()
    {
        OnR2L4Tutorial();
    }

    public static void RaiseResetEvent()
    {
        OnResetEvent();
    }

    public static void RaiseCharacterHitEvent(string s)
    {
        OnCharacterHit(s);
    }

    public static void RaiseWinEvent()
    {
        OnWinEvent();
    }

    public static void RaiseLoseEvent()
    {
        OnLoseEvent();
    }

    public static void RaiseLoseSnakeEvent()
    {
        OnLoseSnakeEvent();
    }

    public static void RaiseCutEvent(string s)
    {
        OnCutEvent(s);
    }

    public static void RaiseFallDownEvent()
    {
        OnFallDownEvent();
    }

    public static void RaiseSmokeDetectEvent()
    {
        OnSmokeDetectEvent();
    }

    public static void RaiseSmokeStopEvent()
    {
        OnSmokeStopEvent();
    }
}