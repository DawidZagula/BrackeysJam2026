using System;

public class LavaFreezer
{
    public event Action<float> OnLavaFrozen;

    public void FreezeLava(float freezeDuration)
    {
        OnLavaFrozen?.Invoke(freezeDuration);
    }
}
