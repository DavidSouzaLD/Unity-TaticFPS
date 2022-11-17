using UnityEngine;

namespace Game.Player
{
    public partial class PlayerController : Singleton<PlayerController>
    {
        [Header("Movement")]
        public LayerMask walkableMask;
        public float walkingSpeed = 2f;
        public float runningSpeed = 5f;
        public float crouchingSpeed = 1f;
        [Space]
        public float jumpingForce = 50f;
        public float gravityScale = 2f;

        [Header("Crouch")]
        public float standHeight = 2f;
        public float crouchHeight = 1.5f;
        public float speedToCrouch = 5f;

        [Header("Roots")]
        public Transform camRoot;

        public static bool isWalking { get; private set; }
        public static bool isRunning { get; private set; }
        public static bool isCrouching { get; private set; }
        public static bool isJumping { get; private set; }
        public static bool isGrounded { get; private set; }

        public static string currentGroundTag { get; private set; }
        private Vector3 currentVelocity;
        private bool jumpRequested;
        private float jumpTimer;
        private float initialControllerHeight;
        private float initialCamHeight;
        private Transform cameraTransform;
        private CharacterController controller;

        private Vector2 InputAxis
        {
            get
            {
                return InputManager.GetAxisRaw("MoveAxis").normalized;
            }
        }

        private Vector3 Direction
        {
            get
            {
                return transform.forward * InputAxis.y + transform.right * InputAxis.x;
            }
        }

        private float CurrentSpeed
        {
            get
            {
                if (isCrouching)
                {
                    return crouchingSpeed;
                }

                if (isWalking && !isRunning)
                {
                    return walkingSpeed;
                }

                if (isRunning)
                {
                    return runningSpeed;
                }

                return 0;
            }
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

        private void Start()
        {
            // Components
            controller = GetComponent<CharacterController>();

            // Jump
            jumpTimer = 0.3f;
        }

        private void FixedUpdate()
        {
            UpdateMove();
        }

        private void Update()
        {
            UpdateCrouch();
            UpdateJump();
        }

        private void UpdateMove()
        {
            isGrounded = controller.isGrounded;

            // Moving
            if (isGrounded && currentVelocity.y < 0)
            {
                currentVelocity.y = 0f;
            }

            isWalking = InputAxis != Vector2.zero;
            isRunning = isWalking && InputAxis.y > 0 && InputManager.Press("Run");

            controller.Move(Direction.normalized * CurrentSpeed * Time.deltaTime);

            // Jump
            if (jumpRequested)
            {
                currentVelocity.y += Mathf.Sqrt(jumpingForce * -3f * (Physics.gravity.y * gravityScale));

                isJumping = true;
                jumpTimer = 0.3f;
                jumpRequested = false;
            }

            // Gravity
            currentVelocity.y += Physics.gravity.y * gravityScale * Time.deltaTime;

            controller.Move(currentVelocity * Time.deltaTime);
        }

        private void UpdateJump()
        {
            bool conditionsToJump =
            InputManager.Click("Jump") && isGrounded && jumpTimer <= 0;

            if (conditionsToJump)
            {
                jumpRequested = true;
            }

            if (jumpTimer > 0)
            {
                jumpTimer -= Time.deltaTime;
            }

            if (jumpTimer <= 0 || isGrounded)
            {
                isJumping = false;
            }
        }

        private void UpdateCrouch()
        {
            bool conditionsToCrouch =
            InputManager.Press("Crouch") && !InputManager.Press("Run");

            if (conditionsToCrouch)
            {
                controller.LerpHeight(crouchHeight, speedToCrouch * Time.deltaTime);
                camRoot.transform.localPosition = new Vector3(
                    camRoot.transform.localPosition.x,
                    Mathf.Lerp(camRoot.transform.localPosition.y, crouchHeight / 2f, speedToCrouch * Time.deltaTime),
                camRoot.transform.localPosition.z
                );

                isCrouching = true;
            }
            else
            {
                controller.LerpHeight(standHeight, speedToCrouch * Time.deltaTime);
                camRoot.transform.localPosition = new Vector3(
                    camRoot.transform.localPosition.x,
                    Mathf.Lerp(camRoot.transform.localPosition.y, standHeight / 2f, speedToCrouch * Time.deltaTime),
                camRoot.transform.localPosition.z
                );

                isCrouching = false;
            }
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            currentGroundTag = hit.transform.tag;
        }
    }
}