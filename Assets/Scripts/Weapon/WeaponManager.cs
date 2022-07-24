using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Global;

    #region Sway

    [Header("[Sway Settings]")]
    [SerializeField] private Transform swayTransform;
    [SerializeField] private Transform horizontalSwayTransform;
    [SerializeField] private float sway_Amount = 0.1f;
    [SerializeField] private float sway_Smooth = 0.1f;
    [SerializeField] private float horizontalSway_Scale = 1f;
    [Space]
    [SerializeField] private float resetSpeed;
    [SerializeField, Range(0f, 1f)] private float sensitivityX = 1f;
    [SerializeField, Range(0f, 1f)] private float sensitivityY = 1f;

    private float sway_Accuracy;
    private Vector3 sway_initialPos;
    private Quaternion sway_initialRot;
    private Quaternion horizontalSway_initialRot;
    private Player Player;
    public static void SetAccuracy(float value = 0f) => Global.sway_Accuracy = Mathf.Clamp(value, 0f, 1f);
    public static void SetMaxAccuracy() => Global.sway_Accuracy = 1;

    #endregion

    #region Retract

    [Header("[Retract Settings]")]
    [SerializeField] private Transform retractRayTransform;
    [SerializeField] private Transform retractTransform;
    [SerializeField] private LayerMask retractLayers;
    [SerializeField] private float retractDistance;
    [SerializeField] private float retractAngle;
    [SerializeField] private float retractSpeed;
    private Quaternion retract_initialRot;

    #endregion

    #region Hit Impact

    [System.Serializable]
    public class Impact
    {
        public string name;
        public GameObject prefab;
    }

    [Header("[HitImpact Settings]")]
    public Impact[] Impacts;

    public static Impact GetImpactWithTag(string _tag)
    {
        foreach (Impact imp in Global.Impacts)
        {
            if (imp.name == _tag)
            {
                return imp;
            }
        }

        return null;
    }

    #endregion

    [Space]

    [Header("[Tracer Settings]")]
    public GameObject tracerPrefab;

    public static GameObject GetTracerPrefab
    {
        get
        {
            return Global.tracerPrefab;
        }
    }

    [Header("[HitMark Settings]")]
    public GameObject hitMark;
    public float hitMarkTime;
    public float timerHitMark;

    private void Awake()
    {
        if (Global == null)
        {
            Global = this;
        }
    }

    private void Start()
    {
        // Sway
        Player = GetComponentInParent<Player>();

        sway_initialPos = swayTransform.localPosition;
        sway_initialRot = swayTransform.localRotation;
        horizontalSway_initialRot = horizontalSwayTransform.localRotation;
        retract_initialRot = retractTransform.localRotation;

        SetMaxAccuracy();
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
            Vector2 cameraAxis = new Vector2(Input.CameraAxis.x * sensitivityX, Input.CameraAxis.y * sensitivityY) * sway_Accuracy;
            float cameraAxisX = Input.MoveAxis.x * sensitivityX * sway_Accuracy;

            // Basic sway
            if (cameraAxis != Vector2.zero)
            {
                swayTransform.localPosition = Vector3.Lerp(swayTransform.localPosition, new Vector3(-cameraAxis.x * sway_Amount, -cameraAxis.y * sway_Amount), sway_Smooth * Time.deltaTime);
                swayTransform.localRotation = Quaternion.Slerp(swayTransform.localRotation, Quaternion.Euler(-cameraAxis.x * sway_Amount, -cameraAxis.y * sway_Amount, swayTransform.localRotation.z), sway_Smooth * Time.deltaTime);
            }

            // Horizontal sway
            if (cameraAxisX != 0f)
            {
                horizontalSwayTransform.localRotation = Quaternion.Slerp(horizontalSwayTransform.localRotation, Quaternion.Euler(horizontalSwayTransform.localRotation.x, horizontalSwayTransform.localRotation.y, -cameraAxisX * sway_Amount * horizontalSway_Scale), sway_Smooth * horizontalSway_Scale * Time.deltaTime);
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
                Quaternion targetRot = Quaternion.Euler(new Vector3((retractAngle / hit.distance), retract_initialRot.y, retract_initialRot.z));
                retractTransform.localRotation = Quaternion.Slerp(retractTransform.localRotation, targetRot, retractSpeed * Time.deltaTime);
            }
        }
        else
        {
            StateLock.Lock("WEAPON_ALL", false);
        }

        retractTransform.localRotation = Quaternion.Slerp(retractTransform.localRotation, retract_initialRot, retractSpeed * Time.deltaTime);
    }

    private void ResetSway()
    {
        if (horizontalSwayTransform.localRotation != horizontalSway_initialRot || sway_Accuracy != 1f)
        {
            horizontalSwayTransform.localRotation = Quaternion.Slerp(horizontalSwayTransform.localRotation, horizontalSway_initialRot, resetSpeed * Time.deltaTime);
        }

        if (swayTransform.localPosition != sway_initialPos || swayTransform.localRotation != sway_initialRot || sway_Accuracy != 1f)
        {
            swayTransform.localPosition = Vector3.Lerp(swayTransform.localPosition, sway_initialPos, resetSpeed * Time.deltaTime);
            swayTransform.localRotation = Quaternion.Slerp(swayTransform.localRotation, sway_initialRot, resetSpeed * Time.deltaTime);
        }
    }

    public static void ApplyHitMark(Vector3 _position)
    {
        if (Global.timerHitMark <= 0)
        {
            Global.hitMark.transform.position = _position;
            Global.hitMark.SetActive(true);
            Global.timerHitMark = Global.hitMarkTime;
        }
    }

    private void ResetHitMark()
    {
        if (timerHitMark > 0)
        {
            if (timerHitMark - Time.deltaTime <= 0)
            {
                Global.hitMark.SetActive(false);
            }

            timerHitMark -= Time.deltaTime;
        }
    }
}