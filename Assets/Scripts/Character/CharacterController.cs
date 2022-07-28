
using System.Collections.Generic;
using UnityEngine;
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
    public class CharacterController : MonoBehaviour
    {
        [SerializeField]
        private CharacterControllerPreset Preset;

        [Header("Roots")]
        [SerializeField] private Transform camRoot;
        [SerializeField] private Transform coverCamRoot;
        [SerializeField] private Transform coverWeaponRoot;

        [Header("Components")]
        [SerializeField] private Animator BasicAnimator;

        // Private
        private float jumpCountdown;
        private float initialCapsuleHeight;
        private Vector3 initialCoverCamPos;
        private Quaternion initialCoverRot;
        private Quaternion initialCoverCamRot;

        // Components
        private Rigidbody Rigidbody;
        private CapsuleCollider CapsuleCollider;
        private CharacterState States;

        public void UseGravity(bool value)
        {
            Rigidbody.useGravity = value;
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

        public Transform GetTransform()
        {
            return transform;
        }

        public Vector2 GetForwardXZ()
        {
            return new Vector2(transform.forward.x, transform.forward.z);
        }

        public float GetLocalYRotation()
        {
            return transform.localEulerAngles.y;
        }

        public bool GetState(string stateName)
        {
            return States.GetState(stateName);
        }

        public void SetState(string stateName, bool value)
        {
            States.SetState(stateName, value);
        }

        private void Awake()
        {
            // Create state
            if (States == null)
            {
                States = new CharacterState();
            }
        }

        private void Start()
        {
            // Setting starts states
            States.SetState("Graviting", true);

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
            if (States.GetState("GroundArea"))
            {
                Vector2 moveAxis = Systems.Input.GetVector2("MoveAxis");
                Vector3 dir1 = transform.forward * moveAxis.y + transform.right * moveAxis.x;
                Vector3 dir2 = Vector3.Cross(transform.right, GroundNormal()) * moveAxis.y + Vector3.Cross(-transform.forward, GroundNormal()) * moveAxis.x;
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
            bool jumpInput = Systems.Input.GetBool("Jump");
            bool conditions = jumpInput && States.GetState("GroundCollision") && jumpCountdown <= 0;

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
            BasicAnimator.SetBool("Air", !States.GetState("GroundArea"));

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
            bool crouchInput = Systems.Input.GetBool("Crouch");
            bool conditions = crouchInput && !States.GetState("Running");

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
            coverCamRoot.position = Vector3.Lerp(coverCamRoot.position, CapsuleTop(), Preset.crouchSpeed * Time.deltaTime);
        }

        private void CoverUpdate()
        {
            float coverInput = Systems.Input.GetFloat("Cover");
            bool conditions = !States.GetState("Running") && coverInput != 0;

            States.SetState("Covering", conditions);

            if (conditions)
            {
                Vector3 targetPos = new Vector3(Preset.coverAmount * Preset.coverCamScale * coverInput, 0f, 0f);
                Quaternion targetRot = Quaternion.Euler(new Vector3(0f, 0f, Preset.coverAmount * -coverInput));

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
            States.SetState("GroundArea", Physics.OverlapSphere(CapsuleBottom(), CapsuleCollider.radius + Preset.groundAreaRadius, Preset.walkableMask).Length > 0);
            States.SetState("Running", Systems.Input.GetBool("Run") && States.GetState("Walking"));
            States.SetState("Sloping", GetSlopeAngle() > 0 && GetSlopeAngle() <= Preset.maxAngleSlope);

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

                if (States.GetState("GroundArea"))
                {
                    BasicAnimator.SetBool("Walking", States.GetState("Walking"));
                    BasicAnimator.SetBool("Running", States.GetState("Running"));
                }
            }
            else
            {
                States.SetState("Walking", false);

                // Reset animations
                BasicAnimator.SetBool("Walking", false);
                BasicAnimator.SetBool("Running", false);
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
    }
}