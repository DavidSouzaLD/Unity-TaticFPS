using UnityEngine;

public class NightVision : MonoBehaviour
{
    [Header("[NightVision Settings]")]
    [SerializeField] private PlayerCamera Camera;
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
        if (PlayerInput.Keys.NightVision)
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
                if (Camera.GetVolume() != "NIGHTVISION")
                {
                    Camera.ApplyVolume("NIGHTVISION");
                    changeControl = true;
                }
            }
        }
        else
        {
            if (changeControl)
            {
                if (Camera.GetVolume() != "BASE")
                {
                    Camera.ApplyVolume("BASE");
                    changeControl = false;
                }
            }
        }
    }
}
