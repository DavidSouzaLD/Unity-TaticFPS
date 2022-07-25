using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance;

    [System.Serializable]
    public class Impact
    {
        public string name;
        public GameObject prefab;
    }

    [Header("Basic")]

    /// <summary>
    /// List of impacts by tag.
    /// </summary>
    [SerializeField] private Impact[] Impacts;

    /// <summary>
    /// Ray tracer bullet prefab.
    /// </summary>
    [SerializeField] private GameObject tracerPrefab;

    [Header("Sway")]

    /// <summary>
    /// Maximum amount of sway movement.
    /// </summary>
    [SerializeField] private float swayAmount = 0.1f;

    /// <summary>
    /// Smooth sway motion speed.
    /// </summary>
    [SerializeField] private float swaySmooth = 0.1f;

    /// <summary>
    /// Horizontal sway scale that multiplies the current sway value.
    /// </summary>
    [SerializeField] private float horizontalSwayScale = 1f;

    [Space]

    /// <summary>
    /// Sway reset speed;
    /// </summary>
    [SerializeField] private float swayResetSpeed;

    /// <summary>
    /// Multiplies the sway axis.
    /// </summary>
    [SerializeField] private Vector2 swayMultiplier;

    [Header("Retract")]

    /// <summary>
    /// Layers that accept retract.
    /// </summary>
    [SerializeField] private LayerMask retractLayers;

    /// <summary>
    /// Maximum distance to check the retract.
    /// </summary>
    [SerializeField] private float retractRayDistance;

    /// <summary>
    /// Maximum retraction angle
    /// </summary>
    [SerializeField] private float retractAngle;

    /// <summary>
    /// Retraction speed
    /// </summary>
    [SerializeField] private float retractSpeed;

    [Header("HitMark")]

    /// <summary>
    /// Hit mark prefab
    /// </summary>
    [SerializeField] private GameObject hitMark;

    /// <summary>
    /// Hit mark sound
    /// </summary>
    [SerializeField] private AudioClip hitMarkSound;

    /// <summary>
    /// Hit mark duration time.
    /// </summary>
    [SerializeField] private float hitMarkTime;

    // Private
    private float swayAccuracy; // Sway scale.
    private float timerHitMark; // Time counter hit mark.
    private Vector3 swayInitialPos; // Initial position of main sway root.
    private Quaternion swayInitialRot; // Initial rotation of main sway root.
    private Quaternion horSwayInitialRot; // Initial rotation of horizontal sway root.
    private Quaternion retractInitialRot; // Initial rotation of retract root.
    private Transform swayRoot; // Root of the main sway.
    private Transform horizontalSwayRoot; // Root of the horizontal sway.
    private Transform retractRayRoot; // Point to cast retraction raycast.
    private Transform retracRoot; // Object to apply retraction.
    private Player Player; // Player component for states conditions.

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

    /// <summary>
    /// Returns the tracer prefab.
    /// </summary>
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
        // Get component
        Player = GetComponentInParent<Player>();

        // Roots
        swayRoot = GameObject.Find("Sway").transform;
        horizontalSwayRoot = GameObject.Find("SwayHorizontal").transform;

        // Setting start values
        swayInitialPos = swayRoot.localPosition;
        swayInitialRot = swayRoot.localRotation;
        horSwayInitialRot = horizontalSwayRoot.localRotation;
        retractInitialRot = retracRoot.localRotation;

        // Setting maximum accuracy
        MaxAccuracy();

        // Error
        if (swayRoot == null || horizontalSwayRoot == null)
        {
            Debug.LogError("SwayRoot or HorizontalSwayRoot not assigned, solve please.");
        }
    }

    private void Update()
    {
        Sway();
        Retract();
        ResetSway();
        ResetHitMark();
    }

    private void Sway()
    {
        if (StateLock.IsLocked("CURSOR_LOCKED"))
        {
            Vector2 cameraAxis = new Vector2(Input.CameraAxis.x * swayMultiplier.x, Input.CameraAxis.y * swayMultiplier.y) * swayAccuracy;
            float cameraAxisX = Input.MoveAxis.x * swayMultiplier.x * swayAccuracy;

            // Basic sway
            if (cameraAxis != Vector2.zero)
            {
                swayRoot.localPosition = Vector3.Lerp(swayRoot.localPosition, new Vector3(-cameraAxis.x * swayAmount, -cameraAxis.y * swayAmount), swaySmooth * Time.deltaTime);
                swayRoot.localRotation = Quaternion.Slerp(swayRoot.localRotation, Quaternion.Euler(-cameraAxis.x * swayAmount, -cameraAxis.y * swayAmount, swayRoot.localRotation.z), swaySmooth * Time.deltaTime);
            }

            // Horizontal sway
            if (cameraAxisX != 0f)
            {
                horizontalSwayRoot.localRotation = Quaternion.Slerp(horizontalSwayRoot.localRotation, Quaternion.Euler(horizontalSwayRoot.localRotation.x, horizontalSwayRoot.localRotation.y, -cameraAxisX * swayAmount * horizontalSwayScale), swaySmooth * horizontalSwayScale * Time.deltaTime);
            }
        }
    }

    private void Retract()
    {
        RaycastHit hit;
        Debug.DrawRay(retractRayRoot.position, retractRayRoot.forward * retractRayDistance, Color.red);

        if (Physics.Raycast(retractRayRoot.position, retractRayRoot.forward, out hit, retractRayDistance, retractLayers))
        {
            if (hit.transform)
            {
                StateLock.Lock("WEAPON_ALL", true);
                Quaternion targetRot = Quaternion.Euler(new Vector3((retractAngle / hit.distance), retractInitialRot.y, retractInitialRot.z));
                retracRoot.localRotation = Quaternion.Slerp(retracRoot.localRotation, targetRot, retractSpeed * Time.deltaTime);
            }
        }
        else
        {
            StateLock.Lock("WEAPON_ALL", false);
        }

        retracRoot.localRotation = Quaternion.Slerp(retracRoot.localRotation, retractInitialRot, retractSpeed * Time.deltaTime);
    }

    private void ResetSway()
    {
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