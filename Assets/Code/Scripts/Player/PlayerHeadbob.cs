using System;
using UnityEngine;
using Code.Interfaces;

namespace Code.Player
{
    public class PlayerHeadbob : MonoBehaviour, IPlayerControllerComponent
    {
        [Serializable]
        public class HeadbobSettings
        {
            [Serializable]
            public class HeadbobCurve
            {
                public float frequency = 1f;
                public float amplitude = 1f;
                public AnimationCurve verticalCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 0f));
                public AnimationCurve horizontalCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 0f));
            }

            public HeadbobSettings()
            {
                standard = new HeadbobCurve();
                walking = new HeadbobCurve();
                running = new HeadbobCurve();
                crouching = new HeadbobCurve();
            }

            public float resetSpeed = 5f;
            public HeadbobCurve standard;
            public HeadbobCurve walking;
            public HeadbobCurve running;
            public HeadbobCurve crouching;
        }

        [Header("Roots")]
        [SerializeField] private Transform headbobRoot;

        private Vector3 defaultHeadbobPosition;
        private float offsetY;
        private float headbobTime;

        public PlayerController playerController { get; set; }

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
            defaultHeadbobPosition = headbobRoot.localPosition;
        }

        private void Update()
        {
            if (playerController.data == null) return;

            if (PlayerController.isGrounded)
            {
                HeadbobSettings settings = playerController.data.headbobSettings;

                if (PlayerController.isCrouching)
                {
                    // Crouching
                    AddFrequency(settings.crouching.frequency);
                    SetHeadbob(settings.crouching);
                }
                else
                {
                    if (!PlayerController.isWalking)
                    {
                        // Standard
                        AddFrequency(settings.standard.frequency);
                        SetHeadbob(settings.standard);
                    }
                    else if (!PlayerController.isRunning)
                    {
                        // Walking
                        AddFrequency(settings.walking.frequency);
                        SetHeadbob(settings.walking);
                    }

                    if (PlayerController.isRunning)
                    {
                        // Running
                        AddFrequency(settings.running.frequency);
                        SetHeadbob(settings.running);
                    }
                }
            }
            else
            {
                ResetHeadbob();
            }
        }

        private void AddFrequency(float frequency)
        {
            headbobTime += Time.deltaTime * frequency;

            if (headbobTime > 1)
            {
                headbobTime = 0;
            }
        }

        private void SetHeadbob(HeadbobSettings.HeadbobCurve curve)
        {
            float amplitude = curve.amplitude;

            headbobRoot.localPosition = new Vector3(
                curve.horizontalCurve.Evaluate(headbobTime) * amplitude,
                curve.verticalCurve.Evaluate(headbobTime) * amplitude,
                headbobRoot.localPosition.z
            );
        }

        private void ResetHeadbob()
        {
            if (headbobRoot.localPosition != defaultHeadbobPosition)
            {
                headbobRoot.localPosition = Vector3.Lerp(
                    headbobRoot.localPosition,
                    defaultHeadbobPosition,
                    playerController.data.headbobSettings.resetSpeed * Time.deltaTime
                );
            }
        }

        private void OnDisable()
        {
            ResetHeadbob();
        }

        private void OnDestroy()
        {
            ResetHeadbob();
        }
    }
}