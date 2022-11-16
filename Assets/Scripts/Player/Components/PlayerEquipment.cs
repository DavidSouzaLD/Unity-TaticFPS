using UnityEngine;
using Game.WeaponSystem;

namespace Game.Player.Components
{
    public class PlayerEquipment : MonoBehaviour
    {
        public bool isPrimary;
        public Weapon primary, secondary;
        private Weapon weaponHiding;
        private bool isChanging;

        private void Start()
        {
            UpdateWeapons();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                isPrimary = true;
                SetWeapon(primary);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                isPrimary = false;
                SetWeapon(secondary);
            }
        }

        private void LateUpdate()
        {
            if (isChanging)
            {
                if (weaponHiding == null) return;

                if (!weaponHiding.gameObject.activeSelf)
                {
                    weaponHiding = null;
                }

                if (weaponHiding.isHiding == false)
                {
                    weaponHiding = null;
                }

                if (weaponHiding == null)
                {
                    UpdateWeapons();
                }
            }
        }

        private void SetWeapon(Weapon weapon)
        {
            Weapon target = isPrimary ? primary : secondary;

            bool conditionsToSwitch =
            WeaponManager.currentWeapon != null &&
            WeaponManager.currentWeapon != target &&
            !WeaponManager.currentWeapon.isHiding &&
            !WeaponManager.currentWeapon.isDrawing &&
            !WeaponManager.currentWeapon.isFiring &&
            !WeaponManager.currentWeapon.isReloading &&
            !WeaponManager.currentWeapon.isAiming;

            if (conditionsToSwitch == false) return;

            weaponHiding = WeaponManager.currentWeapon;
            weaponHiding.weaponAnimation.Play("Hide");
            isChanging = true;
        }

        private void UpdateWeapons()
        {
            if (isPrimary)
            {
                if (WeaponManager.currentWeapon != null)
                {
                    if (WeaponManager.currentWeapon == secondary)
                    {
                        WeaponManager.currentWeapon = primary;
                        primary.gameObject.SetActive(true);
                        secondary.gameObject.SetActive(false);
                    }
                }
                else
                {
                    WeaponManager.currentWeapon = primary;
                    primary.gameObject.SetActive(true);
                    secondary.gameObject.SetActive(false);
                }
            }
            else
            {
                if (WeaponManager.currentWeapon != null)
                {
                    if (WeaponManager.currentWeapon == primary)
                    {
                        WeaponManager.currentWeapon = secondary;
                        secondary.gameObject.SetActive(true);
                        primary.gameObject.SetActive(false);
                    }
                }
                else
                {
                    WeaponManager.currentWeapon = secondary;
                    secondary.gameObject.SetActive(true);
                    primary.gameObject.SetActive(false);
                }
            }
        }
    }
}