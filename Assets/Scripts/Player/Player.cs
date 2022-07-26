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
    /// Up ladder speed limit.
    /// </summary>
    [SerializeField] private float limitLadderVelocity = 4.5f;

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
    /// Returns current speed.
    /// </summary>
    public bool UseGravity
    {
        set
        {
            Rigidbody.useGravity = value;
        }
    }

    /// <summary>
    /// Returns current speed.
    /// </summary>
    public float CurrentSpeed
    {
        get
        {
            return isRunning ? runAcceleration : walkAcceleration;
        }
    }

    /// <summary>
    /// Returns the current speed limit.
    /// </summary>
    public float LimitCurrentSpeed
    {
        get
        {
            return (!isLadder ? (!InputManager.Crouch ? (!isRunning ? limitWalkVelocity : limitRunVelocity) : limitCrouchVelocity) : limitLadderVelocity) + additionalVelocity.magnitude;
        }
    }

    /// <summary>
    /// Returns whether the surface the player is on is curved or not.
    /// </summary>
    public bool Sloped
    {
        get
        {
            return SlopeAngle > 0 && SlopeAngle <= maxAngleToSlope;
        }
    }

    /// <summary>
    /// Returns the angle of the surface the player is standing on.
    /// </summary>
    public float SlopeAngle { get { return Vector3.Angle(transform.up, Normal); } }

    /// <summary>
    /// Returns the rotation Y axis of the player's transform.
    /// </summary>
    public float LocalYRotation { get { return transform.localEulerAngles.y; } }

    /// <summary>
    /// Returns the player's forward XZ axis
    /// </summary>
    public Vector2 ForwardXZ { get { return new Vector2(transform.forward.x, transform.forward.z); } }

    /// <summary>
    /// /// Returns the exact position of the top of the capsule.
    /// </summary>
    public Vector3 CapsuleTop
    {
        get
        {
            return (transform.position - CapsuleCollider.center) + (transform.up * ((CapsuleCollider.height / 2f) + groundHeightOffset));
        }
    }

    /// <summary>
    /// Returns the exact position of the bottom of the capsule.
    /// </summary>
    public Vector3 CapsuleBottom
    {
        get
        {
            return (transform.position + CapsuleCollider.center) - (transform.up * ((CapsuleCollider.height / 2f) + groundHeightOffset));
        }
    }

    /// <summary>
    /// Returns the normal vector of the surface the player is standing on.
    /// </summary>
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

    /// <summary>
    /// Returns the colliders the player is touching the ground.
    /// </summary>
    public Collider[] GroundColliders
    {
        get
        {
            float radius = CapsuleCollider.radius + groundRadiusOverride;
            Collider[] hits = Physics.OverlapSphere(CapsuleBottom, radius, groundableMask);
            return hits;
        }
    }

    /// <summary>
    /// Adds an additional direction that is superior to basic movement.
    /// </summary>
    public void SetAdditionalDirection(Vector3 direction)
    {
        additionalDirection = direction;
    }

    /// <summary>
    /// Resets additional direction to Vector3.zero.
    /// </summary>
    public void ResetAdditionalDirection()
    {
        additionalDirection = Vector3.zero;
    }

    // Private
    private Transform weaponTurn;
    private Transform cameraTurn;
    private float jumpFixTimer = 0f;
    private float startCapsuleHeight;
    private Vector3 startCameraTurnPos;
    private Vector3 additionalVelocity;
    private Vector3 additionalDirection;
    private Quaternion startTurnRot;
    private Quaternion startTurnCameraRot;
    private Transform CameraTransform;
    private Rigidbody Rigidbody;
    private CapsuleCollider CapsuleCollider;

    public static bool isWalking, isCrouched, isJumping, isGrounded, isTurning, isLadder;
    public static bool isAim => WeaponManager.IsAim;
    public static bool isRunning => InputManager.Run && isWalking && InputManager.MoveAxis.y > 0;

    private void Start()
    {
        // Get necessary components
        Rigidbody = GetComponent<Rigidbody>();
        CapsuleCollider = GetComponent<CapsuleCollider>();

        // Get transforms
        CameraTransform = FindManager.Find("Camera", this);
        weaponTurn = FindManager.Find("WeaponTurn", this);
        cameraTurn = FindManager.Find("CameraTurn", this);

        // Start locking cursor
        PlayerCamera.LockCursor(true);

        // Starting values
        startCapsuleHeight = CapsuleCollider.height;

        // Turn
        startTurnRot = weaponTurn.localRotation;
        startCameraTurnPos = cameraTurn.localPosition;
        startTurnCameraRot = cameraTurn.localRotation;

        if (weaponTurn == null || cameraTurn == null)
        {
            DebugManager.DebugAssignedError("WeaponTurn/CameraTurn");
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
    }

    public void UpdateMove()
    {
        isGrounded = GroundColliders.Length > 0;

        if (!isLadder)
        {
            if (isGrounded)
            {
                // Movement
                Vector2 moveAxis = InputManager.MoveAxis;
                Vector3 dir1 = transform.forward * moveAxis.y + transform.right * moveAxis.x;
                Vector3 dir2 = Vector3.Cross(transform.right, Normal) * moveAxis.y + Vector3.Cross(-transform.forward, Normal) * moveAxis.x;
                Vector3 direction = (!Sloped ? dir1 : dir2);

                Move(direction);
            }
        }
        else
        {
            // Movement
            Vector2 moveAxis = InputManager.MoveAxis;
            Vector3 direction = CameraTransform.forward * moveAxis.y + CameraTransform.right * moveAxis.x;

            Move(direction);
        }

        // Air animation
        basicAnimator.SetBool("AIR", !isGrounded);

        // Limit Velocity
        Vector3 flatVel = new Vector3(Rigidbody.velocity.x, !isLadder ? 0f : Rigidbody.velocity.y, Rigidbody.velocity.z);

        if (flatVel.magnitude > LimitCurrentSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * LimitCurrentSpeed;
            Rigidbody.velocity = new Vector3(limitedVel.x, !isLadder ? Rigidbody.velocity.y : limitedVel.y, limitedVel.z);
        }
    }

    private void CrouchUpdate()
    {
        bool _canCrouch = InputManager.Crouch;

        if (_canCrouch)
        {
            isCrouched = true;
            CapsuleCollider.height = Mathf.Lerp(CapsuleCollider.height, crouchHeight, crouchSpeed * Time.deltaTime);
        }
        else
        {
            isCrouched = false;
            CapsuleCollider.height = Mathf.Lerp(CapsuleCollider.height, startCapsuleHeight, crouchSpeed * Time.deltaTime);
        }
    }

    private void JumpUpdate()
    {
        bool jump = InputManager.Jump && isGrounded && jumpFixTimer <= 0;

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
        if (InputManager.Turn != 0)
        {
            isTurning = true;

            Vector3 targetPos = new Vector3(turnHorizontalAmount * turnCameraScale * InputManager.Turn, 0f, 0f);
            Quaternion targetRot = Quaternion.Euler(new Vector3(0f, 0f, turnHorizontalAmount * -InputManager.Turn));

            if (!isAim)
            { // Weapon
                weaponTurn.localRotation = Quaternion.Slerp(weaponTurn.localRotation, targetRot, turnSpeed * Time.deltaTime);
                cameraTurn.localRotation = Quaternion.Slerp(cameraTurn.localRotation, startTurnCameraRot, turnSpeed * Time.deltaTime);
                cameraTurn.localPosition = Vector3.Lerp(cameraTurn.localPosition, startCameraTurnPos, turnSpeed * Time.deltaTime);
            }
            else
            { // CameraTransform
                weaponTurn.localRotation = Quaternion.Slerp(weaponTurn.localRotation, startTurnRot, turnSpeed * Time.deltaTime);
                cameraTurn.localRotation = Quaternion.Slerp(cameraTurn.localRotation, targetRot, turnSpeed * Time.deltaTime);
                cameraTurn.localPosition = Vector3.Lerp(cameraTurn.localPosition, targetPos, turnSpeed * Time.deltaTime);
            }
        }
        else if (weaponTurn.localRotation != startTurnRot || cameraTurn.localRotation != startTurnCameraRot || cameraTurn.localPosition != startCameraTurnPos)
        {
            isTurning = false;

            weaponTurn.localRotation = Quaternion.Slerp(weaponTurn.localRotation, startTurnRot, turnSpeed * Time.deltaTime);
            cameraTurn.localRotation = Quaternion.Slerp(cameraTurn.localRotation, startTurnCameraRot, turnSpeed * Time.deltaTime);
            cameraTurn.localPosition = Vector3.Lerp(cameraTurn.localPosition, startCameraTurnPos, turnSpeed * Time.deltaTime);
        }
    }

    public void Move(Vector3 direction)
    {
        // Direction Ray
        if (!disableGizmos)
        {
            Debug.DrawRay(transform.position, direction * 2f, Color.magenta);
        }

        if (direction != Vector3.zero)
        {
            isWalking = true;
            Rigidbody.AddForce(direction.normalized * CurrentSpeed * 10f, ForceMode.Force);

            if (Rigidbody.drag != moveFriction)
            {
                Rigidbody.drag = moveFriction;
            }

            if (isGrounded)
            {
                if (CurrentSpeed == walkAcceleration)
                {
                    basicAnimator.SetBool("WALK", true);
                    basicAnimator.SetBool("RUN", false);
                }
                else if (CurrentSpeed == runAcceleration)
                {
                    basicAnimator.SetBool("WALK", false);
                    basicAnimator.SetBool("RUN", true);
                }
            }
        }
        else
        {
            isWalking = false;

            // Reset animations
            basicAnimator.SetBool("WALK", false);
            basicAnimator.SetBool("RUN", false);

            if (isGrounded)
            {
                Rigidbody.drag = idleFriction;
            }
            else
            {
                Rigidbody.drag = moveFriction;
            }
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
                Gizmos.color = isGrounded ? Color.green : Color.red;
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