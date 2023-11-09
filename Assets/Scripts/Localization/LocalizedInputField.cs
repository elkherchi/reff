using UnityEngine;
using UnityEngine.UI;

public class LocalizedInputField : MonoBehaviour
{
    InputField inputField;
    [SerializeField] Text correctedTextField;
    //string currentInput = "";
    bool isArabic;

    private void OnEnable()
    {
        inputField = GetComponent<InputField>();
        inputField.onValueChanged.AddListener(LocalizeInput);
    }

    private void LocalizeInput(string input)
    {
        //if (input.Length > currentInput.Length)
        //{
        //    currentInput += input[input.Length - 1];
        //}
        //else
        //{
        //    currentInput = currentInput.Remove(currentInput.Length - 1);
        //}


        //inputField.onValueChanged.RemoveListener(LocalizeInput);
        //inputField.text = ArabicSupport.ArabicFixer.Fix(currentInput, true, isArabic);

        //isArabic = inputField.text != currentInput;
        //inputField.onValueChanged.AddListener(LocalizeInput);

        string correctedText = ArabicSupport.ArabicFixer.Fix(input, true, isArabic);
        isArabic = inputField.text != correctedText;
        correctedTextField.text = correctedText;
    }
}