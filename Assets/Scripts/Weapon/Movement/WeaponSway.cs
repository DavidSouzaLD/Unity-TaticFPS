using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float amount;
    [SerializeField] private float smooth;
    [SerializeField] private float resetSpeed;
    [Space]
    [SerializeField, Range(0f, 1f)] private float sensitivityX = 1f;
    [SerializeField, Range(0f, 1f)] private float sensitivityY = 1f;

    float accuracy;
    Vector3 startPos;
    Quaternion startRot;
    Player Player;

    public void SetAccuracy(float value = 0f) => accuracy = Mathf.Clamp(value, 0f, 1f);
    public void SetMaxAccuracy() => accuracy = 1;

    private void Start()
    {
        Player = GetComponentInParent<Player>();

        startPos = transform.localPosition;
        startRot = transform.localRotation;

        SetMaxAccuracy();
    }

    private void Update()
    {
        if (Player.Camera.CursorLocked)
        {
            Vector2 cameraAxis = new Vector2(PlayerInput.Keys.CameraAxis.x * sensitivityX, PlayerInput.Keys.CameraAxis.y * sensitivityY) * accuracy;

            if (cameraAxis != Vector2.zero)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(-cameraAxis.x * amount, -cameraAxis.y * amount), smooth * Time.deltaTime);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(-cameraAxis.x * amount, -cameraAxis.y * amount, transform.localRotation.z), smooth * Time.deltaTime);
            }

            if (transform.localPosition != startPos || transform.localRotation != startRot || accuracy != 1f)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, startPos, resetSpeed * Time.deltaTime);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, startRot, resetSpeed * Time.deltaTime);
            }
        }
    }
}
