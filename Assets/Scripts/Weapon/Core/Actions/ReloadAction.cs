using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem.Core.Actions
{
    public class ReloadAction : IWeaponAction
    {
        public ReloadAction(Weapon weaponClass, WeaponSO weaponData)
        {
            weapon = weaponClass;
            data = weaponData;
        }

        public Weapon weapon { get; set; }
        public WeaponSO data { get; set; }
        public bool isActive { get; set; }
        public void Start() { }

        public void Update()
        {
            bool conditions =
            InputManager.Click("Reload") &&
            weapon.currentBullets < weapon.bulletsPerMagazine;

            if (conditions)
            {
                // Player animation reload
                CallReload();
                weapon.StartReload?.Invoke();
            }
        }

        public void CallReload()
        {
            if (weapon == null) return;

            // Calculate reload
            if (weapon.currentBullets < weapon.bulletsPerMagazine)
            {
                int bulletsNeeded = 0;

                for (int i = 0; i < weapon.bulletsPerMagazine; i++)
                {
                    if ((weapon.currentBullets + bulletsNeeded) < weapon.bulletsPerMagazine)
                    {
                        bulletsNeeded++;
                    }
                }

                if (weapon.extraBullets >= bulletsNeeded)
                {
                    weapon.currentBullets += bulletsNeeded;
                    weapon.extraBullets -= bulletsNeeded;
                }
                else if (weapon.extraBullets < bulletsNeeded)
                {
                    bulletsNeeded = weapon.extraBullets;
                    weapon.currentBullets += bulletsNeeded;
                    weapon.extraBullets = 0;
                }
            }

            weapon.EndReload?.Invoke();
        }
    }
}