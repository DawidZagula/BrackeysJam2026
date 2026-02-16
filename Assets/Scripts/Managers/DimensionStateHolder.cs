using System;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

public class DimensionStateHolder
{
    private Dimension _dimension;
    
    public Dimension CurrentDimension
    {
        get => _dimension;
        private set
        {
            if (_dimension == value) return;
            _dimension = value;
            OnDimensionChanged?.Invoke(_dimension);
        } 
    }
  
    public event Action<Dimension> OnDimensionChanged;
    
    private InputReader _inputReader;

    [Inject]
    public void Construct(InputReader inputReader)
    {
        _inputReader = inputReader;
        _inputReader.OnDimensionChangePressed 
            += OnDimensionChangePressed;
    }

    private void OnDimensionChangePressed(object sender, System.EventArgs e)
    {
        ChangeDimension();
    }

    private void ChangeDimension()
    {
        CurrentDimension = 
            CurrentDimension == Dimension.Lava 
            ? Dimension.Goofy : Dimension.Lava;

        OnDimensionChanged?.Invoke(CurrentDimension);
    }

    private void OnDestroy()
    {
        _inputReader.OnDimensionChangePressed 
            -= OnDimensionChangePressed;
    }
}
