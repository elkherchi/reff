using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.just.joystick
{
    public class MyCharacterController : MonoBehaviour
    {
        private const float healthUpdate = 0.05f;
        private const float jumpCooldown = 0.1f;
        private const float collectDelay = 0.1f;

        [Header("References")]
        public GameManager gameManager;
        public Transform groundCheck;
        public Joystick joystick;
        public List<GameObject> LifesUI = new List<GameObject>();
        public GameObject healthBar;
        public GameObject redHealthBar;
        public ParticleSystem lifeLostParticle;
        public ParticleSystem noParticle;

        [Space(10)]
        [Header("Player settings")]
        //[HideInInspector]
        public bool grounded = false;
        public float jumpForce = 700;
        public float topSpeed = 10f;
        public int lifes = 2;
        public bool enableSwipeController = true;

        bool facingRight = true;
        Animator anim;
        Rigidbody2D rb;
        readonly float groundRadius = 0.2f;

        [HideInInspector]
        public bool isInShade = false;

        Vector3 playerInitPosition;

        // Bool for jumping from ladder
        [HideInInspector]
        public bool isOnLadder = false;
        //private bool didJumpLadder = false;

        [Space(15)]
        [Header("Set everything to Ground except Player. What will the player consider as ground?")]
        // what layer is considered the ground
        public LayerMask whatIsGround;

        public SpriteRenderer ChildHat;
        public SpriteRenderer ChildNoHat;
        public SpriteRenderer ChildCoat;
        public SpriteRenderer AdultMask;
        public SpriteRenderer AdultHat;
        public SpriteRenderer PecheNotHat;

        private SpriteRenderer currentSprite;
        private List<SpriteRenderer> allSprites;
        [SerializeField] private TomatoController tomatoBucket;
        public delegate void OnCollidedWithCrops();
        public OnCollidedWithCrops collidedWithCrops;
        private float delayBetweenHealthUpdate = 0.325f;
        private Vector3 lastGroundedPosition;
        private float currentCooldown;

        public bool IsHatOn { get; private set; }
        public bool CollectAnimationEnded { get; private set; }
        private int Speed { get; set; }
        private int JumpTrigger { get; set; }
        private int SadBool { get; set; }
        private int BendTrigger { get; set; }
        private int SquatTrigger { get; set; }

        #region  E V E N T S

        void OnEnable()
        {
            // Subscribe the character to the reset event
            EventManager.OnResetEvent += ResetCharacter;
        }

        void OnDisable()
        {
            // Insubsrcibe the character to the reset event
            EventManager.OnResetEvent -= ResetCharacter;
        }

        #endregion

        #region             S T A R T / A W A K E / U P D A T E

        private void Awake()
        {
            anim = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            playerInitPosition = new Vector3(gameObject.transform.localPosition.x, -0.26f, gameObject.transform.localPosition.z);
            GetParametersHash();
            SetAllSprites();
        }

        public void SetAnimatorController(RuntimeAnimatorController controller)
        {
            if (anim == null)
                anim = GetComponent<Animator>();

            anim.runtimeAnimatorController = controller;
        }

        private void GetParametersHash()
        {
            Speed = Animator.StringToHash("Speed");
            JumpTrigger = Animator.StringToHash("JumpTrigger");
            SadBool = Animator.StringToHash("Sad");
            BendTrigger = Animator.StringToHash("BendTrigger");
            SquatTrigger = Animator.StringToHash("SquatTrigger");
        }

        private void SetAllSprites()
        {
            allSprites = new List<SpriteRenderer>()
            {
                ChildHat,
                ChildNoHat,
                ChildCoat,
                AdultHat,
                AdultMask
            };
        }

        // This function will be called in the level manager to start the character
        public void InitializeCharacter()
        {
            IsHatOn = false;
            DisableAllAnimations();
            healthBar.SetActive(false);
        }

        public void ToggleTomatoController(bool toggle)
        {
            tomatoBucket.enabled = toggle;
        }

        public void TriggerBend()
        {
            anim.SetTrigger(BendTrigger);
            CollectAnimationEnded = false;
        }

        public void TriggerSquat()
        {
            anim.SetTrigger(SquatTrigger);
            CollectAnimationEnded = false;
        }

        public void OnCollectAnimationEnd()
        {
            anim.ResetTrigger(SquatTrigger);
            anim.ResetTrigger(BendTrigger);
            StartCoroutine(nameof(DelayedCollectAnimationEnded));
        }

        private IEnumerator DelayedCollectAnimationEnded()
        {
            yield return new WaitForSeconds(collectDelay);
            CollectAnimationEnded = true;
        }

#if UNITY_EDITOR
        void FixedUpdate()
        {
            // If the controller is enabled
            if (!enableSwipeController)
                return;

            // true or false did the ground transform hit the whatisground with the ground radius
            grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);

            if (grounded)
                lastGroundedPosition = transform.position;

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                MoveRight();
                anim.SetFloat(Speed, 1);
                if (!facingRight)
                    Flip();
            }

            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                MoveLeft();
                anim.SetFloat(Speed, 1);
                if (facingRight)
                    Flip();
            }

            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                anim.SetFloat(Speed, 0);
            }

            if (Input.GetKeyDown(KeyCode.Space))
                Jump();

            currentCooldown -= Time.fixedDeltaTime;
        }

