using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollAndParallax : MonoBehaviour
{

    public float backgroundSize;
    public float parallaxSpeed;
    public bool scrolling;
    public bool parallax;

    private Transform cameraTransform;
    public Transform[] layers;
    public float viewZone = 10;
    private int leftIndex;
    private int rightIndex;


    private float lastCameraX;

    // Use this for initialization
    void Start()
    {


        cameraTransform = Camera.main.transform;
        lastCameraX = cameraTransform.position.x;
        layers = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            layers[i] = transform.GetChild(i);
        }

        leftIndex = 0;
        rightIndex = layers.Length - 1;
    }


    private void ScrollLeft()
    {
        layers[rightIndex].localPosition = Vector3.right * (layers[leftIndex].localPosition.x - backgroundSize);
        leftIndex = rightIndex;
        rightIndex--;
        if (rightIndex < 0)
        {
            rightIndex = layers.Length - 1;
        }
    }

    private void ScrollRight()
    {
        layers[leftIndex].localPosition = Vector3.right * (layers[rightIndex].localPosition.x + backgroundSize);
        rightIndex = leftIndex;
        leftIndex++;
        if (leftIndex == layers.Length)
        {
            leftIndex = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (parallax)
        {
            float deltaX = cameraTransform.position.x - lastCameraX;
            transform.position += Vector3.right * (deltaX * parallaxSpeed);
        }

        lastCameraX = cameraTransform.position.x;

        if (scrolling)
        {
            if (cameraTransform.position.x < (layers[leftIndex].transform.position.x + viewZone))
                ScrollLeft();

            if (cameraTransform.position.x > (layers[rightIndex].transform.position.x - viewZone))
                ScrollRight();
        }
    }
}
