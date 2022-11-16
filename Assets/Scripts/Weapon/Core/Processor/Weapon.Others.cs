namespace Game.Weapon
{
    public partial class Weapon
    {
        private void OnEnable()
        {
            if (WeaponManager.Instance.currentWeapon != this)
            {
                WeaponManager.Instance.currentWeapon = this;
            }
        }

        private void OnDisable()
        {
            if (WeaponManager.Instance.currentWeapon == this)
            {
                WeaponManager.Instance.currentWeapon = null;
            }
        }

        private void OnDestroy()
        {
            if (WeaponManager.Instance.currentWeapon == this)
            {
                WeaponManager.Instance.currentWeapon = null;
            }
        }
    }
}