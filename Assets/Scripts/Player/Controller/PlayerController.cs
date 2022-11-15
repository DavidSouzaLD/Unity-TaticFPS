using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] private Transform camRoot;

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
                return PlayerKeys.GetAxisRaw("MoveAxis").normalized;
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
            UpdateMoveAndJump();
        }

        private void Update()
        {
            UpdateCrouch();
            UpdateJump();
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            currentGroundTag = hit.transform.tag;
        }
    }
}