using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;


public class DragBehavior : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public enum FruitType { Fruit, Bomb };
    public FruitType fruitType;

    private bool mouseDown = false;
    private Vector3 startMousePos;
    private Vector3 startPos;
    private float fakeX;
    private float fakeY;
    private float myWidth;
    private float myHeight;

    bool hasReleased = false;
    bool onlyOnce = true;

    Vector3 localmousePos;

    // Player has clicked this object
    public void OnPointerDown(PointerEventData ped)
    {


    }


    // Player has released the touch button
    public void OnPointerUp(PointerEventData ped)
    {

    }

    void OnMouseDown()
    {
        mouseDown = true;
        hasReleased = false;

        // Restrict the startpos to only 1 time - Avoiding position bug if player touches screen fast
        if (onlyOnce)
        {
            startPos = transform.localPosition;
            onlyOnce = false;
        }
        startMousePos = Input.mousePosition;

        // Reset the velocity of the grabbed object
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        // Disable the trigger so we can put it on the basket
        GetComponent<BoxCollider2D>().isTrigger = false;

    }

    void OnMouseUp()
    {
        mouseDown = false;
        hasReleased = true;
    }

    void Update()
    {
        if (mouseDown)
        {
            localmousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.localPosition = new Vector3(localmousePos.x, localmousePos.y, 0);
        }
    }

    void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Basket") && hasReleased && fruitType == FruitType.Fruit)
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            EventManager.RaiseCharacterHitEvent("Seed");
            EventManager.RaiseCutEvent(fruitType.ToString()); // tell the event manager we have hit either a fruit or bomb based on this GO name
        }

        if (coll.gameObject.CompareTag("Basket") && hasReleased && fruitType == FruitType.Bomb)
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            EventManager.RaiseCutEvent(fruitType.ToString()); // tell the event manager we have hit either a fruit or bomb based on this GO name
        }

    }

    IEnumerator PutItemOnTable()
    {
        yield return null;
    }
}