using System.Collections.Generic;
using UnityEngine;

namespace com.just.joystick
{
    public class FastSwipe : MonoBehaviour
    {

        [Header("Game Manager")]
        public GameManager GameManager;
        [Space(10)]

        private Vector2 fingerStart;
        private Vector2 fingerEnd;
        public MyCharacterController character;
        public float tolerance = 0f;
        bool canSwipe = true;

        bool didSwipeRight = false;
        bool didSwipeLeft = false;

        public enum Movement
        {
            Left,
            Right,
            Up,
            Down
        }

        public List<Movement> movements = new List<Movement>();

        private bool HorizontalMovement => Mathf.Abs(fingerEnd.x - fingerStart.x) > tolerance;
        private bool VerticalMovement => Mathf.Abs(fingerEnd.y - fingerStart.y) > tolerance;
        private float HalfWidth => Screen.width * 0.5f;

        void Update()
        {

            //Example usage in Update. Note how I use Input.GetMouseButton instead of Input.touch

            //GetMouseButtonDown(0) instead of TouchPhase.Began
            if (Input.GetMouseButtonDown(0))
            {
                fingerStart = Input.mousePosition;
                fingerEnd = Input.mousePosition;
            }

            //This returns true if the LMB is held down in standalone OR
            //there is a single finger touch on a mobile device
            if (canSwipe && Input.GetMouseButton(0))
            {
                fingerEnd = Input.mousePosition;

                EditorDebugger.Log(fingerStart.x + ",  " + Screen.width / 2);

                //There was some movement! The tolerance variable is to detect some useful movement
                //i.e. an actual swipe rather than some jitter. This is the same as the value of 80
                //you used in your original code.
                if ((HorizontalMovement || VerticalMovement) && (fingerStart.x < HalfWidth) && (fingerEnd.x < HalfWidth))
                {

                    //There is more movement on the X axis than the Y axis
                    if (Mathf.Abs(fingerStart.x - fingerEnd.x) > Mathf.Abs(fingerStart.y - fingerEnd.y) && fingerEnd.x < HalfWidth)
                    {
                        //Right Swipe
                        if ((fingerEnd.x - fingerStart.x) > 0)
                        {
                            EditorDebugger.Log("swipe right");
                            //character.MoveRight ();
                            didSwipeRight = true;
                            didSwipeLeft = false;

                        }
                        else
                        { //Left Swipe
                            EditorDebugger.Log("swipe left");
                            didSwipeLeft = true;
                            didSwipeRight = false;
                        }
                    }

                    //After the checks are performed, set the fingerStart & fingerEnd to be the same
                    fingerStart = fingerEnd;
                }
            }

            //GetMouseButtonUp(0) instead of TouchPhase.Ended
            if (Input.GetMouseButtonUp(0))
            {
                fingerStart = Vector2.zero;
                fingerEnd = Vector2.zero;
                movements.Clear();
                canSwipe = true;
                didSwipeRight = false;
                didSwipeLeft = false;
            }

            if (didSwipeRight)
                character.MoveRight();

            if (didSwipeLeft)
                character.MoveLeft();
        }
    }
}