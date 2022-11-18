using UnityEngine;

namespace Code.Player
{
    [CreateAssetMenu(fileName = "ControllerData", menuName = "Player/ControllerData", order = 0)]
    public class PlayerControllerData : ScriptableObject
    {
        [Header("Camera")]
        public float horizontalSensitivity = 1.5f;
        public float verticalSensitivity = 1.5f;
        public float smoothness = 20f;
        [Space]
        public Vector2 verticalAngleLimit = new Vector2(-90f, 90f);
        public Vector2 freeLookVerticalLimit = new Vector2(-70, 20);
        public Vector2 freeLookHorizontalLimit = new Vector2(-45, 45);

        [Header("Movement")]
        public LayerMask walkingMask;
        public float walkingAcceleration = 2f;
        public float runningAcceleration = 4f;
        public float crouchingAcceleration = 1f;

        [Header("Jump/Gravity")]
        public float jumpingForce = 1f;
        public float jumpRate = 0.3f;
        public float gravityScale = 2f;

        [Header("Crouch")]
        public float crouchSpeed = 5f;
        [Space]
        public float standardHeight = 2f;
        public float standardCamHeight = 1.6f;
        public float crouchHeight = 1f;
        public float crouchCamHeight = 1f;

        [Header("Lean")]
        public float leanAmount = 10f;
        public float leanCamScale = 0.02f;
        public float leanSpeed = 6f;

        [Header("Audio")]
        public float walkStepSpeed = 0.5f;
        public float runStepSpeed = 0.2f;
        public float crouchStepSpeed = 1.5f;
        [Space]
        public float footstepVolume = 0.5f;
    }
}