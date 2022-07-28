using UnityEngine;

[DisallowMultipleComponent]
public class WeaponManager : StaticInstance<WeaponManager>
{
    [Header("Settings")]
    [SerializeField] private LayerMask hittableMask;
    [SerializeField] private GameObject tracerPrefab;

    private Weapon currentWeapon;
    private WeaponSway sway;
    private WeaponRetract retract;
    private WeaponImpacts impacts;
    private WeaponHitmark hitmark;

    // Static components
    public static WeaponSway Sway { get { return Instance.sway; } }
    public static WeaponRetract Retract { get { return Instance.retract; } }
    public static WeaponImpacts Impacts { get { return Instance.impacts; } }
    public static WeaponHitmark Hitmark { get { return Instance.hitmark; } }

    // States current weapon
    public static bool IsAim => HaveCurrentWeapon() && Instance.currentWeapon.GetStates.GetState("Aiming");
    public static bool IsReload => HaveCurrentWeapon() && Instance.currentWeapon.GetStates.GetState("Reloading");
    public static bool IsSafety => HaveCurrentWeapon() && Instance.currentWeapon.GetStates.GetState("Safety");

    // Static
    public static bool HaveCurrentWeapon() => Instance != null && Instance.currentWeapon != null;
    public static LayerMask GetHittableMask() => Instance.hittableMask;
    public static GameObject GetTracerPrefab() => Instance.tracerPrefab;
    public static bool CanSwitchWeapon()
    {
        return
        !Instance.currentWeapon.GetStates.GetState("Safety") &&
        !Instance.currentWeapon.GetStates.GetState("Aiming") &&
        !Instance.currentWeapon.GetStates.GetState("Reloading") &&
        !Instance.currentWeapon.GetStates.GetState("Firing") &&
        !Instance.currentWeapon.GetStates.GetState("Drawing") &&
        !Instance.currentWeapon.GetStates.GetState("Hiding");
    }


    protected override void Awake()
    {
        base.Awake();

        sway = GetComponent<WeaponSway>();
        retract = GetComponent<WeaponRetract>();
        impacts = GetComponent<WeaponImpacts>();
        hitmark = GetComponent<WeaponHitmark>();
    }
}