using UnityEngine;

namespace Game.Character.Gadgets
{
    public class NightVision : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float timeToUse = 1f;
        [SerializeField] private AudioClip nightVisionSound;
        [SerializeField] private Light nightVisionLight;

        [Header("Exposure")]
        [SerializeField] private float exposureSpeed = 1f;
        [SerializeField] private float defaultExposure = 6f;
        [SerializeField] private float enabledExposure = 50f;

        // Private
        private float nightVisionTimer;
        private bool nightVisionMode;
        private bool changeControl;

        public bool Enabled
        {
            get
            {
                return nightVisionMode;
            }
        }

        private void Start()
        {
            // Exposure
            if (nightVisionLight.intensity != enabledExposure)
            {
                nightVisionLight.intensity = enabledExposure;
            }

            NightVisionUpdate();
        }

        private void Update()
        {
            // Night vision
            if (Systems.Input.GetBool("NightVision") && nightVisionTimer <= 0)
            {
                nightVisionMode = !nightVisionMode;
                NightVisionUpdate();
                nightVisionTimer = timeToUse;
            }

            // Exposure
            if (nightVisionMode)
            {
                if (nightVisionLight.intensity != defaultExposure)
                {
                    nightVisionLight.intensity = Mathf.Lerp(nightVisionLight.intensity, defaultExposure, exposureSpeed * Time.deltaTime);
                }
            }

            if (nightVisionTimer > 0)
            {
                nightVisionTimer -= Time.deltaTime;
            }
        }

        private void NightVisionUpdate()
        {
            if (nightVisionMode)
            {
                if (!changeControl)
                {
                    if (CharacterCamera.GetPostProcessing().GetVolume() != "NIGHTVISION")
                    {
                        CharacterCamera.GetPostProcessing().ApplyVolume("NIGHTVISION");

                        changeControl = true;
                    }

                    Systems.Audio.PlaySound(nightVisionSound, 0.5f);
                    nightVisionLight.gameObject.SetActive(true);
                }
            }
            else
            {
                if (changeControl)
                {
                    if (CharacterCamera.GetPostProcessing().GetVolume() != "BASE")
                    {
                        CharacterCamera.GetPostProcessing().ApplyVolume("BASE");
                        changeControl = false;
                    }

                    // Exposure
                    if (nightVisionLight.intensity != enabledExposure)
                    {
                        nightVisionLight.intensity = enabledExposure;
                    }

                    nightVisionLight.gameObject.SetActive(false);
                }
            }
        }
    }
}