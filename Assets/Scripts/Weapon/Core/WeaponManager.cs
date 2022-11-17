using UnityEngine;
using WeaponSystem.Components;

namespace WeaponSystem.Core
{
    [RequireComponent(typeof(Sway))]
    [RequireComponent(typeof(Recoil))]
    [RequireComponent(typeof(Retract))]
    [RequireComponent(typeof(Hitmark))]
    [RequireComponent(typeof(Impacts))]
    public class WeaponManager : MonoBehaviour
    {
        [Header("Settings")]
        public LayerMask hittableMask;
        public GameObject tracerPrefab;

        private static WeaponManager Instance;
        public Weapon currentWeapon { get; set; }
        public Sway sway { get; private set; }
        public Recoil recoil { get; private set; }
        public Retract retract { get; private set; }
        public Hitmark hitmark { get; private set; }
        public Impacts impacts { get; private set; }

        public static LayerMask HittableMask { get { return Instance.hittableMask; } }
        public static GameObject TracerPrefab { get { return Instance.tracerPrefab; } }
        public static Sway Sway { get { return Instance.sway; } }
        public static Recoil Recoil { get { return Instance.recoil; } }
        public static Retract Retract { get { return Instance.retract; } }
        public static Hitmark Hitmark { get { return Instance.hitmark; } }
        public static Impacts Impacts { get { return Instance.impacts; } }
        public static MonoBehaviour GetBehaviour { get { return Instance; } }
        public static Weapon CurrentWeapon
        {
            get
            {
                if (Instance.currentWeapon != null)
                {
                    return Instance.currentWeapon;
                }
                else
                {
                    Instance.currentWeapon = GameObject.FindObjectOfType<Weapon>();
                    return Instance.currentWeapon;
                }
            }
        }

        private void Awake()
        {
            // Setting instance
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }

            // Getting components
            sway = GetComponent<Sway>();
            recoil = GetComponent<Recoil>();
            retract = GetComponent<Retract>();
            hitmark = GetComponent<Hitmark>();
            impacts = GetComponent<Impacts>();
        }
    }
}