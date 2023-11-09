using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace com.just.joystick
{

    public class Level6ManagerRegion2 : ComicsLevel
    {
        public GameObject machine;
        public float maxDistance;
        public float timeToArriveToDestinationInSeconds;
        public float XinitialPos;
        public float XfinalPos;

        private Animator machineAnimator;
        private Tweener machineTween;

        protected override int Region => 2;
        protected override int Level => 6;
        protected override SpriteRenderer LevelCharacter => character.ChildHat;
        protected override string ComicsID => "7";
        protected override int ComicPage => 6;
        private float MachineX => machine.transform.position.x;
        private float CharacterX => character.transform.position.x;

        protected override void OnEnable()
        {
            base.OnEnable();
            machineAnimator = machine.GetComponent<Animator>();
            machineTween = machine.transform.DOMoveX(XfinalPos, timeToArriveToDestinationInSeconds, false).SetEase(Ease.Linear).Pause();
            ResetMachine();
        }

        protected override void EndGame()
        {
            base.EndGame();
            // kill machine coroutine
            machineTween.Pause();
        }

        // This function disables the level info
        public override void DisableLevelInfo()
        {
            base.DisableLevelInfo();
            MoveMachine();
        }

        // This function will move the machine from point a to b
        void MoveMachine()
        {
            ResetMachine();
            machineTween.Restart();
            machineAnimator.enabled = false;
            StartCoroutine(nameof(DistanceCheck));
        }

        void ResetMachine()
        {
            // kill machine coroutine
            machineTween.Pause();
            StopCoroutine(nameof(DistanceCheck));

            machineAnimator.enabled = true;
            machineAnimator.Play("MachineIdle");
        }

        //Stop machine if player is too far away
        IEnumerator DistanceCheck()
        {
            while (true)
            {
                if (machineTween.IsPlaying())
                {
                    if (Mathf.Abs(MachineX - CharacterX) > maxDistance)
                        machineTween.Pause();
                }
                else
                {
                    if (Mathf.Abs(MachineX - CharacterX) <= maxDistance)
                        machineTween.Play();
                }

                yield return null;
            }
        }

        protected override IEnumerator RestartCoroutine()
        {
            yield return base.RestartCoroutine();

            ResetMachine();
            character.DisableAllAnimations();
            character.EnableAnimation(LevelCharacter);
            MoveMachine();
        }
    }
}