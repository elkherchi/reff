using UnityEngine;
using UnityEngine.Events;

public class OnStateChangedEvent : MonoBehaviour
{
    public UnityEvent OnEnabled;
    public UnityEvent OnDisabled;
    private void OnEnable() => OnEnabled.Invoke();
    private void OnDisable() => OnDisabled.Invoke();
}