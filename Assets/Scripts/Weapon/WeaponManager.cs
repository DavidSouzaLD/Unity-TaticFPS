using UnityEngine;

namespace Game.Weapons
{
    [DisallowMultipleComponent]
    public class WeaponManager : Singleton<WeaponManager>
    {
        [Header("Settings")]
        [SerializeField] private LayerMask hittableMask;
        [SerializeField] private GameObject tracerPrefab;
        [SerializeField] private Weapon currentWeapon;

        // Private
        private WeaponSway sway;
        private WeaponRetract retract;
        private WeaponImpacts impacts;
        private WeaponHitmark hitmark;

        public static bool IsAim()
        {
            return HaveCurrentWeapon() && Instance.currentWeapon.GetState("Aiming");
        }

        public static bool IsReload()
        {
            return HaveCurrentWeapon() && Instance.currentWeapon.GetState("Reloading");
        }

        public static bool IsSafety()
        {
            return HaveCurrentWeapon() && Instance.currentWeapon.GetState("Safety");
        }

        public static bool HaveCurrentWeapon()
        {
            return Instance != null && Instance.currentWeapon != null;
        }

        public static Weapon GetCurrentWeapon()
        {
            return Instance.currentWeapon;
        }

        public static void SetCurrentWeapon(Weapon _weapon)
        {
            Instance.currentWeapon = _weapon;
        }

        public static LayerMask GetHittableMask()
        {
            return Instance.hittableMask;
        }

        public static GameObject GetTracerPrefab()
        {
            return Instance.tracerPrefab;
        }

        public static WeaponSway GetSway()
        {
            return Instance.sway;
        }

        public static WeaponRetract GetRetract()
        {
            return Instance.retract;
        }

        public static WeaponImpacts GetImpacts()
        {
            return Instance.impacts;
        }

        public static WeaponHitmark GetHitmark()
        {
            return Instance.hitmark;
        }

        protected override void Awake()
        {
            base.Awake();
            sway = GetComponent<WeaponSway>();
            retract = GetComponent<WeaponRetract>();
            impacts = GetComponent<WeaponImpacts>();
            hitmark = GetComponent<WeaponHitmark>();
        }

        private void LateUpdate()
        {
            if (currentWeapon == null)
            {
                currentWeapon = GameObject.FindObjectOfType<Weapon>();
            }
        }
    }
}