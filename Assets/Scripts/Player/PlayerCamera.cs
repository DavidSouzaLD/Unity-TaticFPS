using UnityEngine;
using UnityEngine.Rendering;

[DisallowMultipleComponent]
[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(Volume))]
public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera Instance;

    [Header("[Camera Settings]")]
    public bool cursorLocked;
    public float cameraSmooth = 20f;
    public Vector2 sensitivity = new Vector2(0.15f, 0.15f);
    public Vector2 clampVertical = new Vector2(-90f, 90f);
    public VolumeProfile[] profiles;

    [Header("[Recoil Settings]")]
    public Transform recoilTransform;
    public float snappiness;
    public float resetSpeed;
    Vector3 currentRotation;
    Vector3 targetRotation;

    // Private
    private Vector2 _cameraRot = Vector2.zero;
    private Quaternion _characterTargetRot, _cameraTargetRot;
    private Transform PlayerTransform;
    private Volume Volume;
    public static Camera Camera;

    /// <summary>
    /// Returns whether the cursor is locked or not
    /// </summary>
    public static bool CursorLocked
    {
        get
        {
            return Instance.cursorLocked;
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        // Components
        PlayerTransform = GetComponentInParent<Player>().transform;
        Volume = GetComponent<Volume>();
        Camera = GetComponent<Camera>();

        // Starting values
        _characterTargetRot = PlayerTransform.localRotation;
        _cameraTargetRot = Camera.transform.localRotation;
    }

    /// <summary>
    /// Update camera movement
    /// </summary>
    private void Update()
    {
        // Camera
        if (Camera)
        {
            StateLock.Lock("CURSOR_LOCKED", cursorLocked);

            if (cursorLocked)
            {
                Vector2 cameraAxis = PlayerInput.Keys.CameraAxis;

                _cameraRot.x = -cameraAxis.y * sensitivity.y;
                _cameraRot.y = cameraAxis.x * sensitivity.x;

                _characterTargetRot *= Quaternion.Euler(0f, _cameraRot.y, 0f);
                _cameraTargetRot *= Quaternion.Euler(_cameraRot.x, 0f, 0f);

                _cameraTargetRot = Rotation.Clamp(_cameraTargetRot, clampVertical.x, clampVertical.y, Rotation.Axis.X);

                PlayerTransform.localRotation = Quaternion.Slerp(PlayerTransform.localRotation, _characterTargetRot, cameraSmooth * Time.deltaTime);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, _cameraTargetRot, cameraSmooth * Time.deltaTime);
            }
        }

        // Recoil
        if (recoilTransform)
        {
            targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, resetSpeed * Time.deltaTime);
            currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.deltaTime);
            recoilTransform.localRotation = Quaternion.Euler(currentRotation);
        }
    }

    /// <summary>
    /// Apply recoil to camera
    /// </summary>
    public static void ApplyRecoil(Vector3 _recoil)
    {
        Instance.targetRotation += new Vector3(-_recoil.x, Random.Range(-_recoil.y, _recoil.y), Random.Range(-_recoil.z, _recoil.z));
    }

    public static void ApplyShake()
    {

    }

    /// <summary>
    /// Apply values in PostProcessing [BASE/NIGHTVISION/CUSTOM]
    /// </summary>
    public static void ApplyVolume(string _volumeName)
    {
        string volumeName = _volumeName.ToUpper();

        switch (volumeName)
        {
            case "BASE":
                Instance.Volume.profile = Instance.profiles[0];
                break;

            case "NIGHTVISION":
                Instance.Volume.profile = Instance.profiles[1];
                break;

            case "CUSTOM":
                Instance.Volume.profile = Instance.profiles[2];
                break;
        }
    }

    /// <summary>
    /// Returns the current volume value
    /// </summary>
    public static string GetVolume()
    {
        string volumeName = "";

        if (Instance.Volume.profile == Instance.profiles[0])
        {
            volumeName = "BASE";
        }

        if (Instance.Volume.profile == Instance.profiles[1])
        {
            volumeName = "NIGHTVISION";
        }

        if (Instance.Volume.profile == Instance.profiles[2])
        {
            volumeName = "CUSTOM";
        }

        return volumeName;
    }

    /// <summary>
    /// Lock the cursor
    /// </summary>
    public static void LockCursor(bool value)
    {
        // Lock cursor in game
        if (value)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Instance.cursorLocked = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Instance.cursorLocked = false;
        }
    }

    public static Vector3 WorldToScreen(Vector3 _position)
    {
        return Camera.WorldToScreenPoint(_position);
    }
}
