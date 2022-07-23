using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Tracer : MonoBehaviour
{
    [Header("[Tracer Settings]")]
    [SerializeField] private float tracerSpeed;
    [SerializeField] public List<Vector3> pos;

    float timer;
    LineRenderer LineRenderer;

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

                timer = tracerSpeed;
            }

            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
        }
    }
}