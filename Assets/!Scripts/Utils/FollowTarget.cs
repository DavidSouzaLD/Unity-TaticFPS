using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private bool update, fixedUpdate, lateUpdate;

    [Header("Smooth")]
    [SerializeField] private bool useSmooth;
    [SerializeField] private float smoothSpeed;

    private void Update()
    {
        if (update)
        {
            UpdateMove();
        }
    }

    private void FixedUpdate()
    {
        if (fixedUpdate)
        {
            UpdateMove();
        }
    }

    private void LateUpdate()
    {
        if (lateUpdate)
        {
            UpdateMove();
        }
    }

    private void UpdateMove()
    {
        if (!useSmooth)
        {
            transform.position = target.position + offset;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, target.position + offset, smoothSpeed * Time.deltaTime);
        }
    }
}
