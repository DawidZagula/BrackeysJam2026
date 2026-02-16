using System;
using UnityEngine;
using Zenject;

public class PickupHolder : MonoBehaviour
{
    public static PickupHolder Instance { get; private set; }

    [Header("Configuration")]
    [SerializeField] private int _maxPickupCount;
    [SerializeField] private int _startPickupCount;

    //Run-time state
    public int CurrentPickupCount { get; private set; }

    public event EventHandler OnUsedPickup;
    public event EventHandler OnAddedPickup;

    private InputReader _inputReader;

    [Inject]
    public void Construct(InputReader inputReader)
    {
        _inputReader = inputReader;
    }
    
    private void Awake()
    {
        Instance = this;

        CurrentPickupCount = _startPickupCount;
    }

    private void Start()
    {
        _inputReader.OnTryUsePickup += InputReader_OnTryUsePickup;
    }

    private void InputReader_OnTryUsePickup(object sender, System.EventArgs e)
    {
        if (CurrentPickupCount > 0)
        {
            CurrentPickupCount--;

            OnUsedPickup?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            //implement visual/ audio response
        }
    }

    public void AddPickup()
    {
        if (CurrentPickupCount >= _maxPickupCount)
            return;

        CurrentPickupCount++;

        OnAddedPickup?.Invoke(this, EventArgs.Empty);
    }
    private void OnDestroy()
    {
        _inputReader.OnTryUsePickup -= InputReader_OnTryUsePickup;
    }
}
