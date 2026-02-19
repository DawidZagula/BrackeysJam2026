using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuTweenOperator : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private RectTransform _backgroundImageRect;
    [SerializeField] private RectTransform _mainMenuRect;
    [Space]
    [SerializeField] private float _tweenDuration = 0.6f;
    [SerializeField] private float _hiddenYBackgroundOffset = 1200f;
    [Space]
    [SerializeField] private bool _shouldAnimate;
    [Space]
    [SerializeField] private LeanTweenType _easeTypeBackground;
    [SerializeField] private LeanTweenType _easeTypeMenu;

    //State
    private Vector2 _backgroundVisiblePosition;
    private Vector2 _menuVisiblePosition;

    private Vector2 _backgroundHiddenPosition;
    private Vector2 _menuHiddenPosition;

    private void Start()
    {
        if (_shouldAnimate)
        {
            _backgroundVisiblePosition = _backgroundImageRect.anchoredPosition;
            _menuVisiblePosition = _mainMenuRect.anchoredPosition;

            _backgroundHiddenPosition =
                 _backgroundVisiblePosition + Vector2.up * _hiddenYBackgroundOffset;
            _menuHiddenPosition =
                _menuVisiblePosition + Vector2.up * _hiddenYBackgroundOffset;

            HideInstant();
        }
    }

    private void HideInstant()
    {
        _backgroundImageRect.anchoredPosition =
            _backgroundVisiblePosition + Vector2.up
            * _hiddenYBackgroundOffset;

        _mainMenuRect.anchoredPosition =
            _menuVisiblePosition + Vector2.up
            * _hiddenYBackgroundOffset;
    }

    public void TweenShow()
    {
        LeanTween.moveY
            (_backgroundImageRect, _backgroundVisiblePosition.y, _tweenDuration)
            .setEase(_easeTypeBackground)
            .setIgnoreTimeScale(true);

        LeanTween.moveY
            (_mainMenuRect, _menuVisiblePosition.y, _tweenDuration)
            .setEase(_easeTypeMenu)
            .setIgnoreTimeScale(true);
    }

    public void TweenHide(Action onFinish = null)
    {
        LeanTween.moveY
            (_backgroundImageRect, _backgroundHiddenPosition.y, _tweenDuration)
            .setEase(_easeTypeBackground)
            .setIgnoreTimeScale(true);

        LeanTween.moveY
            (_mainMenuRect, _menuHiddenPosition.y, _tweenDuration)
            .setEase(_easeTypeMenu)
            .setIgnoreTimeScale(true)
            .setOnComplete(() => onFinish?.Invoke());
    }
}
