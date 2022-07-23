using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Input))]
public class Player : MonoBehaviour
{
    #region Inspector
    [SerializeField] private bool disableGizmos;

    [Header("[Movement Settings]")]
    [SerializeField] private float walkAcceleration = 10f;
    [SerializeField] private float runAcceleration = 15f;
    Vector3 _additionalVelocity;

    [Header("[Limit Settings]")]
    [SerializeField] private float limitWalkVelocity = 5f;
    [SerializeField] private float limitRunVelocity = 7f;
    [SerializeField] private float limitCrouchVelocity = 2f;

    [Header("[Jump Settings]")]
    [SerializeField] private float jumpForce = 25f;
    float _jumpFixTimer = 0f;

    [Header("[Crouch Settings]")]
    [SerializeField] private float crouchHeight = 0.6f;
    [SerializeField] private float crouchSpeed = 5f;
    float _startCapsuleHeight;
    float _startCameraTransformHeight;

    [Header("[Drag Settings]")]
    [SerializeField] private float initialDrag = 0.5f;
    [SerializeField] private float drag = 5f;

    [Header("[Slope Settings]")]
    [SerializeField] private float maxAngleToSlope = 65f;

    [Header("[Ground Settings]")]
    [SerializeField] private LayerMask groundableMask;
    [SerializeField] private float groundHeightOffset = 0f;
    [SerializeField] private float groundRadiusOverride = 0f;

    [Header("[Turn Settings]")]
    [SerializeField] private Transform weaponTurnTransform;
    [SerializeField] private Transform cameraTurnTransform;
    [SerializeField] private float horizontalTurnAmount;
    [SerializeField] private float cameraTurnTransformScale;
    [SerializeField] private float turnSpeed;
    Vector3 cameraTurnTransformStartPos;
    Quaternion startTurnRot, cameraTurnTransformStartRot;

    [Header("[Animators Settings]")]
    [SerializeField] private Animator basicAnimator;
    [SerializeField] private Animator turnAnimator;

    #endregion

    #region Public Components/Info

    [HideInInspector]
    public static bool isWalking, isCrouched, isJumping, isGrounded, isTurning, isAim;
    public static bool isRunning => isWalking && !StateLock.IsLocked("PLAYER_RUN") && PlayerInput.Keys.Run;

    // Components
    [HideInInspector] public Transform CameraTransform;
    [HideInInspector] public Rigidbody Rigidbody;
    [HideInInspector] public CapsuleCollider CapsuleCollider;

    #endregion

    #region Public Variables
    public float CurrentSpeed { get { return isRunning ? runAcceleration : walkAcceleration; } }
    public float LimitCurrentSpeed { get { return (!PlayerInput.Keys.Crouch ? (!isRunning ? limitWalkVelocity : limitRunVelocity) : limitCrouchVelocity) + _additionalVelocity.magnitude; } }
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

    #endregion

    private void Start()
    {
        // Get necessary components
        CameraTransform = GetComponentInChildren<PlayerCamera>().transform;
        Rigidbody = GetComponent<Rigidbody>();
        CapsuleCollider = GetComponent<CapsuleCollider>();

        // Start locking cursor
        PlayerCamera.LockCursor(true);

        // Starting values
        _startCameraTransformHeight = CameraTransform.transform.localPosition.y;
        _startCapsuleHeight = CapsuleCollider.height;

        // Turn
        startTurnRot = weaponTurnTransform.localRotation;
        cameraTurnTransformStartPos = cameraTurnTransform.localPosition;
        cameraTurnTransformStartRot = cameraTurnTransform.localRotation;
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
            Vector2 moveAxis = PlayerInput.Keys.MoveAxis;
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

                if (Rigidbody.drag != initialDrag)
                {
                    Rigidbody.drag = initialDrag;
                }

                if (Grounded)
                {
                    Rigidbody.AddForce(direction.normalized * CurrentSpeed * 10f, ForceMode.Force);

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

                if (Grounded)
                {
                    Rigidbody.drag = drag;
                }
                else
                {
                    Rigidbody.drag = initialDrag;
                }
            }
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
        bool _canCrouch = !StateLock.IsLocked("PLAYER_CROUCH") && PlayerInput.Keys.Crouch;

        if (_canCrouch)
        {
            isCrouched = true;

            StateLock.Lock("PLAYER_RUN", true);
            CapsuleCollider.height = Mathf.Lerp(CapsuleCollider.height, crouchHeight, crouchSpeed * Time.deltaTime);
            CameraTransform.transform.position = Vector3.Lerp(CameraTransform.transform.position,
            CapsuleTop, crouchSpeed * Time.deltaTime);
        }
        else
        {
            isCrouched = false;

            CapsuleCollider.height = Mathf.Lerp(CapsuleCollider.height, _startCapsuleHeight, crouchSpeed * Time.deltaTime);
            CameraTransform.transform.localPosition = Vector3.Lerp(CameraTransform.transform.localPosition,
            new Vector3(
                CameraTransform.transform.localPosition.x,
                _startCameraTransformHeight,
                CameraTransform.transform.localPosition.z
            ), crouchSpeed * Time.deltaTime);
        }
    }

