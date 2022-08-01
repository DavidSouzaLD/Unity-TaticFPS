using UnityEngine;

namespace Game.Character.Gadgets
{
    public class NightVision : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float timeToUse = 1f;
        [SerializeField] private AudioClip nightVisionSound;

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
            NightVisionUpdate();
        }

        private void Update()
        {
            // Night vision
            if (Systems.Input.GetBool("NightVision") && nightVisionTimer <= 0)
            {
                nightVisionMode = !nightVisionMode;

                if (nightVisionMode)
                {
                    Systems.Audio.PlaySound(nightVisionSound, 0.5f);
                }

                NightVisionUpdate();
                nightVisionTimer = timeToUse;
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
                }
            }
        }
    }
}