using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ButtonHoverScaler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Cached References")]
    [SerializeField] private TextMeshProUGUI _targetText;

    [Header("Configuration")]
    [SerializeField] private float _hoverFontSizeMultiplier = 1.15f;
    [SerializeField] private float _transitionDuration = 0.15f;

    // Runtime
    private float _baseFontSize;
    private float _currentFontSize;
    private float _targetFontSize;
    private float _velocity;

    private bool _isHoveredOrSelected;

    private void OnEnable()
    {
        ResetState();

        if (IsPointerOverThis())
        {
            _isHoveredOrSelected = true;
            RefreshTargetSize();
            _currentFontSize = _targetFontSize;
            _targetText.fontSize = _currentFontSize;
        }
    }

    private bool IsPointerOverThis()
    {
        if (EventSystem.current == null)
            return false;

        if (Mouse.current == null)
            return false;

        Vector2 pointerPosition = Mouse.current.position.ReadValue();

        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = pointerPosition
        };

        List<RaycastResult> results = new();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject == gameObject ||
                result.gameObject.transform.IsChildOf(transform))
            {
                return true;
            }
        }

        return false;
    }

    private void Awake()
    {
        _baseFontSize = _targetText.fontSize;
        _currentFontSize = _baseFontSize;
        _targetFontSize = _baseFontSize;
    }

    private void Update()
    {
        if (Mathf.Approximately(_currentFontSize, _targetFontSize))
        {
            return;
        }

        _currentFontSize = Mathf.SmoothDamp(
            _currentFontSize,
            _targetFontSize,
            ref _velocity,
            _transitionDuration,
            Mathf.Infinity,
            Time.unscaledDeltaTime
        );

        _targetText.fontSize = _currentFontSize;
    }

    private void RefreshTargetSize()
    {
        _targetFontSize = _isHoveredOrSelected
            ? _baseFontSize * _hoverFontSizeMultiplier
            : _baseFontSize;
    }

    private void OnDisable()
    {
        ResetState();
    }

    private void ResetState()
    {
        _isHoveredOrSelected = false;
        _velocity = 0f;
        _currentFontSize = _baseFontSize;
        _targetFontSize = _baseFontSize;

        if (_targetText != null)
        {
            _targetText.fontSize = _baseFontSize;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isHoveredOrSelected = true;
        RefreshTargetSize();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isHoveredOrSelected = false;
        RefreshTargetSize();
    }
}
