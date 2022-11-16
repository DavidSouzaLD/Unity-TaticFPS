using System.Collections;
using UnityEngine;

namespace Game.Weapon
{
    public partial class Weapon
    {
        private IEnumerator ApplyMuzzle(float muzzleTime)
        {
            muzzleObject.SetActive(true);
            yield return new WaitForSeconds(muzzleTime);
            muzzleObject.SetActive(false);
        }

        private IEnumerator HideEnumerator()
        {
            isHiding = true;
            yield return new WaitForSeconds(data.hideTime);
            Destroy(gameObject);
        }
    }
}