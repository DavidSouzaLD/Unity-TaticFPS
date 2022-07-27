using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    public class States
    {
        public class State
        {
            [HideInInspector]
            public string name;
            public bool value;

            public State(string _name, bool _value = false)
            {
                name = _name;
                value = _value;
            }
        }

        private List<State> myStates;

        public void InitStates()
        {
            myStates = new List<State>()
            {
                new State("Walking"),
                new State("Running"),
                new State("Crouching"),
                new State("Jumping"),
                new State("Grounded"),
                new State("Covering"),
                new State("Climbing"),
                new State("Sloping"),
                new State("Aiming"),
            };
        }

        public void SetState(string stateName, bool value)
        {
            for (int i = 0; i < myStates.Count; i++)
            {
                if (myStates[i].name.ToUpper().Equals(stateName.ToUpper()))
                {
                    if (myStates[i].value != value)
                    {
                        myStates[i].value = value;
                    }
                    return;
                }
            }
            Debug.LogError("(PlayerController) State not finded! + " + stateName);
        }

        public bool GetState(string stateName)
        {
            for (int i = 0; i < myStates.Count; i++)
            {
                if (myStates[i].name.ToUpper().Equals(stateName.ToUpper()))
                {
                    return myStates[i].value;
                }
            }
            return false;
        }
    }

    [Header("Movement")]
    [SerializeField] private float walkingSpeed = 10f;
    [SerializeField] private float runningSpeed = 15f;
    [SerializeField] private float maxAngleSlope = 65f;
    [Space]
    [SerializeField] private float maxWalkingSpeed = 3f;
    [SerializeField] private float maxRunningSpeed = 4.5f;
    [SerializeField] private float maxCrouchingSpeed = 1.5f;
    [SerializeField] private float maxClimbingSpeed = 1f;
    [Space]
    [SerializeField] private float jumpingForce = 25f;

    [Header("Friction")]
    [SerializeField] private float initialFriction = 5f;
    [SerializeField] private float movingFriction = 0.5f;

    [Header("Crouch")]
    [SerializeField] private float crouchHeight = 1.5f;
    [SerializeField] private float crouchSpeed = 5f;

    [Header("Ground")]
    [SerializeField] private LayerMask walkableMask;
    [SerializeField] private float groundCheckHeight = -0.35f;
    [SerializeField] private float groundCheckRadius = 0.1f;

    [Header("Cover")]
    [SerializeField] private float coverAmount = 15f;
    [SerializeField] private float coverCamScale = 0.03f;
    [SerializeField] private float coverSpeed = 8f;

    [Header("Components")]
    [SerializeField] private Animator Animator;
    [SerializeField] private Rigidbody Rigidbody;
    [SerializeField] private CapsuleCollider CapsuleCollider;

    public static Vector3 CapsuleTop
    {
        get
        {
            return (Instance.transform.position - Instance.CapsuleCollider.center) + (Instance.transform.up * ((Instance.CapsuleCollider.height / 2f) + Instance.groundCheckHeight));
        }
    }

    public static Vector3 CapsuleBottom
    {
        get
        {
            return (Instance.transform.position + Instance.CapsuleCollider.center) - (Instance.transform.up * ((Instance.CapsuleCollider.height / 2f) + Instance.groundCheckHeight));
        }
    }

    public static bool UseGravity
    {
        set
        {
            Instance.Rigidbody.useGravity = value;
        }
    }

    public static Collider[] GetColliders()
    {
        float radius = Instance.CapsuleCollider.radius + Instance.groundCheckRadius;
        Collider[] hits = Physics.OverlapSphere(CapsuleBottom, radius, Instance.walkableMask);
        return hits;
    }

    public static Vector3 GetNormal()
    {
        RaycastHit hit;
        if (Physics.Raycast(CapsuleBottom, -Instance.transform.up, out hit, (Instance.CapsuleCollider.height / 2f) * 1.5f, Instance.walkableMask))
        {
            Debug.DrawRay(hit.point, hit.normal, Color.yellow);
            return hit.normal;
        }
        return Vector3.one;
    }

    public static Transform GetTransform() => Instance.transform;
    public static Vector2 GetForwardXZ() => new Vector2(Instance.transform.forward.x, Instance.transform.forward.z);
    public static float GetLocalYRotation() => Instance.transform.localEulerAngles.y;
    public static float GetSlopeAngle() => Vector3.Angle(Instance.transform.up, GetNormal());
    public static void SetState(string stateName, bool value) => PlayerState.SetState(stateName, value);
    public static bool GetState(string stateName) => PlayerState.GetState(stateName);
    public static void SetAdditionalDirection(Vector3 direction) => Instance.additionalDirection = direction;
    public static void ResetAdditionalDirection() => Instance.additionalDirection = Vector3.zero;

    private float jumpCountdown;
    private float initialCapsuleHeight;
    private Vector3 additionalVelocity;
    private Vector3 additionalDirection;
    private Vector3 initialCoverCamPos;
    private Quaternion initialCoverRot;
    private Quaternion initialCoverCamRot;
    private Transform cameraRoot;
    private Transform coverWeaponRoot;
    private Transform coverCamRoot;
    private static PlayerController Instance;
    private static States PlayerState;

    private void Awake()
    {
        // Create states
        PlayerState = new States();
        PlayerState.InitStates();

        // Create instance
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        // Components
        Rigidbody = GetComponent<Rigidbody>();
        CapsuleCollider = GetComponent<CapsuleCollider>();

        // Transforms
        cameraRoot = FindManager.Find("Camera");
        coverWeaponRoot = FindManager.Find("WeaponCover");
        coverCamRoot = FindManager.Find("CameraCover");

        // Cover
        initialCoverRot = coverWeaponRoot.localRotation;
        initialCoverCamPos = coverCamRoot.localPosition;
        initialCoverCamRot = coverCamRoot.localRotation;

        // Others
        initialCapsuleHeight = CapsuleCollider.height;
        PlayerCamera.LockCursor(true);
    }

    private void FixedUpdate()
    {
        PhysicsUpdate();
    }

    private void Update()
    {
        JumpUpdate();
        CrouchUpdate();
        CoverUpdate();
        StateUpdate();
    }

    private void PhysicsUpdate()
    {
        // Moving
        if (!GetState("Climbing"))
        {
            if (GetState("Grounded"))
            {
                Vector2 moveAxis = InputManager.MoveAxis;
                Vector3 dir1 = transform.forward * moveAxis.y + transform.right * moveAxis.x;
                Vector3 dir2 = Vector3.Cross(transform.right, GetNormal()) * moveAxis.y + Vector3.Cross(-transform.forward, GetNormal()) * moveAxis.x;
                Vector3 direction = (!GetState("Sloping") ? dir1 : dir2);
                Move(direction);
            }
        }
        else
        {
            // Climbing ladder
            Vector2 moveAxis = InputManager.MoveAxis;
            Vector3 direction = cameraRoot.forward * moveAxis.y + cameraRoot.right * moveAxis.x;
            Move(direction);
        }

        // Limit Velocity
        float limitedSpeed = (!GetState("Climbing") ? (!GetState("Crouching") ? (!GetState("Running") ? maxWalkingSpeed : maxRunningSpeed) : maxCrouchingSpeed) : maxClimbingSpeed) + additionalVelocity.magnitude;
        Vector3 flatVel = new Vector3(Rigidbody.velocity.x, !GetState("Climbing") ? 0f : Rigidbody.velocity.y, Rigidbody.velocity.z);

        if (flatVel.magnitude > limitedSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * limitedSpeed;
            Rigidbody.velocity = new Vector3(limitedVel.x, !GetState("Climbing") ? Rigidbody.velocity.y : limitedVel.y, limitedVel.z);
        }
    }

    private void JumpUpdate()
    {
        bool conditions = InputManager.Jump && GetState("Grounded") && !GetState("Climbing") && jumpCountdown <= 0;

        if (conditions)
        {
            PlayerState.SetState("Jumping", true);
            Rigidbody.AddForce(transform.up * jumpingForce, ForceMode.Impulse);
            jumpCountdown = 0.15f;
        }

        if (jumpCountdown > 0)
        {
            PlayerState.SetState("Jumping", false);
            jumpCountdown -= Time.deltaTime;
        }

        // Air animation
        Animator.SetBool("Air", !GetState("Grounded"));
    }

    private void CrouchUpdate()
    {
        bool conditions = InputManager.Crouch && !GetState("Running") && !GetState("Climbing") && GetState("Grounded");

        if (conditions)
        {
            PlayerState.SetState("Crouching", true);
            CapsuleCollider.height = Mathf.Lerp(CapsuleCollider.height, crouchHeight, crouchSpeed * Time.deltaTime);
        }
        else
        {
            PlayerState.SetState("Crouching", false);
            CapsuleCollider.height = Mathf.Lerp(CapsuleCollider.height, initialCapsuleHeight, crouchSpeed * Time.deltaTime);
        }
    }

    private void CoverUpdate()
    {
        bool conditions = InputManager.Turn != 0 && !GetState("Running") && GetState("Ground");

        if (conditions)
        {
            PlayerState.SetState("Covering", true);

            Vector3 targetPos = new Vector3(coverAmount * coverCamScale * InputManager.Turn, 0f, 0f);
            Quaternion targetRot = Quaternion.Euler(new Vector3(0f, 0f, coverAmount * -InputManager.Turn));

            if (!GetState("Aiming"))
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
            PlayerState.SetState("Covering", false);

            coverWeaponRoot.localRotation = Quaternion.Slerp(coverWeaponRoot.localRotation, initialCoverRot, coverSpeed * Time.deltaTime);
            coverCamRoot.localPosition = Vector3.Lerp(coverCamRoot.localPosition, initialCoverCamPos, coverSpeed * Time.deltaTime);
            coverCamRoot.localRotation = Quaternion.Slerp(coverCamRoot.localRotation, initialCoverCamRot, coverSpeed * Time.deltaTime);
        }
    }

    private void StateUpdate()
    {
        PlayerState.SetState("Grounded", GetColliders().Length > 0);
        PlayerState.SetState("Running", GetState("Walking") && InputManager.Run && InputManager.MoveAxis.y > 0);
        PlayerState.SetState("Sloping", GetSlopeAngle() > 0 && GetSlopeAngle() <= maxAngleSlope);
    }

    private void Move(Vector3 direction)
    {
        // Direction Ray
        Debug.DrawRay(transform.position, direction, Color.magenta);

        if (direction != Vector3.zero)
        {
            float currentSpeed = !GetState("Running") ? walkingSpeed : runningSpeed;
            PlayerState.SetState("Walking", true);

            Rigidbody.AddForce(direction.normalized * currentSpeed * 10f, ForceMode.Force);

            if (Rigidbody.drag != movingFriction)
            {
                Rigidbody.drag = movingFriction;
            }

            if (GetState("Grounded"))
            {
                if (currentSpeed == walkingSpeed)
                {
                    Animator.SetBool("Walking", true);
                    Animator.SetBool("Running", false);
                }
                else if (currentSpeed == runningSpeed)
                {
                    Animator.SetBool("Waking", false);
                    Animator.SetBool("Running", true);
                }
            }
        }
        else
        {
            PlayerState.SetState("Walking", false);

            // Reset animations
            Animator.SetBool("WALK", false);
            Animator.SetBool("RUN", false);

            if (GetState("Grounded"))
            {
                Rigidbody.drag = initialFriction;
            }
            else
            {
                Rigidbody.drag = movingFriction;
            }
        }
    }

    private void OnDrawGizmos()
    {
        // Ground check
        if (PlayerState != null)
        {
            if (CapsuleCollider)
            {
                float radius = CapsuleCollider.radius + groundCheckRadius;
                Gizmos.color = GetState("Grounded") ? Color.green : Color.red;
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