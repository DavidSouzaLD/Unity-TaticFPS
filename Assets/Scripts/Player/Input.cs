using UnityEngine;

public class Input : MonoBehaviour
{
    private static Input Instance;
    private InputMap Map;
    private bool _haveMap => Map != null;

    private void Awake()
    {
        Map = new InputMap();

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        Map.Enable();
    }

    private void OnDisable()
    {
        Map.Disable();
    }

    #region Player Keys

    public static Vector2 CameraAxis
    {
        get
        {
            return Instance._haveMap ? Instance.Map.Player.CameraAxis.ReadValue<Vector2>() : Vector2.zero;
        }
    }

    public static Vector2 MoveAxis
    {
        get
        {
            return Instance._haveMap ? Instance.Map.Player.MoveAxis.ReadValue<Vector2>().normalized : Vector2.zero;
        }
    }

    public static bool Run
    {
        get
        {
            return Instance._haveMap ? Instance.Map.Player.Run.ReadValue<float>() != 0 : false;
        }
    }

    public static bool Crouch
    {
        get
        {
            return Instance._haveMap ? Instance.Map.Player.Crouch.ReadValue<float>() != 0 : false;
        }
    }

    public static bool Jump
    {
        get
        {
            return Instance._haveMap ? Instance.Map.Player.Jump.ReadValue<float>() != 0 : false;
        }
    }

    public static bool Interact
    {
        get
        {
            return Instance._haveMap ? Instance.Map.Player.Interact.ReadValue<float>() != 0 : false;
        }
    }

    public static bool FireAuto
    {
        get
        {
            return Instance._haveMap ? Instance.Map.Player.FireAuto.ReadValue<float>() != 0 : false;
        }
    }

    public static bool FireTap
    {
        get
        {
            return Instance._haveMap ? Instance.Map.Player.FireTap.ReadValue<float>() != 0 : false;
        }
    }

    public static bool Aim
    {
        get
        {
            return Instance._haveMap ? Instance.Map.Player.Aim.ReadValue<float>() != 0 : false;
        }
    }

    public static bool Reload
    {
        get
        {
            return Instance._haveMap ? Instance.Map.Player.Reload.ReadValue<float>() != 0 : false;
        }
    }

    public static bool NightVision
    {
        get
        {
            return Instance._haveMap ? Instance.Map.Player.NightVision.ReadValue<float>() != 0 : false;
        }
    }

    public static bool FreeVision
    {
        get
        {
            return Instance._haveMap ? Instance.Map.Player.FreeVision.ReadValue<float>() != 0 : false;
        }
    }

    public static bool Flashlight
    {
        get
        {
            return Instance._haveMap ? Instance.Map.Player.Flashlight.ReadValue<float>() != 0 : false;
        }
    }

    public static bool Laser
    {
        get
        {
            return Instance._haveMap ? Instance.Map.Player.Laser.ReadValue<float>() != 0 : false;
        }
    }

    public static bool EquipPrimary
    {
        get
        {
            return Instance._haveMap ? Instance.Map.Player.EquipPrimary.ReadValue<float>() != 0 : false;
        }
    }

    public static bool EquipSecondary
    {
        get
        {
            return Instance._haveMap ? Instance.Map.Player.EquipSecondary.ReadValue<float>() != 0 : false;
        }
    }

    public static float Turn
    {
        get
        {
            return Instance._haveMap ? Instance.Map.Player.Turn.ReadValue<float>() : 0;
        }
    }

    public static bool Inspect
    {
        get
        {
            return Instance._haveMap ? Instance.Map.Player.Inspect.ReadValue<float>() != 0 : false;
        }
    }

    #endregion

    #region Customize Keys

    public static bool CustomizeRotation
    {
        get
        {
            return Instance._haveMap ? Instance.Map.Customize.Rotation.ReadValue<float>() != 0 : false;
        }
    }

    public static bool CustomizeReset
    {
        get
        {
            return Instance._haveMap ? Instance.Map.Customize.ResetRotation.ReadValue<float>() != 0 : false;
        }
    }

    public static float Scroll
    {
        get
        {
            return Instance._haveMap ? Instance.Map.Customize.Scroll.ReadValue<float>() : 0f;
        }
    }

    #endregion
}
