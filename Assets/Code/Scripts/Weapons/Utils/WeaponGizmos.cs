using UnityEngine;

namespace Code.Weapons.Utils
{
    [RequireComponent(typeof(Weapon))]
    public class WeaponGizmos : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private bool showGizmos;

        [Header("Colors")]
        [SerializeField] private Color bulletTracingColor = new Color32(255, 0, 0, 255);
        [SerializeField] private Color effectiveDistanceColor = new Color32(0, 255, 0, 255);

        public Weapon weapon { get; private set; }

        private void OnValidate()
        {
            if (weapon != null) return;
            GetComponents();
        }

        private void GetComponents()
        {
            weapon = GetComponent<Weapon>();
        }

        private void OnDrawGizmos()
        {
            if (!showGizmos) return;

            Gizmos.color = effectiveDistanceColor;
            Vector3 p1 = weapon.fireRoot.position;
            Vector3 predictedBulletVelocity = weapon.fireRoot.forward * weapon.data.maxBulletDistance;
            float stepSize = 0.01f;

            for (float step = 0f; step < 1; step += stepSize)
            {
                if (step > (weapon.data.effectiveBulletDistance / weapon.data.maxBulletDistance))
                {
                    Gizmos.color = bulletTracingColor;
                    predictedBulletVelocity += (Physics.gravity * stepSize) * weapon.data.bulletGravityScale;
                }

                Vector3 p2 = p1 + predictedBulletVelocity * stepSize;
                Gizmos.DrawLine(p1, p2);
                p1 = p2;
            }
        }
    }
}