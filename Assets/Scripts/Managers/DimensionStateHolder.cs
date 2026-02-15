using System;
using UnityEngine;

public class DimensionStateHolder : MonoBehaviour
{
    [Header("For debugging only")]
    [SerializeField] private Dimension _currentDimension;

    public event EventHandler<OnDimensionChangedEventArgs> OnDimensionChanged;
    public class OnDimensionChangedEventArgs : EventArgs
    {
        public Dimension newDimension { get; }

        public OnDimensionChangedEventArgs(Dimension newDimension)
        {
            this.newDimension = newDimension;
        }
    }

    private void Start()
    {
        InputReader.Instance.OnDimensionChangePressed 
            += InputReader_OnDimensionChangePressed;
    }

    private void InputReader_OnDimensionChangePressed(object sender, System.EventArgs e)
    {
        ChangeDimension();
    }

    private void ChangeDimension()
    {
        _currentDimension = 
            _currentDimension == Dimension.Lava 
            ? Dimension.Goofy : Dimension.Lava;

        OnDimensionChanged?.Invoke(this, new OnDimensionChangedEventArgs(_currentDimension));
    }

    private void OnDestroy()
    {
        InputReader.Instance.OnDimensionChangePressed 
            -= InputReader_OnDimensionChangePressed;
    }
}
