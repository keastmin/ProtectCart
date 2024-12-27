using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PerspVirtualCameraRotate : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private Transform _anchor; // 회전 기준점
    [SerializeField] private float _constraintAngle = 30f; // 최대 허용 각도
    [SerializeField] private float _sensX = 350f; // 마우스 감도 X
    [SerializeField] private float _sensY = 350f; // 마우스 감도 Y
    [SerializeField] private float _lerpSpeed = 20f; // 제한 각도 복귀 속도
    [SerializeField] private float _thresholdX = -10f; // 발사 각도 조절 X
    [SerializeField] private float _thresholdY = 5f; // 발사 각도 조절 Y

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

    private Vector3 _originForward; // 초기 카메라 방향
    private float _originX; // 초기 회전 값 X
    private float _originY; // 초기 회전 값 Y
    private float _rotateX; // 회전 값 X
    private float _rotateY; // 회전 값 Y
    private Quaternion _tentativeRotation; // 최종적으로 적용될 회전값

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
        // 초기 방향을 월드 좌표계에서 카메라의 정면 방향으로 설정
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
        // 마우스 회전 입력
        float mouseX = Input.GetAxisRaw("Mouse X") * _sensX * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * _sensY * Time.deltaTime;

        // 기존 회전값에 입력값 더하기
        _rotateX -= mouseY;
        _rotateY += mouseX;
    }

    private void HandleCameraRotation()
    {
        // 회전값 제한
        if (_rotateX < _originX - _constraintAngle) _rotateX = Mathf.Lerp(_rotateX, _originX - _constraintAngle, Time.deltaTime * _lerpSpeed);
        else if (_rotateX > _originX + _constraintAngle) _rotateX = Mathf.Lerp(_rotateX, _originX + _constraintAngle, Time.deltaTime * _lerpSpeed);

        if (_rotateY < _originY - _constraintAngle) _rotateY = Mathf.Lerp(_rotateY, _originY - _constraintAngle, Time.deltaTime * _lerpSpeed);
        else if (_rotateY > _originY + _constraintAngle) _rotateY = Mathf.Lerp(_rotateY, _originY + _constraintAngle, Time.deltaTime * _lerpSpeed);

        // 잠정적으로 회전을 적용해볼 값 계산
        _tentativeRotation = Quaternion.Euler(_rotateX, _rotateY, 0);

        // 최종 회전값 적용
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
