using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

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

    private void Awake()
    {
        if (_targetText == null)
        {
            Debug.LogError($"{nameof(ButtonHoverScaler)}: Target TextMeshProUGUI not assigned.", this);
            enabled = false;
            return;
        }

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
