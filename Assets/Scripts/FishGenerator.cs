using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishGenerator : MonoBehaviour
{
    public GameObject fishPrefab;



    void Start()
    {
        StartCoroutine(createFish());
    }

    void Update()
    {

    }

    private IEnumerator createFish()
    {
        yield return new WaitForSeconds(Random.Range(1f, 4f));

        GameObject fish = Instantiate(fishPrefab);


        float y = Random.Range(-4.34f, -1.9f);
        float x;
        bool rightFish = Random.Range(0, 2) == 1;

        if (rightFish)
        {
            x = 11;
            fish.GetComponent<Fish>().movment.x = -0.4f;
            fish.GetComponent<Transform>().Rotate(0f, 180f, 0f);

        }
        else
        {
            x = -11;
            fish.GetComponent<Fish>().movment.x = 0.4f;
        }
        fish.GetComponent<Transform>().position = new Vector3(x, y, 1);


        StartCoroutine(createFish());
    }
}
