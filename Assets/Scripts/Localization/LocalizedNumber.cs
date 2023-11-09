public class LocalizedNumber : LocalizedText
{
    public int Number;
    private bool isUpdated;

    private void OnEnable()
    {
        if (textField != null && !isUpdated)
            UpdateTextField();
    }

    protected override void ChangeLanguage(bool isArabic)
    {
        arabicText = isArabic;
        isUpdated = false;
        if (gameObject.activeInHierarchy)
            UpdateTextField();
    }

    protected override void UpdateTextField()
    {
        if (arabicText)
            textField.text = ArabicSupport.ArabicFixer.Fix(Number.ToString(), true, true);
        else
            textField.text = Number.ToString();

        isUpdated = true;
    }
}