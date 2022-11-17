using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem.Core
{
    public static class WeaponUtils
    {
        public static WeaponMagazine[] CreateMagazines(int quantity, int bulletsPerMagazine)
        {
            List<WeaponMagazine> magazines = new List<WeaponMagazine>();

            for (int i = 0; i < quantity; i++)
            {
                magazines.Add(new WeaponMagazine(bulletsPerMagazine));
            }

            return magazines.ToArray();
        }
    }
}