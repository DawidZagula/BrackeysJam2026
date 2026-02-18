using System;
using UnityEngine;
using Zenject;

public class Player : MonoBehaviour
{
    [SerializeField] private bool _isGravityChangeAvailable;
    public bool UsedGravityChangeFirstTime {  get; private set; }

    private InputReader _inputReader;
    private DimensionStateHolder _dimensionStateHolder;

    [Inject]
    public void Construct(InputReader inputReader, DimensionStateHolder dimensionStateHolder)
    {
        _inputReader = inputReader;
        _inputReader.OnDimensionChangePressed += OnDimensionChangePressed; ;
        _dimensionStateHolder = dimensionStateHolder;
    }

    private void OnDimensionChangePressed(object sender, EventArgs e)
    {
        if (_isGravityChangeAvailable)
        {
            if (!UsedGravityChangeFirstTime)
            {
                UsedGravityChangeFirstTime = true;
            }

            _dimensionStateHolder.ChangeDimension();
        }
    }

    public void ToggleGravityChangeAvailable(bool newState)
    {
        _isGravityChangeAvailable = newState;
    }
            
}


