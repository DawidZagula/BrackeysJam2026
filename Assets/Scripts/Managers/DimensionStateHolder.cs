using System;
using UnityEngine;

public class DimensionStateHolder : MonoBehaviour
{
    public static DimensionStateHolder Instance { get; private set; }
    
    public Dimension CurrentDimension { get; private set; }

    public event EventHandler<OnDimensionChangedEventArgs> OnDimensionChanged;
    public class OnDimensionChangedEventArgs : EventArgs
    {
        public Dimension newDimension { get; }

        public OnDimensionChangedEventArgs(Dimension newDimension)
        {
            this.newDimension = newDimension;
        }
    }

    private void Awake()
    {
        Instance = this;
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
        CurrentDimension = 
            CurrentDimension == Dimension.Lava 
            ? Dimension.Goofy : Dimension.Lava;

        OnDimensionChanged?.Invoke(this, new OnDimensionChangedEventArgs(CurrentDimension));
    }

    private void OnDestroy()
    {
        InputReader.Instance.OnDimensionChangePressed 
            -= InputReader_OnDimensionChangePressed;
    }
}
