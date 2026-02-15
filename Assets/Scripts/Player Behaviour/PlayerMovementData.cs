using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/ Player Movement Data")]
public class PlayerMovementData : ScriptableObject
{
    [Header("Gravity")]
    [HideInInspector] public float gravityScale = 1.7333333f;                                                              

    [Header("Move")]
    public float moveSpeed;
    [Space(20)]

    [Header("Jump")]
    public float jumpHeight;
    public float jumpTimeToApex;
    public float jumpCutGravityMultiplier; 
    [HideInInspector] public float jumpForce = 26;
    [Space(20)]


    [Header("Fall")]
    public float fallGravityMultiplier = 2;
    public float maxFallSpeed = 18;

    [Header("Player Helpers")]
    [Range(0.01f, 0.5f)] public float coyoteTime;
    [Range(0.01f, 0.5f)] public float jumpInputBufferTime;

}
