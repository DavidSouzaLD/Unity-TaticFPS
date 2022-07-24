using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private InputMap Map;
    private bool _haveMap => Map != null;
    public static PlayerInput Keys;

    private void Awake()
    {
        Map = new InputMap();

        if (Keys == null)
        {
            Keys = this;
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

    public Vector2 CameraAxis
    {
        get
        {
            return _haveMap ? Map.Keys.CameraAxis.ReadValue<Vector2>() : Vector2.zero;
        }
    }

    public Vector2 MoveAxis
    {
        get
        {
            return _haveMap ? Map.Keys.MoveAxis.ReadValue<Vector2>().normalized : Vector2.zero;
        }
    }

    public bool Run
    {
        get
        {
            return _haveMap ? Map.Keys.Run.ReadValue<float>() != 0 : false;
        }
    }

    public bool Crouch
    {
        get
        {
            return _haveMap ? Map.Keys.Crouch.ReadValue<float>() != 0 : false;
        }
    }

    public bool Jump
    {
        get
        {
            return _haveMap ? Map.Keys.Jump.ReadValue<float>() != 0 : false;
        }
    }

    public bool Interact
    {
        get
        {
            return _haveMap ? Map.Keys.Interact.ReadValue<float>() != 0 : false;
        }
    }

    public bool FireAuto
    {
        get
        {
            return _haveMap ? Map.Keys.FireAuto.ReadValue<float>() != 0 : false;
        }
    }

    public bool FireTap
    {
        get
        {
            return _haveMap ? Map.Keys.FireTap.ReadValue<float>() != 0 : false;
        }
    }

    public bool Aim
    {
        get
        {
            return _haveMap ? Map.Keys.Aim.ReadValue<float>() != 0 : false;
        }
    }

    public bool Reload
    {
        get
        {
            return _haveMap ? Map.Keys.Reload.ReadValue<float>() != 0 : false;
        }
    }

    public bool NightVision
    {
        get
        {
            return _haveMap ? Map.Keys.NightVision.ReadValue<float>() != 0 : false;
        }
    }

    public bool FreeVision
    {
        get
        {
            return _haveMap ? Map.Keys.FreeVision.ReadValue<float>() != 0 : false;
        }
    }

    public bool Flashlight
    {
        get
        {
            return _haveMap ? Map.Keys.Flashlight.ReadValue<float>() != 0 : false;
        }
    }

    public bool Laser
    {
        get
        {
            return _haveMap ? Map.Keys.Laser.ReadValue<float>() != 0 : false;
        }
    }

    public bool EquipPrimary
    {
        get
        {
            return _haveMap ? Map.Keys.EquipPrimary.ReadValue<float>() != 0 : false;
        }
    }

    public bool EquipSecondary
    {
        get
        {
            return _haveMap ? Map.Keys.EquipSecondary.ReadValue<float>() != 0 : false;
        }
    }

    public float Turn
    {
        get
        {
            return _haveMap ? Map.Keys.Turn.ReadValue<float>() : 0;
        }
    }

    public bool Custom
    {
        get
        {
            return _haveMap ? Map.Keys.Custom.ReadValue<float>() != 0 : false;
        }
    }

    public bool Inspect
    {
        get
        {
            return _haveMap ? Map.Keys.Inspect.ReadValue<float>() != 0 : false;
        }
    }
}
