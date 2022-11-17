using UnityEngine;

namespace WeaponSystem.Core.Actions
{
    public class AimAction : WeaponAction
    {
        public AimAction(Weapon weaponClass, WeaponSO weaponData)
        {
            weapon = weaponClass;
            data = weaponData;
        }

        public Weapon weapon { get; set; }
        public WeaponSO data { get; set; }
        public bool isActive { get; set; }
        public void Start() { }
        public void Update() { }
    }
}