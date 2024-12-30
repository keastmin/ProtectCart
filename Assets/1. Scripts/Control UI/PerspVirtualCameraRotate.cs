using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PerspVirtualCameraRotate : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private Transform _anchor;
    [SerializeField] private float _constraintAngle = 30f;
    [SerializeField] private float _sensX = 350f;
    [SerializeField] private float _sensY = 350f;
    [SerializeField] private float _lerpSpeed = 20f; 
    [SerializeField] private float _thresholdX = -10f;
    [SerializeField] private float _thresholdY = 5f;

    [Header("Fire Info")]
    [SerializeField] private float _maxChargeValue = 192f;
    private float _currentChargeValue = 0f;
    public FireInfo PlayerFireInfo => _playerFireInfo;
    private FireInfo _playerFireInfo = FireInfo.Empty;

    [Header("Player Follow UI")]
    [SerializeField] private Transform _playerAnchor;
    [SerializeField] private RectTransform _sliderContainer;
    [SerializeField] private Slider _slider;
    [SerializeField] private Camera _uiCamera;

    private Vector3 _originForward;
    private float _originX;
    private float _originY;
    private float _rotateX;
    private float _rotateY; 
    private Quaternion _tentativeRotation;

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _playerFireInfo = FireInfo.Empty;
    }

    private void Start()
    {
        _originForward = _anchor.forward;
        _rotateX = _anchor.eulerAngles.x;
        _rotateY = _anchor.eulerAngles.y;

        _originX = _rotateX;
        _originY = _rotateY;

        Debug.Log($"{_originForward}, {_rotateX}, {_rotateY}");
    }

    private void Update()
    {
        MouseInput();   
        HandleCameraRotation();
        SetFireInfo();

        UpdateSliderPosition();
    }

    private void MouseInput()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * _sensX * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * _sensY * Time.deltaTime;

        _rotateX -= mouseY;
        _rotateY += mouseX;
    }

    private void HandleCameraRotation()
    {
        if (_rotateX < _originX - _constraintAngle) _rotateX = Mathf.Lerp(_rotateX, _originX - _constraintAngle, Time.deltaTime * _lerpSpeed);
        else if (_rotateX > _originX + _constraintAngle) _rotateX = Mathf.Lerp(_rotateX, _originX + _constraintAngle, Time.deltaTime * _lerpSpeed);

        if (_rotateY < _originY - _constraintAngle) _rotateY = Mathf.Lerp(_rotateY, _originY - _constraintAngle, Time.deltaTime * _lerpSpeed);
        else if (_rotateY > _originY + _constraintAngle) _rotateY = Mathf.Lerp(_rotateY, _originY + _constraintAngle, Time.deltaTime * _lerpSpeed);

        _tentativeRotation = Quaternion.Euler(_rotateX, _rotateY, 0);

        _anchor.rotation = _tentativeRotation;
    }

    private void SetFireInfo()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _playerFireInfo = FireInfo.Empty;
            _playerFireInfo.IsAiming = true;
        }
        else if (Input.GetMouseButtonDown(1))
        {
            _currentChargeValue = 0f;
            _playerFireInfo = FireInfo.Empty;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _currentChargeValue = 0f;
            _playerFireInfo.IsShooting = true;
            _playerFireInfo.IsAiming = false;
        }

        if (PlayerFireInfo.IsAiming)
        {
            float speed = CalculateSpeed() / _maxChargeValue;
            _playerFireInfo.Direction = CalculateDirection();
            _playerFireInfo.Speed = speed;
            _playerFireInfo.IsAiming = true;
            _slider.value = speed;
        }
        else
        {
            _slider.value = Mathf.Lerp(_slider.value, 0, Time.deltaTime * 30f);
        }
    }

    private Vector3 CalculateDirection()
    {
        Quaternion aimRotation = Quaternion.Euler(_rotateX + _thresholdX, _rotateY + _thresholdY, 0);
        Vector3 tentativeForward = aimRotation * Vector3.forward;
        return tentativeForward;
    }

    private float CalculateSpeed()
    {
        _currentChargeValue = Mathf.MoveTowards(_currentChargeValue, _maxChargeValue, Time.deltaTime * 200f);
        return _currentChargeValue;
    }

    private void UpdateSliderPosition()
    {
        Vector3 worldPosition = _playerAnchor.position;
        Vector3 screenPosition = _uiCamera.WorldToScreenPoint(worldPosition);

        _sliderContainer.anchoredPosition = screenPosition;
    }
}
