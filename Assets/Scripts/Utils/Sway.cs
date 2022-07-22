using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sway : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float amount;
    [SerializeField] private float smooth;
    [SerializeField] private float resetSpeed;
    [Space]
    [SerializeField, Range(0f, 1f)] private float sensitivityX = 1f;
    [SerializeField, Range(0f, 1f)] private float sensitivityY = 1f;

    Vector3 startPos;
    Quaternion startRot;

    private void Start()
    {
        transform.localPosition = startPos;
        transform.localRotation = startRot;
    }

    private void Update()
    {
        Vector2 cameraAxis = new Vector2(PlayerInput.Keys.CameraAxis.x * sensitivityX, PlayerInput.Keys.CameraAxis.y * sensitivityY);

        if (cameraAxis != Vector2.zero)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(-cameraAxis.x * amount, -cameraAxis.y * amount), smooth * Time.deltaTime);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(-cameraAxis.x * amount, -cameraAxis.y * amount, transform.localRotation.z), smooth * Time.deltaTime);
        }

        if (transform.localPosition != startPos || transform.localRotation != startRot)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, startPos, resetSpeed * Time.deltaTime);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, startRot, resetSpeed * Time.deltaTime);
        }
    }
}
