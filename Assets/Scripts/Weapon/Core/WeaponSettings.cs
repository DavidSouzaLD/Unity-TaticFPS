using UnityEngine;

namespace WeaponSystem.Core
{
    public class WeaponSettings : MonoBehaviour
    {
        private static WeaponSettings Instance;

        [Header("Settings")]
        public LayerMask hittableMask;
        public GameObject tracerPrefab;
        public Weapon currentWeapon;

        public static LayerMask HittableMask
        {
            get
            {
                return Instance.hittableMask;
            }
        }

        public static GameObject TracerPrefab
        {
            get
            {
                return Instance.tracerPrefab;
            }
        }

        public static Weapon CurrentWeapon
        {
            get
            {
                return Instance.currentWeapon;
            }
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
        }
    }
}