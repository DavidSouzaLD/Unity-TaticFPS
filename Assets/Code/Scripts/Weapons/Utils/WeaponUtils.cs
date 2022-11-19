using System.Collections.Generic;

namespace Code.Weapons.Utils
{
    public static class WeaponUtils
    {
        public static List<Weapon.Magazine> CreateMagazinePackage(int quantity, int bulletsQuantity)
        {
            List<Weapon.Magazine> magazines = new List<Weapon.Magazine>();

            for (int i = 0; i < quantity; i++)
            {
                Weapon.Magazine newMagazine = new Weapon.Magazine();
                newMagazine.bullets = bulletsQuantity;
                magazines.Add(newMagazine);
            }

            return magazines;
        }
    }
}