using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Game.Utilities;

namespace Game.Character
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Camera))]
    [RequireComponent(typeof(Volume))]
    public class CameraState : States
    {
        public CameraState()
        {
            states = new List<State>()
            {
                new State("CursorLocked"),
                new State("NightVision"),
                new State("FreeLook")
            };
        }
    }

    public class CharacterCamera : Singleton<CharacterCamera>
    {
        [System.Serializable]
        public class PostProcessing
        {
            public VolumeProfile[] profiles;

            public string GetVolume()
            {
                string volumeName = "";

                if (Instance.Volume.profile == profiles[0])
                {
                    volumeName = "BASE";
                }

                if (Instance.Volume.profile == profiles[1])
                {
                    volumeName = "NIGHTVISION";
                }

                return volumeName;
            }

            public void ApplyVolume(string _volumeName)
            {
                string volumeName = _volumeName.ToUpper();

                switch (volumeName)
                {
                    case "BASE":
                        Instance.Volume.profile = profiles[0];
                        break;

                    case "NIGHTVISION":
                        Instance.Volume.profile = profiles[1];
                        break;
                }
            }
        }

        [Header("Settings")]
        [SerializeField] private Vector2 sensitivity = new Vector2(0.15f, 0.15f);
        [SerializeField] private Vector2 clampVertical = new Vector2(-90f, 90f);
        [SerializeField] private float camSmooth = 20f;

        [Header("Cameras")]
        [SerializeField] private Camera mainCam;
        [SerializeField] private Camera freeLookCam;

        [Header("Roots")]
        [SerializeField] private Transform characterTransform;

        [Header("Volume")]
        [SerializeField] private Volume Volume;
        [SerializeField] private PostProcessing postProcessing;

        // Private
        private bool changedFreeLook;
        private float mainRotateX, mainRotateY;
        private float freeRotateX, freeRotateY;
        private const float resetSpeed = 5f;
        private float sensitivityScale;
        private Quaternion camTargetRot;
        private Quaternion characerTargetRot;
        private CameraState States;
        private Camera currentCam;

        public static void SetSensitivityScale(float scale)
        {
            Instance.sensitivityScale = Mathf.Clamp(scale, 0, 1);
        }

        public static void MaxSensitivityScale()
        {
            Instance.sensitivityScale = 1f;
        }

        public static PostProcessing GetPostProcessing()
        {
            return Instance.postProcessing;
        }

        public static Camera GetCamera()
        {
            return Instance.mainCam;
        }

        public static Camera GetCurrentCamera()
        {
            return Instance.currentCam;
        }

        public static Vector3 WorldToScreen(Vector3 _position)
        {
            return Instance.currentCam.WorldToScreenPoint(_position);
        }

        public static bool GetState(string _stateName)
        {
            return Instance.States.GetState(_stateName);
        }

        public static void SetState(string _stateName, bool _value)
        {
            Instance.States.SetState(_stateName, _value);
        }

        protected override void Awake()
        {
            base.Awake();

            // Create state
            States = new CameraState();
        }

        private void Start()
        {
            // Components
            currentCam = mainCam;

            MaxSensitivityScale();
            LockCursor(true);
        }

        private void Update()
        {
            // Camera
            if (currentCam != null)
            {
                Vector2 camAxis = Systems.Input.GetVector2("CameraAxis");

                if (!Systems.Input.GetBool("FreeLook"))
                {
                    if (changedFreeLook)
                    {
                        currentCam = mainCam;
                        mainCam.enabled = true;
                        freeLookCam.enabled = false;
                        changedFreeLook = false;
                    }

                    mainRotateX += (-camAxis.y * sensitivity.y) * sensitivityScale;
                    mainRotateY += (camAxis.x * sensitivity.x) * sensitivityScale;

                    mainRotateX = Mathf.Clamp(mainRotateX, -90f, 90f);

                    camTargetRot = Quaternion.Euler(mainRotateX, 0f, 0f);
                    characerTargetRot = Quaternion.Euler(0f, mainRotateY, 0f);
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
                    }

                    freeRotateX += (-camAxis.y * sensitivity.y) * sensitivityScale;
                    freeRotateY += (camAxis.x * sensitivity.x) * sensitivityScale;

                    freeRotateX = Mathf.Clamp(freeRotateX, -70f, 20f);
                    freeRotateY = Mathf.Clamp(freeRotateY, -45f, 45f);

                    camTargetRot = Quaternion.Euler(freeRotateX, freeRotateY, 0f);
                }
            }

            currentCam.transform.localRotation = Quaternion.Slerp(currentCam.transform.localRotation, camTargetRot, camSmooth * Time.deltaTime);
            characterTransform.localRotation = Quaternion.Slerp(characterTransform.transform.localRotation, characerTargetRot, camSmooth * Time.deltaTime);
        }

        public void LockCursor(bool value)
        {
            // Lock cursor in game
            if (value)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                States.SetState("CursorLocked", true);
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                States.SetState("CursorLocked", false);
            }
        }
    }
}