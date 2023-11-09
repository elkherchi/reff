using UnityEngine;
using DG.Tweening;

public class RotateScript : MonoBehaviour
{
    public float rotationSpeed;

    private void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}