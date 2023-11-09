using UnityEngine;
using UnityEngine.UI;

public abstract class LocalizedText : MonoBehaviour
{
    protected Text textField;
    protected bool arabicText;

    public virtual void SubscribeToEvent()
    {
        textField = GetComponent<Text>();
        LocalizationManager.OnLanguageChanged += StartChange;
    }

    private void StartChange(bool isArabic)
    {
        if (this == null)
        {
            LocalizationManager.OnLanguageChanged -= StartChange;
            return;
        }

        ChangeLanguage(isArabic);
    }

    protected abstract void ChangeLanguage(bool isArabic);
    protected abstract void UpdateTextField();
    protected virtual void OnDestroy() => LocalizationManager.OnLanguageChanged -= StartChange;
}