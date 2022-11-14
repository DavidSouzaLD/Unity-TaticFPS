using UnityEngine;
using Game.Player;

namespace Game.Weapon
{
    public class WeaponSway : MonoBehaviour
    {
        [Header("Settings")]
        public Vector2 swayMultiplier = new Vector2(1f, 0.5f);
        public float swayAmount = 0.1f;
        public float swaySmooth = 0.1f;
        public float swayResetSpeed = 5f;
        public float swayHorizontalScale = 1f;
        [Space]
        public float swayAccuracy;

        [Header("Roots")]
        public Transform swayLocation;
        public Transform horizontalSwayLocation;

        // Private
        private Vector3 swayInitialPos;
        private Quaternion swayInitialRot;
        private Quaternion horSwayInitialRot;

        private void Start()
        {
            // Setting start values
            swayInitialPos = swayLocation.localPosition;
            swayInitialRot = swayLocation.localRotation;
            horSwayInitialRot = horizontalSwayLocation.localRotation;
        }

        private void Update()
        {
            Vector2 cameraInput = PlayerKeys.GetAxis("CameraAxis");
            Vector2 moveInput = PlayerKeys.GetAxis("MoveAxis");
            Vector2 cameraAxis = new Vector2(cameraInput.x * swayMultiplier.x, cameraInput.y * swayMultiplier.y) * swayAccuracy;
            float cameraAxisX = moveInput.x * swayMultiplier.x * swayAccuracy;

            if (cameraAxis != Vector2.zero)
            {
                swayLocation.localPosition = Vector3.Lerp(swayLocation.localPosition, new Vector3(-cameraAxis.x * swayAmount, -cameraAxis.y * swayAmount), swaySmooth * Time.deltaTime);
                swayLocation.localRotation = Quaternion.Slerp(swayLocation.localRotation, Quaternion.Euler(-cameraAxis.x * swayAmount, -cameraAxis.y * swayAmount, swayLocation.localRotation.z), swaySmooth * Time.deltaTime);
            }

            if (cameraAxisX != 0f)
            {
                horizontalSwayLocation.localRotation = Quaternion.Slerp(horizontalSwayLocation.localRotation, Quaternion.Euler(horizontalSwayLocation.localRotation.x, horizontalSwayLocation.localRotation.y, -cameraAxisX * swayAmount * swayHorizontalScale), swaySmooth * swayHorizontalScale * Time.deltaTime);
            }

            if (horizontalSwayLocation.localRotation != horSwayInitialRot || swayAccuracy != 1f)
            {
                horizontalSwayLocation.localRotation = Quaternion.Slerp(horizontalSwayLocation.localRotation, horSwayInitialRot, swayResetSpeed * Time.deltaTime);
            }

            if (swayLocation.localPosition != swayInitialPos || swayLocation.localRotation != swayInitialRot || swayAccuracy != 1f)
            {
                swayLocation.localPosition = Vector3.Lerp(swayLocation.localPosition, swayInitialPos, swayResetSpeed * Time.deltaTime);
                swayLocation.localRotation = Quaternion.Slerp(swayLocation.localRotation, swayInitialRot, swayResetSpeed * Time.deltaTime);
            }
        }
    }
}