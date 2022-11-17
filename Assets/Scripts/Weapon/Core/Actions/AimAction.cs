using UnityEngine;
using Game.Player;

namespace WeaponSystem.Core.Actions
{
    public class AimAction : IWeaponAction
    {
        public AimAction(Weapon weaponClass, WeaponSO weaponData)
        {
            weapon = weaponClass;
            data = weaponData;
        }

        public Weapon weapon { get; set; }
        public WeaponSO data { get; set; }
        public bool isActive { get; set; }
        private Vector3 initialAimPos;
        private Quaternion initialAimRot;
        private Vector3 defaultAimPos;
        private Quaternion defaultAimRot;

        public void Start()
        {
            initialAimPos = weapon.transform.localPosition;
            initialAimRot = weapon.transform.localRotation;
        }

        public void Update()
        {
            bool conditionsToAim =
            InputManager.Press("Aim") &&
            !PlayerController.isRunning && PlayerCamera.cursorLocked &&
            !weapon.isReloading && !WeaponManager.Retract.isRetracting;

            bool conditionsToReset =
            (weapon.transform.localPosition != initialAimPos || weapon.transform.localRotation != initialAimRot) &&
            !conditionsToAim;

            if (conditionsToAim)
            {
                isActive = true;

                PlayerCamera.senseScale = data.aimSenseScale;
                WeaponManager.Sway.swayScale = data.aimSwayScale;
                WeaponManager.Sway.swayHorScale = 0f;

                weapon.transform.localPosition = Vector3.Lerp(weapon.transform.localPosition, data.aimPosition, data.aimSpeed * Time.deltaTime);
                weapon.transform.localRotation = Quaternion.Slerp(weapon.transform.localRotation, data.aimRotation, data.aimSpeed * Time.deltaTime);
            }
            else
            {
                PlayerCamera.senseScale = 1;
                WeaponManager.Sway.swayHorScale = WeaponManager.Sway.defaultSwayHorScale;

                isActive = false;
            }

            if (conditionsToReset)
            {
                WeaponManager.Sway.swayScale = 1;
                weapon.transform.localPosition = Vector3.Lerp(weapon.transform.localPosition, initialAimPos, data.aimSpeed * Time.deltaTime);
                weapon.transform.localRotation = Quaternion.Slerp(weapon.transform.localRotation, initialAimRot, data.aimSpeed * Time.deltaTime);
            }
        }
    }
}