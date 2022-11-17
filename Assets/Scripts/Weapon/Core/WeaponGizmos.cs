using UnityEngine;

namespace WeaponSystem.Core
{
    [RequireComponent(typeof(Weapon))]
    public class WeaponGizmos : MonoBehaviour
    {
        [Header("Settings")]
        public WeaponSO data;
        public bool showGizmos;
        private Transform fireRoot;

        private void OnValidate()
        {
            if (fireRoot == null)
            {
                fireRoot = GetComponent<Weapon>()?.fireRoot;
            }
        }

        private void OnDrawGizmos()
        {
            if (showGizmos == false || data == null || Application.isPlaying || fireRoot == null) return;

            // Bullet tracing preview
            Gizmos.color = Color.green;
            Vector3 point1 = fireRoot.position;
            Vector3 predictedBulletVelocity = fireRoot.forward * data.maxBulletDistance;
            float stepSize = 0.01f;

            for (float step = 0f; step < 1; step += stepSize)
            {
                if (step > (data.effectiveDistance / data.maxBulletDistance))
                {
                    Gizmos.color = Color.red;
                    predictedBulletVelocity += (Physics.gravity * stepSize) * data.bulletGravityScale;
                }

                Vector3 point2 = point1 + predictedBulletVelocity * stepSize;
                Gizmos.DrawLine(point1, point2);
                point1 = point2;
            }
        }
    }
}