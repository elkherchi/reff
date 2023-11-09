using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace com.just.joystick
{
    public class Level3Region3Manager : ComicsLevel
    {
        public GameObject jumpButton;
        public RectTransform tomatoBox;
        public RectTransform bucketParent;
        public RectTransform tomatoParent;
        public Image bucket;

        [Space(10)]
        [Header("Tomatos")]
        public RectTransform[] tomatoes;
        public Animator tomatoBoxAnim;

        [Header("Tween Parameters")]
        [SerializeField] private Color bucketStartColor;
        [SerializeField] private Color bucketTargetColor;
        [SerializeField] private float bucketColorDuration;
        [SerializeField] private float bucketMoveDuration = 1;
        [SerializeField] private float bucketReturnDuration = 0.75f;
        [SerializeField] private Vector2 bucketOffset = new Vector2(-125, 150);
        [SerializeField] private float bucketRotateDuration = 0.25f;
        [SerializeField] private float tomatoMoveDuration = 0.5f;
        [SerializeField] private float tomatoRotateSpeed = 45;
        [SerializeField] private float durationMultiplier = 1;
        [SerializeField] private float minimumTomatoWait = 0.1f;
        [SerializeField] private float maximumTomatoWait = 0.25f;
        private Tween lastTween;
        private Color initialBucketColor;
        private int tomatosCollected = 0;
        private Vector3 bucketPosition;
        private Vector3 bucketScale;
        private bool isBucketTweeing;
        private const int MaxTomato = 3;

        protected override int Region => 3;
        protected override int Level => 3;
        protected override SpriteRenderer LevelCharacter => character.AdultMask;
        protected override string ComicsID => "9";
        protected override int ComicPage => 8;

        private float BucketMoveDuration => bucketMoveDuration * durationMultiplier;
        private float BucketReturnDuration => bucketReturnDuration * durationMultiplier;
        private float BucketRotateDuration => bucketRotateDuration * durationMultiplier;
        private float TomatoMoveDuration => tomatoMoveDuration * durationMultiplier;

        // When the level starts
        protected override void OnEnable()
        {
            base.OnEnable();
            initialBucketColor = bucket.color;
            bucketScale = bucketParent.localScale;
            character.collidedWithCrops += ShowBucket;
            jumpButton.SetActive(false);
            InitializeLevel();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            jumpButton.SetActive(true);
            bucket.color = initialBucketColor;
            character.collidedWithCrops -= ShowBucket;
        }

        public override void DisableLevelInfo()
        {
            base.DisableLevelInfo();
            gameManager.ToggleJoystick(true);
        }

        private void InitializeLevel()
        {
            isBucketTweeing = false;
            ResetUITomatos();
            tomatosCollected = 0;
            ResetBucketParent();
            ResetBucket();
            tomatoBoxAnim.Play("TomatoButtonIdleAnimation");
            character.OnCollectAnimationEnd();
            character.ToggleTomatoController(true);
            bucketParent.gameObject.SetActive(false);
            foreach (RectTransform tomato in tomatoes)
                ResetTomato(tomato);
        }

        #region       M A I N         F U N C T I O N S 

        void ShowBucket()
        {
            bucketParent.gameObject.SetActive(true);
        }

        // This function enables a tomato
        void EnableUITomato()
        {
            //character.ShowTomatoes();

            for (int i = 0; i < tomatoes.Length; i++)
            {
                if (!tomatoes[i].gameObject.activeSelf)
                {
                    tomatoes[i].gameObject.SetActive(true);
                    tomatoes[i].SetParent(bucketParent.GetChild(tomatosCollected - 1));
                    tomatoes[i].localScale = Vector3.one;
                    SetTomatoAnchors(tomatoes[i]);
                    bucket.transform.SetAsLastSibling();
                    return;
                }
            }
        }

        public void EmptyTomatoes()
        {
            if (tomatosCollected == 0 || !bucketParent.gameObject.activeSelf)
                return;

            tomatoBoxAnim.Play("TomatoButtonIdleAnimation");
            character.ToggleTomatoController(false);
            TweenBucket();
            isBucketTweeing = true;
            tomatosCollected = 0;
        }

        private void TweenBucket()
        {
            ResetBucket();
            InitializeBucketParent();
            bucketParent.DOScale(bucketScale * 1.25f, BucketMoveDuration).SetEase(Ease.InOutQuad);
            Sequence sequence = DOTween.Sequence();
            Vector2 target = tomatoBox.anchoredPosition;
            target.AddScalar(bucketOffset.x, bucketOffset.y);
            sequence.Append(bucketParent.DOAnchorPos(target, BucketMoveDuration));
            sequence.Append(bucketParent.DORotateQuaternion(Quaternion.Euler(0, 0, -120), BucketRotateDuration));
            sequence.SetEase(Ease.InOutQuad);
            sequence.OnComplete(() => StartCoroutine(nameof(TweenTomatoes)));
        }

        private IEnumerator TweenTomatoes()
        {
            for (int i = 0; i < tomatoes.Length; i++)
            {
                tomatoes[i].transform.SetParent(tomatoParent);

                RectTransform tomato = tomatoes[i];
                InitializeTomato(tomato);

                lastTween = tomato.DOAnchorPos(tomatoBox.anchoredPosition, TomatoMoveDuration).
                    SetEase(Ease.InOutQuad).
                    OnUpdate(() => tomato.transform.Rotate(0, 0, tomatoRotateSpeed * Time.timeScale)).
                    OnComplete(() => ResetTomato(tomato));

                yield return new WaitForSeconds(Random.Range(minimumTomatoWait, maximumTomatoWait));
            }

            yield return lastTween.WaitForCompletion();
            bucketParent.DORotateQuaternion(Quaternion.identity, BucketRotateDuration);
            bucketParent.DOScale(bucketScale, BucketReturnDuration);
            bucketParent.DOMove(bucketPosition, BucketReturnDuration);

            yield return new WaitForSeconds(BucketReturnDuration);
            ResetBucketParent();
            character.ToggleTomatoController(true);
            isBucketTweeing = false;
        }

        private void SetTomatoAnchors(RectTransform tomato)
        {
            Vector2 center = new Vector2(0.5f, 0.5f);
            tomato.SetAnchors(center);
            tomato.anchoredPosition = Vector3.zero;
        }

        private void InitializeTomato(RectTransform tomato)
        {
            Vector3 pos = tomato.transform.position;
            tomato.SetAnchors(Vector2.right);
            tomato.transform.position = pos;
        }

        private void ResetBucket()
        {
            bucket.DOKill();
            bucket.color = initialBucketColor;
        }

        private void InitializeBucketParent()
        {
            bucketPosition = bucketParent.position;
            bucketParent.SetAnchors(tomatoBox.anchorMin);
            bucketParent.position = bucketPosition;
        }

        private void ResetBucketParent()
        {
            bucketParent.DOKill();
            bucketParent.localScale = bucketScale;
            bucketParent.rotation = Quaternion.identity;
            bucketParent.anchoredPosition = Vector2.zero;
        }

        private void ResetTomato(RectTransform tomato)
        {
            tomato.rotation = Quaternion.identity;
            tomato.DOKill();
            tomato.gameObject.SetActive(false);
        }

        public void ResetUITomatos()
        {
            for (int i = 0; i < tomatoes.Length; i++)
                tomatoes[i].gameObject.SetActive(false);

            tomatosCollected = 0;
        }

        #endregion

        protected override IEnumerator RestartCoroutine()
        {
            yield return base.RestartCoroutine();
            InitializeLevel();
            StopCoroutine(nameof(TweenTomatoes));
            gameManager.ToggleJoystick(true);
        }

        public void WrongAnswer()
        {
            if (isBucketTweeing || !character.CollectAnimationEnded)
                return;

            // remove life
            character.RemoveLife();
            character.TriggerBend();
        }

        public void CorrectAnswer(GameObject go)
        {
            if (isBucketTweeing || !character.CollectAnimationEnded)
                return;

            // Check if the player cannot carry anymore, then lose
            if (tomatosCollected > MaxTomato - 1)
            {
                EventManager.RaiseLoseEvent();
                return;
            }

            // disable the chilren of this crop, which are images
            foreach (Transform child in go.transform)
            {
                bool isTomatoImage = child.CompareTag("tomato_image");
                if (isTomatoImage)
                    child.GetComponent<SpriteRenderer>().enabled = false;
                else
                    child.gameObject.SetActive(false);
            }

            bucketParent.gameObject.SetActive(false);
            character.TriggerSquat();
            StartCoroutine(nameof(LateCorrectAnswer), go);
            // Add seed as if tomato
            PlayerHit("Seed");
            tomatosCollected++;
            // Add tomato to UI
            EnableUITomato();

            // Check if the player reached max inventory, then enable warning
            if (tomatosCollected >= MaxTomato)
            {

                tomatoBoxAnim.Play("TomatoButtonAnimation");
                bucket.color = bucketStartColor;
                bucket.DOColor(bucketTargetColor, bucketColorDuration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
            }
        }

        IEnumerator LateCorrectAnswer(GameObject go)
        {
            yield return new WaitForSeconds(0.46f);
            go.GetComponent<SpriteRenderer>().enabled = false;
            go.GetComponent<BoxCollider2D>().enabled = false;

            yield return new WaitUntil(() => character.CollectAnimationEnded);

            gameManager.ToggleJoystick(true);
            character.enableSwipeController = true;

            yield return null;
        }
    }
}