    private void JumpUpdate()
    {
        bool jump = !StateLock.IsLocked("PLAYER_JUMP") && PlayerInput.Keys.Jump && Grounded && _jumpFixTimer <= 0;

        if (jump)
        {
            isJumping = true;
            Rigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            _jumpFixTimer = 0.15f;
        }

        if (_jumpFixTimer > 0)
        {
            isJumping = false;
            _jumpFixTimer -= Time.deltaTime;
        }
    }

    private void TurnUpdate()
    {
        bool _canTurn = !StateLock.IsLocked("PLAYER_TURN") && !isRunning;

        if (_canTurn)
        {
            // Right
            if (PlayerInput.Keys.Turn != 0)
            {
                isTurning = true;

                Vector3 targetPos = new Vector3(horizontalTurnAmount * cameraTurnTransformScale * PlayerInput.Keys.Turn, 0f, 0f);
                Quaternion targetRot = Quaternion.Euler(new Vector3(0f, 0f, horizontalTurnAmount * -PlayerInput.Keys.Turn));

                if (!isAim)
                { // Weapon
                    weaponTurnTransform.localRotation = Quaternion.Slerp(weaponTurnTransform.localRotation, targetRot, turnSpeed * Time.deltaTime);
                    cameraTurnTransform.localRotation = Quaternion.Slerp(cameraTurnTransform.localRotation, cameraTurnTransformStartRot, turnSpeed * Time.deltaTime);
                    cameraTurnTransform.localPosition = Vector3.Lerp(cameraTurnTransform.localPosition, cameraTurnTransformStartPos, turnSpeed * Time.deltaTime);
                }
                else
                { // CameraTransform
                    cameraTurnTransform.localPosition = Vector3.Lerp(cameraTurnTransform.localPosition, targetPos, turnSpeed * Time.deltaTime);
                    cameraTurnTransform.localRotation = Quaternion.Slerp(cameraTurnTransform.localRotation, targetRot, turnSpeed * Time.deltaTime);
                    weaponTurnTransform.localRotation = Quaternion.Slerp(weaponTurnTransform.localRotation, startTurnRot, turnSpeed * Time.deltaTime);
                }
            }
            else if (weaponTurnTransform.localRotation != startTurnRot || cameraTurnTransform.localRotation != cameraTurnTransformStartRot || cameraTurnTransform.localPosition != cameraTurnTransformStartPos)
            {
                isTurning = false;

                weaponTurnTransform.localRotation = Quaternion.Slerp(weaponTurnTransform.localRotation, startTurnRot, turnSpeed * Time.deltaTime);
                cameraTurnTransform.localRotation = Quaternion.Slerp(cameraTurnTransform.localRotation, cameraTurnTransformStartRot, turnSpeed * Time.deltaTime);
                cameraTurnTransform.localPosition = Vector3.Lerp(cameraTurnTransform.localPosition, cameraTurnTransformStartPos, turnSpeed * Time.deltaTime);
            }
        }
    }

    private void AnimatorUpdate()
    {
        if (StateLock.IsLocked("PLAYER_MOVEMENT") || StateLock.IsLocked("PLAYER_BASIC_ANIM"))
        {
            basicAnimator.SetBool("WALK", false);
            basicAnimator.SetBool("RUN", false);
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