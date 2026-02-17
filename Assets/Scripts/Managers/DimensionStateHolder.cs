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
    

    //private void OnDimensionChangePressed(object sender, System.EventArgs e)
    //{
    //    ChangeDimension();
    //}
    
    public void ChangeDimension()
    {
        CurrentDimension = 
            CurrentDimension == Dimension.Lava 
            ? Dimension.Goofy : Dimension.Lava;
    }

}
