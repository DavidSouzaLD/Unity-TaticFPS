using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapons
{
    public class WeaponProjectile : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private Transform dropRoot;

        public void Drop()
        {
            GameObject projectile = Instantiate(projectilePrefab, dropRoot.position, dropRoot.rotation);

        }

        private void OnDrawGizmosSelected()
        {
            if (dropRoot != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawRay(dropRoot.position, dropRoot.forward);
            }
        }
    }
}