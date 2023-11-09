using UnityEngine;

public class AnimationPlayer : MonoBehaviour
{
    public new string animation;
    void OnEnable()
    {
        GetComponent<Animation>().Play(animation);
    }
}