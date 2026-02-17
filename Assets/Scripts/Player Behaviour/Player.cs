using System;
using UnityEngine;
using Zenject;

public class Player : MonoBehaviour
{
    [SerializeField] private bool _isGravityChangeAvailable;

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
            _dimensionStateHolder.ChangeDimension();
        }
    }

    private AbilityHolder _abilityHolder;

    public void ToggleGravityChangeAvailable(bool newState)
    {
        _isGravityChangeAvailable = newState;
    }
            
}


