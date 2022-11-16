using System.Collections;
using UnityEngine;

namespace Game.WeaponSystem
{
    public partial class Weapon
    {
        private IEnumerator ApplyMuzzle(float muzzleTime)
        {
            muzzleObject.SetActive(true);
            yield return new WaitForSeconds(muzzleTime);
            muzzleObject.SetActive(false);
        }
    }
}