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
            };
        }
    }

    public class CharacterCamera : Singleton<CharacterCamera>
    {
        [Header("Settings")]
        [SerializeField] private Vector2 sensitivity = new Vector2(0.15f, 0.15f);
        [SerializeField] private Vector2 clampVertical = new Vector2(-90f, 90f);
        [SerializeField] private float camSmooth = 20f;

        [Header("Volume")]
        [SerializeField] private VolumeProfile gameProfile;
        [SerializeField] private VolumeProfile nightProfile;

        [Header("Roots")]
        [SerializeField] private Transform playerTransform;

        // Private
        private const float resetSpeed = 5f;
        private float sensitivityScale;
        private Vector2 camRot;
        private Quaternion characterTargetRot;
        private Quaternion camTargetRot;
        private Volume Volume;
        private Camera Camera;
        private CameraState States;

        public static void SetSensitivityScale(float scale)
        {
            Instance.sensitivityScale = Mathf.Clamp(scale, 0, 1);
        }

        public static void MaxSensitivityScale()
        {
            Instance.sensitivityScale = 1f;
        }

        public static Camera GetCamera()
        {
            return Instance.Camera;
        }

        public static Vector3 WorldToScreen(Vector3 _position)
        {
            return Instance.Camera.WorldToScreenPoint(_position);
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
            Volume = GetComponent<Volume>();
            Camera = GetComponent<Camera>();

            // Starting values
            characterTargetRot = playerTransform.localRotation;
            camTargetRot = Camera.transform.localRotation;

            MaxSensitivityScale();
            LockCursor(true);
        }

        private void Update()
        {
            // Camera
            if (Camera != null)
            {
                Vector2 camAxis = Systems.Input.GetVector2("CameraAxis");

                camRot.x = (-camAxis.y * sensitivity.y) * sensitivityScale;
                camRot.y = (camAxis.x * sensitivity.x) * sensitivityScale;

                characterTargetRot *= Quaternion.Euler(0f, camRot.y, 0f);
                camTargetRot *= Quaternion.Euler(camRot.x, 0f, 0f);

                camTargetRot = camTargetRot.ClampRotation(clampVertical.x, clampVertical.y, RotationExtension.Axis.X);

                playerTransform.localRotation = Quaternion.Slerp(playerTransform.localRotation, characterTargetRot, camSmooth * Time.deltaTime);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, camTargetRot, camSmooth * Time.deltaTime);
            }
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