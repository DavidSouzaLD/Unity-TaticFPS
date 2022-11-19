using UnityEngine;

namespace Code.Weapons
{
    public class WeaponManager : MonoBehaviour
    {
        private static WeaponManager Instance;
        public LayerMask hittableMask;
        public Weapon currentWeapon { get; set; }

        public static LayerMask HittableMask { get { return Instance.hittableMask; } }
        public static Weapon CurrentWeapon { get { return Instance.currentWeapon; } }

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