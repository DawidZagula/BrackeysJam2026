using UnityEngine;
using UnityEngine.UI;

public class DistanceFromEnemyIndicatorUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image _indicatorBar;
    [SerializeField] private Image _enemyIcon;
    [SerializeField] private DistanceFromEnemyIndicator _distanceFromEnemyIndicator;

    private float _bottomIconYPosition;
    private float _topIconYPosition;

    private void Start()
    {
        _bottomIconYPosition = _enemyIcon.transform.localPosition.y;
        _topIconYPosition = -_bottomIconYPosition;
    }

    private void Update()
    {
        float fillAmount = 1f - _distanceFromEnemyIndicator.GetNormalizedDistance();

        UpdateBar(fillAmount);
        UpdateIconPosition(fillAmount);
    }

    private void UpdateBar(float fillAmount)
    {
        _indicatorBar.fillAmount = fillAmount;
    }

    private void UpdateIconPosition(float fillAmount)
    {
        Vector3 position = _enemyIcon.transform.localPosition;
        position.y = Mathf.Lerp(_bottomIconYPosition, _topIconYPosition, fillAmount);
        _enemyIcon.transform.localPosition = position;
    }
}
