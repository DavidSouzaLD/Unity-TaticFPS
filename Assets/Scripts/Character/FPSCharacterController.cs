using System.Collections.Generic;
using UnityEngine;
using Game.Weapons;
using Game.Utilities;

namespace Game.Character
{
    public class CharacterState : States
    {
        public CharacterState()
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

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class FPSCharacterController : Singleton<FPSCharacterController>
    {
        [SerializeField]
        private CharacterControllerPreset Preset;

        [Header("Roots")]
        [SerializeField] private Transform camRoot;
        [SerializeField] private Transform coverWeaponRoot;
        [SerializeField] private Transform coverCamRoot;

        // Private
        private float jumpCountdown;
        private float initialCapsuleHeight;
        private Vector3 initialCoverCamPos;
        private Quaternion initialCoverRot;
        private Quaternion initialCoverCamRot;
        private Rigidbody Rigidbody;
        private CapsuleCollider CapsuleCollider;
        private CharacterState States;

        public void UseGravity(bool _value)
        {
            Rigidbody.useGravity = _value;
        }

        public Vector3 CapsuleTop()
        {
            Vector3 position = (transform.position - CapsuleCollider.center);
            Vector3 direction = (transform.up * ((CapsuleCollider.height / 2f) + Preset.groundAreaHeight));
            return position + direction;
        }

        public Vector3 CapsuleBottom()
        {
            Vector3 position = (transform.position + CapsuleCollider.center);
            Vector3 direction = (transform.up * ((CapsuleCollider.height / 2f) + Preset.groundAreaHeight));
            return position - direction;
        }

        public Vector3 GroundNormal()
        {
            RaycastHit hit;
            Vector3 position = CapsuleBottom();
            Vector3 direction = -transform.up;
            float distance = (CapsuleCollider.height / 2f) * 1.5f;

            if (Physics.Raycast(position, direction, out hit, distance, Preset.walkableMask))
            {
                Debug.DrawRay(hit.point, hit.normal, Color.yellow);
                return hit.normal;
            }

            return Vector3.zero;
        }

        public float GetSlopeAngle()
        {
            return Vector3.Angle(transform.up, GroundNormal());
        }

        public static Transform GetTransform()
        {
            return Instance.transform;
        }

        public static Vector2 GetForwardXZ()
        {
            return new Vector2(Instance.transform.forward.x, Instance.transform.forward.z);
        }

        public static float GetLocalYRotation()
        {
            return Instance.transform.localEulerAngles.y;
        }

        public static bool GetState(string _stateName)
        {
            return Instance.States.GetState(_stateName);
        }

        public static void SetState(string _stateName, bool _value)
        {
            Instance.States.SetState(_stateName, _value);
        }

        protected override void Awake()
        {
            base.Awake();

            // Create state
            if (States == null)
            {
                States = new CharacterState();
            }
        }

        private void Start()
        {
            // Setting starts states
            SetState("Graviting", true);

            // Components
            Rigidbody = GetComponent<Rigidbody>();
            CapsuleCollider = transform.GetComponent<CapsuleCollider>();

            // Jump
            jumpCountdown = 0.3f;

            // Cover
            initialCoverRot = coverWeaponRoot.localRotation;
            initialCoverCamPos = coverCamRoot.localPosition;
            initialCoverCamRot = coverCamRoot.localRotation;

            // Others
            initialCapsuleHeight = CapsuleCollider.height;
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
            Vector2 moveInput = Systems.Input.GetVector2("MoveAxis");

            if (moveInput != Vector2.zero)
            {
                if (GetState("GroundArea"))
                {
                    Vector3 dir1 = transform.forward * moveInput.y + transform.right * moveInput.x;
                    Vector3 dir2 = Vector3.Cross(transform.right, GroundNormal()) * moveInput.y + Vector3.Cross(-transform.forward, GroundNormal()) * moveInput.x;
                    Vector3 direction = (!GetState("Sloping") ? dir1 : dir2);

                    Move(direction);
                    SetState("Walking", true);
                }
            }
            else
            {
                SetState("Walking", false);
            }

            // Additional gravity
            if (Rigidbody.velocity.y < 0)
            {
                Rigidbody.AddForce(transform.up * Physics.gravity.y * Preset.gravityScale * Time.deltaTime);
            }

            // Drag / Friction
            if (GetState("GroundCollision"))
            {
                if (GetState("Walking"))
                {
                    Rigidbody.drag = Preset.movingDrag;
                }
                else
                {
                    Rigidbody.drag = Preset.idleDrag;
                }
            }
            else
            {
                Rigidbody.drag = Preset.airDrag;
            }

            // Limit Velocity
            float limitedSpeed = (!GetState("Crouching") ? (!GetState("Running") ? Preset.maxWalkingSpeed : Preset.maxRunningSpeed) : Preset.maxCrouchingSpeed);
            Vector3 flatVel = new Vector3(Rigidbody.velocity.x, 0f, Rigidbody.velocity.z);

            if (flatVel.magnitude > limitedSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * limitedSpeed;
                Rigidbody.velocity = new Vector3(limitedVel.x, Rigidbody.velocity.y, limitedVel.z);
            }
        }

        private void JumpUpdate()
        {
            bool inputConditions = Systems.Input.GetBool("Jump");
            bool stateConditions = GetState("GroundCollision");
            bool differenceConditions = jumpCountdown <= 0;
            bool conditions = inputConditions && stateConditions && differenceConditions;

            if (conditions)
            {
                SetState("Jumping", true);
                Rigidbody.AddForce(transform.up * Preset.jumpingForce, ForceMode.Impulse);
                jumpCountdown = 0.3f;
            }

            if (jumpCountdown > 0)
            {
                SetState("Jumping", false);
                jumpCountdown -= Time.deltaTime;
            }
        }

        private void CrouchUpdate()
        {
            bool inputConditions = Systems.Input.GetBool("Crouch");
            bool stateConditions = !GetState("Running");
            bool conditions = inputConditions && stateConditions;

            SetState("Crouching", conditions);

            if (conditions)
            {
                CapsuleCollider.height = Mathf.Lerp(CapsuleCollider.height, Preset.crouchHeight, Preset.crouchSpeed * Time.deltaTime);
            }
            else
            {
                CapsuleCollider.height = Mathf.Lerp(CapsuleCollider.height, initialCapsuleHeight, Preset.crouchSpeed * Time.deltaTime);
            }

            // Set camera in top capsule
            coverCamRoot.position = Vector3.Lerp(coverCamRoot.position, CapsuleTop(), Preset.crouchSpeed * Time.deltaTime);
        }

        private void CoverUpdate()
        {
            float coverInput = Systems.Input.GetFloat("Cover");
            bool inputCondition = coverInput != 0;
            bool stateCondition = !GetState("Running");
            bool conditions = inputCondition && stateCondition;

            SetState("Covering", conditions);

            if (conditions)
            {
                Vector3 targetPos = new Vector3(Preset.coverAmount * Preset.coverCamScale * coverInput, 0f, 0f);
                Quaternion targetRot = Quaternion.Euler(new Vector3(0f, 0f, Preset.coverAmount * -coverInput));
                Debug.Log(WeaponManager.IsAim());
                if (!WeaponManager.IsAim())
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
            SetState("GroundArea", Physics.OverlapSphere(CapsuleBottom(), CapsuleCollider.radius + Preset.groundAreaRadius, Preset.walkableMask).Length > 0);
            SetState("Sloping", GetSlopeAngle() > 0 && GetSlopeAngle() <= Preset.maxAngleSlope);

            // Running
            bool inputConditions = Systems.Input.GetBool("Run") && Systems.Input.GetVector2("MoveAxis").y > 0;
            bool playerConditions = GetState("Walking");
            bool weaponConditions = !WeaponManager.IsAim();
            bool runningConditions = inputConditions && playerConditions && weaponConditions;
            SetState("Running", runningConditions);

            // Getting
            Rigidbody.useGravity = GetState("Graviting");
        }

        private void Move(Vector3 direction)
        {
            // Direction Ray
            Debug.DrawRay(transform.position, direction, Color.magenta);

            if (direction != Vector3.zero)
            {
                // Moving / Running
                float currentSpeed = !GetState("Running") ? Preset.walkingSpeed : Preset.runningSpeed;
                Rigidbody.AddForce(direction.normalized * currentSpeed * 10f, ForceMode.Force);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if ((Preset.walkableMask.value & (1 << other.transform.gameObject.layer)) > 0)
            {
                SetState("GroundCollision", true);
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if ((Preset.walkableMask.value & (1 << other.transform.gameObject.layer)) > 0)
            {
                SetState("GroundCollision", false);
            }
        }

        private void OnDrawGizmos()
        {
            // Ground check
            if (States != null)
            {
                if (CapsuleCollider != null)
                {
                    float radius = CapsuleCollider.radius + Preset.groundAreaRadius;
                    Gizmos.color = States.GetState("GroundArea") ? Color.green : Color.red;
                    Gizmos.DrawWireSphere(CapsuleBottom(), radius);

                    // Normal check
                    Gizmos.color = Color.magenta;
                    Gizmos.DrawRay(CapsuleBottom(), -transform.up * (CapsuleCollider.height / 2f) * 1.5f);
                }
                else
                {
                    CapsuleCollider = GetComponent<CapsuleCollider>();
                }
            }
            else
            {
                States = new CharacterState();
            }
        }
    }
}