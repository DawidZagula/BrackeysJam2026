using UnityEngine;

public class TweenScale : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private float _scalingTime;
    [SerializeField] private Vector2 _targetScale;
    [SerializeField] private LeanTweenType _easeType;

    private void Start()
    {
        LeanTween.scale(gameObject, _targetScale, _scalingTime)
            .setLoopPingPong()
            .setEase(_easeType);
    }
}
