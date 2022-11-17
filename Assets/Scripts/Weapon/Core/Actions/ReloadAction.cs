using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem.Core.Actions
{
    public class ReloadAction : WeaponAction
    {
        public ReloadAction(Weapon weaponClass, WeaponSO weaponData)
        {
            weapon = weaponClass;
            data = weaponData;
        }

        public Weapon weapon { get; set; }
        public WeaponSO data { get; set; }
        public bool isActive { get; set; }

        public void Start()
        {
            // Delegating check magazines void
            weapon.EndReload += CheckMagazines;

            // Optional
            weapon.currentMagazine.currentBullets = data.bulletsPerMagazine;
        }

        public void Update()
        {
            bool conditions = false;

            if (conditions)
            {
                // Player animation reload
                weapon.StartReload?.Invoke();
            }
        }

        public void CallReload()
        {
            if (weapon.currentMagazine == null) return;

            WeaponMagazine magazine = weapon.currentMagazine;
            WeaponMagazine otherMagazine = null;

            // Calculate reload
            if (magazine.currentBullets < data.bulletsPerMagazine)
            {
                int bulletsNeeded = 0;

                for (int i = 0; i < data.bulletsPerMagazine; i++)
                {
                    if ((magazine.currentBullets + bulletsNeeded) < data.bulletsPerMagazine)
                    {
                        bulletsNeeded++;
                    }
                }

                // Getting other magazine
                for (int i = 0; i < weapon.magazines.Length; i++)
                {
                    if (weapon.magazines[i].currentBullets >= bulletsNeeded)
                    {
                        otherMagazine = weapon.magazines[i];
                        break;
                    }
                }

                if (otherMagazine == null) return;

                if (otherMagazine.currentBullets >= bulletsNeeded)
                {
                    magazine.currentBullets += bulletsNeeded;
                    otherMagazine.currentBullets -= bulletsNeeded;
                }
                else if (otherMagazine.currentBullets < bulletsNeeded)
                {
                    bulletsNeeded = otherMagazine.currentBullets;
                    magazine.currentBullets += bulletsNeeded;
                    otherMagazine.currentBullets = 0;
                }
            }

            weapon.EndReload?.Invoke();
        }

        private void CheckMagazines()
        {
            List<WeaponMagazine> magazines = new List<WeaponMagazine>(weapon.magazines);

            for (int i = 0; i < magazines.Count; i++)
            {
                if (magazines[i].currentBullets <= 0)
                {
                    magazines.Remove(weapon.magazines[i]);
                }
            }

            weapon.magazines = magazines.ToArray();
        }
    }
}