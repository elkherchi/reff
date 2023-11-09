using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.just.joystick
{
    public class TouchPhaseScript : MonoBehaviour
    {

        private Vector3 fp;   //First touch position
        private Vector3 lp;   //Last touch position
        private Vector3 fp2;   //second touch position
        private Vector3 lp2;   // second Last touch position
        private float dragDistance;  //minimum distance for a swipe to be registered
        private float dragDistance2;  //minimum distance for a swipe to be registered
        public MyCharacterController character;
        bool isDraggingRight = false;
        bool isDraggingLeft = false;
        bool canJump = true;
        bool isPressingOneFinger = false;

        void Start()
        {
            dragDistance = Screen.height * 15 / 100; //dragDistance is 15% height of the screen
            dragDistance2 = Screen.height * 15 / 100; //dragDistance is 15% height of the screen
        }

        void Update()
        {
            if (Input.touchCount == 1) // user is touching the screen with a single touch
            {
                Touch touch = Input.GetTouch(0); // get the touch
                if (touch.phase == TouchPhase.Began) //check for the first touch
                {
                    fp = touch.position;
                    lp = touch.position;
                    isPressingOneFinger = true;
                }
                else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
                {
                    lp = touch.position;

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
                                if (isPressingOneFinger && canJump)
                                {
                                    character.Jump();
                                    canJump = false;
                                    isPressingOneFinger = false;
                                }
                            }
                            else
                            {   //Down swipe
                                EditorDebugger.Log("Down Swipe");
                            }
                        }


                    }
                    else if (touch.phase == TouchPhase.Ended)
                    { //check if the finger is removed from the screen
                        lp = touch.position;  //last touch position. Ommitted if you use list
                    }
                    else
                    {   //It's a tap as the drag distance is less than 20% of the screen height
                        EditorDebugger.Log("Tap");
                    }
                }



                if (touch.phase == TouchPhase.Ended)
                {
                    isDraggingLeft = false;
                    isDraggingRight = false;
                    canJump = true;
                }
            }



            if (Input.touchCount == 2) // user is touching the screen with another touch
            {

                Touch touch2 = Input.GetTouch(0); // get the touch
                if (touch2.phase == TouchPhase.Began) //check for the first touch
                {
                    fp2 = touch2.position;
                    lp2 = touch2.position;
                    //isPressingOneFinger = true;
                    character.Jump();

                }
                //			else if (touch2.phase == TouchPhase.Moved) // update the last position based on where they moved
                //			{
                //				lp2 = touch2.position;
                //
                //				//Check if drag distance is greater than 20% of the screen height
                //				if (Mathf.Abs (lp2.x - fp2.x) > dragDistance2 || Mathf.Abs (lp2.y - fp2.y) > dragDistance2) {//It's a drag
                //					//check if the drag is vertical or horizontal
                //					if (Mathf.Abs (lp2.x - fp2.x) > Mathf.Abs (lp2.y - fp2.y)) {   //If the horizontal movement is greater than the vertical movement...
                //						if ((lp2.x > fp2.x)) {  //If the movement was to the right)//Right swipe
                //							Debug.Log ("Right Swipe");
                //						} else {   //Left swipe
                //							Debug.Log ("Left Swipe");
                //						}
                //					} else {   //the vertical movement is greater than the horizontal movement
                //						if (lp2.y > fp2.y) {  //If the movement was up//Up swipe
                //							Debug.Log ("Up Swipe");
                //							//if (!isPressingOneFinger && canJump) {
                //							character.Jump ();
                //								//canJump = false;
                //							//}
                //						} else {   //Down swipe
                //							Debug.Log ("Down Swipe");
                //						}
                //					}
                //
                //
                //				} else if (touch2.phase == TouchPhase.Ended) { //check if the finger is removed from the screen
                //					lp2 = touch2.position;  //last touch position. Ommitted if you use list
                //				}
                //				else
                //				{   //It's a tap as the drag distance is less than 20% of the screen height
                //					Debug.Log("Tap");
                //				}
                //			}
                //
                //
                //			if (touch2.phase == TouchPhase.Ended) {
                //				canJump = true;
                //
                //			}

            }




            if (isDraggingLeft)
                character.MoveLeft();
            if (isDraggingRight)
                character.MoveRight();


        }
    }
}
