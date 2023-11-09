using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEditor;
using UnityEngine.UIElements;

public class AnimationCreatorWindow : EditorWindow
{

    VisualElement dataContainer;

    [MenuItem("Tools/ Animation Creator")]
    public static void OpenWindow()
    {
        AnimationCreatorWindow editor = GetWindow<AnimationCreatorWindow>();
        // Adds a title to the window.
        editor.titleContent = new GUIContent("Animation Creator");
    }

    private void OnEnable()
    {
        rootVisualElement.Clear();
        VisualTreeAsset animationCreator = Resources.Load<VisualTreeAsset>("AnimationCreator");
        TemplateContainer container = animationCreator.CloneTree();
        dataContainer = container.Q<VisualElement>("AnimationDataContainer");
        rootVisualElement.Add(container);
        container.Q<Button>("AddAnimationData").clicked += AddAnimationData;
        container.Q<Button>("CreateAnimation").clicked += CreateAnimationData;
    }

    private void AddAnimationData()
    {
        VisualTreeAsset animationData = Resources.Load<VisualTreeAsset>("AnimationData");
        TemplateContainer data = animationData.CloneTree();
        Button deleteButton = data.Q<Button>("DeleteButton");
        deleteButton.clicked += () => dataContainer.Remove(data);
        dataContainer.Add(data);
    }

    private void CreateAnimationData()
    {
        if (dataContainer.childCount == 0)
            throw new System.Exception("No animation data available");

        while (dataContainer.childCount > 0)
        {
            VisualElement child = dataContainer.ElementAt(0);
            AnimationClip clip = new AnimationClip();

            string SpritePath = child.Q<TextField>("SpritePath").text;
            string AnimationDirectory = child.Q<TextField>("AnimationDirectory").text;
            string AnimationName = child.Q<TextField>("AnimationName").text;
            string RelativePath = child.Q<TextField>("RelativePath").text;
            int frameRate = child.Q<IntegerField>("FrameRate").value;

            List<ObjectReferenceKeyframe> keyframes = GetKeyframes(SpritePath, frameRate);
            clip.frameRate = frameRate;
            EditorCurveBinding curveBinding = EditorCurveBinding.PPtrCurve(RelativePath, typeof(SpriteRenderer), "m_Sprite");
            AnimationUtility.SetObjectReferenceCurve(clip, curveBinding, keyframes.ToArray());

            if (string.IsNullOrWhiteSpace(AnimationName))
                throw new System.Exception("Animation name can't be empty");

            if (AnimationDirectory == string.Empty)
                AssetDatabase.CreateAsset(clip, $"Assets/{AnimationName}.anim");
            else if (Directory.Exists($"{Application.dataPath}/{AnimationDirectory}"))
                AssetDatabase.CreateAsset(clip, $"Assets/{AnimationDirectory}/{AnimationName}.anim");
            else
                throw new System.Exception("Animation Directory doesn't exists");

            dataContainer.Remove(child);
        }
    }

    private List<ObjectReferenceKeyframe> GetKeyframes(string directory, int frameRate)
    {
        if (!Directory.Exists(directory))
            throw new System.Exception("Sprite Directory doesn't exists");

        List<ObjectReferenceKeyframe> keyframes = new List<ObjectReferenceKeyframe>();

        int count = 0;
        foreach (string assetPath in Directory.GetFiles(directory, "*.png"))
        {
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
            keyframes.Add(new ObjectReferenceKeyframe()
            {
                time = (float)count / frameRate,
                value = sprite
            });
            count++;
        }

        return keyframes;
    }
}