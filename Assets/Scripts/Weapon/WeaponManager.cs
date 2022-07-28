using UnityEngine;

[DisallowMultipleComponent]
public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance;

    [Header("Settings")]
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
    public static bool IsAim => HaveCurrentWeapon() && Instance.currentWeapon.States.GetState("Aiming");
    public static bool IsReload => HaveCurrentWeapon() && Instance.currentWeapon.States.GetState("Reloading");
    public static bool IsSafety => HaveCurrentWeapon() && Instance.currentWeapon.States.GetState("Safety");

    // Static
    public static bool HaveCurrentWeapon() => Instance != null && Instance.currentWeapon != null;
    public static GameObject GetTracerPrefab() => Instance.tracerPrefab;
    public static bool CanSwitchWeapon()
    {
        return
        !Instance.currentWeapon.States.GetState("Safety") &&
        !Instance.currentWeapon.States.GetState("Aiming") &&
        !Instance.currentWeapon.States.GetState("Reloading") &&
        !Instance.currentWeapon.States.GetState("Firing") &&
        !Instance.currentWeapon.States.GetState("Drawing") &&
        !Instance.currentWeapon.States.GetState("Hiding");
    }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        sway = GetComponent<WeaponSway>();
        retract = GetComponent<WeaponRetract>();
        impacts = GetComponent<WeaponImpacts>();
        hitmark = GetComponent<WeaponHitmark>();
    }
}