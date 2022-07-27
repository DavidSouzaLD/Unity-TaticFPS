using UnityEngine;

[DisallowMultipleComponent]
public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance;
    public static bool haveCurrentWeapon = Instance != null && Instance.currentWeapon != null;

    [System.Serializable]
    public class Impact
    {
        public string name;
        public GameObject prefab;
    }

    [Header("Basic")]
    [SerializeField] private Weapon currentWeapon;
    [SerializeField] private GameObject tracerPrefab;

    [Header("Sway")]
    [SerializeField] private Vector2 swayMultiplier;
    [SerializeField] private float swayAmount = 0.1f;
    [SerializeField] private float swaySmooth = 0.1f;
    [SerializeField] private float swayResetSpeed;
    [SerializeField] private float swayHorizontalScale = 1f;

    [Header("Retract")]
    [SerializeField] private LayerMask rectractMask;
    [SerializeField] private float retractRayDistance;
    [SerializeField] private float retractAngle;
    [SerializeField] private float retractSpeed;

    [Header("HitMark")]
    [SerializeField] private AudioClip hitMarkSound;
    [SerializeField] private float hitMarkTime;

    [Header("Impact")]
    [SerializeField] private Impact[] impacts;

    // Private
    private float swayAccuracy;
    private float timerHitMark;
    private Vector3 swayInitialPos;
    private Quaternion swayInitialRot;
    private Quaternion horSwayInitialRot;
    private Quaternion retractInitialRot;
    private Transform swayRoot;
    private Transform horizontalSwayRoot;
    private Transform retractRayRoot;
    private Transform retracRoot;
    private GameObject hitMark;

    public static bool IsAim
    {
        get
        {
            return haveCurrentWeapon ? Instance.currentWeapon.States.GetState("Aiming") : false;
        }
    }

    public static bool IsReload
    {
        get
        {
            return haveCurrentWeapon ? Instance.currentWeapon.States.GetState("Reloading") : false;
        }
    }

    public static bool IsSafety
    {
        get
        {
            return haveCurrentWeapon ? Instance.currentWeapon.States.GetState("Safety") : false;
        }
    }

    public static Impact GetImpactWithTag(string tag)
    {
        foreach (Impact imp in Instance.impacts)
        {
            if (imp.name == tag)
            {
                return imp;
            }
        }

        return null;
    }

    public static GameObject GetTracerPrefab()
    {
        return Instance.tracerPrefab;
    }

    public static AudioClip GetHitMarkSound()
    {
        return Instance.hitMarkSound;
    }

    public static void SwayAccuracy(float value = 0f)
    {
        Instance.swayAccuracy = Mathf.Clamp(value, 0f, 1f);
    }

    public static void MaxAccuracy()
    {
        SwayAccuracy(1f);
    }

    public static void SetCurrentWeapon(Weapon weapon)
    {
        Instance.currentWeapon = weapon;
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
        // Get components
        swayRoot = FindManager.Find("Sway");
        horizontalSwayRoot = FindManager.Find("SwayHorizontal");
        retracRoot = FindManager.Find("Retract");
        retractRayRoot = FindManager.Find("Camera");
        hitMark = FindManager.Find("HitMark").gameObject;

        // Setting start values
        swayInitialPos = swayRoot.localPosition;
        swayInitialRot = swayRoot.localRotation;
        horSwayInitialRot = horizontalSwayRoot.localRotation;
        retractInitialRot = retracRoot.localRotation;

        // Others
        hitMark.SetActive(false);
        MaxAccuracy();
    }

    private void Update()
    {
        SwayUpdate();
        RetractUpdate();
        HitmarkUpdate();
    }

    private void SwayUpdate()
    {
        Vector2 cameraAxis = new Vector2(InputManager.CameraAxis.x * swayMultiplier.x, InputManager.CameraAxis.y * swayMultiplier.y) * swayAccuracy;
        float cameraAxisX = InputManager.MoveAxis.x * swayMultiplier.x * swayAccuracy;

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

    private void RetractUpdate()
    {
        RaycastHit hit;
        Debug.DrawRay(retractRayRoot.position, retractRayRoot.forward * retractRayDistance, Color.red);

        if (Physics.Raycast(retractRayRoot.position, retractRayRoot.forward, out hit, retractRayDistance, rectractMask))
        {
            if (hit.transform)
            {
                Quaternion targetRot = Quaternion.Euler(new Vector3((retractAngle / hit.distance), retractInitialRot.y, retractInitialRot.z));
                retracRoot.localRotation = Quaternion.Slerp(retracRoot.localRotation, targetRot, retractSpeed * Time.deltaTime);
            }
        }

        retracRoot.localRotation = Quaternion.Slerp(retracRoot.localRotation, retractInitialRot, retractSpeed * Time.deltaTime);
    }

    private void HitmarkUpdate()
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

    public static void ApplyHitMark(Vector3 _position)
    {
        if (Instance.timerHitMark <= 0)
        {
            Instance.hitMark.transform.position = _position;
            Instance.hitMark.SetActive(true);
            Instance.timerHitMark = Instance.hitMarkTime;
        }
    }
}