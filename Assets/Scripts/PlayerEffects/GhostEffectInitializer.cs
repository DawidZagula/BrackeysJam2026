using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class GhostEffectInitializer : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private float _ghostDelay;

    [Header("References")]
    [SerializeField] private GameObject _ghostPrefab;
    private Rigidbody2D _rigidbody;

    //runtime state
    private float _ghostDelaySeconds;
    private bool _instantiateEffect;

    private DimensionStateHolder _dimensionStateHolder;

    [Inject]
    public void Construct(DimensionStateHolder dimensionStateHolder)
    {
        _dimensionStateHolder = dimensionStateHolder;
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
 

    private void Start()
    {
        _ghostDelaySeconds = _ghostDelay;
    }
   

    private void Update()
    {
        ToggleInstantiateEffect();

        if (!_instantiateEffect) { return; }

        if (_ghostDelaySeconds > 0)
        {
            _ghostDelaySeconds -= Time.deltaTime;

        }
        else
        {
            GameObject  ghostInstance 
                = Instantiate(_ghostPrefab, transform.position, transform.rotation);

            _ghostDelaySeconds = _ghostDelay;
        }
    }

    private void ToggleInstantiateEffect()
    {
        if (_rigidbody.linearVelocity != Vector2.zero 
            && _dimensionStateHolder.CurrentDimension == Dimension.Goofy)
        {
            if (!_instantiateEffect)
            {
                _instantiateEffect = true;
            }
            return;
        }
        if (_instantiateEffect)
        {
            _instantiateEffect = false;
        }
    }
}
