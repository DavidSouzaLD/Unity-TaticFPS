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
        private string currentGroundTag;
        private float jumpTimer;
        private float footstepTimer;
        private float initialControllerHeight;
        private float initialCamHeight;
        private Vector3 currentVelocity;
        private Vector3 initialCoverCamPos;
        private Quaternion initialCoverRot;
        private Quaternion initialCoverCamRot;
        Transform cameraTransform;
        private CharacterController Controller;
        private CharacterState States;

        public void UseGravity(bool _value)
        {
            SetState("Graviting", _value);
        }

        public float GetSlopeAngle()
        {
            return Vector3.Angle(transform.up, GetGroundNormal());
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

        public Vector3 GetGroundNormal()
        {
            Vector3 position = Controller.GetBottomCenterPosition();
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
            jumpTimer = 0.3f;

            // Cover
            initialCoverRot = coverWeaponRoot.localRotation;
            initialCoverCamPos = coverCamRoot.localPosition;
            initialCoverCamRot = coverCamRoot.localRotation;

            // Others
            initialControllerHeight = Controller.height;
            initialCamHeight = camRoot.localPosition.y;
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
            if (Controller.isGrounded && currentVelocity.y < 0)
            {
                currentVelocity.y = 0f;
            }

            Vector2 moveInput = Systems.Input.GetVector2("MoveAxis");

            if (moveInput != Vector2.zero)
            {
                float currentSpeed = !GetState("Crouching") ? (!GetState("Running") ? Preset.walkingSpeed : Preset.runningSpeed) : Preset.crouchingSpeed;
                Vector3 dir1 = transform.forward * moveInput.y + transform.right * moveInput.x;
                Vector3 dir2 = Vector3.Cross(transform.right, GetGroundNormal()) * moveInput.y + Vector3.Cross(-transform.forward, GetGroundNormal()) * moveInput.x;
                Vector3 direction = (!GetState("Sloping") ? dir1 : dir2);

                Controller.Move(direction.normalized * currentSpeed * Time.deltaTime);

                SetState("Walking", true);
            }
            else
            {
                SetState("Walking", false);
            }

            // Jump
            bool inputJumpConditions = Systems.Input.GetBool("Jump");
            bool stateJumpConditions = GetState("GroundCollision");
            bool differenceJumpConditions = jumpTimer <= 0;
            bool jumpConditions = inputJumpConditions && stateJumpConditions && differenceJumpConditions;

            if (jumpConditions)
            {
                SetState("Jumping", true);
                currentVelocity.y += Mathf.Sqrt(Preset.jumpingForce * -3f * (Physics.gravity.y * Preset.gravityScale));
                jumpTimer = 0.3f;
            }

            // Gravity
            if (GetState("Graviting"))
            {
                currentVelocity.y += Physics.gravity.y * Preset.gravityScale * Time.deltaTime;
            }

            Controller.Move(currentVelocity * Time.deltaTime);
        }

        private void JumpUpdate()
        {
            if (jumpTimer > 0)
            {
                jumpTimer -= Time.deltaTime;
            }

            if (jumpTimer <= 0)
            {
                SetState("Jumping", false);
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
                Controller.LerpHeight(Preset.crouchHeight, Preset.speedToCrouch * Time.deltaTime);
                camRoot.transform.localPosition = new Vector3(
                    camRoot.transform.localPosition.x,
                    Mathf.Lerp(camRoot.transform.localPosition.y, Preset.crouchHeight / 2f, Preset.speedToCrouch * Time.deltaTime),
                camRoot.transform.localPosition.z
                );
            }
            else
            {
                Controller.LerpHeight(Preset.standHeight, Preset.speedToCrouch * Time.deltaTime);
                camRoot.transform.localPosition = new Vector3(
                    camRoot.transform.localPosition.x,
                    Mathf.Lerp(camRoot.transform.localPosition.y, Preset.standHeight / 2f, Preset.speedToCrouch * Time.deltaTime),
                camRoot.transform.localPosition.z
                );
            }
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
            Vector3 position = Controller.GetBottomPosition();
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
            SetState("GroundArea", Physics.OverlapSphere(Controller.GetBottomCenterPosition(), Controller.radius + Preset.groundAreaRadius, Preset.walkableMask).Length > 0);
            SetState("Sloping", GetSlopeAngle() > 0 && GetSlopeAngle() <= Controller.slopeLimit);

            // Running
            bool inputConditions = Systems.Input.GetBool("Run") && Systems.Input.GetVector2("MoveAxis").y > 0;
            bool playerConditions = GetState("Walking");
            bool weaponConditions = !WeaponManager.IsAim();
            bool runningConditions = inputConditions && playerConditions && weaponConditions;
            SetState("Running", runningConditions);
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
                    Vector3 positionCheck = Controller.GetBottomPosition();
                    float radiusCheck = Controller.radius * Preset.groundRadius;
                    Gizmos.DrawWireSphere(positionCheck, radiusCheck);

                    // Ground area
                    Gizmos.color = States.GetState("GroundArea") ? Color.green : Color.blue;
                    float radiusArea = Controller.radius + Preset.groundAreaRadius;
                    Gizmos.DrawWireSphere(Controller.GetBottomCenterPosition(), radiusArea);
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