using System;
using UnityEngine;

public class GravityChanger : MonoBehaviour
{
   public static GravityChanger Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        DimensionStateHolder.Instance.OnDimensionChanged 
            += DimensionStateHolder_OnDimensionChanged;
    }

    private void DimensionStateHolder_OnDimensionChanged(object sender, DimensionStateHolder.OnDimensionChangedEventArgs e)
    {
        ChangeGravity(e.newDimension);
    }

    private void OnDestroy()
    {
        DimensionStateHolder.Instance.OnDimensionChanged
            -= DimensionStateHolder_OnDimensionChanged;
    }

    private void ChangeGravity(Dimension newDimension)
    {
        bool invertGravity = newDimension == Dimension.Goofy;

        Physics2D.gravity = -Physics2D.gravity;
    }

}