#else
        void FixedUpdate()
        {
            // If the controller is enabled
            if (!enableSwipeController)
                return;

            // true or false did the ground transform hit the whatisground with the ground radius
            grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);

            if (joystick.CustomData.x > 0)
            {
                MoveRight();

                if (!facingRight)
                    Flip();
            }
            else
            {
                MoveLeft();

                if (joystick.CustomData.x < 0 && facingRight)
                    Flip();
            }

            anim.SetFloat(Speed, Mathf.Abs(joystick.Value));
            currentCooldown -= Time.fixedDeltaTime;
        }
#endif

        #endregion

        #region           M O V E M E N T

        void Flip()
        {
            facingRight = !facingRight;
            // get the local scale
            Vector3 theScale = transform.localScale;
            // flip the x axis
            theScale.x *= -1;
            // appl it to the local scale
            transform.localScale = theScale;

            Vector3 healthbarScale = healthBar.transform.localScale;
            healthbarScale.x *= -1;
            // Rotate healthbar too, because its child of player
            healthBar.transform.localScale = healthbarScale;
        }

        public void Jump()
        {
            if (grounded && currentCooldown < 0)
            {
                grounded = false;
                Vector2 velocity = rb.velocity;
                velocity.y = 0;
                rb.velocity = velocity;
                rb.AddForce(new Vector2(0, jumpForce));
                anim.SetTrigger(JumpTrigger);
                currentCooldown = jumpCooldown;
            }
        }

        public void MoveToLastGroundedPosition()
        {
            transform.position = lastGroundedPosition;
        }

#if UNITY_EDITOR

        public void MoveRight()
        {
            rb.velocity = new Vector2(topSpeed, rb.velocity.y);
        }

        public void MoveLeft()
        {
            rb.velocity = new Vector2(-topSpeed, rb.velocity.y);
        }
#else

        public void MoveRight()
        {
            rb.velocity = new Vector2(joystick.Value * topSpeed, rb.velocity.y);
        }

        public void MoveLeft()
        {
            rb.velocity = new Vector2(-joystick.Value * topSpeed, rb.velocity.y);
        }

