using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapon
{
    [RequireComponent(typeof(LineRenderer))]
    public class WeaponTracer : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float tracerDestroySpeed;
        [HideInInspector] public List<Vector3> pos;

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