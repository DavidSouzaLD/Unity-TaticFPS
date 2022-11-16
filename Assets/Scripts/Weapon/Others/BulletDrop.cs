using UnityEngine;

namespace Game.Weapon.Others
{
    public class BulletDrop : MonoBehaviour
    {
        [Header("Settings")]
        public GameObject shellPrefab;

        public void Drop()
        {
            GameObject projectile = Instantiate(shellPrefab, transform.position, transform.rotation);
        }

        private void OnDrawGizmosSelected()
        {
            if (transform != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawRay(transform.position, transform.forward);
            }
        }
    }
}