using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Game.Player;

namespace Game.Weapon
{
    public partial class Weapon : MonoBehaviour
    {
        [Header("Settings")]
        public WeaponData data;
        public Transform fireTransform;
        public MuzzleFlash muzzleObject;

        [Header("Data")]
        public int bulletsPerMagazine = 12;
        public int currentBullets = 12;
        public int extraBullets = 24;

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
        private Transform defaultFireTransform;

        // Components
        private WeaponAnimation weaponAnimation;
        private Projectile projectile;

        public bool haveBullets { get { return currentBullets > 0; } }

        public void SetMode(WeaponMode mode)
        {
            data.weaponMode = mode;
        }

        public void ResetAim()
        {
            data.aimPosition = defaultAimPos;
            data.aimRotation = defaultAimRot;
        }

        public void ResetFireTransform()
        {
            fireTransform = defaultFireTransform;
        }

        private void Start()
        {
            // Default
            defaultFireTransform = fireTransform;
            defaultAimPos = data.aimPosition;
            defaultAimRot = data.aimRotation;

            // Aim
            initialAimPos = transform.localPosition;
            initialAimRot = transform.localRotation;

            // Delegates
            onFiring?.Invoke();
            onSecureChanged?.Invoke();

            // Get components
            weaponAnimation = GetComponentInChildren<WeaponAnimation>();
            projectile = GetComponentInChildren<Projectile>();

            // Setting
            weaponAnimation.Init();
        }

        private void OnEnable()
        {
            WeaponManager.Instance.currentWeapon = this;
        }

        private void Update()
        {
            UpdateFire();
            UpdateReload();
            UpdateAim();
            UpdateMode();
        }

        private void UpdateFire()
        {
            // Conditions
            bool modeConditions = (data.weaponMode == WeaponMode.Combat);
            bool playerConditions = !PlayerController.isRunning && PlayerCamera.cursorLocked;
            bool weaponManagerConditions = !Retract.isRetracting;
            bool statusConditions = !isDrawing && !isHiding && !isReloading;
            bool conditions = modeConditions && playerConditions && weaponManagerConditions && statusConditions;

            if (conditions)
            {
                switch (data.fireMode)
                {
                    case WeaponFireMode.Semi:
                        if (PlayerKeys.Click("Fire"))
                        {
                            firingTimer = data.firerate * 3f;
                            CalculateFire();
                        }
                        break;

                    case WeaponFireMode.Auto:
                        if (PlayerKeys.Press("Fire"))
                        {
                            firingTimer = data.firerate * 3f;
                            CalculateFire();
                        }
                        break;
                }
            }

            // Is firing
            if (firingTimer >= 0)
            {
                isFiring = true;
                firingTimer -= Time.deltaTime;
            }
            else
            {
                isFiring = false;
            }

            if (firerateTimer >= 0)
            {
                firerateTimer -= Time.deltaTime;
            }
        }

        public void UpdateReload()
        {
            bool inputConditions = PlayerKeys.Click("Reload");
            bool modeConditions = (data.weaponMode == WeaponMode.Combat);
            bool playerConditions = !PlayerController.isRunning && PlayerCamera.cursorLocked;
            bool weaponManagerConditions = !Retract.isRetracting;
            bool statusConditions = !isReloading && !isDrawing && !isHiding;
            bool reloadConditions = extraBullets > 0 && currentBullets < bulletsPerMagazine;
            bool conditions = inputConditions && modeConditions && playerConditions && weaponManagerConditions && statusConditions && reloadConditions;

            if (conditions)
            {
                // Animation no-bullet
                weaponAnimation.Play("Reload");
            }
        }

        private void UpdateAim()
        {
            bool inputConditions = PlayerKeys.Press("Aim");
            bool modeConditions = (data.weaponMode == WeaponMode.Combat);
            bool playerConditions = !PlayerController.isRunning && PlayerCamera.cursorLocked;
            bool weaponManagerConditions = !Retract.isRetracting;
            bool statusConditions = !isReloading && !isDrawing && !isHiding;
            bool conditions = inputConditions && modeConditions && playerConditions && weaponManagerConditions && statusConditions;

            if (conditions)
            {
                isAiming = true;

                PlayerCamera.sensitivityScale = data.aimSensitivityScale;
                Sway.swayScale = data.aimSwayScale;

                transform.localPosition = Vector3.Lerp(transform.localPosition, data.aimPosition, data.aimSpeed * Time.deltaTime);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, data.aimRotation, data.aimSpeed * Time.deltaTime);
            }
            else
            {
                PlayerCamera.sensitivityScale = 1;
                isAiming = false;
            }

            bool differenceConditions = (transform.localPosition != initialAimPos || transform.localRotation != initialAimRot);
            bool resetConditions = !conditions && differenceConditions;

