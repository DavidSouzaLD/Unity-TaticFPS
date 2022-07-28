using UnityEngine;

[CreateAssetMenu(fileName = "PlayerControllerPreset", menuName = "PPM22/Create ControllerPreset")]
public class PlayerControllerPreset : ScriptableObject
{
    [Header("Movement")]
    public LayerMask walkableMask;
    public float walkingSpeed = 10f;
    public float runningSpeed = 15f;
    public float maxAngleSlope = 65f;
    [Space]
    public float maxWalkingSpeed = 3f;
    public float maxRunningSpeed = 4.5f;
    public float maxCrouchingSpeed = 1.5f;
    [Space]
    public float jumpingForce = 50f;
    public float gravityScale = 2f;

    [Header("Friction")]
    public float initialFriction = 5f;
    public float movingFriction = 0.5f;

    [Header("Crouch")]
    public float crouchHeight = 1.5f;
    public float crouchSpeed = 5f;

    [Header("Cover")]
    public float coverAmount = 20f;
    public float coverCamScale = 0.02f;
    public float coverSpeed = 6f;

    [Header("Ground Area")]
    public float groundAreaHeight = -0.35f;
    public float groundAreaRadius = 0.3f;
}