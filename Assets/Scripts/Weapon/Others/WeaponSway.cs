using UnityEngine;

namespace Game.Weapons
{
    public class WeaponSway : Singleton<WeaponSway>
    {
        [Header("Settings")]
        [SerializeField] private Vector2 swayMultiplier = new Vector2(1f, 0.5f);
        [SerializeField] private float swayAmount = 0.1f;
        [SerializeField] private float swaySmooth = 0.1f;
        [SerializeField] private float swayResetSpeed = 5f;
        [SerializeField] private float swayHorizontalScale = 1f;

        [Header("Roots")]
        [SerializeField] private Transform swayRoot;
        [SerializeField] private Transform horizontalSwayRoot;

        // Private
        private float swayAccuracy;
        private Vector3 swayInitialPos;
        private Quaternion swayInitialRot;
        private Quaternion horSwayInitialRot;

        public static void SwayAccuracy(float value = 0f)
        {
            Instance.swayAccuracy = Mathf.Clamp(value, 0f, 1f);
        }

        public static void MaxAccuracy()
        {
            SwayAccuracy(1f);
        }

        private void Start()
        {
            // Setting start values
            swayInitialPos = swayRoot.localPosition;
            swayInitialRot = swayRoot.localRotation;
            horSwayInitialRot = horizontalSwayRoot.localRotation;

            MaxAccuracy();
        }

        private void Update()
        {
            Vector2 cameraInput = Systems.Input.GetVector2("CameraAxis");
            Vector2 moveInput = Systems.Input.GetVector2("MoveAxis");
            Vector2 cameraAxis = new Vector2(cameraInput.x * swayMultiplier.x, cameraInput.y * swayMultiplier.y) * swayAccuracy;
            float cameraAxisX = moveInput.x * swayMultiplier.x * swayAccuracy;

            if (cameraAxis != Vector2.zero)
            {
                swayRoot.localPosition = Vector3.Lerp(swayRoot.localPosition, new Vector3(-cameraAxis.x * swayAmount, -cameraAxis.y * swayAmount), swaySmooth * Time.deltaTime);
                swayRoot.localRotation = Quaternion.Slerp(swayRoot.localRotation, Quaternion.Euler(-cameraAxis.x * swayAmount, -cameraAxis.y * swayAmount, swayRoot.localRotation.z), swaySmooth * Time.deltaTime);
            }

            if (cameraAxisX != 0f)
            {
                horizontalSwayRoot.localRotation = Quaternion.Slerp(horizontalSwayRoot.localRotation, Quaternion.Euler(horizontalSwayRoot.localRotation.x, horizontalSwayRoot.localRotation.y, -cameraAxisX * swayAmount * swayHorizontalScale), swaySmooth * swayHorizontalScale * Time.deltaTime);
            }

            if (horizontalSwayRoot.localRotation != horSwayInitialRot || swayAccuracy != 1f)
            {
                horizontalSwayRoot.localRotation = Quaternion.Slerp(horizontalSwayRoot.localRotation, horSwayInitialRot, swayResetSpeed * Time.deltaTime);
            }

            if (swayRoot.localPosition != swayInitialPos || swayRoot.localRotation != swayInitialRot || swayAccuracy != 1f)
            {
                swayRoot.localPosition = Vector3.Lerp(swayRoot.localPosition, swayInitialPos, swayResetSpeed * Time.deltaTime);
                swayRoot.localRotation = Quaternion.Slerp(swayRoot.localRotation, swayInitialRot, swayResetSpeed * Time.deltaTime);
            }
        }
    }
}