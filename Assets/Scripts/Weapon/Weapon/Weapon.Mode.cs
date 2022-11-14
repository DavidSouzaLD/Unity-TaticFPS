using Game.Player;

namespace Game.Weapon
{
    // Fire
    public partial class Weapon
    {
        private void UpdateMode()
        {
            bool inputConditions = PlayerKeys.Click("Secure");
            bool stateConditions = !isReloading && !isAiming && !isFiring && !isDrawing && !isHiding;
            bool conditions = inputConditions && stateConditions;

            if (conditions)
            {
                switch (weaponMode)
                {
                    case WeaponMode.Secure: weaponMode = WeaponMode.Combat; break;
                    case WeaponMode.Combat: weaponMode = WeaponMode.Secure; break;
                }

                isSafety = weaponMode == WeaponMode.Secure;

                // Delegate
                onSecureChanged?.Invoke();
            }
        }
    }
}