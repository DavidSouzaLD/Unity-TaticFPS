using System.Collections;
using UnityEngine;

namespace Game.WeaponSystem
{
    public partial class Weapon
    {
        private void OnEnable()
        {
            isFiring = false;
            isAiming = false;
            isReloading = false;
            isSafety = false;
        }

        private void OnDisable()
        {
            isFiring = false;
            isAiming = false;
            isReloading = false;
            isSafety = false;
        }
    }
}