using UnityEngine;
using System.Collections;

namespace com.just.joystick
{
    public class Swiper : MonoBehaviour
    {

        Vector2 screenWidth;

        bool AActive = false;
        bool BActive = false;

        int touchA;
        int touchB;
        private Vector3 fp;   //First touch position
        private Vector3 lp;   //Last touch position
        private Vector3 fp2;   //First touch position
        private Vector3 lp2;   //Last touch position
        private float dragDistance;  //minimum distance for a swipe to be registered
        private float dragDistance2;  //minimum distance for a swipe to be registered
        bool isDraggingRight = false;
        bool isDraggingLeft = false;
        public MyCharacterController character;
        bool canJump = true;

        bool couldBeSwipe;

        public Touch touch;

        //
        string AStatus;
        string BStatus;
        string switchStatus;

        // Use this for initialization
        void Start()
        {
            screenWidth = new Vector2(Screen.width / 2, 0);
            dragDistance = Screen.height * 15 / 100; //dragDistance is 15% height of the screen
            dragDistance2 = Screen.height * 15 / 100; //dragDistance is 15% height of the screen
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.touchCount > 0)
            {
                foreach (Touch touch in Input.touches)
                {
                    if (touch.position.x < screenWidth.x)
                    {
                        AStatus = "A is touching";
                        touchA = touch.fingerId;
                        AActive = true;

                        DoSwipe(touch);

                    }
                    if (touch.position.x > screenWidth.x)
                    {
                        BStatus = "B is touching";
                        touchB = touch.fingerId;
                        BActive = true;

                        DoSwipe(touch);
                    }
                }//end foreach
            }//end if touchcount


            if (isDraggingLeft)
                character.MoveLeft();
            if (isDraggingRight)
                character.MoveRight();

        }

        void DoSwipe(Touch touch)
        {
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    switchStatus = "BEGAN";
                    fp = touch.position;
                    lp = touch.position;
                    fp2 = touch.position;
                    lp2 = touch.position;
                    couldBeSwipe = true;
                    break;


                case TouchPhase.Moved:
                    switchStatus = "MOVED";

                    // First finger move detection
                    if (AActive == true)
                    {
                        switchStatus = "MOVED-AAA";
                        lp = touch.position;

                        //if (touch.position.x < screenWidth.x) {

                        //Check if drag distance is greater than 20% of the screen height
                        if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
                        {//It's a drag
                         //check if the drag is vertical or horizontal
                            if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
                            {   //If the horizontal movement is greater than the vertical movement...
                                if ((lp.x > fp.x))
                                {  //If the movement was to the right)//Right swipe
                                    EditorDebugger.Log("Right Swipe");
                                    //character.MoveRight ();
                                    isDraggingRight = true;
                                }
                                else
                                {   //Left swipe
                                    EditorDebugger.Log("Left Swipe");
                                    //character.MoveLeft ();
                                    isDraggingLeft = true;
                                }
                            }
                            else
                            {   //the vertical movement is greater than the horizontal movement
                                if (lp.y > fp.y)
                                {  //If the movement was up//Up swipe
                                    EditorDebugger.Log("Up Swipe");
                                    //if (canJump) {
                                    //character.Jump ();
                                    //canJump = false;
                                    //}
                                }
                                else
                                {   //Down swipe
                                    EditorDebugger.Log("Down Swipe");
                                }
                            }
                            //}

                        }

                    }


                    // Second finger move detection
                    if (BActive == true)
                    {
                        //if (touch.position.x > screenWidth.x)
                        //{
                        couldBeSwipe = false;
                        switchStatus = "MOVED-BBB";
                        lp2 = touch.position;

                        //Check if drag distance is greater than 20% of the screen height
                        if (Mathf.Abs(lp2.x - fp2.x) > dragDistance2 || Mathf.Abs(lp2.y - fp2.y) > dragDistance2)
                        {//It's a drag
                         //check if the drag is vertical or horizontal
                            if (Mathf.Abs(lp2.x - fp2.x) > Mathf.Abs(lp2.y - fp2.y))
                            {

                                //the vertical movement is greater than the horizontal movement
                                if (lp2.y > fp2.y)
                                {  //If the movement was up//Up swipe
                                    EditorDebugger.Log("Up Swipe");
                                    if (canJump)
                                    {
                                        character.Jump();
                                        canJump = false;
                                    }
                                }
                                else
                                {   //Down swipe
                                    EditorDebugger.Log("Down Swipe");
                                }


                            }
                            //}

                        }
                    }
                    break;


                case TouchPhase.Ended:
                    if (couldBeSwipe) { switchStatus = "ENDED-CORRECT"; }
                    else { switchStatus = "ENDED"; }

                    if (touch.fingerId == touchA)
                    {
                        // End finger one
                        if (AActive == true)
                        {
                            AActive = false;
                            isDraggingLeft = false;
                            isDraggingRight = false;
                        }
                    }
                    else if (touch.fingerId == touchB)
                    {
                        // End finger two
                        if (BActive == true)
                        {
                            BActive = false;
                            canJump = true;
                        }
                    }

                    break;
            }
        }

        void OnGUI()
        {
            GUI.Box(new Rect(5, 5, 200, 200), "");
            GUI.Box(new Rect(0, 0, screenWidth.x, Screen.height), "");
            GUI.Label(new Rect(10, 10, 180, 180), "AStatus: " + AStatus);
            GUI.Label(new Rect(10, 30, 180, 180), "BStatus: " + BStatus);
            GUI.Label(new Rect(10, 50, 180, 180), "touchA: " + touchA.ToString());
            GUI.Label(new Rect(10, 70, 180, 180), "touchB: " + touchB.ToString());
            GUI.Label(new Rect(10, 90, 180, 180), "switch: " + switchStatus);
            GUI.Label(new Rect(10, 110, 180, 180), "AActive: " + AActive.ToString());
            GUI.Label(new Rect(10, 130, 180, 180), "BActive: " + BActive.ToString());
            GUI.Label(new Rect(10, 150, 180, 180), "touchCount: " + Input.touchCount.ToString());
            //--
            GUI.Label(new Rect(10, 170, 180, 180), "fingerId: " + touch.fingerId.ToString());
        }
    }
}