using System;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

public class DimensionStateHolder
{
    private Dimension _dimension;
    private float _SFXVolume = 0.05f;
    
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
    
    public void ChangeDimension()
    {
        CurrentDimension = 
            CurrentDimension == Dimension.Lava 
            ? Dimension.Goofy : Dimension.Lava;

        AudioPlayer.AudioName soundToPlay =
            CurrentDimension == Dimension.Lava ?
            AudioPlayer.AudioName.ChangeDimension2 :
            AudioPlayer.AudioName.ChangeDimension;

        AudioPlayer.Instance.PlaySound(soundToPlay, _SFXVolume);
    }

}
