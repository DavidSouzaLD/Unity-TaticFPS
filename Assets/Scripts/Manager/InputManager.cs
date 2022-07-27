using UnityEngine;

[DisallowMultipleComponent]
public class InputManager : MonoBehaviour
{
    private static InputManager Instance;
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
            Destroy(this);
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

    public static bool Marker
    {
        get
        {
            return Instance._haveMap ? Instance.Map.Player.Marker.ReadValue<float>() != 0 : false;
        }
    }

    public static bool WeaponFireAuto
    {
        get
        {
            return Instance._haveMap ? Instance.Map.Player.WeaponFireAuto.ReadValue<float>() != 0 : false;
        }
    }

    public static bool WeaponFireTap
    {
        get
        {
            return Instance._haveMap ? Instance.Map.Player.WeaponFireTap.ReadValue<float>() != 0 : false;
        }
    }

    public static bool WeaponAim
    {
        get
        {
            return Instance._haveMap ? Instance.Map.Player.WeaponAim.ReadValue<float>() != 0 : false;
        }
    }

    public static bool WeaponReload
    {
        get
        {
            return Instance._haveMap ? Instance.Map.Player.WeaponReload.ReadValue<float>() != 0 : false;
        }
    }

    public static bool WeaponSafety
    {
        get
        {
            return Instance._haveMap ? Instance.Map.Player.WeaponSafety.ReadValue<float>() != 0 : false;
        }
    }

    public static bool NightVision
    {
        get
        {
            return Instance._haveMap ? Instance.Map.Player.NightVision.ReadValue<float>() != 0 : false;
        }
    }

    public static bool WeaponHandguard
    {
        get
        {
            return Instance._haveMap ? Instance.Map.Player.WeaponHanguard.ReadValue<float>() != 0 : false;
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

    public static float Cover
    {
        get
        {
            return Instance._haveMap ? Instance.Map.Player.Cover.ReadValue<float>() : 0;
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
