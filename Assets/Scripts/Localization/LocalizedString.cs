using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LocalizedString : LocalizedText
{
    public string StringKey = "";
    private string textValue;
    private float initialAlpha;
    [SerializeField] Font englishFont;
    [SerializeField] Font arabicFont;

    public bool UpdatingText { get; set; }
    private bool TextContentUpdated => textField.text == textValue;

    private void OnEnable()
    {
        if (textField != null && !TextContentUpdated)
            UpdateTextField();
    }

    private void OnDisable()
    {
        if (UpdatingText)
            ResetColor();

        UpdatingText = false;
    }

    private void ResetColor()
    {
        Color color = textField.color;
        color.a = initialAlpha;
        textField.color = color;
    }

    public override void SubscribeToEvent()
    {
        base.SubscribeToEvent();
        initialAlpha = textField.color.a;
    }

    protected override void ChangeLanguage(bool isArabic)
    {
        textValue = LocalizationManager.GetFromDictionary(StringKey);
        arabicText = isArabic;
        if (gameObject.activeInHierarchy)
            UpdateTextField();
    }

    protected override void UpdateTextField()
    {
        UpdateAlignment();
        if (arabicText)
        {
            textField.font = arabicFont;
            StartCoroutine(FixText());
        }
        else
        {
            textField.font = englishFont;
            textField.text = textValue;
        }
    }

    private IEnumerator FixText()
    {
        UpdatingText = true;
        yield return StartCoroutine(FixArabicText.FixText(textField, textValue));
        textValue = textField.text;
        UpdatingText = false;
    }

    private void UpdateAlignment()
    {
        TextAnchor textAlignment = textField.alignment;
        if (IsAnchoredCenter(textAlignment))
            return;

        if (arabicText && IsAnchoredLeft(textAlignment))
            textField.alignment = GetFlippedAnchor(textAlignment);
        else if (!arabicText && IsAnchoredRight(textAlignment))
            textField.alignment = GetFlippedAnchor(textAlignment);
    }

    private bool IsAnchoredCenter(TextAnchor anchor)
    {
        return anchor == TextAnchor.MiddleCenter || anchor == TextAnchor.LowerCenter || anchor == TextAnchor.UpperCenter;
    }

    private bool IsAnchoredLeft(TextAnchor anchor)
    {
        return anchor == TextAnchor.MiddleLeft || anchor == TextAnchor.LowerLeft || anchor == TextAnchor.UpperLeft;
    }

    private bool IsAnchoredRight(TextAnchor anchor)
    {
        return anchor == TextAnchor.MiddleRight || anchor == TextAnchor.LowerRight || anchor == TextAnchor.UpperRight;
    }

    private TextAnchor GetFlippedAnchor(TextAnchor anchor)
    {
        return anchor switch
        {
            TextAnchor.UpperLeft => TextAnchor.UpperRight,
            TextAnchor.UpperRight => TextAnchor.UpperLeft,
            TextAnchor.MiddleLeft => TextAnchor.MiddleRight,
            TextAnchor.MiddleRight => TextAnchor.MiddleLeft,
            TextAnchor.LowerLeft => TextAnchor.LowerRight,
            TextAnchor.LowerRight => TextAnchor.LowerLeft,
            _ => anchor
        };
    }

    #region EDITOR FUNCTIONS
#if UNITY_EDITOR

    [ContextMenu("Convert Multiline Text")]
    public void Convert()
    {
        textField = GetComponent<Text>();
        string content = textField.text;

        content = content.Replace("\n", "\\n").Replace("\"", "\\\"");
        EditorDebugger.Log(content);
        System.IO.File.WriteAllText(Application.dataPath + "/converted.txt", content);
    }

#endif
    #endregion
}