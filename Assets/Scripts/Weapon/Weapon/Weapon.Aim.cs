using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Player;

namespace Game.Weapon
{
    // Fire
    public partial class Weapon
    {
        private void UpdateAim()
        {
            bool inputConditions = PlayerKeys.Press("Aim");
            bool modeConditions = (weaponMode == WeaponMode.Combat);
            bool playerConditions = !PlayerController.isRunning && PlayerCamera.cursorLocked;
            bool weaponManagerConditions = !weaponRetract.isRetracting;
            bool stateConditions = !isReloading && !isDrawing && !isHiding;
            bool conditions = inputConditions && modeConditions && playerConditions && weaponManagerConditions && stateConditions;

            if (conditions)
            {
                isAiming = true;

                PlayerCamera.sensitivityScale = aimSensitivityScale;
                weaponSway.swayAccuracy = aimSwayScale;

                transform.localPosition = Vector3.Lerp(transform.localPosition, aimPosition, aimSpeed * Time.deltaTime);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, aimRotation, aimSpeed * Time.deltaTime);
            }
            else
            {
                PlayerCamera.sensitivityScale = 1;
                isAiming = false;
            }

            bool differenceConditions = (transform.localPosition != initialAimPos || transform.localRotation != initialAimRot);
            bool resetConditions = !conditions && differenceConditions;

            if (resetConditions)
            {
                weaponSway.swayAccuracy = 1; ;
                transform.localPosition = Vector3.Lerp(transform.localPosition, initialAimPos, aimSpeed * Time.deltaTime);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, initialAimRot, aimSpeed * Time.deltaTime);
            }
        }
    }
}