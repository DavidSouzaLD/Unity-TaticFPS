using System.Collections.Generic;
using UnityEngine;

public class PlayerState : StateBase
{
    public PlayerState()
    {
        states = new List<State>()
            {
                new State("Walking"),
                new State("Running"),
                new State("Crouching"),
                new State("Jumping"),
                new State("GroundArea"),
                new State("GroundCollision"),
                new State("Covering"),
                new State("Sloping"),
                new State("Aiming"),
                new State("Graviting"),
            };
    }
}

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerController : StaticInstance<PlayerController>
{
    [SerializeField] private PlayerControllerPreset Preset;

    private float jumpCountdown;
    private float initialCapsuleHeight;
    private Vector3 initialCoverCamPos;
    private Quaternion initialCoverRot;
    private Quaternion initialCoverCamRot;
    private Transform cameraRoot;
    private Transform coverWeaponRoot;
    private Transform coverCamRoot;
    private Animator Animator;
    private Rigidbody Rigidbody;
    private CapsuleCollider CapsuleCollider;
    private PlayerState States;

    public Vector3 GetNormal()
    {
        RaycastHit hit;

        if (Physics.Raycast(CapsuleBottom, -transform.up, out hit, (CapsuleCollider.height / 2f) * 1.5f, Preset.walkableMask))
        {
            Debug.DrawRay(hit.point, hit.normal, Color.yellow);
            return hit.normal;
        }

        return Vector3.one;
    }

    public Collider[] GetColliders() => Physics.OverlapSphere(CapsuleBottom, CapsuleCollider.radius + Preset.groundAreaRadius, Preset.walkableMask);
    public static Transform GetTransform() => Instance.transform;
    public static Vector2 GetForwardXZ() => new Vector2(Instance.transform.forward.x, Instance.transform.forward.z);
    public static Vector3 GetCoveringPos() => new Vector3(Instance.Preset.coverAmount * Instance.Preset.coverCamScale * PlayerInput.Cover, 0f, 0f);
    public static Quaternion GetCoveringRot() => Quaternion.Euler(new Vector3(0f, 0f, Instance.Preset.coverAmount * -PlayerInput.Cover));
    public static float GetLocalYRotation() => Instance.transform.localEulerAngles.y;
    public static float GetSlopeAngle() => Vector3.Angle(Instance.transform.up, Instance.GetNormal());
    public static bool GetState(string stateName) => GetState(stateName);
    public static void SetState(string stateName, bool value) => SetState(stateName, value);
    public static void UseGravity(bool value) => Instance.Rigidbody.useGravity = value;
    public static Vector3 CapsuleTop => (Instance.transform.position - Instance.CapsuleCollider.center) + (Instance.transform.up * ((Instance.CapsuleCollider.height / 2f) + Instance.Preset.groundAreaHeight));
    public static Vector3 CapsuleBottom => (Instance.transform.position + Instance.CapsuleCollider.center) - (Instance.transform.up * ((Instance.CapsuleCollider.height / 2f) + Instance.Preset.groundAreaHeight));
    public static PlayerState GetStates => Instance.States;

    protected override void Awake()
    {
        base.Awake();

        // Player settings
        States = new PlayerState();
    }

    private void Start()
    {
        // Setting starts states
        States.SetState("Graviting", true);

        // Components
        Animator = FindManager.Find("Basic").GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody>();
        CapsuleCollider = transform.GetComponent<CapsuleCollider>();

        // Transforms
        cameraRoot = FindManager.Find("Camera");
        coverWeaponRoot = FindManager.Find("WeaponCover");
        coverCamRoot = FindManager.Find("CameraCover");

        // Jump
        jumpCountdown = 0.3f;

        // Cover
        initialCoverRot = coverWeaponRoot.localRotation;
        initialCoverCamPos = coverCamRoot.localPosition;
        initialCoverCamRot = coverCamRoot.localRotation;

        // Others
        initialCapsuleHeight = CapsuleCollider.height;
        PlayerCamera.LockCursor(true);
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
        if (States.GetState("GroundArea"))
        {
            Vector2 moveAxis = PlayerInput.MoveAxis;
            Vector3 dir1 = transform.forward * moveAxis.y + transform.right * moveAxis.x;
            Vector3 dir2 = Vector3.Cross(transform.right, GetNormal()) * moveAxis.y + Vector3.Cross(-transform.forward, GetNormal()) * moveAxis.x;
            Vector3 direction = (!States.GetState("Sloping") ? dir1 : dir2);
            Move(direction);
        }

        // Additional gravity
        if (Rigidbody.velocity.y < 0)
        {
            Rigidbody.AddForce(transform.up * Physics.gravity.y * Preset.gravityScale * Time.deltaTime);
        }

        // Limit Velocity
        float limitedSpeed = (!States.GetState("Crouching") ? (!States.GetState("Running") ? Preset.maxWalkingSpeed : Preset.maxRunningSpeed) : Preset.maxCrouchingSpeed);
        Vector3 flatVel = new Vector3(Rigidbody.velocity.x, 0f, Rigidbody.velocity.z);

        if (flatVel.magnitude > limitedSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * limitedSpeed;
            Rigidbody.velocity = new Vector3(limitedVel.x, Rigidbody.velocity.y, limitedVel.z);
        }
    }

    private void JumpUpdate()
    {
        bool conditions =
        PlayerInput.Jump &&
        States.GetState("GroundCollision") &&
        jumpCountdown <= 0;

        if (conditions)
        {
            States.SetState("Jumping", true);
            Rigidbody.AddForce(transform.up * Preset.jumpingForce, ForceMode.Impulse);
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
                Rigidbody.drag = Preset.initialFriction;
            }
            else
            {
                Rigidbody.drag = Preset.movingFriction;
            }
        }
    }

    private void CrouchUpdate()
    {
        bool conditions =
        PlayerInput.Crouch &&
        !States.GetState("Running");

        States.SetState("Crouching", conditions);

        if (conditions)
        {
            CapsuleCollider.height = Mathf.Lerp(CapsuleCollider.height, Preset.crouchHeight, Preset.crouchSpeed * Time.deltaTime);
        }
        else
        {
            CapsuleCollider.height = Mathf.Lerp(CapsuleCollider.height, initialCapsuleHeight, Preset.crouchSpeed * Time.deltaTime);
        }

        // Set camera in top capsule
        coverCamRoot.position = Vector3.Lerp(coverCamRoot.position, CapsuleTop, Preset.crouchSpeed * Time.deltaTime);
    }

    private void CoverUpdate()
    {
        bool conditions =
        !WeaponManager.IsSafety &&
        !States.GetState("Running") &&
        PlayerInput.Cover != 0;

        States.SetState("Covering", conditions);

        if (conditions)
        {
            Vector3 targetPos = GetCoveringPos();
            Quaternion targetRot = GetCoveringRot();

            if (!States.GetState("Aiming"))
            {
                // Weapon
                coverWeaponRoot.localRotation = Quaternion.Slerp(coverWeaponRoot.localRotation, targetRot, Preset.coverSpeed * Time.deltaTime);
                coverCamRoot.localRotation = Quaternion.Slerp(coverCamRoot.localRotation, initialCoverCamRot, Preset.coverSpeed * Time.deltaTime);
                coverCamRoot.localPosition = Vector3.Lerp(coverCamRoot.localPosition, initialCoverCamPos, Preset.coverSpeed * Time.deltaTime);
            }
            else
            {
                // Camera
                coverWeaponRoot.localRotation = Quaternion.Slerp(coverWeaponRoot.localRotation, initialCoverRot, Preset.coverSpeed * Time.deltaTime);
                coverCamRoot.localRotation = Quaternion.Slerp(coverCamRoot.localRotation, targetRot, Preset.coverSpeed * Time.deltaTime);
                coverCamRoot.localPosition = Vector3.Lerp(coverCamRoot.localPosition, targetPos, Preset.coverSpeed * Time.deltaTime);
            }
        }
        else if (coverWeaponRoot.localRotation != initialCoverRot || coverCamRoot.localRotation != initialCoverCamRot || coverCamRoot.localPosition != initialCoverCamPos)
        {
            coverWeaponRoot.localRotation = Quaternion.Slerp(coverWeaponRoot.localRotation, initialCoverRot, Preset.coverSpeed * Time.deltaTime);
            coverCamRoot.localRotation = Quaternion.Slerp(coverCamRoot.localRotation, initialCoverCamRot, Preset.coverSpeed * Time.deltaTime);
            coverCamRoot.localPosition = Vector3.Lerp(coverCamRoot.localPosition, initialCoverCamPos, Preset.coverSpeed * Time.deltaTime);
        }
    }

    private void StateUpdate()
    {
        // Setting
        States.SetState("GroundArea", GetColliders().Length > 0);
        States.SetState("Running", PlayerInput.Run && States.GetState("Walking"));
        States.SetState("Sloping", GetSlopeAngle() > 0 && GetSlopeAngle() <= Preset.maxAngleSlope);
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
            float currentSpeed = !States.GetState("Running") ? Preset.walkingSpeed : Preset.runningSpeed;
            States.SetState("Walking", true);

            Rigidbody.AddForce(direction.normalized * currentSpeed * 10f, ForceMode.Force);

            if (Rigidbody.drag != Preset.movingFriction)
            {
                Rigidbody.drag = Preset.movingFriction;
            }

            if (!WeaponManager.IsAim)
            {
                if (States.GetState("GroundArea"))
                {
                    Animator.SetBool("Walking", States.GetState("Walking"));
                    Animator.SetBool("Running", States.GetState("Running"));
                }
            }
            else
            {
                Animator.SetBool("Walking", false);
                Animator.SetBool("Running", false);
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
        if ((Preset.walkableMask.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            States.SetState("GroundCollision", true);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if ((Preset.walkableMask.value & (1 << other.transform.gameObject.layer)) > 0)
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
                float radius = CapsuleCollider.radius + Preset.groundAreaRadius;
                Gizmos.color = States.GetState("GroundArea") ? Color.green : Color.red;
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