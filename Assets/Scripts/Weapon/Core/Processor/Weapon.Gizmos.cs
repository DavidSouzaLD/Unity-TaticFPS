using UnityEngine;

namespace Game.WeaponSystem
{
    public partial class Weapon
    {
        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
            {
                if (fireRoot != null)
                {
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
    }
}