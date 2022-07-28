using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum WeaponMode { Safety, Combat }
    public enum FireMode { Semi, Auto }
    public string weaponName = "Weapon";

    [Header("Settings")]
    public LayerMask hittableMask;
    public WeaponMode weaponMode;
    public FireMode fireMode;
    public Transform muzzlePoint;
    public float firerate = 1f;

    [Header("Switch")]
    public float drawTime;
    public float hideTime;

    [Header("Data")]
    public int bulletsPerMagazine = 12;
    public int currentBullets = 12;
    public int extraBullets = 24;

    [Header("Bullet")]
    public float bulletHitForce = 100f;
    public float bulletVelocity = 25f;
    public int bulletsPerFire = 1;
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
    public AudioClip initialReloadSound;
    public AudioClip middleReloadSound;
    public AudioClip endReloadSound;

    // Default
    [HideInInspector] public Transform defaultMuzzlePoint;
    [HideInInspector] public Vector3 defaultAimPos;

    // Privates
    [HideInInspector] public float drawTimer, hideTimer;
    [HideInInspector] public float firerateTimer;
    [HideInInspector] public float firingTimer;
    [HideInInspector] public float aimSensitivityScale;
    [HideInInspector] public Vector3 initialAimPos;
    [HideInInspector] public Vector3 initialRecoilPos;
    [HideInInspector] public Quaternion initialAimRot;
    [HideInInspector] public Quaternion initialRecoilRot;
    [HideInInspector] public Transform recoilRoot;
    [HideInInspector] public Animator Animator;
    [HideInInspector] public AudioSource Source;
    [HideInInspector] public WeaponEvents Events;
    [HideInInspector] public WeaponState States;
    [HideInInspector] public WeaponFunctions Functions;

    private void Awake()
    {
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
        SwitchUpdate();
        Fire();
        Reload();
        Aim();
        Recoil();
        Mode();
    }

    private void SwitchUpdate()
    {
        States.SetState("Drawing", drawTimer > 0);
        States.SetState("Hiding", hideTimer > 0);

        if (drawTimer > 0)
        {
            drawTimer -= Time.deltaTime;
        }

        if (hideTimer > 0)
        {
            hideTimer -= Time.deltaTime;
        }
    }

    private void Fire()
    {
        bool conditions =
        (weaponMode == WeaponMode.Combat) &&
        !PlayerController.GetStates.GetState("Running") &&
        !States.GetState("Drawing") &&
        !States.GetState("Hiding") &&
        !States.GetState("Reloading") &&
        PlayerCamera.isCursorLocked;

        if (PlayerInput.WeaponFireTap)
        {
            if (conditions)
            {
                switch (fireMode)
                {
                    case FireMode.Semi:
                        firingTimer = firerate * 3f;
                        Functions.CalculateFire();
                        break;

                    case FireMode.Auto:
                        firingTimer = firerate * 3f;
                        Functions.CalculateFire();
                        break;
                }
            }
            Animator.SetBool("NoBullet", !Functions.HaveBullets());
        }

        // Is firing
        if (firingTimer >= 0)
        {
            States.SetState("Firing", true);
            firingTimer -= Time.deltaTime;
        }
        else
        {
            States.SetState("Firing", false);
        }

        if (firerateTimer >= 0)
        {
            firerateTimer -= Time.deltaTime;
        }
    }

    private void Reload()
    {
        bool conditions =
        (weaponMode == WeaponMode.Combat) &&
        !PlayerController.GetStates.GetState("Running") &&
        !States.GetState("Reloading") &&
        !States.GetState("Drawing") &&
        !States.GetState("Hiding") &&
        PlayerInput.WeaponReload &&
        PlayerCamera.isCursorLocked &&
        extraBullets > 0 &&
        currentBullets < bulletsPerMagazine;

        if (conditions)
        {
            // Animation no-bullet
            Functions.PlayAnimation("Reload");
        }
    }

    private void Aim()
    {
        bool conditions =
        (weaponMode == WeaponMode.Combat) &&
        !PlayerController.GetStates.GetState("Running") &&
        !States.GetState("Reloading") &&
        !States.GetState("Drawing") &&
        !States.GetState("Hiding") &&
        PlayerInput.WeaponAim &&
        PlayerCamera.isCursorLocked;

        if (conditions)
        {
            States.SetState("Aiming", true);

            PlayerCamera.SetSensitivityScale(aimSensitivityScale);
            WeaponManager.Sway.SwayAccuracy(aimSwayScale);

            transform.localPosition = Vector3.Lerp(transform.localPosition, aimPosition, aimSpeed * Time.deltaTime);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, aimRotation, aimSpeed * Time.deltaTime);
        }
        else
        {
            States.SetState("Aiming", false);
            PlayerCamera.MaxSensitivityScale();
        }

        bool resetConditions =
        (!PlayerInput.WeaponAim || PlayerInput.Run) &&
        (transform.localPosition != initialAimPos ||
        transform.localRotation != initialAimRot) ||
        (weaponMode == WeaponMode.Safety);

        if (resetConditions)
        {
            WeaponManager.Sway.MaxAccuracy();
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialAimPos, aimSpeed * Time.deltaTime);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, initialAimRot, aimSpeed * Time.deltaTime);
        }
    }
    private void Recoil()
    {
        if (recoilRoot.localPosition != initialRecoilPos || recoilRoot.localRotation != initialRecoilRot)
        {
            recoilRoot.localPosition = Vector3.Lerp(recoilRoot.localPosition, initialRecoilPos, recoilResetSpeed * Time.deltaTime);
            recoilRoot.localRotation = Quaternion.Slerp(recoilRoot.localRotation, initialRecoilRot, recoilResetSpeed * Time.deltaTime);
        }
    }

    private void Mode()
    {
        bool conditions =
        !States.GetState("Reloading") &&
        !States.GetState("Aiming") &&
        !States.GetState("Firing") &&
        !States.GetState("Drawing") &&
        !States.GetState("Hiding");

        if (PlayerInput.WeaponSafety)
        {
            switch (weaponMode)
            {
                case WeaponMode.Safety: weaponMode = WeaponMode.Combat; break;
                case WeaponMode.Combat: weaponMode = WeaponMode.Safety; break;
            }

            States.SetState("Safety", weaponMode == WeaponMode.Safety);
        }
    }

    private void OnEnable()
    {
        // Setting switch values
        drawTimer = drawTime;
        hideTimer = hideTime;
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