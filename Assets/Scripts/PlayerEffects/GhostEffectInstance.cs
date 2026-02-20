using UnityEngine;

public class GhostEffectInstance : MonoBehaviour
{
    // called from animator
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
