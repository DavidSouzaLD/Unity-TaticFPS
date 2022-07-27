using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    private static PlayerController Instance;

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
    public float coverAmount = 15f;
    public float coverCamScale = 0.01f;
    public float coverSpeed = 8f;

    [Header("Ground Area")]
    public float groundAreaHeight = -0.35f;
    public float groundAreaRadius = 0.3f;

    [Header("Components")]
    public Animator Animator;
    public Rigidbody Rigidbody;
    public CapsuleCollider CapsuleCollider;

    [HideInInspector] public float jumpCountdown;
    [HideInInspector] public float initialCapsuleHeight;
    [HideInInspector] public Vector3 additionalVelocity;
    [HideInInspector] public Vector3 additionalDirection;
    [HideInInspector] public Vector3 initialCoverCamPos;
    [HideInInspector] public Quaternion initialCoverRot;
    [HideInInspector] public Quaternion initialCoverCamRot;
    [HideInInspector] public Transform cameraRoot;
    [HideInInspector] public Transform coverWeaponRoot;
    [HideInInspector] public Transform coverCamRoot;
    [HideInInspector] public PlayerState States;
    [HideInInspector] public PlayerFunctions Functions;

    public static PlayerState GetStates { get { return Instance.States; } }
    public static PlayerFunctions GetFunctions { get { return Instance.Functions; } }

    private void Awake()
    {
        // Create instance
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        // Player settings
        States = new PlayerState();
        Functions = new PlayerFunctions(this, States);
    }

    private void Start()
    {
        Functions.Start();
    }

    private void Update()
    {
        JumpUpdate();
        CrouchUpdate();
        CoverUpdate();
        StateUpdate();
    }

    private void FixedUpdate()
    {
        // Moving
        bool conditions = true;

        if (conditions)
        {
            if (States.GetState("GroundArea"))
            {
                Vector2 moveAxis = InputManager.MoveAxis;
                Vector3 dir1 = transform.forward * moveAxis.y + transform.right * moveAxis.x;
                Vector3 dir2 = Vector3.Cross(transform.right, Functions.GetNormal()) * moveAxis.y + Vector3.Cross(-transform.forward, Functions.GetNormal()) * moveAxis.x;
                Vector3 direction = (!States.GetState("Sloping") ? dir1 : dir2);
                Move(direction);
            }
        }

        // Additional gravity
        if (Rigidbody.velocity.y < 0)
        {
            Rigidbody.AddForce(transform.up * Physics.gravity.y * gravityScale * Time.deltaTime);
        }

        // Limit Velocity
        float limitedSpeed = (!States.GetState("Crouching") ? (!States.GetState("Running") ? maxWalkingSpeed : maxRunningSpeed) : maxCrouchingSpeed) + additionalVelocity.magnitude;
        Vector3 flatVel = new Vector3(Rigidbody.velocity.x, 0f, Rigidbody.velocity.z);

        if (flatVel.magnitude > limitedSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * limitedSpeed;
            Rigidbody.velocity = new Vector3(limitedVel.x, Rigidbody.velocity.y, limitedVel.z);
        }
    }

    private void JumpUpdate()
    {
        bool conditions = InputManager.Jump && States.GetState("GroundCollision") && jumpCountdown <= 0;

        if (conditions)
        {
            States.SetState("Jumping", true);
            Rigidbody.AddForce(transform.up * jumpingForce, ForceMode.Impulse);
            jumpCountdown = 0.3f;
        }

        if (jumpCountdown > 0)
        {
            States.SetState("Jumping", false);
            jumpCountdown -= Time.deltaTime;
        }

        // Air animation
        Animator.SetBool("Air", !States.GetState("GroundArea"));

        // Removing drag in air
        if (!States.GetState("Walking"))
        {
            if (States.GetState("GroundCollision"))
            {
                Rigidbody.drag = initialFriction;
            }
            else
            {
                Rigidbody.drag = movingFriction;
            }
        }
    }

    private void CrouchUpdate()
    {
        bool conditions = InputManager.Crouch && !States.GetState("Running");
        States.SetState("Crouching", conditions);

        if (conditions)
        {
            CapsuleCollider.height = Mathf.Lerp(CapsuleCollider.height, crouchHeight, crouchSpeed * Time.deltaTime);
        }
        else
        {
            CapsuleCollider.height = Mathf.Lerp(CapsuleCollider.height, initialCapsuleHeight, crouchSpeed * Time.deltaTime);
        }

        // Set camera in top capsule
        cameraRoot.position = Vector3.Lerp(cameraRoot.position, Functions.CapsuleTop, crouchSpeed * Time.deltaTime);
    }

    private void CoverUpdate()
    {
        bool conditions = InputManager.Cover != 0 && !States.GetState("Running");
        States.SetState("Covering", conditions);

        if (conditions)
        {
            Vector3 targetPos = new Vector3(coverAmount * coverCamScale * InputManager.Cover, 0f, 0f);
            Quaternion targetRot = Quaternion.Euler(new Vector3(0f, 0f, coverAmount * -InputManager.Cover));

            if (!States.GetState("Aiming"))
            {
                // Weapon
                coverWeaponRoot.localRotation = Quaternion.Slerp(coverWeaponRoot.localRotation, targetRot, coverSpeed * Time.deltaTime);
                coverCamRoot.localPosition = Vector3.Lerp(coverCamRoot.localPosition, initialCoverCamPos, coverSpeed * Time.deltaTime);
                coverCamRoot.localRotation = Quaternion.Slerp(coverCamRoot.localRotation, initialCoverCamRot, coverSpeed * Time.deltaTime);
            }
            else
            {
                // CameraTransform
                coverWeaponRoot.localRotation = Quaternion.Slerp(coverWeaponRoot.localRotation, initialCoverRot, coverSpeed * Time.deltaTime);
                coverCamRoot.localPosition = Vector3.Lerp(coverCamRoot.localPosition, targetPos, coverSpeed * Time.deltaTime);
                coverCamRoot.localRotation = Quaternion.Slerp(coverCamRoot.localRotation, targetRot, coverSpeed * Time.deltaTime);
            }
        }
        else if (coverWeaponRoot.localRotation != initialCoverRot || coverCamRoot.localRotation != initialCoverCamRot || coverCamRoot.localPosition != initialCoverCamPos)
        {
            coverWeaponRoot.localRotation = Quaternion.Slerp(coverWeaponRoot.localRotation, initialCoverRot, coverSpeed * Time.deltaTime);
            coverCamRoot.localPosition = Vector3.Lerp(coverCamRoot.localPosition, initialCoverCamPos, coverSpeed * Time.deltaTime);
            coverCamRoot.localRotation = Quaternion.Slerp(coverCamRoot.localRotation, initialCoverCamRot, coverSpeed * Time.deltaTime);
        }
    }

    private void StateUpdate()
    {
        // Setting
        States.SetState("GroundArea", Functions.GetColliders().Length > 0);
        States.SetState("Running", States.GetState("Walking") && InputManager.Run && InputManager.MoveAxis.y > 0);
        States.SetState("Sloping", Functions.GetSlopeAngle() > 0 && Functions.GetSlopeAngle() <= maxAngleSlope);
        States.SetState("Aiming", WeaponManager.IsAim);

        // Getting
        Rigidbody.useGravity = States.GetState("Graviting");
    }

    private void Move(Vector3 direction)
    {
        // Direction Ray
        Debug.DrawRay(transform.position, direction, Color.magenta);

        if (direction != Vector3.zero)
        {
            float currentSpeed = !States.GetState("Running") ? walkingSpeed : runningSpeed;
            States.SetState("Walking", true);

            Rigidbody.AddForce(direction.normalized * currentSpeed * 10f, ForceMode.Force);

            if (Rigidbody.drag != movingFriction)
            {
                Rigidbody.drag = movingFriction;
            }

            if (States.GetState("GroundArea"))
            {
                Animator.SetBool("Walking", States.GetState("Walking"));
                Animator.SetBool("Running", States.GetState("Running"));
            }
        }
        else
        {
            States.SetState("Walking", false);

            // Reset animations
            Animator.SetBool("WALK", false);
            Animator.SetBool("RUN", false);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if ((walkableMask.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            States.SetState("GroundCollision", true);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if ((walkableMask.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            States.SetState("GroundCollision", false);
        }
    }

    private void OnDrawGizmos()
    {
        // Ground check
        if (States != null)
        {
            if (CapsuleCollider)
            {
                float radius = CapsuleCollider.radius + groundAreaRadius;
                Gizmos.color = States.GetState("GroundArea") ? Color.green : Color.red;
                Gizmos.DrawWireSphere(Functions.CapsuleBottom, radius);

                // Normal check
                Gizmos.color = Color.magenta;
                Gizmos.DrawRay(Functions.CapsuleBottom, -transform.up * (CapsuleCollider.height / 2f) * 1.5f);
            }
            else
            {
                CapsuleCollider = GetComponent<CapsuleCollider>();
            }
        }
    }
}