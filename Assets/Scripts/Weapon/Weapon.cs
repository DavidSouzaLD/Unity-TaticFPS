using UnityEngine;

public class Weapon : MonoBehaviour
{
    private static Weapon Instance;
    public enum FireMode { Semi, Auto }

    [Header("Settings")]
    public LayerMask hittableMask;
    public FireMode fireMode;
    public Transform muzzlePoint;
    public float firerate = 1f;

    [Header("Data")]
    public int bulletsPerMagazine = 12;
    public int currentBullets = 12;
    public int extraBullets = 24;

    [Header("Bullet")]
    public float bulletHitForce = 100f;
    public float bulletVelocity = 25f;
    public int bulletsPerFire = 1;

    [SerializeField, Range(0, 20)]
    public float bulletGravityScale = 1;
    public float maxBulletDistance;

    [Header("Aim")]
    public float aimSpeed = 5f;
    public float aimSwayScale = 0.2f;
    public Vector3 aimPosition;
    public Quaternion aimRotation;

    [Header("Recoil")]
    public float recoilResetSpeed = 5f;
    public Vector3 recoilForcePos;
    public Vector3 recoilForceRot;
    public Vector3 recoilForceCamera;

    [Header("Sounds")]
    public AudioClip[] fireSounds;
    public AudioClip noBulletSound;
    public AudioClip startReloadSound;
    public AudioClip middleReloadSound;
    public AudioClip endReloadSound;

    // Default
    [HideInInspector] public Transform defaultMuzzlePoint;
    [HideInInspector] public Vector3 defaultAimPos;

    // Privates
    [HideInInspector] public float firerateTimer;
    [HideInInspector] public float aimSensitivityScale;
    [HideInInspector] public Vector3 startAimPos;
    [HideInInspector] public Vector3 startRecoilPos;
    [HideInInspector] public Quaternion startAimRot;
    [HideInInspector] public Quaternion startRecoilRot;
    [HideInInspector] public Transform recoilRoot;
    [HideInInspector] public WeaponEvents Events;
    [HideInInspector] public Animator Animator;
    [HideInInspector] public AudioSource Source;
    [HideInInspector] public WeaponState States;
    [HideInInspector] public WeaponFunctions Functions;

    private void Awake()
    {
        // Create instance
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        // Weapon settings
        States = new WeaponState();
        Functions = new WeaponFunctions(this);
    }

    private void Start()
    {
        Functions.Start();
    }

    private void Update()
    {
        Fire();
        Reload();
        Aim();
        Recoil();
    }

    private void Fire()
    {
        bool conditions = !PlayerController.GetStates.GetState("Running") && PlayerCamera.isCursorLocked && !States.GetState("Reloading");

        if (conditions)
        {
            switch (fireMode)
            {
                case FireMode.Semi:

                    if (InputManager.FireTap)
                    {
                        Functions.CalculateFire();
                    }

                    break;

                case FireMode.Auto:

                    if (InputManager.FireAuto)
                    {
                        Functions.CalculateFire();
                    }

                    break;
            }

            if (firerateTimer >= 0)
            {
                firerateTimer -= Time.deltaTime;
            }
        }
    }

    private void Reload()
    {
        bool conditions = InputManager.Reload && !PlayerController.GetStates.GetState("Running") && PlayerCamera.isCursorLocked && extraBullets > 0 && currentBullets < bulletsPerMagazine && !States.GetState("Reloading");

        if (conditions)
        {
            Functions.PlayAnimation("RELOAD");
        }

        // Animation no-bullet
        Animator.SetBool("NO_BULLET", !Functions.HaveBullets());
    }

    private void Aim()
    {
        bool conditions = InputManager.Aim && !PlayerController.GetStates.GetState("Running") && PlayerCamera.isCursorLocked && !States.GetState("Reloading");

        if (conditions)
        {
            States.SetState("Aiming", true);

            PlayerCamera.SetSensitivityScale(aimSensitivityScale);
            WeaponManager.SwayAccuracy(aimSwayScale);

            transform.localPosition = Vector3.Lerp(transform.localPosition, aimPosition, aimSpeed * Time.deltaTime);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, aimRotation, aimSpeed * Time.deltaTime);
        }
        else
        {
            States.SetState("Aiming", false);
            PlayerCamera.MaxSensitivityScale();
        }

        bool resetConditions = (!InputManager.Aim || InputManager.Run) && (transform.localPosition != startAimPos || transform.localRotation != startAimRot);

        if (resetConditions)
        {
            WeaponManager.MaxAccuracy();
            transform.localPosition = Vector3.Lerp(transform.localPosition, startAimPos, aimSpeed * Time.deltaTime);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, startAimRot, aimSpeed * Time.deltaTime);
        }
    }
    private void Recoil()
    {
        // Reset recoil
        if (recoilRoot.localPosition != startRecoilPos || recoilRoot.localRotation != startRecoilRot)
        {
            recoilRoot.localPosition = Vector3.Lerp(recoilRoot.localPosition, startRecoilPos, recoilResetSpeed * Time.deltaTime);
            recoilRoot.localRotation = Quaternion.Slerp(recoilRoot.localRotation, startRecoilRot, recoilResetSpeed * Time.deltaTime);
        }
    }

    private void OnEnable()
    {
        WeaponManager.SetCurrentWeapon(this);
    }

    private void OnDisable()
    {
        WeaponManager.SetCurrentWeapon(null);
    }

    private void OnDrawGizmos()
    {
        if (muzzlePoint)
        {
            Gizmos.color = Color.red;
            Vector3 point1 = muzzlePoint.position;
            Vector3 predictedBulletVelocity = muzzlePoint.forward * maxBulletDistance;
            float stepSize = 0.01f;

            for (float step = 0f; step < 1; step += stepSize)
            {
                predictedBulletVelocity += (Physics.gravity * stepSize) * bulletGravityScale;
                Vector3 point2 = point1 + predictedBulletVelocity * stepSize;
                Gizmos.DrawLine(point1, point2);
                point1 = point2;
            }
        }
    }
}