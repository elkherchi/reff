using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hook : MonoBehaviour
{
    private int offset = 4;
    private bool catchingFish;
    public GameObject fishOnHook;
    public Text score;
    private int scoreInt;


    void Update()
    {
        if (SystemInfo.deviceType == DeviceType.Desktop)
        {

            Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

            if(worldPosition.y + offset < 7.8f && worldPosition.y + offset > 0)
            {
                transform.position = new Vector3(transform.position.x, worldPosition.y + offset, 1);
                if(worldPosition.y + offset > 7.4f && catchingFish)
                {
                    catchingFish = false;
                    fishOnHook.SetActive(false);
                    addScore();
                }
            }
            
        } else if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);


            if (touchPosition.y + offset < 7.8f && touchPosition.y + offset > 0)
            {
                transform.position = new Vector3(transform.position.x, touchPosition.y + offset, 1);
                if (touchPosition.y + offset > 7.4f && catchingFish)
                {
                    catchingFish = false;
                    fishOnHook.SetActive(false);
                    addScore();
                }
            } 
        } 
    }

    public void catchFish(GameObject fish)
    {
        if (!catchingFish)
        {
            fishOnHook.SetActive(true);
            catchingFish = true;
            Destroy(fish);
        } 
    }

    private void addScore()
    {
        scoreInt++;
        score.text = "Score: " + scoreInt;
    }
}
