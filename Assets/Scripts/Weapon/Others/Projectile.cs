using UnityEngine;

namespace Game.Weapon
{
    public class Projectile : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private Transform dropLocation;

        public void Drop()
        {
            GameObject projectile = Instantiate(projectilePrefab, dropLocation.position, dropLocation.rotation);
        }

        private void OnDrawGizmosSelected()
        {
            if (dropLocation != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawRay(dropLocation.position, dropLocation.forward);
            }
        }
    }
}