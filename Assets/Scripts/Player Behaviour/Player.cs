using System;
using UnityEngine;
using Zenject;

public class Player : MonoBehaviour
{
    
    private InputReader _inputReader;

    [SerializeField] private bool _isGravityAvailable = false;

    public event Action OnDimensionChange;

    [Inject]
    public void Construct(InputReader inputReader)
    {
        _inputReader = inputReader;
        _inputReader.OnDimensionChangePressed += OnDimensionChangePressed;
    }

    private void OnDimensionChangePressed(object sender, System.EventArgs e)
    {
        if (_isGravityAvailable)
        {
            OnDimensionChange?.Invoke();
        }
    }

    public void ToggleGravityChangeAvailable(bool newState)
    {
        _isGravityAvailable = newState;
    }
            
}
