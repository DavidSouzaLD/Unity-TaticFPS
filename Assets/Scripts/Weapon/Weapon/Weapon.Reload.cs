using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Player;

namespace Game.Weapon
{
    // Fire
    public partial class Weapon
    {
        public void UpdateReload()
        {
            bool inputConditions = PlayerKeys.Click("Reload");
            bool modeConditions = (weaponMode == WeaponMode.Combat);
            bool playerConditions = !PlayerController.isRunning && PlayerCamera.cursorLocked;
            bool weaponManagerConditions = !weaponRetract.isRetracting;
            bool stateConditions = !isReloading && !isDrawing && !isHiding;
            bool reloadConditions = extraBullets > 0 && currentBullets < bulletsPerMagazine;
            bool conditions = inputConditions && modeConditions && playerConditions && weaponManagerConditions && stateConditions && reloadConditions;

            if (conditions)
            {
                // Animation no-bullet
                weaponAnimation.Play("Reload");
            }
        }

        public void CalculateReload()
        {
            if (currentBullets < bulletsPerMagazine)
            {
                int necessaryBullets = 0;

                for (int i = 0; i < bulletsPerMagazine; i++)
                {
                    if ((currentBullets + necessaryBullets) < bulletsPerMagazine)
                    {
                        necessaryBullets++;
                    }
                }

                if (extraBullets >= necessaryBullets)
                {
                    currentBullets += necessaryBullets;
                    extraBullets -= necessaryBullets;
                }
                else if (extraBullets < necessaryBullets)
                {
                    necessaryBullets = extraBullets;
                    currentBullets += necessaryBullets;
                    extraBullets = 0;
                }
            }
        }
    }
}