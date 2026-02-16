using System;
using UnityEngine;

public class DistanceFromEnemyIndicator : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private GroundMover _groundMover;

    [Header("Configuration")]
    [SerializeField] private float _MaxDistance;

    //Run-time State
    private float _distance;
    private float _groundHalfHeight;

    private void Start()
    {
        _groundHalfHeight = _groundMover.GetDeathAreaHeight() / 2;
    }

    private void Update()
    {
        UpdateDistance();
    }

    private void UpdateDistance()
    {
        const float minDistance = 0f;

        float groundColliderTop = _groundMover.transform.position.y + _groundHalfHeight;

        float minDistancePoint = Camera.main.transform.position.y; 

        _distance = minDistancePoint - groundColliderTop;

        _distance = Mathf.Clamp(_distance, minDistance, _MaxDistance);

    }

    public float GetNormalizedDistance() => _distance / _MaxDistance;
    
}
