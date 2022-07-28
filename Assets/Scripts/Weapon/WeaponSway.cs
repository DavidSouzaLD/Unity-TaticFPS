using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    private static WeaponSway Instance;

    [Header("Settings")]
    [SerializeField] private Vector2 swayMultiplier;
    [SerializeField] private float swayAmount = 0.1f;
    [SerializeField] private float swaySmooth = 0.1f;
    [SerializeField] private float swayResetSpeed;
    [SerializeField] private float swayHorizontalScale = 1f;

    private float swayAccuracy;
    private Vector3 swayInitialPos;
    private Quaternion swayInitialRot;
    private Quaternion horSwayInitialRot;
    private Transform swayRoot;
    private Transform horizontalSwayRoot;

    public void SwayAccuracy(float value = 0f)
    {
        swayAccuracy = Mathf.Clamp(value, 0f, 1f);
    }

    public void MaxAccuracy()
    {
        SwayAccuracy(1f);
    }

    private void Start()
    {
        // Get components
        swayRoot = FindManager.Find("Sway");
        horizontalSwayRoot = FindManager.Find("SwayHorizontal");

        // Setting start values
        swayInitialPos = swayRoot.localPosition;
        swayInitialRot = swayRoot.localRotation;
        horSwayInitialRot = horizontalSwayRoot.localRotation;

        MaxAccuracy();
    }

    private void Update()
    {
        Vector2 cameraAxis = new Vector2(PlayerInput.CameraAxis.x * swayMultiplier.x, PlayerInput.CameraAxis.y * swayMultiplier.y) * swayAccuracy;
        float cameraAxisX = PlayerInput.MoveAxis.x * swayMultiplier.x * swayAccuracy;

        if (cameraAxis != Vector2.zero)
        {
            swayRoot.localPosition = Vector3.Lerp(swayRoot.localPosition, new Vector3(-cameraAxis.x * swayAmount, -cameraAxis.y * swayAmount), swaySmooth * Time.deltaTime);
            swayRoot.localRotation = Quaternion.Slerp(swayRoot.localRotation, Quaternion.Euler(-cameraAxis.x * swayAmount, -cameraAxis.y * swayAmount, swayRoot.localRotation.z), swaySmooth * Time.deltaTime);
        }

        if (cameraAxisX != 0f)
        {
            horizontalSwayRoot.localRotation = Quaternion.Slerp(horizontalSwayRoot.localRotation, Quaternion.Euler(horizontalSwayRoot.localRotation.x, horizontalSwayRoot.localRotation.y, -cameraAxisX * swayAmount * swayHorizontalScale), swaySmooth * swayHorizontalScale * Time.deltaTime);
        }

        if (horizontalSwayRoot.localRotation != horSwayInitialRot || swayAccuracy != 1f)
        {
            horizontalSwayRoot.localRotation = Quaternion.Slerp(horizontalSwayRoot.localRotation, horSwayInitialRot, swayResetSpeed * Time.deltaTime);
        }

        if (swayRoot.localPosition != swayInitialPos || swayRoot.localRotation != swayInitialRot || swayAccuracy != 1f)
        {
            swayRoot.localPosition = Vector3.Lerp(swayRoot.localPosition, swayInitialPos, swayResetSpeed * Time.deltaTime);
            swayRoot.localRotation = Quaternion.Slerp(swayRoot.localRotation, swayInitialRot, swayResetSpeed * Time.deltaTime);
        }
    }
}
