namespace WeaponSystem.Core
{
    [System.Serializable]
    public class WeaponMagazine
    {
        public WeaponMagazine(int bulletsToMagazine)
        {
            currentBullets = bulletsToMagazine;
        }

        public int currentBullets;
    }
}