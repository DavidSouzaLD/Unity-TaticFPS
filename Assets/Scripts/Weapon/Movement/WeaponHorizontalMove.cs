using UnityEngine;

public class WeaponHorizontalMove : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float amount;
    [SerializeField] private float smooth;
    [SerializeField] private float resetSpeed;
    [SerializeField, Range(0f, 1f)] private float sensitivityX = 1f;

    float accuracy;
    Quaternion startRot;

    public void SetAccuracy(float value = 0f) => accuracy = Mathf.Clamp(value, 0f, 1f);
    public void SetMaxAccuracy() => accuracy = 1;

    private void Start()
    {
        startRot = transform.localRotation;
        SetMaxAccuracy();
    }

    private void Update()
    {
        float cameraAxis = PlayerInput.Keys.MoveAxis.x * sensitivityX * accuracy;

        if (cameraAxis != 0f)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, -cameraAxis * amount), smooth * Time.deltaTime);
        }

        if (transform.localRotation != startRot || accuracy != 1f)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, startRot, resetSpeed * Time.deltaTime);
        }
    }
}