            if (resetConditions)
            {
                Sway.swayScale = 1;
                transform.localPosition = Vector3.Lerp(transform.localPosition, initialAimPos, data.aimSpeed * Time.deltaTime);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, initialAimRot, data.aimSpeed * Time.deltaTime);
            }
        }

        private void UpdateMode()
        {
            bool inputConditions = PlayerKeys.Click("Secure");
            bool statusConditions = !isReloading && !isAiming && !isFiring && !isDrawing && !isHiding;
            bool conditions = inputConditions && statusConditions;

            if (conditions)
            {
                switch (data.weaponMode)
                {
                    case WeaponMode.Secure: data.weaponMode = WeaponMode.Combat; break;
                    case WeaponMode.Combat: data.weaponMode = WeaponMode.Secure; break;
                }

                isSafety = data.weaponMode == WeaponMode.Secure;

                // Delegate
                onSecureChanged?.Invoke();
            }
        }

        public void CalculateFire()
        {
            if (currentBullets >= data.bulletsPerFire && firerateTimer <= 0)
            {
                // Tracer
                List<Vector3> tracerPositions = new List<Vector3>();
                LineRenderer tracer = GameObject.Instantiate(WeaponManager.Instance.tracerPrefab, fireTransform.position, Quaternion.identity).GetComponent<LineRenderer>();
                BulletTracer tracerScript = tracer.gameObject.GetComponent<BulletTracer>();

                Vector3 point1 = fireTransform.position;
                Vector3 predictedBulletVelocity = fireTransform.forward * data.maxBulletDistance;
                float stepSize = 0.01f;

                // Tracer start position
                tracerPositions.Add(point1);

                for (float step = 0f; step < 1; step += stepSize)
                {
                    predictedBulletVelocity += (Physics.gravity * stepSize) * data.bulletGravityScale;
                    Vector3 point2 = point1 + predictedBulletVelocity * stepSize;
                    tracerPositions.Add(point2);

                    Ray ray = new Ray(point1, point2 - point1);
                    RaycastHit hit;

                    Debug.DrawLine(point1, point2, Color.green);

                    if (Physics.Raycast(ray, out hit, (point2 - point1).magnitude, WeaponManager.Instance.hittableMask))
                    {
                        if (hit.transform)
                        {
                            float distance = (fireTransform.position - hit.point).sqrMagnitude;
                            float time = distance / (data.bulletVelocity * 1000f);
                            StartCoroutine(CalculateDelay(time, hit));
                            break;
                        }
                    }

                    point1 = point2;
                }

                tracerScript.pos = tracerPositions;

                currentBullets -= data.bulletsPerFire;
                firerateTimer = data.firerate;

                // Visuals
                WeaponManager.Instance.PlaySound("Fire");
                weaponAnimation.Play("Fire");

                // Events
                Recoil.ApplyRecoil(data.recoilForcePos, data.recoilForceRot, data.recoilForceCam);
                StartCoroutine(ApplyMuzzle(data.muzzleTime));
                projectile.Drop();

                // Delegate
                onFiring?.Invoke();
            }
        }

        public IEnumerator CalculateDelay(float time, RaycastHit hit)
        {
            yield return new WaitForSeconds(time);

            // Force
            Rigidbody HitBody = hit.transform.GetComponent<Rigidbody>();
            if (HitBody)
            {
                HitBody.AddForceAtPosition(fireTransform.forward * data.bulletHitForce, hit.point);
            }

            // Apply damage to IHealth
            IDamageable<float> hitHealth = hit.collider.GetComponent<IDamageable<float>>();

            if (hitHealth != null)
            {
                hitHealth.TakeDamage(Random.Range(data.minDamage, data.maxDamage));
            }

            // Hitmark in enemy
            if (hit.transform.tag.Equals("Enemy"))
            {
                Hitmark.ApplyHitMark(PlayerCamera.WorldToScreen(hit.point));
                WeaponManager.Instance.PlaySound("Hitmark", 0.3f);
            }

            // Bullet hole
            if (Impacts.GetImpactWithTag(hit.transform.tag) != null)
            {
                GameObject impact = GameObject.Instantiate(Impacts.GetImpactWithTag(hit.transform.tag).prefab, hit.point, Quaternion.LookRotation(hit.normal));
                impact.transform.parent = hit.collider.transform;
                impact.transform.position += impact.transform.forward * 0.001f;

                MonoBehaviour.Destroy(impact, 5f);
            }
        }

        public void CalculateReload()
        {
            if (currentBullets < bulletsPerMagazine)
            {
                int necessaryBullets = 0;

                for (int i = 0; i < bulletsPerMagazine; i++)
                {
                    if ((currentBullets + necessaryBullets) < bulletsPerMagazine)
                    {
                        necessaryBullets++;
                    }
                }

                if (extraBullets >= necessaryBullets)
                {
                    currentBullets += necessaryBullets;
                    extraBullets -= necessaryBullets;
                }
                else if (extraBullets < necessaryBullets)
                {
                    necessaryBullets = extraBullets;
                    currentBullets += necessaryBullets;
                    extraBullets = 0;
                }
            }
        }

        private IEnumerator ApplyMuzzle(float muzzleTime)
        {
            muzzleObject.gameObject.SetActive(true);
            yield return new WaitForSeconds(muzzleTime);
            muzzleObject.gameObject.SetActive(false);
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
            {
                if (fireTransform != null)
                {
                    Gizmos.color = Color.green;
                    Vector3 point1 = fireTransform.position;
                    Vector3 predictedBulletVelocity = fireTransform.forward * data.maxBulletDistance;
                    float stepSize = 0.01f;

                    for (float step = 0f; step < 1; step += stepSize)
                    {
                        if (step > (data.effectiveDistance / data.maxBulletDistance))
                        {
                            Gizmos.color = Color.red;
                            predictedBulletVelocity += (Physics.gravity * stepSize) * data.bulletGravityScale;
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