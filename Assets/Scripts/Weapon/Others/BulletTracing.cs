using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem.Others
{
    [RequireComponent(typeof(LineRenderer))]
    public class BulletTracing : MonoBehaviour
    {
        [Header("Settings")]
        public float destroyTime = 0.01f;
        public List<Vector3> pos { get; set; }

        private float tracingTimer;
        private LineRenderer lineRenderer;

        private void Start()
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        private void LateUpdate()
        {
            if (lineRenderer == null) return;

            tracingTimer = Mathf.Clamp(tracingTimer, 0, float.MaxValue);

            if (tracingTimer <= 0)
            {
                if (pos.Count > 1)
                {
                    pos.RemoveAt(0);
                }
                else
                {
                    Destroy(gameObject);
                }

                lineRenderer.positionCount = pos.Count;
                lineRenderer.SetPositions(pos.ToArray());
                tracingTimer = destroyTime;
            }

            if (tracingTimer > 0)
            {
                tracingTimer -= Time.deltaTime;
            }
        }
    }
}