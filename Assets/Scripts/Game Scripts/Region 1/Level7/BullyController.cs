using UnityEngine;

public class BullyController : MonoBehaviour
{
    [SerializeField] private new SpriteRenderer renderer;
    [SerializeField] private new Collider2D collider;
    [SerializeField] private Animator anim;
    [SerializeField] private string animationClip;

    private void OnEnable()
    {
        anim.Play(animationClip);
        EventManager.OnResetEvent += TurnOn;
    }

    private void OnDisable()
    {
        EventManager.OnResetEvent -= TurnOn;
    }

    public void TurnOff()
    {
        renderer.enabled = false;
        collider.enabled = false;
    }

    private void TurnOn()
    {
        renderer.enabled = true;
        collider.enabled = true;
    }
}