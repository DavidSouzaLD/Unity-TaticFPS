using UnityEngine;

namespace WeaponSystem.Others
{
    [RequireComponent(typeof(LineRenderer))]
    public class BulletDropping : MonoBehaviour
    {
        [Header("Settings")]
        public GameObject bulletPrefab;

        public void Drop()
        {
            GameObject projectile = Instantiate(bulletPrefab, transform.position, transform.rotation);
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