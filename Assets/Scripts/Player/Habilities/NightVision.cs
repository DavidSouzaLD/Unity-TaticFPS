using UnityEngine;

public class NightVision : MonoBehaviour
{
    [Header("[NightVision Settings]")]
    [SerializeField] private bool nightVisionMode;
    bool changeControl;

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
        if (InputManager.NightVision)
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
                if (PlayerCamera.GetVolume() != "NIGHTVISION")
                {
                    PlayerCamera.ApplyVolume("NIGHTVISION");
                    changeControl = true;
                }
            }
        }
        else
        {
            if (changeControl)
            {
                if (PlayerCamera.GetVolume() != "BASE")
                {
                    PlayerCamera.ApplyVolume("BASE");
                    changeControl = false;
                }
            }
        }
    }
}
