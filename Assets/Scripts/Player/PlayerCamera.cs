using UnityEngine;

namespace Game.Player
{
    public class PlayerCamera : Singleton<PlayerCamera>
    {
        [Header("Settings")]
        [SerializeField] private Vector2 sensitivity = new Vector2(0.15f, 0.15f);
        [SerializeField] private Vector2 clampVertical = new Vector2(-90f, 90f);
        [SerializeField] private float camSmooth = 20f;

        [Header("Cameras")]
        [SerializeField] private Camera mainCam;
        [SerializeField] private Camera freeLookCam;

        [Header("Roots")]
        [SerializeField] private Transform cameraMaskRoot;
        [SerializeField] private Transform characterRoot;

        [Range(0f, 1f)]
        public static float sensitivityScale;

        // Private
        private bool changedFreeLook;
        private float mainRotateX, mainRotateY;
        private float freeRotateX, freeRotateY;
        private const float resetSpeed = 5f;
        private Quaternion camTargetRot;
        private Quaternion characerTargetRot;
        private Camera currentCam;
        public static bool cursorLocked { get; private set; }

        public Camera GetCamera()
        {
            return mainCam;
        }

        public Camera GetCurrentCamera()
        {
            return currentCam;
        }

        public static Vector3 WorldToScreen(Vector3 _position)
        {
            return Instance.currentCam.WorldToScreenPoint(_position);
        }

        private void Start()
        {
            // Components
            currentCam = mainCam;
            LockCursor(true);
        }

        private void Update()
        {
            // Camera
            if (currentCam != null)
            {
                Vector2 camAxis = PlayerKeys.GetAxis("CameraAxis");

                if (!PlayerKeys.Press("FreeLook"))
                {
                    if (changedFreeLook)
                    {
                        currentCam = mainCam;
                        mainCam.enabled = true;
                        freeLookCam.enabled = false;
                        changedFreeLook = false;

                        // Set mask cam rotation
                        cameraMaskRoot.parent = mainCam.transform;
                        cameraMaskRoot.localEulerAngles = Vector3.zero;
                    }

                    mainRotateX += (-camAxis.y * sensitivity.y) * sensitivityScale;
                    mainRotateY += (camAxis.x * sensitivity.x) * sensitivityScale;

                    mainRotateX = Mathf.Clamp(mainRotateX, -90f, 90f);

                    camTargetRot = Quaternion.Euler(mainRotateX, 0f, 0f);
                    characerTargetRot = Quaternion.Euler(0f, mainRotateY, 0f);

                    cameraMaskRoot.localPosition = Vector3.zero;
                }
                else
                {
                    if (!changedFreeLook)
                    {
                        freeRotateX = 0f;
                        freeRotateY = 0f;
                        currentCam = freeLookCam;
                        mainCam.enabled = false;
                        freeLookCam.enabled = true;
                        changedFreeLook = true;

                        // Set mask cam rotation
                        cameraMaskRoot.parent = freeLookCam.transform;
                        cameraMaskRoot.localEulerAngles = Vector3.zero;
                    }

                    freeRotateX += (-camAxis.y * sensitivity.y) * sensitivityScale;
                    freeRotateY += (camAxis.x * sensitivity.x) * sensitivityScale;

                    freeRotateX = Mathf.Clamp(freeRotateX, -70f, 20f);
                    freeRotateY = Mathf.Clamp(freeRotateY, -45f, 45f);

                    camTargetRot = Quaternion.Euler(freeRotateX, freeRotateY, 0f);

                    cameraMaskRoot.localPosition = Vector3.zero;
                }
            }

            currentCam.transform.localRotation = Quaternion.Slerp(currentCam.transform.localRotation, camTargetRot, camSmooth * Time.deltaTime);
            characterRoot.localRotation = Quaternion.Slerp(characterRoot.transform.localRotation, characerTargetRot, camSmooth * Time.deltaTime);
        }

        public void LockCursor(bool value)
        {
            cursorLocked = value;

            // Lock cursor in game
            if (cursorLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
}