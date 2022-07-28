using UnityEngine;
using UnityEngine.Rendering;

[DisallowMultipleComponent]
[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(Volume))]
public class PlayerCamera : MonoBehaviour
{
    private static PlayerCamera Instance;
    public static bool isCursorLocked = true;

    [Header("Camera")]
    [SerializeField] private float camSmooth = 20f;
    [SerializeField] private Vector2 sensitivity = new Vector2(0.15f, 0.15f);
    [SerializeField] private Vector2 clampVertical = new Vector2(-90f, 90f);
    [SerializeField] private VolumeProfile[] profiles;

    [Header("Recoil")]
    [SerializeField] private float recoilSpeed = 8f;
    [SerializeField] private float resetSpeed = 5f;

    // Private
    private float sensitivityScale;
    private Vector2 camRot;
    private Vector3 currentRotation;
    private Vector3 targetRotation;
    private Quaternion characterTargetRot;
    private Quaternion camTargetRot;
    private Transform camRecoilRoot;
    private Transform playerTransform;
    private Volume Volume;
    private Camera Camera;

    public static Camera GetCamera
    {
        get
        {
            return Instance.Camera;
        }
    }
    public static Vector3 WorldToScreen(Vector3 _position)
    {
        return Instance.Camera.WorldToScreenPoint(_position);
    }

    public static void SetSensitivityScale(float scale)
    {
        Instance.sensitivityScale = Mathf.Clamp(scale, 0, 1);
    }

    public static void MaxSensitivityScale()
    {
        Instance.sensitivityScale = 1f;
    }

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
        playerTransform = FindManager.Find("Player");
        camRecoilRoot = FindManager.Find("CameraRecoil");

        // Starting values
        characterTargetRot = playerTransform.localRotation;
        camTargetRot = Camera.transform.localRotation;

        MaxSensitivityScale();
    }

    private void Update()
    {
        // Camera
        if (Camera)
        {
            Vector2 camAxis = PlayerInput.CameraAxis;

            camRot.x = (-camAxis.y * sensitivity.y) * sensitivityScale;
            camRot.y = (camAxis.x * sensitivity.x) * sensitivityScale;

            characterTargetRot *= Quaternion.Euler(0f, camRot.y, 0f);
            camTargetRot *= Quaternion.Euler(camRot.x, 0f, 0f);

            camTargetRot = RotationExtension.Clamp(camTargetRot, clampVertical.x, clampVertical.y, RotationExtension.Axis.X);

            playerTransform.localRotation = Quaternion.Slerp(playerTransform.localRotation, characterTargetRot, camSmooth * Time.deltaTime);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, camTargetRot, camSmooth * Time.deltaTime);
        }

        // Recoil
        if (camRecoilRoot)
        {
            targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, resetSpeed * Time.deltaTime);
            currentRotation = Vector3.Slerp(currentRotation, targetRotation, recoilSpeed * Time.deltaTime);
            camRecoilRoot.localRotation = Quaternion.Euler(currentRotation);
        }
    }

    public static void ApplyRecoil(Vector3 _recoil)
    {
        Instance.targetRotation += new Vector3(-_recoil.x, Random.Range(-_recoil.y, _recoil.y), Random.Range(-_recoil.z, _recoil.z));
    }

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

    public static void LockCursor(bool value)
    {
        // Lock cursor in game
        if (value)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            isCursorLocked = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            isCursorLocked = false;
        }
    }
}
