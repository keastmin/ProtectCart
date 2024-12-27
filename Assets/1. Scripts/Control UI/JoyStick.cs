using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [Header("Drag UI")]
    [SerializeField] private GameObject _dragAnchor;
    [SerializeField] private GameObject _dragPosition;

    [Header("Drag Value")]
    [SerializeField] private float _maxDragDistance = 192f;
    [SerializeField] private float _aimingStartDistance = 19.2f;

    // position data
    private RectTransform _dragAnchorTransform;
    private RectTransform _dragPositionTransform;
    private Vector2 _dragStartPos = Vector2.zero;
    private Vector2 _currentDragPos = Vector2.zero;

    public FireInfo PlayerFireInfo => _playerFireInfo;
    private FireInfo _playerFireInfo = FireInfo.Empty;

    private void Awake()
    {
        _dragAnchorTransform = _dragAnchor.GetComponent<RectTransform>();
        _dragPositionTransform = _dragPosition.GetComponent<RectTransform>();
    }

    private void OnDisable()
    {
        _dragAnchor.SetActive(false);
        _dragPosition.SetActive(false);
        _playerFireInfo = FireInfo.Empty;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            _playerFireInfo = FireInfo.Empty;

            _dragStartPos = eventData.position;
            _currentDragPos = eventData.position;

            // Set the drag anchor and drag position to the down position
            _dragAnchor.SetActive(true);
            _dragPosition.SetActive(true);
            _dragAnchorTransform.anchoredPosition = _dragStartPos;
            _dragPositionTransform.anchoredPosition = _dragStartPos;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            _playerFireInfo.IsShooting = true;
            _playerFireInfo.IsAiming = false;

            _dragAnchor.SetActive(false);
            _dragPosition.SetActive(false);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Vector2 currentCursorPos = eventData.position;
            Vector2 direction = Vector2.zero;
            float distance = Vector2.Distance(_dragStartPos, currentCursorPos);

            if(distance > _maxDragDistance)
            {
                _currentDragPos = _dragStartPos + (_maxDragDistance * (currentCursorPos - _dragStartPos).normalized);
                distance = _maxDragDistance;
            }
            else
            {
                _currentDragPos = currentCursorPos;
            }
            _dragPositionTransform.anchoredPosition = _currentDragPos;
            direction = (_dragStartPos - _currentDragPos).normalized;

            // set the fire info
            _playerFireInfo.IsAiming = (distance >= _aimingStartDistance) ? true : false;
            _playerFireInfo.Speed = distance / _maxDragDistance;
            _playerFireInfo.Direction = new Vector3(direction.x, direction.y, 0);        
        }
    }
}
