using UnityEngine;

namespace WeaponSystem.Components
{
    public class Sway : WeaponComponent
    {
        [Header("Settings")]
        public Vector2 swayMultiplier = new Vector2(1f, 0.5f);
        public float swayAmount = 0.1f;
        public float swaySmooth = 0.1f;
        public float swayResetSpeed = 5f;

        [Header("Transforms")]
        public Transform swayTransform;
        public Transform horSwayTransform;

        public float defaultSwayHorScale { get; set; }
        public float swayScale { get; set; }
        public float swayHorScale { get; set; }

        private Vector3 swayInitialPos;
        private Quaternion swayInitialRot;
        private Quaternion horSwayInitialRot;

        public override void Start()
        {
            // Setting start values
            swayScale = 1f;
            swayHorScale = 8f;
            defaultSwayHorScale = swayHorScale;
            swayInitialPos = swayTransform.localPosition;
            swayInitialRot = swayTransform.localRotation;
            horSwayInitialRot = horSwayTransform.localRotation;
        }

        public override void Update()
        {
            // Clamp SwayScale
            swayScale = Mathf.Clamp(swayScale, 0f, 1f);

            // Apply sway
            Vector2 cameraInput = InputManager.GetAxis("CameraAxis");
            Vector2 moveInput = InputManager.GetAxis("MoveAxis");
            Vector2 cameraAxis = new Vector2(cameraInput.x * swayMultiplier.x, cameraInput.y * swayMultiplier.y) * swayScale;
            float cameraAxisX = moveInput.x * swayMultiplier.x * swayScale;

            if (cameraAxis != Vector2.zero)
            {
                swayTransform.localPosition = Vector3.Lerp(swayTransform.localPosition, new Vector3(-cameraAxis.x * swayAmount, -cameraAxis.y * swayAmount), swaySmooth * Time.deltaTime);
                swayTransform.localRotation = Quaternion.Slerp(swayTransform.localRotation, Quaternion.Euler(-cameraAxis.x * swayAmount, -cameraAxis.y * swayAmount, swayTransform.localRotation.z), swaySmooth * Time.deltaTime);
            }

            if (cameraAxisX != 0f)
            {
                horSwayTransform.localRotation = Quaternion.Slerp(horSwayTransform.localRotation, Quaternion.Euler(horSwayTransform.localRotation.x, horSwayTransform.localRotation.y, -cameraAxisX * swayAmount * swayHorScale), swaySmooth * swayHorScale * Time.deltaTime);
            }

            if (horSwayTransform.localRotation != horSwayInitialRot || swayScale != 1f)
            {
                horSwayTransform.localRotation = Quaternion.Slerp(horSwayTransform.localRotation, horSwayInitialRot, swayResetSpeed * Time.deltaTime);
            }

            if (swayTransform.localPosition != swayInitialPos || swayTransform.localRotation != swayInitialRot || swayScale != 1f)
            {
                swayTransform.localPosition = Vector3.Lerp(swayTransform.localPosition, swayInitialPos, swayResetSpeed * Time.deltaTime);
                swayTransform.localRotation = Quaternion.Slerp(swayTransform.localRotation, swayInitialRot, swayResetSpeed * Time.deltaTime);
            }
        }
    }
}