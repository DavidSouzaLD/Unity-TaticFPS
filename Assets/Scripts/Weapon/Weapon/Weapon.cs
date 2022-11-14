using UnityEngine;
using Game.Player;

namespace Game.Weapon
{
    public partial class Weapon : MonoBehaviour
    {
        [Header("Settings")]
        public WeaponMode weaponMode;
        public WeaponFireMode fireMode;
        public float firerate = 1f;

        [Header("Data")]
        public int bulletsPerMagazine = 12;
        public int currentBullets = 12;
        public int extraBullets = 24;

        [Header("Aim")]
        public float aimSpeed = 5f;
        public Vector3 aimPosition;
        public Quaternion aimRotation;
        [Space]
        [Range(0f, 1f)] public float aimSensitivityScale = 1f;
        [Range(0f, 1f)] public float aimSwayScale = 0.2f;

        [Header("Damage")]
        public float minDamage = 35f;
        public float maxDamage = 60f;

        [Header("Switch")]
        public float drawTime;
        public float hideTime;

        [Header("Bullet")]
        public float bulletHitForce = 100f;
        public float bulletVelocity = 25f;
        public int bulletsPerFire = 1;
        public float bulletGravityScale = 1;
        public float maxBulletDistance = 100f;
        public float effectiveDistance = 70f;

        [Header("Recoil")]
        public Vector3 recoilForcePos;
        public Vector3 recoilForceRot;
        public Vector3 recoilForceCam;

        [Header("Sounds")]
        public AudioClip[] fireSounds;
        public AudioClip noBulletSound;
        public AudioClip startReloadSound;
        public AudioClip middleReloadSound;
        public AudioClip endReloadSound;

        [Header("Locations")]
        public Transform fireLocation;

        public AudioClip overrideFireSound { get; set; }

        // Delegate
        public delegate void firing();
        public delegate void secureChanged();
        public delegate void startReload();
        public delegate void endReload();

        // Delegate callbacks
        public firing onFiring;
        public secureChanged onSecureChanged;
        public startReload onStartReload;
        public endReload onEndReload;

        // Status
        public bool isFiring { get; private set; }
        public bool isAiming { get; private set; }
        public bool isReloading { get; set; }
        public bool isDrawing { get; private set; }
        public bool isHiding { get; private set; }
        public bool isSafety { get; private set; }
        public bool canFire { get; private set; }

        // Private
        private float drawTimer;
        private float hideTimer;
        private float firerateTimer;
        private float firingTimer;
        private Vector3 initialAimPos;
        private Quaternion initialAimRot;
        private Vector3 defaultAimPos;
        private Quaternion defaultAimRot;
        private Transform defaultFireLocation;

        // Components
        private WeaponSway weaponSway;
        private WeaponRecoil weaponRecoil;
        private WeaponRetract weaponRetract;
        private WeaponAnimation weaponAnimation;
        private WeaponSound weaponSound;
        private Projectile projectile;

        public bool haveBullets { get { return currentBullets > 0; } }

        public void SetMode(WeaponMode mode)
        {
            weaponMode = mode;
        }

        public void ResetAim()
        {
            aimPosition = defaultAimPos;
            aimRotation = defaultAimRot;
        }

        public void ResetFireLocation()
        {
            fireLocation = defaultFireLocation;
        }

        private void Start()
        {
            // Default
            defaultFireLocation = fireLocation;
            defaultAimPos = aimPosition;
            defaultAimRot = aimRotation;

            // Aim
            initialAimPos = transform.localPosition;
            initialAimRot = transform.localRotation;

            // Delegates
            onFiring?.Invoke();
            onSecureChanged?.Invoke();

            // Get components
            weaponSway = GetComponentInParent<WeaponSway>();
            weaponRecoil = GetComponentInParent<WeaponRecoil>();
            weaponRetract = GetComponentInParent<WeaponRetract>();
            weaponAnimation = GetComponentInChildren<WeaponAnimation>();
            weaponSound = GetComponentInChildren<WeaponSound>();
            projectile = GetComponentInChildren<Projectile>();

            // Setting
            weaponAnimation.Init(this, weaponSound);
            weaponSound.Init(this);
        }

        private void OnEnable()
        {
            WeaponManager.CurrentWeapon = this;
        }

        private void Update()
        {
            UpdateFire();
            UpdateReload();
            UpdateAim();
            UpdateMode();
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
            {
                if (fireLocation != null)
                {
                    Gizmos.color = Color.green;
                    Vector3 point1 = fireLocation.position;
                    Vector3 predictedBulletVelocity = fireLocation.forward * maxBulletDistance;
                    float stepSize = 0.01f;

                    for (float step = 0f; step < 1; step += stepSize)
                    {
                        if (step > (effectiveDistance / maxBulletDistance))
                        {
                            Gizmos.color = Color.red;
                            predictedBulletVelocity += (Physics.gravity * stepSize) * bulletGravityScale;
                        }

                        Vector3 point2 = point1 + predictedBulletVelocity * stepSize;
                        Gizmos.DrawLine(point1, point2);
                        point1 = point2;
                    }
                }
            }
        }
    }
}