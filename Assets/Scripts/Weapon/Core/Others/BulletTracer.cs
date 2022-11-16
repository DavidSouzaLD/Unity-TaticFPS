using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapon.Others
{
    [RequireComponent(typeof(LineRenderer))]
    public class BulletTracer : MonoBehaviour
    {
        [Header("Settings")]
        public float tracerDestroySpeed;
        public List<Vector3> pos { get; set; }

        // Private
        private float timer;
        private LineRenderer LineRenderer;

        private void Start()
        {
            LineRenderer = GetComponent<LineRenderer>();
        }

        private void LateUpdate()
        {
            if (LineRenderer)
            {
                timer = Mathf.Clamp(timer, 0, float.MaxValue);

                if (timer <= 0)
                {
                    if (pos.Count > 1)
                    {
                        pos.RemoveAt(0);
                    }
                    else
                    {
                        Destroy(gameObject);
                    }

                    LineRenderer.positionCount = pos.Count;
                    LineRenderer.SetPositions(pos.ToArray());

                    timer = tracerDestroySpeed;
                }

                if (timer > 0)
                {
                    timer -= Time.deltaTime;
                }
            }
        }
    }
}