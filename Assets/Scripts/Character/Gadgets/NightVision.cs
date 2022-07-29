using UnityEngine;

namespace Game.Character.Gadgets
{
    public class NightVision : MonoBehaviour
    {
        [Header("Settings")]
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
            if (Systems.Input.GetBool("NightVision"))
            {
                nightVisionMode = !nightVisionMode;
                NightVisionUpdate();
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