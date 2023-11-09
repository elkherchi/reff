using UnityEngine;
using UnityEngine.UI;

namespace com.just.joystick
{
    public class Resetter : MonoBehaviour
    {

        public enum Type { Crop, CropQuestion, Sticker, Ladder, LadderOn, LadderOff, Tomato, Window, Tools, UI }
        public Type type;

        private Collider2D objectCollider;
        private SpriteRenderer objectSprite;

        // Use this for initialization
        void Awake()
        {
            // Subscribe to the game manager reset event
            EventManager.OnResetEvent += Reset;

            objectCollider = GetComponent<Collider2D>();
            objectSprite = GetComponent<SpriteRenderer>();
        }
        void OnDestroy()
        {
            EventManager.OnResetEvent -= Reset;
        }


        // This function checks if this gameobject is turned off, then enable it 
        void Reset()
        {
            if (gameObject.activeSelf)
            {
                // Enable the sprite again
                if (objectSprite)
                    objectSprite.enabled = true;

                //Enable the box collider 2d again
                if (objectCollider)
                    objectCollider.enabled = true;
            }
            else
                gameObject.SetActive(true);

            switch (type)
            {
                case Type.Crop:
                    objectSprite.enabled = true;
                    objectCollider.enabled = true;
                    break;
                case Type.Tools:
                    GetComponent<Image>().enabled = true;
                    break;
                case Type.Tomato:
                    objectSprite.enabled = true;
                    break;
                case Type.CropQuestion:
                    gameObject.SetActive(false);
                    break;
                case Type.Sticker:
                    gameObject.SetActive(false);
                    break;
                case Type.Ladder:
                    objectCollider.enabled = true;
                    break;
                case Type.LadderOn:
                    objectCollider.enabled = true;
                    break;
                case Type.LadderOff:
                    objectCollider.enabled = false;
                    break;
                case Type.Window:
                    objectSprite.enabled = true;
                    transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
                    break;
                default:
                    break;
            }
        }
    }
}