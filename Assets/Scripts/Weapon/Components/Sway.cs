using UnityEngine;
using Game.Player;

namespace Game.Weapon
{
    public class Sway : Singleton<Sway>
    {
        [Header("Settings")]
        public Vector2 swayMultiplier = new Vector2(1f, 0.5f);
        public float swayAmount = 0.1f;
        public float swaySmooth = 0.1f;
        public float swayResetSpeed = 5f;
        public float swayHorizontalScale = 1f;

        [Header("Transforms")]
        public Transform swayTransform;
        public Transform horizontalSwayTransform;

        private Vector3 swayInitialPos;
        private Quaternion swayInitialRot;
        private Quaternion horSwayInitialRot;
        public static float swayScale = 1f;

        private void Start()
        {
            // Setting start values
            swayScale = 1f;
            swayInitialPos = swayTransform.localPosition;
            swayInitialRot = swayTransform.localRotation;
            horSwayInitialRot = horizontalSwayTransform.localRotation;
        }

        private void Update()
        {
            // Clamp SwayScale
            swayScale = Mathf.Clamp(swayScale, 0f, 1f);

            // Apply sway
            Vector2 cameraInput = PlayerKeys.GetAxis("CameraAxis");
            Vector2 moveInput = PlayerKeys.GetAxis("MoveAxis");
            Vector2 cameraAxis = new Vector2(cameraInput.x * swayMultiplier.x, cameraInput.y * swayMultiplier.y) * swayScale;
            float cameraAxisX = moveInput.x * swayMultiplier.x * swayScale;

            if (cameraAxis != Vector2.zero)
            {
                swayTransform.localPosition = Vector3.Lerp(swayTransform.localPosition, new Vector3(-cameraAxis.x * swayAmount, -cameraAxis.y * swayAmount), swaySmooth * Time.deltaTime);
                swayTransform.localRotation = Quaternion.Slerp(swayTransform.localRotation, Quaternion.Euler(-cameraAxis.x * swayAmount, -cameraAxis.y * swayAmount, swayTransform.localRotation.z), swaySmooth * Time.deltaTime);
            }

            if (cameraAxisX != 0f)
            {
                horizontalSwayTransform.localRotation = Quaternion.Slerp(horizontalSwayTransform.localRotation, Quaternion.Euler(horizontalSwayTransform.localRotation.x, horizontalSwayTransform.localRotation.y, -cameraAxisX * swayAmount * swayHorizontalScale), swaySmooth * swayHorizontalScale * Time.deltaTime);
            }

            if (horizontalSwayTransform.localRotation != horSwayInitialRot || swayScale != 1f)
            {
                horizontalSwayTransform.localRotation = Quaternion.Slerp(horizontalSwayTransform.localRotation, horSwayInitialRot, swayResetSpeed * Time.deltaTime);
            }

            if (swayTransform.localPosition != swayInitialPos || swayTransform.localRotation != swayInitialRot || swayScale != 1f)
            {
                swayTransform.localPosition = Vector3.Lerp(swayTransform.localPosition, swayInitialPos, swayResetSpeed * Time.deltaTime);
                swayTransform.localRotation = Quaternion.Slerp(swayTransform.localRotation, swayInitialRot, swayResetSpeed * Time.deltaTime);
            }
        }
    }
}