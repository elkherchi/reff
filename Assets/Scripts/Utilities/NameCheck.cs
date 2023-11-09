using UnityEngine;
using UnityEngine.UI;

public class NameCheck : MonoBehaviour
{
    [SerializeField] private Button nextButton;
    [SerializeField] private InputField input;
    private bool audioSourceEnded;

    private void OnEnable() => audioSourceEnded = !LocalizationManager.IsArabic;
    public void CheckNameLength(string nameInput) => nextButton.interactable = nameInput.Length > 0 && audioSourceEnded;
    public void CheckNameLength()
    {
        audioSourceEnded = true;
        nextButton.interactable = input.text.Length > 0 && audioSourceEnded;
    }
}