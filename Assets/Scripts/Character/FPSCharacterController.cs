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
    [RequireComponent(typeof(CharacterController))]
    public class FPSCharacterController : Singleton<FPSCharacterController>
    {
        [SerializeField]
        private CharacterControllerPreset Preset;

        [Header("Roots")]
        [SerializeField] private Transform camRoot;
        [SerializeField] private Transform coverWeaponRoot;
        [SerializeField] private Transform coverCamRoot;

        // Private
        private bool useGravity;
        private string currentGroundTag;
        private float jumpTimer;
        private float footstepTimer;
        private float initialControllerHeight;
        private Vector3 currentVelocity;
        private Vector3 initialCoverCamPos;
        private Quaternion initialCoverRot;
        private Quaternion initialCoverCamRot;
        Transform cameraTransform;
        private CharacterController Controller;
        private CharacterState States;

        public void UseGravity(bool _value)
        {
            useGravity = _value;
        }

        public Vector3 ControllerCenterTop()
        {
            Vector3 position = ControllerTop();
            position.y -= Controller.radius;
            return position;
        }

        public Vector3 ControllerCenterBottom()
        {
            Vector3 position = ControllerBottom();
            position.y += Controller.radius;
            return position;
        }

        public Vector3 ControllerTop()
        {
            Vector3 position = (transform.position + Controller.center) + new Vector3(0f, (Controller.height / 2f), 0f);
            return position;
        }

        public Vector3 ControllerBottom()
        {
            Vector3 position = (transform.position + Controller.center) - new Vector3(0f, (Controller.height / 2f), 0f);
            return position;
        }

        public Vector3 GroundNormal()
        {
            Vector3 position = ControllerCenterBottom();
            Vector3 direction = -transform.up;
            float distance = (Controller.height / 2f) * 1.5f;
            RaycastHit hit = RaycastExtension.RaycastWithMask(position, direction, distance, Preset.walkableMask);

            if (hit.collider != null)
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
            Controller = GetComponent<CharacterController>();

            // Jump
            jumpTimer = 1f;

            // Cover
            initialCoverRot = coverWeaponRoot.localRotation;
            initialCoverCamPos = coverCamRoot.localPosition;
            initialCoverCamRot = coverCamRoot.localRotation;

            // Others
            initialControllerHeight = Controller.height;
        }

        private void Update()
        {
            JumpUpdate();
            CrouchUpdate();
            CoverUpdate();
            FootstepUpdate();
            GroundCheckUpdate();
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
                    float currentSpeed = !GetState("Running") ? Preset.walkingSpeed : Preset.runningSpeed;
                    Vector3 dir1 = transform.forward * moveInput.y + transform.right * moveInput.x;
                    Vector3 dir2 = Vector3.Cross(transform.right, GroundNormal()) * moveInput.y + Vector3.Cross(-transform.forward, GroundNormal()) * moveInput.x;
                    Vector3 direction = (!GetState("Sloping") ? dir1 : dir2);

                    currentVelocity += direction.normalized * currentSpeed * Time.deltaTime;

                    SetState("Walking", true);
                }
            }
            else
            {
                SetState("Walking", false);
            }

            // Additional gravity
            if (useGravity)
            {
                currentVelocity += transform.up * Physics.gravity.y * Preset.gravityScale * Time.deltaTime;
            }

            Move(ref currentVelocity);
        }

        private void JumpUpdate()
        {
            // Jump
            bool inputJumpConditions = Systems.Input.GetBool("Jump");
            bool stateJumpConditions = GetState("GroundCollision");
            bool differenceJumpConditions = jumpTimer <= 0;
            bool jumpConditions = inputJumpConditions && stateJumpConditions && differenceJumpConditions;

            if (jumpConditions)
            {
                SetState("Jumping", true);
                currentVelocity += transform.up * Preset.jumpingForce;
                jumpTimer = 1f;
            }

            if (jumpTimer > 0)
            {
                jumpTimer -= Time.deltaTime;
            }

            if (jumpTimer <= 0)
            {
                SetState("Jumping", false);
            }

            Move(ref currentVelocity);
        }

        private void CrouchUpdate()
        {
            bool inputConditions = Systems.Input.GetBool("Crouch");
            bool stateConditions = !GetState("Running");
            bool conditions = inputConditions && stateConditions;

            SetState("Crouching", conditions);

            if (conditions)
            {
                Controller.height = Mathf.Lerp(Controller.height, Preset.crouchHeight, Preset.crouchSpeed * Time.deltaTime);
            }
            else
            {
                Controller.height = Mathf.Lerp(Controller.height, initialControllerHeight, Preset.crouchSpeed * Time.deltaTime);
            }

            // Set camera in top Controller
            coverCamRoot.position = Vector3.Lerp(coverCamRoot.position, ControllerCenterTop(), Preset.crouchSpeed * Time.deltaTime);
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

        private void FootstepUpdate()
        {
            bool conditions = GetState("GroundCollision") && GetState("Walking");
            footstepTimer -= Time.deltaTime;

            if (conditions)
            {
                float footstepSpeed = !GetState("Crouching") ? (!GetState("Running") ? (GetState("Walking") ? Preset.baseStepSpeed : 0) : Preset.runStepSpeed) : Preset.crouchStepSpeed;

                if (footstepTimer <= 0)
                {
                    AudioClip[] clips = Preset.GetFootstepsWithTag(currentGroundTag).clips;

                    if (clips.Length > 0)
                    {
                        Systems.Audio.PlaySound(clips[Random.Range(0, clips.Length)], Preset.footstepVolume);
                    }

                    footstepTimer = footstepSpeed;
                }
            }
        }

        private void GroundCheckUpdate()
        {
            Vector3 position = ControllerBottom();
            float radius = Controller.radius * Preset.groundRadius;
            LayerMask mask = Preset.walkableMask;

            Collider[] colliders = Physics.OverlapSphere(position, radius, mask);

            if (colliders.Length > 0)
            {
                SetState("GroundCollision", true);
                currentGroundTag = colliders[0].transform.tag;
            }
            else
            {
                SetState("GroundCollision", false);
            }
        }

        private void StateUpdate()
        {
            // Setting
            SetState("GroundArea", Physics.OverlapSphere(ControllerCenterBottom(), Controller.radius + Preset.groundAreaRadius, Preset.walkableMask).Length > 0);
            SetState("Sloping", GetSlopeAngle() > 0 && GetSlopeAngle() <= Controller.slopeLimit);

            // Running
            bool inputConditions = Systems.Input.GetBool("Run") && Systems.Input.GetVector2("MoveAxis").y > 0;
            bool playerConditions = GetState("Walking");
            bool weaponConditions = !WeaponManager.IsAim();
            bool runningConditions = inputConditions && playerConditions && weaponConditions;
            SetState("Running", runningConditions);

            // Getting
            useGravity = GetState("Graviting");
        }

        private void Move(ref Vector3 _currentVelocity)
        {
            Controller.Move(_currentVelocity);
            _currentVelocity = Vector3.zero;
        }

        private void OnDrawGizmos()
        {
            // Ground check
            if (States != null)
            {
                if (Controller != null)
                {
                    // Ground check
                    Gizmos.color = Color.blue;
                    Vector3 positionCheck = ControllerBottom();
                    float radiusCheck = Controller.radius * Preset.groundRadius;
                    Gizmos.DrawWireSphere(positionCheck, radiusCheck);

                    // Ground area
                    Gizmos.color = States.GetState("GroundArea") ? Color.green : Color.blue;
                    float radiusArea = Controller.radius + Preset.groundAreaRadius;
                    Gizmos.DrawWireSphere(ControllerCenterBottom(), radiusArea);
                }
                else
                {
                    Controller = GetComponent<CharacterController>();
                }
            }
            else
            {
                States = new CharacterState();
            }
        }
    }
}