using UnityEngine;
using Game.Weapons;

namespace Game.Character
{
    [DisallowMultipleComponent]
    public class CharacterEquipment : MonoBehaviour
    {
        public enum WeaponType { PrimaryWeapon, SecondaryWeapon }

        [Header("Settings")]
        [SerializeField] private WeaponType currentType;
        [SerializeField] private Weapon primaryWeapon;
        [SerializeField] private Weapon secondaryWeapon;

        private void Start()
        {
            if (primaryWeapon.gameObject.activeSelf)
            {
                currentType = WeaponType.PrimaryWeapon;
            }

            if (secondaryWeapon.gameObject.activeSelf)
            {
                currentType = WeaponType.SecondaryWeapon;
            }
        }

        private void Update()
        {
            if (Systems.Input.GetBool("EquipPrimary") && currentType == WeaponType.SecondaryWeapon)
            {
                SwitchWeapons(WeaponType.PrimaryWeapon);
            }

            if (Systems.Input.GetBool("EquipSecondary") && currentType == WeaponType.PrimaryWeapon)
            {
                SwitchWeapons(WeaponType.SecondaryWeapon);
            }
        }

        private void SwitchWeapons(WeaponType _type)
        {
            bool conditions =
            !WeaponManager.GetCurrentWeapon().GetState("Safety") &&
            !WeaponManager.GetCurrentWeapon().GetState("Aiming") &&
            !WeaponManager.GetCurrentWeapon().GetState("Reloading") &&
            !WeaponManager.GetCurrentWeapon().GetState("Firing") &&
            !WeaponManager.GetCurrentWeapon().GetState("Drawing") &&
            !WeaponManager.GetCurrentWeapon().GetState("Hiding");

            if (conditions)
            {
                // Setting a animation exit for current weapon
                WeaponManager.GetCurrentWeapon().GetWeaponAnimation().Play("Exit");

                // The weapon is deactivated via an event in the script's animation (Weapon Animation).
                switch (_type)
                {
                    case WeaponType.PrimaryWeapon:

                        WeaponManager.GetCurrentWeapon().ExitWeapon();
                        WeaponManager.SetCurrentWeapon(primaryWeapon);
                        WeaponManager.GetCurrentWeapon().EnterWeapon();
                        primaryWeapon.gameObject.SetActive(true);
                        currentType = WeaponType.PrimaryWeapon;

                        break;

                    case WeaponType.SecondaryWeapon:

                        WeaponManager.GetCurrentWeapon().ExitWeapon();
                        WeaponManager.SetCurrentWeapon(secondaryWeapon);
                        WeaponManager.GetCurrentWeapon().EnterWeapon();
                        secondaryWeapon.gameObject.SetActive(true);
                        currentType = WeaponType.SecondaryWeapon;

                        break;
                }
            }
        }
    }
}