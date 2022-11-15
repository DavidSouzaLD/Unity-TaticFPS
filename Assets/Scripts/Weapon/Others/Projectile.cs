using UnityEngine;

namespace Game.Weapon
{
    public class Projectile : MonoBehaviour
    {
        [Header("Settings")]
        public GameObject projectilePrefab;
        public Transform dropTransform;

        public void Drop()
        {
            GameObject projectile = Instantiate(projectilePrefab, dropTransform.position, dropTransform.rotation);
        }

        private void OnDrawGizmosSelected()
        {
            if (dropTransform != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawRay(dropTransform.position, dropTransform.forward);
            }
        }
    }
}