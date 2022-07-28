using UnityEngine;

[DisallowMultipleComponent]
public class PlayerInput : StaticInstance<PlayerInput>
{
    private InputMap Map;
    private static InputMap CheckMap
    {
        get
        {
            return (Instance.Map != null) ? Instance.Map : null;
        }
    }
    protected override void Awake()
    {
        base.Awake();
        Map = new InputMap();
    }

    private void OnEnable() => Map.Enable();
    private void OnDisable() => Map.Disable();

    #region PlayerKeys
    public static Vector2 CameraAxis => CheckMap.Player.CameraAxis.ReadValue<Vector2>();
    public static Vector2 MoveAxis => CheckMap.Player.MoveAxis.ReadValue<Vector2>().normalized;
    public static float Cover => CheckMap.Player.Cover.ReadValue<float>();
    public static bool Run => CheckMap.Player.Run.ReadValue<float>() != 0;
    public static bool Crouch => CheckMap.Player.Crouch.ReadValue<float>() != 0;
    public static bool Jump => CheckMap.Player.Jump.ReadValue<float>() != 0;
    public static bool Interact => CheckMap.Player.Interact.ReadValue<float>() != 0;
    public static bool Marker => CheckMap.Player.Marker.ReadValue<float>() != 0;
    public static bool WeaponFireAuto => CheckMap.Player.WeaponFireAuto.ReadValue<float>() != 0;
    public static bool WeaponFireTap => CheckMap.Player.WeaponFireTap.ReadValue<float>() != 0;
    public static bool WeaponAim => CheckMap.Player.WeaponAim.ReadValue<float>() != 0;
    public static bool WeaponReload => CheckMap.Player.WeaponReload.ReadValue<float>() != 0;
    public static bool WeaponSafety => CheckMap.Player.WeaponSafety.ReadValue<float>() != 0;
    public static bool NightVision => CheckMap.Player.NightVision.ReadValue<float>() != 0;
    public static bool WeaponHandguard => CheckMap.Player.WeaponHanguard.ReadValue<float>() != 0;
    public static bool EquipPrimary => CheckMap.Player.EquipPrimary.ReadValue<float>() != 0;
    public static bool EquipSecondary => CheckMap.Player.EquipSecondary.ReadValue<float>() != 0;
    #endregion

    #region CustomizeKeys
    public static float Scroll => CheckMap.Customize.Scroll.ReadValue<float>();
    public static bool CustomizeRotation => CheckMap.Customize.Rotation.ReadValue<float>() != 0;
    public static bool CustomizeReset => CheckMap.Customize.ResetRotation.ReadValue<float>() != 0;
    #endregion
}