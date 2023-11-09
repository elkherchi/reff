using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class FixArabicText
{
    public static IEnumerator FixText(Text textField, string textValue)
    {
        Color initial = textField.color;
        float initialAlpha = initial.a;
        initial.a = 0;
        textField.color = initial;
        int generatedLinesCount;
        string textContent;
        string TextHolder = string.Empty;
        string[] lines = textValue.Split('\n');
        Stack<string> FixedText = new Stack<string>();

        for (int lineIndex = 0; lineIndex < lines.Length; lineIndex++)
        {
            textField.text = lines[lineIndex];
            yield return new WaitForEndOfFrame();

            generatedLinesCount = textField.cachedTextGenerator.lineCount;
            textContent = textField.text;
            int startIndex;
            int endIndex;
            int length;

            for (int k = 0; k < generatedLinesCount; k++)
            {
                startIndex = textField.cachedTextGenerator.lines[k].startCharIdx;
                endIndex = (k == generatedLinesCount - 1) ? textContent.Length : textField.cachedTextGenerator.lines[k + 1].startCharIdx;
                length = endIndex - startIndex;
                FixedText.Push(textContent.Substring(startIndex, length));
            }

            while (FixedText.Count > 0)
            {
                TextHolder += FixedText.Pop();

                if (FixedText.Count > 0)
                    TextHolder += '\n';
            }

            if (lineIndex + 1 < lines.Length)
                TextHolder += '\n';
        }

        initial.a = initialAlpha;
        textField.text = TextHolder;
        textField.color = initial;
    }
}