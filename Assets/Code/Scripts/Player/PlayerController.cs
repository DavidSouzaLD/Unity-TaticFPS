using UnityEngine;
using Code.Systems.InputSystem;
using Code.Extensions;

namespace Code.Player
{
    [RequireComponent(typeof(PlayerCamera))]
    [RequireComponent(typeof(PlayerLean))]
    [RequireComponent(typeof(PlayerCompass))]
    [RequireComponent(typeof(PlayerFootsteps))]
    [RequireComponent(typeof(PlayerHeadbob))]
    public class PlayerController : MonoBehaviour
    {
        public PlayerControllerData data;

        // Jump
        private Vector3 verticalVelocity;
        private float jumpTimer;
        private bool jumpRequested;

        // Crouch
        private float initialCamHeight;

        public static bool isWalking { get; private set; }
        public static bool isRunning { get; private set; }
        public static bool isCrouching { get; private set; }
        public static bool isJumping { get; private set; }
        public static bool isGrounded { get; private set; }
        public static bool isLeaning { get; private set; }
        public static bool isRetracting { get; private set; }

        public CharacterController characterController { get; private set; }
        public PlayerCamera playerCamera { get; private set; }
        public PlayerLean playerLean { get; private set; }
        public PlayerCompass playerCompass { get; private set; }
        public PlayerFootsteps playerFootsteps { get; private set; }
        public PlayerHeadbob playerHeadbob { get; private set; }

        public static string currentGroundTag { get; private set; }

        public float currentAcceleration
        {
            get
            {
                if (isCrouching)
                {
                    return data.crouchingAcceleration;
                }

                if (isRunning)
                {
                    return data.runningAcceleration;
                }

                if (isWalking)
                {
                    return data.walkingAcceleration;
                }

                return 0;
            }
        }

        public Vector3 direction
        {
            get
            {
                Vector2 moveAxis = InputSystem.GetAxisRaw("MoveAxis");
                return transform.forward * moveAxis.y + transform.right * moveAxis.x;
            }
        }

        public Vector2 forwardXZ
        {
            get
            {
                return new Vector2(transform.forward.x, transform.forward.z);
            }
        }

        public float localYRotation
        {
            get
            {
                return transform.localEulerAngles.y;
            }
        }

        private void OnValidate()
        {
            FixControllerCenter();
        }

        private void Start()
        {
            GetComponents();
            InitVariables();
        }

        private void GetComponents()
        {
            characterController = GetComponent<CharacterController>();
            playerCamera = GetComponent<PlayerCamera>();
            playerLean = GetComponent<PlayerLean>();
            playerCompass = GetComponent<PlayerCompass>();
            playerFootsteps = GetComponent<PlayerFootsteps>();
            playerHeadbob = GetComponent<PlayerHeadbob>();
        }

        private void InitVariables()
        {
            // Set components
            playerCamera.SetPlayerController(this);
            playerLean.SetPlayerController(this);
            playerCompass.SetPlayerController(this);
            playerFootsteps.SetPlayerController(this);
            playerHeadbob.SetPlayerController(this);

            // Init variables
            jumpTimer = data.jumpRate;
            initialCamHeight = playerCamera.standardCam.transform.localPosition.y;
        }

        private void Update()
        {
            // Jump
            bool jump = InputSystem.OnClick("Jump") && isGrounded && jumpTimer <= 0;

            if (jump)
            {
                jumpRequested = true;
            }

            if (jumpTimer > 0)
            {
                jumpTimer -= Time.deltaTime;
            }

            if (isJumping && isGrounded)
            {
                isJumping = false;
            }

            // Crouch
            isCrouching = InputSystem.OnPressing("Crouch") && !isRunning;
            float crouchSpeed = data.crouchSpeed * Time.deltaTime;
            Transform camRoot = playerCamera.standardCam.transform;

            if (isCrouching)
            {
                SetControllerHeight(data.crouchHeight, crouchSpeed);
                SetCameraHeight(data.crouchCamHeight, crouchSpeed);
            }
            else
            {
                SetControllerHeight(data.standardHeight, crouchSpeed);
                SetCameraHeight(data.standardCamHeight, crouchSpeed);
            }

            // Lean
            isLeaning = playerLean.isLeaning;
        }

        private void FixedUpdate()
        {
            // Reset vertical velocity
            if (isGrounded && verticalVelocity.y < 0)
            {
                verticalVelocity.y = 0f;
            }

            // Set status
            isWalking = InputSystem.GetAxisRaw("MoveAxis") != Vector2.zero;
            isRunning = InputSystem.OnPressing("Run") && isWalking && InputSystem.GetAxisRaw("MoveAxis").y > 0;
            isGrounded = characterController.isGrounded;

            // Movement
            Vector3 currentVelocity = direction.normalized * currentAcceleration * Time.deltaTime;
            characterController.Move(currentVelocity);

            // Jump
            if (jumpRequested)
            {
                verticalVelocity.y += Mathf.Sqrt(data.jumpingForce * -3f * (Physics.gravity.y * data.gravityScale));

                isJumping = true;
                jumpTimer = data.jumpRate;
                jumpRequested = false;
            }

            // Gravity
            verticalVelocity.y += Physics.gravity.y * data.gravityScale * Time.deltaTime;

            // Apply vertical velocity
            characterController.Move(verticalVelocity * Time.deltaTime);
        }

        private void FixControllerCenter()
        {
            if (data == null && Application.isPlaying) return;

            if (characterController == null)
            {
                characterController = GetComponent<CharacterController>();
            }

            if (characterController != null)
            {
                characterController.center = new Vector3(
                    characterController.center.x,
                    data.standardHeight / 2f,
                    characterController.center.z
                );
            }
        }

        private void SetControllerHeight(float height, float speed)
        {
            characterController.LerpHeight(height, speed);
        }

        private void SetCameraHeight(float height, float speed)
        {
            Transform camRoot = playerCamera.standardCam.transform;
            float lerpHeight = Mathf.Lerp(camRoot.localPosition.y, height, speed);

            camRoot.localPosition = new Vector3(
                    camRoot.localPosition.x,
                    lerpHeight,
                    camRoot.localPosition.z
                );
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            currentGroundTag = hit.transform.tag;
        }
    }
}