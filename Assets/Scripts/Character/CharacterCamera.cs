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

    public class CharacterCamera : MonoBehaviour
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
        [SerializeField] private Transform camRecoilRoot;

        // Private
        private const float recoilSpeed = 8f;
        private const float resetSpeed = 5f;
        private float sensitivityScale;
        private Vector2 camRot;
        private Vector3 currentRotation;
        private Vector3 targetRotation;
        private Quaternion characterTargetRot;
        private Quaternion camTargetRot;
        private Volume Volume;
        private Camera Camera;
        private CameraState States;

        public void SetSensitivityScale(float scale)
        {
            sensitivityScale = Mathf.Clamp(scale, 0, 1);
        }

        public void MaxSensitivityScale()
        {
            sensitivityScale = 1f;
        }

        public Camera GetCamera()
        {
            return Camera;
        }

        public Vector3 WorldToScreen(Vector3 _position)
        {
            return Camera.WorldToScreenPoint(_position);
        }

        private void Awake()
        {
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

            // Recoil
            if (camRecoilRoot)
            {
                targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, resetSpeed * Time.deltaTime);
                currentRotation = Vector3.Slerp(currentRotation, targetRotation, recoilSpeed * Time.deltaTime);
                camRecoilRoot.localRotation = Quaternion.Euler(currentRotation);
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

        public void ApplyRecoil(Vector3 _recoil)
        {
            targetRotation += new Vector3(-_recoil.x, Random.Range(-_recoil.y, _recoil.y), Random.Range(-_recoil.z, _recoil.z));
        }
    }
}