using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LocalizedString)),CanEditMultipleObjects]
public class LocalizedTextInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        LocalizedString text = (LocalizedString)target;
        GUIStyle red = new GUIStyle(EditorStyles.label);
        red.normal.textColor = Color.red;

        if (text.StringKey == "")
        {
            GUILayout.Label("Current Key is invalid", red);
            return;
        }

        List<string> fieldNames = new List<string>();
        foreach (FieldInfo field in typeof(TextValues).GetFields().OrderBy(fieldInfo => fieldInfo.Name))
        {
            if (field.Name.ToLower().StartsWith(text.StringKey.ToLower()))
                fieldNames.Add(field.Name);
        }

        if (!fieldNames.Contains(text.StringKey))
            GUILayout.Label("Current Key is invalid", red);

        GUILayout.BeginScrollView(Vector2.zero);

        foreach (string name in fieldNames)
        {
            if (GUILayout.Button(name))
            {
                text.StringKey = name;
                GUI.FocusControl(null);
            }
        }

        GUILayout.EndScrollView();
    }
}