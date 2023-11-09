using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.just.joystick
{
    public enum JoystickType
    {
        Static = 0, Dynamic = 1
    }

    public class Joystick : MonoBehaviour
    {
        public delegate void StartDragEventHandler(Joystick joystick);
        public delegate void StopDragEventHandler(Joystick joystick);
        public delegate void UpdateEventHandler(Joystick joystick, float angle, float value);
        public delegate void PressureEventHandler(Joystick joystick, int fingerId, Vector2 position, float pressure, Vector2 deltaPosition, float deltaTime);
        public delegate void PressureTapEventHandler(Joystick joystick, int fingerId, Vector2 position, float pressure, Vector2 deltaPosition, float deltaTime);

        public event StartDragEventHandler OnStartDrag;
        public event StopDragEventHandler OnStopDrag;
        public event UpdateEventHandler OnUpdate;
        public event PressureEventHandler OnPressure;
        public event PressureTapEventHandler OnPressureTap;

        private static readonly List<int> DraggingPointerIds = new List<int>();

        [SerializeField] private Image _backgroundImage;
        [SerializeField] private Image _foregroundImage;
        [SerializeField] private JoystickType _type;
        [SerializeField] private int _segmentCount;
        [SerializeField] private float _segmentOffset;
        [SerializeField] private float _dragMaxRadius = 200f;
        [SerializeField] private bool _tweenDrag = true;
        [SerializeField] private float _tweenDragEaseFactor = 5f;
        [SerializeField] private float _fadeInDuration = .15f;
        [SerializeField] private float _fadeOutDuration = .15f;
        [SerializeField] private bool _pressureTap = true;
        [SerializeField] private float _pressureTapFactor = 0.75f;

        public Bounds ScreenBounds;
        public bool ApplyScreenBounds;

        public string Id { get; private set; }
        public bool Initialised { get; private set; }
        public bool Disabled { get; private set; }
        public bool Hidden { get; private set; }
        public bool Destroyed { get; private set; }
        public bool Active
        {
            get { return Initialised && !Disabled && !Hidden && !Destroyed; }
        }

        public bool Dragging
        {
            get { return _dragging; }
        }

        public float Angle { get; private set; }
        public float Value { get; private set; }
        public Vector3 CustomData { get; set; }

        private RectTransform _rectTransform;
        private RectTransform _foregroundImageRectTransform;

        private float[] _segmentAngles;
        private float _lastAngle;
        private float _lastValue;
        private float _dragMaxSqrRadius;
        private bool _dragging;
        private int _draggingPointerId = -1;
        private Vector3 _dragTargetPosition;
        private bool _tweening;
        private bool _isPressureTapping;

        private float _backgroundImageAlpha;
        private float _foregroundImageAlpha;

        private void Awake()
        {
            Init(gameObject.name);
        }

        private void OnDisable()
        {
            Value = 0;

            while (DraggingPointerIds.Count > 0)
                StopDrag(DraggingPointerIds[0]);
        }

        private void Init(string id)
        {
            Id = id;

            _rectTransform = GetComponent<RectTransform>();
            _foregroundImageRectTransform = _foregroundImage.rectTransform;

            _backgroundImageAlpha = _backgroundImage.color.a;
            _foregroundImageAlpha = _foregroundImage.color.a;

            InitType();
            InitSegments();
            InitDragMaxSqrRadius();
            InitTweenDragEaseFactor();

            Initialised = true;
        }

        private void InitType()
        {
            if (_type == JoystickType.Dynamic)
                Hide();
            else
                Show();
        }

        private void InitSegments()
        {
            if (_segmentCount <= 0) return;

            _segmentAngles = new float[_segmentCount];
            var intersegmentAngle = 360f / _segmentCount;
            for (var i = 0; i < _segmentCount; i++)
                _segmentAngles[i] = _segmentOffset + intersegmentAngle * i;
        }

        private void InitDragMaxSqrRadius()
        {
            _dragMaxSqrRadius = _dragMaxRadius * _dragMaxRadius;
        }

        private void InitTweenDragEaseFactor()
        {
            if (_tweenDragEaseFactor < 1f)
                _tweenDragEaseFactor = 1f;
        }

        public void Enable()
        {
            Disabled = false;
        }

        public void Disable()
        {
            Disabled = true;
        }

        public void SwitchEnable()
        {
            if (Disabled)
                Enable();
            else
                Disable();
        }

        public void Show(bool animated = false)
        {
            _backgroundImage.enabled = true;
            _foregroundImage.enabled = true;

            Hidden = false;

            if (!animated) return;

            _backgroundImage.CrossFadeAlpha(_backgroundImageAlpha, _fadeInDuration, false);
            _foregroundImage.CrossFadeAlpha(_foregroundImageAlpha, _fadeInDuration, false);
        }

        public void Hide(bool animated = false)
        {
            if (!animated)
            {
                _backgroundImage.enabled = false;
                _foregroundImage.enabled = false;
            }
            else
            {
                _backgroundImage.CrossFadeAlpha(0f, _fadeOutDuration, false);
                _foregroundImage.CrossFadeAlpha(0f, _fadeOutDuration, false);
            }

            Hidden = true;
        }

        public virtual void SwitchShow()
        {
            if (Hidden)
                Show();
            else
                Hide();
        }

        private void Update()
        {
            if (_type == JoystickType.Static)
                InternalUpdate();
        }

        private void LateUpdate()
        {
            if (_type == JoystickType.Dynamic)
                InternalUpdate();
        }

        private void InternalUpdate()
        {
            if (!Initialised || Disabled || Destroyed) return;

            var isTouchSupported = Input.touchSupported;
            if (!isTouchSupported || Input.touches.Length > 0)
            {
                if (!_dragging)
                {
                    if (!isTouchSupported)
                    {
                        if (Input.GetMouseButtonDown(0) && !DraggingPointerIds.Contains(0))
                            TryStartDrag(0, Input.mousePosition);
                    }
                    else
                    {
                        foreach (var touch in Input.touches)
                        {
                            if (touch.phase != TouchPhase.Began || DraggingPointerIds.Contains(touch.fingerId))
                                continue;

                            TryStartDrag(touch.fingerId, touch.position);
                            if (_dragging)
                                break;
                        }
                    }
                }
                else
                {
                    if (!isTouchSupported)
                    {
                        if (Input.GetMouseButton(_draggingPointerId))
                            UpdateDragPosition(Input.mousePosition);
                        else
                            StopDrag(_draggingPointerId);
                    }
                    else
                    {
                        foreach (var touch in Input.touches)
                        {
                            if (touch.fingerId != _draggingPointerId) continue;

                            if (Input.touchPressureSupported && _pressureTap &&
                                (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary))
                            {
                                var minTapPressure = touch.maximumPossiblePressure * _pressureTapFactor;
                                if (touch.pressure >= minTapPressure)
                                {
                                    if (OnPressure != null)
                                        OnPressure(this, touch.fingerId, touch.position, touch.pressure,
                                            touch.deltaPosition, touch.deltaTime);

                                    if (!_isPressureTapping)
                                    {
                                        _isPressureTapping = true;
                                        if (OnPressureTap != null)
                                            OnPressureTap(this, touch.fingerId, touch.position, touch.pressure,
                                                touch.deltaPosition, touch.deltaTime);
                                    }
                                }
                                else
                                    _isPressureTapping = false;
                            }

                            switch (touch.phase)
                            {
                                case TouchPhase.Moved:
                                    // case TouchPhase.Stationary:
                                    UpdateDragPosition(touch.position);
                                    break;
                                case TouchPhase.Canceled:
                                case TouchPhase.Ended:
                                    StopDrag(touch.fingerId);
                                    break;
                            }
                        }
                    }

                }
            }

            if (!_tweening) return;

            if ((_dragTargetPosition - _foregroundImageRectTransform.position).sqrMagnitude < 25f)
            {
                _tweening = false;
                _foregroundImageRectTransform.position = _dragTargetPosition;
            }
            else
                _foregroundImageRectTransform.position += (_dragTargetPosition - _foregroundImageRectTransform.position) / _tweenDragEaseFactor;
        }

        private void TryStartDrag(int pointerId, Vector3 position)
        {
            //if (ApplyScreenBounds)
            //    if (!ScreenBounds.Contains(position)) return;

            if (position.x > Screen.width * 0.5f) return;

            if (_type == JoystickType.Dynamic)
            {
                _rectTransform.position = position;
                Show(true);
            }
            else
            {
                var distance = position - _rectTransform.position;
                if (distance.sqrMagnitude > _dragMaxSqrRadius)
                    return;
            }

            DraggingPointerIds.Add(pointerId);

            _dragging = true;
            _draggingPointerId = pointerId;

            _tweening = false;
            _dragTargetPosition = _foregroundImageRectTransform.position;

            if (OnStartDrag != null)
                OnStartDrag(this);
        }

        private void StopDrag(int pointerId)
        {
            DraggingPointerIds.Remove(pointerId);

            if (_type == JoystickType.Dynamic)
                Hide(true);

            _dragging = false;
            _draggingPointerId = -1;

            if (OnStopDrag != null)
                OnStopDrag(this);

            UpdateDragPosition(transform.position);
        }

        private void UpdateDragPosition(Vector3 position)
        {
            var distance = position - _rectTransform.position;
            if (distance.sqrMagnitude > _dragMaxSqrRadius)
            {
                position = _rectTransform.position + distance.normalized * _dragMaxRadius;
                distance = position - _rectTransform.position;
            }

            var radAngle = Mathf.Atan2(-distance.x, -distance.y);

            var angle = radAngle * Mathf.Rad2Deg + 180f;
            var value = distance.sqrMagnitude / _dragMaxSqrRadius;
            CustomData = distance;
            //Debug.Log (dist);

            if (_segmentCount > 0)
            {
                var minAngularDistance = float.MaxValue;
                var closestSegmentAngle = 0f;
                for (var i = 0; i < _segmentCount; i++)
                {
                    var angularDistance = Mathf.Abs(Mathf.DeltaAngle(angle, _segmentAngles[i]));
                    if (!(angularDistance < minAngularDistance)) continue;

                    minAngularDistance = angularDistance;
                    closestSegmentAngle = _segmentAngles[i];
                }

                angle = closestSegmentAngle;
                radAngle = angle * Mathf.Deg2Rad;
                position = _rectTransform.position + new Vector3(Mathf.Sin(radAngle), Mathf.Cos(radAngle), 0f) * distance.magnitude;
            }

            if (!Mathf.Approximately(angle, _lastAngle) || !Mathf.Approximately(value, _lastValue))
            {
                _lastAngle = angle;
                _lastValue = value;

                Angle = angle;
                Value = value;

                if (OnUpdate != null)
                    OnUpdate(this, angle, value);
            }

            if (_tweenDrag)
            {
                _tweening = true;
                _dragTargetPosition = position;
            }
            else
                _foregroundImageRectTransform.position = position;
        }

        private void OnDestroy()
        {
            if (_draggingPointerId != -1)
                DraggingPointerIds.Remove(_draggingPointerId);

            Destroyed = true;
        }
    }
}
