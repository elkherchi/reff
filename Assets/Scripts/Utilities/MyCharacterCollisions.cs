using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.just.joystick
{
    public class MyCharacterCollisions : MonoBehaviour
    {

        public MyCharacterController characterController;
        public GameObject jumpButton;

        #region          T R I G G E R  +  C O L L I S I O N

        void OnTriggerEnter2D(Collider2D coll)
        {
            if (coll.CompareTag("Bully"))
            {
                Level7Manager.stop(coll.gameObject);
            }

            else if (coll.CompareTag("SunFatigue"))
            {
                // Call sun fatigue coroutine
                //characterController.fatigueCoroutineRunning = true;
                //characterController.StopAllCoroutines ();
                //characterController.StartCoroutine("SunFatigueCoroutine");
            }

            else if (coll.CompareTag("FatigueShade"))
            {
                // Call sun fatigue coroutine
                characterController.isInShade = true;
            }


            else if (coll.CompareTag("LadderOn"))
            {
                coll.GetComponent<LadderScript>().TurnOnLadderOff();
                characterController.isOnLadder = true;
            }

            else if (coll.CompareTag("LadderOff"))
            {
                characterController.isOnLadder = false;
                coll.GetComponent<LadderScript>().TurnOffLadder();
            }

            // If we hit an obsticle
            else if (coll.CompareTag("Obsticle"))
            {
                //PlayerGotHit();
            }

            // if player hits smoke detector
            else if (coll.CompareTag("SmokeDetector"))
            {
                EventManager.RaiseSmokeDetectEvent();
                characterController.gameManager.window = coll.transform.parent.transform.Find("ShaderClosed").gameObject;
                characterController.gameManager.smokeDetector = coll.GetComponent<BoxCollider2D>();
            }

            // If we hit an obsticle
            else if (coll.CompareTag("SharpTool"))
            {
                // remove life
                characterController.RemoveLife();
            }


            // If the player hits deadly object
            else if (coll.CompareTag("LoseTrigger"))
            {

                // Raise lose event
                EventManager.RaiseLoseEvent();
            }

            // If the player has fell underground
            else if (coll.CompareTag("FallTrigger"))
            {
                characterController.DisableCharacterColliders();
                characterController.MoveToLastGroundedPosition();
                // Raise lose event
                EventManager.RaiseFallDownEvent();
            }

            // If the player hits a house with sticker
            else if (coll.CompareTag("HouseSticker"))
            {
                EventManager.RaiseR2L4TutorialEvent();

                characterController.gameManager.sticker = coll.gameObject;
            }

            else if (coll.CompareTag("Seed"))
            {

                // disable the seed by disabling the sprite and the collider
                coll.GetComponent<SpriteRenderer>().enabled = false;
                coll.GetComponent<BoxCollider2D>().enabled = false;
                if (coll.GetComponent<ParticlePlayScript>())
                    coll.GetComponent<ParticlePlayScript>().PlayParticle();

                // Tell the world i have hit a seed
                EventManager.RaiseCharacterHitEvent("Seed");
                //Debug.Log ("event has been called");
            }

            // If the player reached the end of the level
            else if (coll.CompareTag("LevelEnd"))
            {
                // Raise win event
                EventManager.RaiseWinEvent();
            }

            // If the player hits a crop, enable the question and answer
            else if (coll.CompareTag("Crop"))
            {

                // stop the character mmovement
                characterController.StopCharacterMovement();

                // stop the joystick
                characterController.gameManager.joystick.SetActive(false);

                jumpButton.SetActive(false);
                characterController.collidedWithCrops?.Invoke();

                // enable the chilren of this crop, which are images
                for (int i = 0; i < coll.transform.childCount; i++)
                {
                    coll.transform.GetChild(i).gameObject.SetActive(true);
                }
            }
        }

        void OnTriggerExit2D(Collider2D coll)
        {

            if (coll.CompareTag("FatigueShade"))
            {
                // enable hit from sun again
                characterController.isInShade = false;
            }

            // If the player hits a house with sticker
            else if (coll.CompareTag("HouseSticker"))
            {
                characterController.gameManager.sticker = null;

                if (!coll.transform.GetChild(0).gameObject.activeSelf)
                    characterController.RemoveLife();
            }

            // If the player goes outside smoke zone
            else if (coll.CompareTag("SmokeDetector"))
            {
                characterController.gameManager.window = null;
            }
        }

        #endregion
    }
}