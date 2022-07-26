using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class Player : MonoBehaviour
{
    [Header("Movement")]

    /// <summary>
    /// Acceleration walking.
    /// </summary>
    [SerializeField] private float walkAcceleration = 10f;

    /// <summary>
    /// Acceleration running.
    /// </summary>
    [SerializeField] private float runAcceleration = 15f;

    /// <summary>
    /// Maximum angle for climbing steps and curved surfaces.
    /// </summary>
    [SerializeField] private float maxAngleToSlope = 65f;

    [Header("Limit")]

    /// <summary>
    /// Movement speed limit.
    /// </summary>
    [SerializeField] private float limitWalkVelocity = 3f;

    /// <summary>
    /// Running speed limit.
    /// </summary>
    [SerializeField] private float limitRunVelocity = 4.5f;

    /// <summary>
    /// Squat speed limit.
    /// </summary>
    [SerializeField] private float limitCrouchVelocity = 1.5f;

    [Header("Jump")]

    /// <summary>
    /// Force applied to the jump.
    /// </summary>
    [SerializeField] private float jumpForce = 25f;

    [Header("Crouch")]

    /// <summary>
    /// Height of crouch.
    /// </summary>
    [SerializeField] private float crouchHeight = 1.5f;

    /// <summary>
    /// Speed for apply crouch movement.
    /// </summary>
    [SerializeField] private float crouchSpeed = 5f;

    [Header("Friction")]

    /// <summary>
    /// Friction that is applied when standing still.
    /// </summary>
    [SerializeField] private float idleFriction = 5f;

    /// <summary>
    /// Friction that is applied when moving.
    /// </summary>
    [SerializeField] private float moveFriction = 0.5f;

    [Header("Ground")]

    /// <summary>
    /// Layer mask for objects that are walkable.
    /// </summary>
    [SerializeField] private LayerMask groundableMask;

    /// <summary>
    /// Offset of ground height check.
    /// </summary>
    [SerializeField] private float groundHeightOffset = -0.35f;

    /// <summary>
    /// Ground radius override a capsule collider size.
    /// </summary>
    [SerializeField] private float groundRadiusOverride = 0f;

    [Header("Turn")]

    /// <summary>
    /// Horizontal movement amount.
    /// </summary>
    [SerializeField] private float turnHorizontalAmount = 15f;

    /// <summary>
    /// Turn scale on camera.
    /// </summary>
    [SerializeField] private float turnCameraScale = 0.02f;

    /// <summary>
    /// Turn change speed.
    /// </summary>
    [SerializeField] private float turnSpeed = 8f;

    [Header("Animators")]

    /// <summary>
    /// Animator of basics animations.
    /// </summary>
    [SerializeField] private Animator basicAnimator;

    [Space]

    /// <summary>
    /// Disable all gizmos of player.
    /// </summary>
    [SerializeField] private bool disableGizmos;

    /// <summary>
    /// Player states.
    /// </summary>
    [HideInInspector]
    public static bool isWalking, isCrouched, isJumping, isGrounded, isTurning, isAim;
    public static bool isRunning => isWalking && !StateLock.IsLocked("PLAYER_RUN") && Input.Run && Input.MoveAxis.y > 0;

    // Private
    private Transform weaponTurn;
    private Transform cameraTurn;
    private float jumpFixTimer = 0f;
    private float startCapsuleHeight;
    private float startCameraHeight;
    private Vector3 startCameraTurnPos;
    private Vector3 additionalVelocity;
    private Quaternion startTurnRot;
    private Quaternion startTurnCameraRot;
    private Transform CameraTransform;
    private Rigidbody Rigidbody;
    private CapsuleCollider CapsuleCollider;

    public float CurrentSpeed { get { return isRunning ? runAcceleration : walkAcceleration; } }
    public float LimitCurrentSpeed { get { return (!Input.Crouch ? (!isRunning ? limitWalkVelocity : limitRunVelocity) : limitCrouchVelocity) + additionalVelocity.magnitude; } }
    public float SlopeAngle { get { return Vector3.Angle(transform.up, Normal); } }
    public bool Grounded { get { return GroundColliders.Length > 0; } }
    public bool Sloped { get { return SlopeAngle > 0 && SlopeAngle <= maxAngleToSlope; } }
    public float LocalYRotation { get { return transform.localEulerAngles.y; } }
    public Vector2 ForwardXZ { get { return new Vector2(transform.forward.x, transform.forward.z); } }
    public Vector3 CapsuleTop { get { return (transform.position - CapsuleCollider.center) + (transform.up * ((CapsuleCollider.height / 2f) + groundHeightOffset)); } }
    public Vector3 CapsuleBottom { get { return (transform.position + CapsuleCollider.center) - (transform.up * ((CapsuleCollider.height / 2f) + groundHeightOffset)); } }

    public Vector3 Normal
    {
        get
        {
            RaycastHit hit;
            if (Physics.Raycast(CapsuleBottom, -transform.up, out hit, (CapsuleCollider.height / 2f) * 1.5f, groundableMask))
            {
                if (!disableGizmos)
                {
                    Debug.DrawRay(hit.point, hit.normal, Color.yellow);
                }

                return hit.normal;
            }
            return Vector3.one;
        }
    }

    public Collider[] GroundColliders
    {
        get
        {
            float radius = CapsuleCollider.radius + groundRadiusOverride;
            Collider[] hits = Physics.OverlapSphere(CapsuleBottom, radius, groundableMask);
            return hits;
        }
    }

    private void Start()
    {
        // Get necessary components
        CameraTransform = GetComponentInChildren<PlayerCamera>().transform;
        Rigidbody = GetComponent<Rigidbody>();
        CapsuleCollider = GetComponent<CapsuleCollider>();

        // Get transforms
        weaponTurn = GameObject.Find("WeaponTurn").transform;
        cameraTurn = GameObject.Find("CameraTurn").transform;

        // Start locking cursor
        PlayerCamera.LockCursor(true);

        // Starting values
        startCameraHeight = CameraTransform.transform.localPosition.y;
        startCapsuleHeight = CapsuleCollider.height;

        // Turn
        startTurnRot = weaponTurn.localRotation;
        startCameraTurnPos = cameraTurn.localPosition;
        startTurnCameraRot = cameraTurn.localRotation;

        if (weaponTurn == null || cameraTurn == null)
        {
            Debug.LogError("(WeaponTurn/CameraTurn) not assigned, solve please.");
        }
    }

    private void FixedUpdate()
    {
        UpdateMove();
    }

    private void Update()
    {
        CrouchUpdate();
        JumpUpdate();
        TurnUpdate();
        AnimatorUpdate();
    }

    public void UpdateMove()
    {
        if (!StateLock.IsLocked("PLAYER_MOVEMENT"))
        {
            // Movement
            Vector2 moveAxis = Input.MoveAxis;
            Vector3 dir1 = transform.forward * moveAxis.y + transform.right * moveAxis.x;
            Vector3 dir2 = Vector3.Cross(transform.right, Normal) * moveAxis.y + Vector3.Cross(-transform.forward, Normal) * moveAxis.x;
            Vector3 direction = !Sloped ? dir1 : dir2;

            // Direction Ray
            if (!disableGizmos)
            {
                Debug.DrawRay(transform.position, direction * 2f, Color.magenta);
            }

            isGrounded = Grounded;

            if (moveAxis != Vector2.zero)
            {
                isWalking = true;

                if (Rigidbody.drag != moveFriction)
                {
                    Rigidbody.drag = moveFriction;
                }

                if (Grounded)
                {
                    Rigidbody.AddForce(direction.normalized * CurrentSpeed * 10f, ForceMode.Force);

                    if (!StateLock.IsLocked("PLAYER_BASIC_ANIM"))
                    {
                        if (CurrentSpeed == walkAcceleration)
                        {
                            StateLock.Lock("PLAYER_TURN", this, false);
                            basicAnimator.SetBool("WALK", true);
                            basicAnimator.SetBool("RUN", false);
                        }
                        else if (CurrentSpeed == runAcceleration)
                        {
                            StateLock.Lock("PLAYER_TURN", this, true);
                            basicAnimator.SetBool("WALK", false);
                            basicAnimator.SetBool("RUN", true);
                        }
                    }
                }
            }
            else
            {
                isWalking = false;

                // Reset animations
                basicAnimator.SetBool("WALK", false);
                basicAnimator.SetBool("RUN", false);

                if (Grounded)
                {
                    Rigidbody.drag = idleFriction;
                }
                else
                {
                    Rigidbody.drag = moveFriction;
                }
            }
        }

        if (!StateLock.IsLocked("PLAYER_BASIC_ANIM"))
        {
            basicAnimator.SetBool("AIR", !isGrounded);
        }

        // Limit Velocity
        Vector3 flatVel = new Vector3(Rigidbody.velocity.x, 0f, Rigidbody.velocity.z);

        if (flatVel.magnitude > LimitCurrentSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * LimitCurrentSpeed;
            Rigidbody.velocity = new Vector3(limitedVel.x, Rigidbody.velocity.y, limitedVel.z);
        }
    }

    private void CrouchUpdate()
    {
        bool _canCrouch = !StateLock.IsLocked("PLAYER_CROUCH") && Input.Crouch;

        if (_canCrouch)
        {
            isCrouched = true;
            StateLock.Lock("PLAYER_RUN", this, true);

            CapsuleCollider.height = Mathf.Lerp(CapsuleCollider.height, crouchHeight, crouchSpeed * Time.deltaTime);
            CameraTransform.transform.position = Vector3.Lerp(CameraTransform.transform.position,
            CapsuleTop, crouchSpeed * Time.deltaTime);
        }
        else
        {
            isCrouched = false;
            StateLock.Lock("PLAYER_RUN", this, false);

            CapsuleCollider.height = Mathf.Lerp(CapsuleCollider.height, startCapsuleHeight, crouchSpeed * Time.deltaTime);
            CameraTransform.transform.localPosition = Vector3.Lerp(CameraTransform.transform.localPosition,
            new Vector3(
                CameraTransform.transform.localPosition.x,
                startCameraHeight,
                CameraTransform.transform.localPosition.z
            ), crouchSpeed * Time.deltaTime);
        }
    }

    private void JumpUpdate()
    {
        bool jump = !StateLock.IsLocked("PLAYER_JUMP") && Input.Jump && Grounded && jumpFixTimer <= 0;

        if (jump)
        {
            isJumping = true;
            Rigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            jumpFixTimer = 0.15f;
        }

        if (jumpFixTimer > 0)
        {
            isJumping = false;
            jumpFixTimer -= Time.deltaTime;
        }
    }

    private void TurnUpdate()
    {
        bool _canTurn = !StateLock.IsLocked("PLAYER_TURN");

        if (_canTurn)
        {
            // Right
            if (Input.Turn != 0 && !isRunning)
            {
                isTurning = true;
                StateLock.Lock("PLAYER_BASIC_ANIM", this, true);
                StateLock.Lock("PLAYER_RUN", this, true);

                Vector3 targetPos = new Vector3(turnHorizontalAmount * turnCameraScale * Input.Turn, 0f, 0f);
                Quaternion targetRot = Quaternion.Euler(new Vector3(0f, 0f, turnHorizontalAmount * -Input.Turn));

                if (!isAim)
                { // Weapon
                    weaponTurn.localRotation = Quaternion.Slerp(weaponTurn.localRotation, targetRot, turnSpeed * Time.deltaTime);
                    cameraTurn.localRotation = Quaternion.Slerp(cameraTurn.localRotation, startTurnCameraRot, turnSpeed * Time.deltaTime);
                    cameraTurn.localPosition = Vector3.Lerp(cameraTurn.localPosition, startCameraTurnPos, turnSpeed * Time.deltaTime);
                }
                else
                { // CameraTransform
                    cameraTurn.localPosition = Vector3.Lerp(cameraTurn.localPosition, targetPos, turnSpeed * Time.deltaTime);
                    cameraTurn.localRotation = Quaternion.Slerp(cameraTurn.localRotation, targetRot, turnSpeed * Time.deltaTime);
                    weaponTurn.localRotation = Quaternion.Slerp(weaponTurn.localRotation, startTurnRot, turnSpeed * Time.deltaTime);
                }
            }
            else if (weaponTurn.localRotation != startTurnRot || cameraTurn.localRotation != startTurnCameraRot || cameraTurn.localPosition != startCameraTurnPos)
            {
                isTurning = false;
                StateLock.Lock("PLAYER_BASIC_ANIM", this, false);
                StateLock.Lock("PLAYER_RUN", this, false);

                weaponTurn.localRotation = Quaternion.Slerp(weaponTurn.localRotation, startTurnRot, turnSpeed * Time.deltaTime);
                cameraTurn.localRotation = Quaternion.Slerp(cameraTurn.localRotation, startTurnCameraRot, turnSpeed * Time.deltaTime);
                cameraTurn.localPosition = Vector3.Lerp(cameraTurn.localPosition, startCameraTurnPos, turnSpeed * Time.deltaTime);
            }
        }

        if (!_canTurn)
        {
            isTurning = false;
            StateLock.Lock("PLAYER_BASIC_ANIM", this, false);
            StateLock.Lock("PLAYER_RUN", this, false);

            weaponTurn.localRotation = Quaternion.Slerp(weaponTurn.localRotation, startTurnRot, turnSpeed * Time.deltaTime);
            cameraTurn.localRotation = Quaternion.Slerp(cameraTurn.localRotation, startTurnCameraRot, turnSpeed * Time.deltaTime);
            cameraTurn.localPosition = Vector3.Lerp(cameraTurn.localPosition, startCameraTurnPos, turnSpeed * Time.deltaTime);
        }
    }

    private void AnimatorUpdate()
    {
        if (StateLock.IsLocked("PLAYER_MOVEMENT") || StateLock.IsLocked("PLAYER_BASIC_ANIM"))
        {
            basicAnimator.SetBool("WALK", false);
            basicAnimator.SetBool("RUN", false);
            basicAnimator.Rebind();
            basicAnimator.enabled = false;
        }
        else
        {
            basicAnimator.enabled = true;
        }
    }

    private void OnDrawGizmos()
    {
        if (!disableGizmos)
        {
            // Ground check
            if (CapsuleCollider)
            {
                float radius = CapsuleCollider.radius + groundRadiusOverride;
                Gizmos.color = Grounded ? Color.green : Color.red;
                Gizmos.DrawWireSphere(CapsuleBottom, radius);

                // Normal check
                Gizmos.color = Color.magenta;
                Gizmos.DrawRay(CapsuleBottom, -transform.up * (CapsuleCollider.height / 2f) * 1.5f);
            }
            else
            {
                CapsuleCollider = GetComponent<CapsuleCollider>();
            }
        }
    }
}