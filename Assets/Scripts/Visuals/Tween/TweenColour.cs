using UnityEngine;

public class TweenColour : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private Color _targetColour;
    [SerializeField] private float _changeTime;
    [SerializeField] private LeanTweenType _easeType;

    private void Start()
    {
        LeanTween.color(gameObject, _targetColour, _changeTime)
            .setLoopPingPong()
            .setEase(_easeType);
    }
}
