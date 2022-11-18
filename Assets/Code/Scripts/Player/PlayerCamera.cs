using UnityEngine;
using Code.Systems.InputSystem;
using Code.Interfaces;

namespace Code.Player
{
    public class PlayerCamera : MonoBehaviour, IPlayerControllerComponent
    {
        [Header("Cameras")]
        public Camera standardCam;
        public Camera freeLookCam;

        [Header("Roots")]
        [SerializeField] private Transform weaponMaskRoot;

        private bool clickFreeLook;
        private float mainRotateX;
        private float mainRotateY;
        private float freeRotateX;
        private float freeRotateY;
        private const float resetSpeed = 5f;
        private Quaternion camTargetRot;
        private Quaternion characerTargetRot;
        private Camera currentCam;

        public PlayerController playerController { get; set; }
        public bool isCursorLocked { get; private set; }
        public float sensitivityScale { get; set; }

        public Vector3 WorldToScreen(Vector3 _position)
        {
            return currentCam.WorldToScreenPoint(_position);
        }

        public void SetPlayerController(PlayerController playerController)
        {
            this.playerController = playerController;
        }

        private void Start()
        {
            InitVariables();
        }

        private void InitVariables()
        {
            SetCursorLock(true);
            currentCam = standardCam;
            sensitivityScale = 1f;
            clickFreeLook = false;
        }

        private void Update()
        {
            if (currentCam == null || playerController == null) return;

            Vector2 camAxis = InputSystem.GetAxis("CameraAxis");
            bool isFreeLook = InputSystem.OnPressing("FreeLook");

            if (isFreeLook)
            {
                if (!clickFreeLook)
                {
                    // Reseting
                    freeRotateX = 0f;
                    freeRotateY = 0f;
                    currentCam = freeLookCam;
                    standardCam.enabled = false;
                    freeLookCam.enabled = true;
                    clickFreeLook = true;

                    freeLookCam.transform.rotation = standardCam.transform.rotation;

                    // Set mask cam rotation
                    weaponMaskRoot.parent = freeLookCam.transform;
                    weaponMaskRoot.localEulerAngles = Vector3.zero;
                }

                freeRotateX += (-camAxis.y * playerController.data.verticalSensitivity) * sensitivityScale;
                freeRotateY += (camAxis.x * playerController.data.horizontalSensitivity) * sensitivityScale;

                freeRotateX = Mathf.Clamp(freeRotateX, playerController.data.freeLookVerticalLimit.x, playerController.data.freeLookVerticalLimit.y);
                freeRotateY = Mathf.Clamp(freeRotateY, playerController.data.freeLookHorizontalLimit.x, playerController.data.freeLookHorizontalLimit.y);

                camTargetRot = Quaternion.Euler(freeRotateX, freeRotateY, 0f);

                weaponMaskRoot.localPosition = Vector3.zero;
            }
            else
            {
                if (clickFreeLook)
                {
                    // Reseting
                    currentCam = standardCam;
                    standardCam.enabled = true;
                    freeLookCam.enabled = false;
                    clickFreeLook = false;

                    // Set mask cam rotation
                    weaponMaskRoot.parent = standardCam.transform;
                    weaponMaskRoot.localEulerAngles = Vector3.zero;
                }

                mainRotateX += (-camAxis.y * playerController.data.verticalSensitivity) * sensitivityScale;
                mainRotateY += (camAxis.x * playerController.data.horizontalSensitivity) * sensitivityScale;

                mainRotateX = Mathf.Clamp(mainRotateX, -90f, 90f);

                camTargetRot = Quaternion.Euler(mainRotateX, 0f, 0f);
                characerTargetRot = Quaternion.Euler(0f, mainRotateY, 0f);

                weaponMaskRoot.localPosition = Vector3.zero;
            }

            currentCam.transform.localRotation = Quaternion.Slerp(currentCam.transform.localRotation, camTargetRot, playerController.data.smoothness * Time.deltaTime);
            playerController.transform.localRotation = Quaternion.Slerp(playerController.transform.localRotation, characerTargetRot, playerController.data.smoothness * Time.deltaTime);
        }

        public void SetCursorLock(bool locked)
        {
            if (locked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            isCursorLocked = locked;
        }
    }
}