#endif

        #endregion

        #region         C O R O U T I N E S  +  U T I L S

        // This function disables all character animations until one level manage enables the needed one
        public void DisableAllAnimations()
        {
            anim.SetFloat(Speed, 0);

            foreach (SpriteRenderer renderer in allSprites)
                renderer.enabled = false;
        }

        // This function enables the needed animation
        public void EnableAnimation(SpriteRenderer sprite)
        {
            currentSprite = sprite;
            currentSprite.enabled = true;
        }

        public void ResetSadBool()
        {
            anim.SetBool(SadBool, false);
        }

        public void MakeCharacterSad()
        {
            anim.SetBool(SadBool, true);
        }

        public void PlayNoParticleSystem()
        {
            ResetSadBool();
            noParticle.Play();
        }

        // This function will trigger when the player takes full damage from sun
        public void RemoveLife()
        {
            if (lifes > 0)
            {

                lifes--;  // remove 1 life

                LifesUI[lifes].SetActive(false); // Turn off last life UI from the UI
                                                 // remove life from array
                                                 //LifesUI.Remove(LifesUI[LifesUI.Count-1]);

                StartCoroutine(nameof(PlayerHitEffect)); // call the hit effect coroutine

                // reset player life
                redHealthBar.transform.localScale = new Vector3(1, 1, 1);

                // play particle system
                lifeLostParticle.Play();

                if (lifes <= 0)
                {
                    // Raise lose event
                    EventManager.RaiseLoseEvent();
                    return;
                }

            }
            else
            {
                StopCoroutine(nameof(LifeCoroutine));
                EventManager.RaiseLoseEvent();
            }
        }


        // This function resets lifes
        public void ResetLifes()
        {
            lifes = 3;
            redHealthBar.transform.localScale = new Vector3(1, 1, 1);

            // turn on all lifes
            for (int i = 0; i < LifesUI.Count; i++)
            {
                LifesUI[i].SetActive(true);
            }
        }


        // This coroutine will make the off/on effect when the player hits an obsticle
        IEnumerator PlayerHitEffect()
        {
            // Number of how many times the sprite will turn off and on
            int nmb = 3;
            for (int i = 0; i < nmb; i++)
            {
                currentSprite.enabled = false;
                yield return new WaitForSeconds(0.1f);
                currentSprite.enabled = true;
                yield return new WaitForSeconds(0.1f);
            }

            yield return null;
        }

        public void StartLifeCoroutine() => StartCoroutine(nameof(LifeCoroutine));

        IEnumerator LifeCoroutine()
        {
            healthBar.SetActive(true);
            float x;
            Vector3 scale = redHealthBar.transform.localScale;

            while (true)
            {
                yield return new WaitForSeconds(delayBetweenHealthUpdate);
                x = redHealthBar.transform.localScale.x;
                x = Mathf.Clamp01(x + (IsHatOn || isInShade ? healthUpdate : -healthUpdate));
                scale.x = x;
                redHealthBar.transform.localScale = scale;

                if (redHealthBar.transform.localScale.x <= 0)
                {
                    redHealthBar.transform.localScale = Vector3.zero;
                    RemoveLife();
                }
            }
        }

        public void ToggleHat()
        {
            IsHatOn = !IsHatOn;
            UpdateCurrentSprite(ChildHat);
        }

        public void ToggleCoat()
        {
            IsHatOn = !IsHatOn;
            UpdateCurrentSprite(ChildCoat);
        }

        private void UpdateCurrentSprite(SpriteRenderer withAccessory)
        {
            bool spriteState = currentSprite.enabled;
            currentSprite.enabled = false;
            currentSprite = IsHatOn ? withAccessory : ChildNoHat;
            currentSprite.enabled = spriteState;
        }

        // This function is called by the game manager in order to reset the character's position
        public void ResetCharacter()
        {
            EditorDebugger.Log("Character Reset Event has been called."); 

            DisableCharacterColliders();

            // reset lifes
            ResetLifes();

            // reset velocity
            rb.velocity = Vector3.zero;
            gameObject.transform.localPosition = playerInitPosition;
            StopCharacterMovement();
            Invoke(nameof(EnableCharacterCollider), 0.5f);
        }

        public void DisableCharacterColliders()
        {
            rb.isKinematic = true;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
        }

        void EnableCharacterCollider()
        {
            rb.isKinematic = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
            gameObject.GetComponent<CircleCollider2D>().enabled = true;
        }

        public void StopCharacterMovement()
        {
            rb.velocity = Vector3.zero;
            enableSwipeController = false;
            anim.SetFloat(Speed, 0);
        }

        #endregion

        #region C O L L I S I O N   E N T E R

        void OnCollisionEnter2D(Collision2D coll)
        {
            if (coll.gameObject.CompareTag("Ladder"))
            {

                //didJumpLadder = false;
            }

            // Ladder hit
            if (coll.gameObject.CompareTag("Ground"))
            {
                if (isOnLadder)
                {
                    RemoveLife();
                }
                isOnLadder = false;
                //didJumpLadder = false;
            }
        }

        #endregion
    }
}