using System;
using UnityEngine;

public class PickupHolder : MonoBehaviour
{
    public static PickupHolder Instance { get; private set; }

    [Header("Configuration")]
    [SerializeField] private int _maxPickupCount;
    [SerializeField] private int _startPickupCount;

    //Run-time state
    public int CurrentPickupCount { get; private set; }

    public event EventHandler OnUsedPickup;

    private void Awake()
    {
        Instance = this;

        CurrentPickupCount = _startPickupCount;
    }

    private void Start()
    {
        InputReader.Instance.OnTryUsePickup += InputReader_OnTryUsePickup;
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

    private void OnDestroy()
    {
        InputReader.Instance.OnTryUsePickup -= InputReader_OnTryUsePickup;
    }
}
