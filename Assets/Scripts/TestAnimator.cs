using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;

public class TestAnimator : MonoBehaviour
{
    //public Text text;

    //private void OnEnable()
    //{
    //    text.text = ArabicSupport.ArabicFixer.Fix(text.text, true, false, true);
    //    StartCoroutine(FixArabicText.FixText(text, text.text));
    //}
    //public AnimationClip clip;
    //public Sprite sprite;
    //public List<string> paths;
    //public List<string> fileNames;

    //[ContextMenu("Check Animation")]
    //public void CheckClip()
    //{
    //    ObjectReferenceKeyframe[] keyframes = AnimationUtility.GetObjectReferenceCurve(clip, EditorCurveBinding.PPtrCurve("AdultHat", typeof(SpriteRenderer), "m_Sprite"));
    //    foreach (ObjectReferenceKeyframe keyframe in keyframes)
    //    {
    //        Debug.Log(keyframe.value);
    //    }
    //}

    //[ContextMenu("Create Animation")]
    //public void Create()
    //{
    //    AnimationClip clipa = new AnimationClip();
    //    ObjectReferenceKeyframe[] keyframes = new ObjectReferenceKeyframe[3];
    //    ObjectReferenceKeyframe frame = new ObjectReferenceKeyframe
    //    {
    //        time = 0,
    //        value = sprite
    //    };

    //    keyframes.SetValue(frame, 0);

    //    EditorCurveBinding curveBinding = EditorCurveBinding.PPtrCurve("AdultHat", typeof(SpriteRenderer), "m_Sprite");
    //    AnimationUtility.SetObjectReferenceCurve(clipa, curveBinding, keyframes);
    //    Debug.Log(clipa.length);
    //    AssetDatabase.CreateAsset(clipa, "Assets/a.anim");
    //}

    //[ContextMenu("Create Sprite Animation")]
    //public void CreateSpriteAnimation()
    //{
    //    if (paths.Count != fileNames.Count)
    //        throw new System.Exception("Path and File name don't match");

    //    int pathCount = 0;
    //    foreach (string path in paths)
    //    {
    //        AnimationClip clipa = new AnimationClip();
    //        List<ObjectReferenceKeyframe> keyframes = new List<ObjectReferenceKeyframe>();
    //        int frameRate = 30;
    //        int count = 0;
    //        foreach (string assetPath in Directory.GetFiles(path, "*.png"))
    //        {
    //            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
    //            keyframes.Add(new ObjectReferenceKeyframe()
    //            {
    //                time = (float)count / frameRate,
    //                value = sprite
    //            });
    //            count++;
    //        }

    //        clipa.frameRate = frameRate;
    //        EditorCurveBinding curveBinding = EditorCurveBinding.PPtrCurve("AdultHat", typeof(SpriteRenderer), "m_Sprite");
    //        AnimationUtility.SetObjectReferenceCurve(clipa, curveBinding, keyframes.ToArray());

    //        AssetDatabase.CreateAsset(clipa, $"Assets/{fileNames[pathCount]}.anim");
    //        pathCount++;
    //    }
    //}

    //private Animator anim;
    public Sprite old;
    public Sprite newSprite;

    [ContextMenu("Check")]
    public void CheckForSprite()
    {
        foreach (SpriteRenderer renderer in FindObjectsOfType<SpriteRenderer>(true))
        {
            if (renderer.sprite == old)
                EditorDebugger.Log(renderer.name, renderer.gameObject);
        }
    }

    [ContextMenu("Update")]
    public void UpdateSprite()
    {
        foreach (SpriteRenderer renderer in FindObjectsOfType<SpriteRenderer>(true))
        {
            if (renderer.sprite == old)
                renderer.sprite = newSprite;
        }
    }

    //void Start()
    //{
    //    anim = GetComponent<Animator>();
    //}

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Alpha0))
    //    {
    //        anim.SetTrigger("ChildNoHat");
    //    }

    //    if (Input.GetKeyDown(KeyCode.Alpha1))
    //    {
    //        anim.SetTrigger("ChildWithHat");
    //    }

    //    if (Input.GetKeyDown(KeyCode.Alpha2))
    //    {
    //        anim.SetTrigger("ChildCoat");
    //    }

    //    if (Input.GetKeyDown(KeyCode.Alpha3))
    //    {
    //        anim.SetTrigger("AdultHat");
    //    }

    //    if (Input.GetKeyDown(KeyCode.Alpha4))
    //    {
    //        anim.SetTrigger("AdultMask");
    //    }

    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        anim.SetTrigger("Jump");
    //    }
    //}
}