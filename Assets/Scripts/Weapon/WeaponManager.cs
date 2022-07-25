using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance;

    [Header("[Basic Settings]")]
    [SerializeField] private GameObject tracerPrefab;
    [SerializeField] private Impact[] Impacts;

    [Header("[Sway Settings]")]
    [SerializeField] private Transform swayTransform;
    [SerializeField] private Transform horizontalSwayTransform;
    [SerializeField] private float swayAmount = 0.1f;
    [SerializeField] private float swaySmooth = 0.1f;
    [SerializeField] private float hswayScale = 1f;
    [Space]
    [SerializeField] private float resetSpeed;
    [SerializeField] private Vector2 swayMultiplier;

    [Header("[Retract Settings]")]
    [SerializeField] private LayerMask retractLayers;
    [SerializeField] private Transform retractRayTransform;
    [SerializeField] private Transform retractTransform;
    [SerializeField] private float retractDistance;
    [SerializeField] private float retractAngle;
    [SerializeField] private float retractSpeed;

    [System.Serializable]
    public class Impact
    {
        public string name;
        public GameObject prefab;
    }

    [Header("[HitMark Settings]")]
    [SerializeField] private GameObject hitMark;
    [SerializeField] private AudioClip hitMarkSound;
    [SerializeField] private float hitMarkTime;

    // Private
    private float swayAccuracy;
    private float timerHitMark;
    private Vector3 swayInitialPos;
    private Quaternion swayInitialRot;
    private Quaternion hswayInitialRot;
    private Quaternion retractInitialRot;
    private Player PlayerCode;

    /// <summary>
    /// Precision that the sway will work from (0 to 1).
    /// </summary>
    public static void SwayAccuracy(float value = 0f)
    {
        Instance.swayAccuracy = Mathf.Clamp(value, 0f, 1f);
    }

    /// <summary>
    /// Leave the sway accuracy at maximum. (1)
    /// </summary>
    public static void MaxAccuracy()
    {
        SwayAccuracy(1f);
    }

    /// <summary>
    /// Returns the type of impact based on the object's tag.
    /// </summary>
    public static Impact GetImpactWithTag(string tag)
    {
        foreach (Impact imp in Instance.Impacts)
        {
            if (imp.name == tag)
            {
                return imp;
            }
        }

        return null;
    }

    /// <summary>
    /// Returns the hitmark sound.
    /// </summary>
    public static AudioClip GetHitMarkSound()
    {
        return Instance.hitMarkSound;
    }

    public static GameObject GetTracerPrefab
    {
        get
        {
            return Instance.tracerPrefab;
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
        // Sway
        PlayerCode = GetComponentInParent<Player>();

        swayInitialPos = swayTransform.localPosition;
        swayInitialRot = swayTransform.localRotation;
        hswayInitialRot = horizontalSwayTransform.localRotation;
        retractInitialRot = retractTransform.localRotation;

        MaxAccuracy();
    }

    private void Update()
    {
        SwayUpdate();
        RetractUpdate();
        ResetSway();
        ResetHitMark();
    }

    private void SwayUpdate()
    {
        if (StateLock.IsLocked("CURSOR_LOCKED"))
        {
            Vector2 cameraAxis = new Vector2(Input.CameraAxis.x * swayMultiplier.x, Input.CameraAxis.y * swayMultiplier.y) * swayAccuracy;
            float cameraAxisX = Input.MoveAxis.x * swayMultiplier.x * swayAccuracy;

            // Basic sway
            if (cameraAxis != Vector2.zero)
            {
                swayTransform.localPosition = Vector3.Lerp(swayTransform.localPosition, new Vector3(-cameraAxis.x * swayAmount, -cameraAxis.y * swayAmount), swaySmooth * Time.deltaTime);
                swayTransform.localRotation = Quaternion.Slerp(swayTransform.localRotation, Quaternion.Euler(-cameraAxis.x * swayAmount, -cameraAxis.y * swayAmount, swayTransform.localRotation.z), swaySmooth * Time.deltaTime);
            }

            // Horizontal sway
            if (cameraAxisX != 0f)
            {
                horizontalSwayTransform.localRotation = Quaternion.Slerp(horizontalSwayTransform.localRotation, Quaternion.Euler(horizontalSwayTransform.localRotation.x, horizontalSwayTransform.localRotation.y, -cameraAxisX * swayAmount * hswayScale), swaySmooth * hswayScale * Time.deltaTime);
            }
        }
    }

    private void RetractUpdate()
    {
        RaycastHit hit;
        Debug.DrawRay(retractRayTransform.position, retractRayTransform.forward * retractDistance, Color.red);

        if (Physics.Raycast(retractRayTransform.position, retractRayTransform.forward, out hit, retractDistance, retractLayers))
        {
            if (hit.transform)
            {
                StateLock.Lock("WEAPON_ALL", true);
                Quaternion targetRot = Quaternion.Euler(new Vector3((retractAngle / hit.distance), retractInitialRot.y, retractInitialRot.z));
                retractTransform.localRotation = Quaternion.Slerp(retractTransform.localRotation, targetRot, retractSpeed * Time.deltaTime);
            }
        }
        else
        {
            StateLock.Lock("WEAPON_ALL", false);
        }

        retractTransform.localRotation = Quaternion.Slerp(retractTransform.localRotation, retractInitialRot, retractSpeed * Time.deltaTime);
    }

    private void ResetSway()
    {
        if (horizontalSwayTransform.localRotation != hswayInitialRot || swayAccuracy != 1f)
        {
            horizontalSwayTransform.localRotation = Quaternion.Slerp(horizontalSwayTransform.localRotation, hswayInitialRot, resetSpeed * Time.deltaTime);
        }

        if (swayTransform.localPosition != swayInitialPos || swayTransform.localRotation != swayInitialRot || swayAccuracy != 1f)
        {
            swayTransform.localPosition = Vector3.Lerp(swayTransform.localPosition, swayInitialPos, resetSpeed * Time.deltaTime);
            swayTransform.localRotation = Quaternion.Slerp(swayTransform.localRotation, swayInitialRot, resetSpeed * Time.deltaTime);
        }
    }

    public static void ApplyHitMark(Vector3 _position)
    {
        if (Instance.timerHitMark <= 0)
        {
            Instance.hitMark.transform.position = _position;
            Instance.hitMark.SetActive(true);
            Instance.timerHitMark = Instance.hitMarkTime;
        }
    }

    private void ResetHitMark()
    {
        if (timerHitMark > 0)
        {
            if (timerHitMark - Time.deltaTime <= 0)
            {
                Instance.hitMark.SetActive(false);
            }

            timerHitMark -= Time.deltaTime;
        }
    }
}