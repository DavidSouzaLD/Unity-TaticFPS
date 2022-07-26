using UnityEngine;
using UnityEngine.Rendering;

[DisallowMultipleComponent]
[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(Volume))]
public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera Instance;

    [Header("Camera")]

    /// <summary>
    /// Lock the cursor on the screen.
    /// </summary>
    [SerializeField] private bool isCursorLocked = true;

    /// <summary>
    /// Smoothing camera movement.
    /// </summary>
    [SerializeField] private float cameraSmooth = 20f;

    /// <summary>
    /// Mouse/Gamepad sensitivity for camera.
    /// </summary>
    [SerializeField] private Vector2 sensitivity = new Vector2(0.15f, 0.15f);

    /// <summary>
    /// Axis limitation for camera.
    /// </summary>
    [SerializeField] private Vector2 clampVertical = new Vector2(-90f, 90f);

    /// <summary>
    /// Post processing profiles to activate via script.
    /// </summary>
    [SerializeField] private VolumeProfile[] profiles;

    [Header("Recoil")]

    /// <summary>
    /// Speed at which recoil is applied.
    /// </summary>
    [SerializeField] private float recoilSpeed = 8f;

    /// <summary>
    /// Speed with which the recoil returns to its initial position.
    /// </summary>
    [SerializeField] private float resetSpeed = 5f;

    // Private
    private float sensitivityScale;
    private Vector2 cameraRot;
    private Vector3 currentRotation;
    private Vector3 targetRotation;
    private Quaternion characterTargetRot;
    private Quaternion cameraTargetRot;
    private Transform cameraRecoilRoot;
    private Transform PlayerTransform;
    private Volume Volume;
    private Camera Camera;

    public static bool IsCursorLocked() => Instance.isCursorLocked;
    public static void SetSensitivityScale(float scale) => Instance.sensitivityScale = Mathf.Clamp(scale, 0, 1);
    public static void MaxSensitivityScale() => Instance.sensitivityScale = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        // Components
        Volume = GetComponent<Volume>();
        Camera = GetComponent<Camera>();

        // Get transforms
        PlayerTransform = FindManager.Find("Player", this);
        cameraRecoilRoot = FindManager.Find("CameraRecoil", this);

        // Starting values
        characterTargetRot = PlayerTransform.localRotation;
        cameraTargetRot = Camera.transform.localRotation;

        MaxSensitivityScale();

        if (cameraRecoilRoot == null)
        {
            DebugManager.DebugAssignedError("CameraRecoilRoot");
        }
    }

    private void Update()
    {
        // Camera
        if (Camera)
        {
            if (!LockManager.IsLocked("PLAYER_ALL") && !LockManager.IsLocked("PLAYER_CAMERA"))
            {
                Vector2 cameraAxis = InputManager.CameraAxis;

                cameraRot.x = (-cameraAxis.y * sensitivity.y) * sensitivityScale;
                cameraRot.y = (cameraAxis.x * sensitivity.x) * sensitivityScale;

                characterTargetRot *= Quaternion.Euler(0f, cameraRot.y, 0f);
                cameraTargetRot *= Quaternion.Euler(cameraRot.x, 0f, 0f);

                cameraTargetRot = RotationExtension.Clamp(cameraTargetRot, clampVertical.x, clampVertical.y, RotationExtension.Axis.X);

                PlayerTransform.localRotation = Quaternion.Slerp(PlayerTransform.localRotation, characterTargetRot, cameraSmooth * Time.deltaTime);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, cameraTargetRot, cameraSmooth * Time.deltaTime);
            }
        }

        // Cursor 
        LockManager.Lock("CAMERA", "CURSOR_LOCKED", isCursorLocked);

        if (LockManager.IsLocked("CURSOR_LOCKED"))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Instance.isCursorLocked = false;
        }

        // Recoil
        if (cameraRecoilRoot)
        {
            targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, resetSpeed * Time.deltaTime);
            currentRotation = Vector3.Slerp(currentRotation, targetRotation, recoilSpeed * Time.deltaTime);
            cameraRecoilRoot.localRotation = Quaternion.Euler(currentRotation);
        }
    }

    /// <summary>
    /// Apply recoil to camera.
    /// </summary>
    public static void ApplyRecoil(Vector3 _recoil)
    {
        Instance.targetRotation += new Vector3(-_recoil.x, Random.Range(-_recoil.y, _recoil.y), Random.Range(-_recoil.z, _recoil.z));
    }

    /// <summary>
    /// Apply shake to camera.
    /// </summary>
    public static void ApplyShake()
    {

    }

    /// <summary>
    /// Apply values in PostProcessing [BASE/NIGHTVISION/CUSTOM].
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
    /// Returns the current volume value.
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
    /// Lock the cursor.
    /// </summary>
    public static void LockCursor(bool value)
    {
        // Lock cursor in game
        if (value)
        {
            Instance.isCursorLocked = true;
        }
        else
        {
            Instance.isCursorLocked = false;
        }
    }

    /// <summary>
    /// Returns the point in the UI based on the submitted position.
    /// </summary>
    public static Vector3 WorldToScreen(Vector3 _position)
    {
        return Instance.Camera.WorldToScreenPoint(_position);
    }
}
