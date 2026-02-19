using UnityEngine;

public class TweenPosition : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private bool _isVerticalTween;
    [SerializeField] private float _targetVerticalPosition;
    [SerializeField] private float _verticalTweenTravelTime;
    [SerializeField] private LeanTweenType _verticalEaseType;
    [Space]
    [SerializeField] private bool _isHorizontalTween;
    [SerializeField] private float _targetHorizontalPosition;
    [SerializeField] private float _horizontalTweenTravelTime;
    [SerializeField] private LeanTweenType _horizontalEaseType;

    private void Start()
    {
       if (_isVerticalTween)
        {
            LeanTween.moveLocalY(gameObject, _targetVerticalPosition, _verticalTweenTravelTime).
                setLoopPingPong().
                setEase(_verticalEaseType);
        }

       if (_isHorizontalTween)
        {
            LeanTween.moveLocalX(gameObject, _targetHorizontalPosition, _horizontalTweenTravelTime).
                setLoopPingPong().
                setEase(_horizontalEaseType);
        }
    }
}
