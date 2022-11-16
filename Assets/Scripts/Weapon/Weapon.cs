using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Game.Player;
using Game.Weapon.Enums;
using Game.Weapon.Components;
using Game.Weapon.Others;

namespace Game.Weapon
{
    public partial class Weapon : MonoBehaviour
    {
        [Header("Settings")]
        public WeaponData data;
        public Transform fireRoot;
        public GameObject muzzleObject;

        [Header("Bullets")]
        public int bulletsPerMagazine = 12;
        public int currentBullets = 12;
        public int extraBullets = 24;

        public delegate void firing();
        public delegate void secureChanged();
        public delegate void startReload();
        public delegate void endReload();

        public firing onFiring;
        public secureChanged onSecureChanged;
        public startReload onStartReload;
        public endReload onEndReload;

        public bool isFiring { get; private set; }
        public bool isAiming { get; private set; }
        public bool isReloading { get; set; }
        public bool isDrawing { get; private set; }
        public bool isHiding { get; private set; }
        public bool isSafety { get; private set; }
        public bool haveBullets { get { return currentBullets > 0; } }
        public AudioClip overrideFireSound { get; set; }

        private float drawTimer;
        private float hideTimer;
        private float firerateTimer;
        private float firingTimer;
        private Vector3 initialAimPos;
        private Quaternion initialAimRot;
        private Vector3 defaultAimPos;
        private Quaternion defaultAimRot;
        private Transform defaultFireRoot;
        private BulletDrop bulletDrop;
        private WeaponAnimation weaponAnimation;

        public void ResetAim()
        {
            data.aimPosition = defaultAimPos;
            data.aimRotation = defaultAimRot;
        }

        public void ResetFireRoot()
        {
            fireRoot = defaultFireRoot;
        }

        private void OnEnable()
        {
            WeaponManager.Instance.currentWeapon = this;
        }

        private void Start()
        {
            // Default
            defaultFireRoot = fireRoot;
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
            bulletDrop = GetComponentInChildren<BulletDrop>();

            // Setting
            weaponAnimation.Init();
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
            bool conditionsToFire =
            (PlayerKeys.Click("Fire") || PlayerKeys.Press("Fire")) &&
            !PlayerController.isRunning && PlayerCamera.cursorLocked &&
             !isDrawing && !isHiding && !isReloading && !Retract.isRetracting;

            if (conditionsToFire)
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

            UpdateFirerate();
        }

        private void UpdateFirerate()
        {
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

        private void UpdateReload()
        {
            bool conditionsToReload =
            PlayerKeys.Click("Reload") &&
            (data.weaponMode == WeaponMode.Combat) &&
            !PlayerController.isRunning && PlayerCamera.cursorLocked &&
            !isReloading && !isDrawing && !isHiding && !Retract.isRetracting &&
            extraBullets > 0;

            if (conditionsToReload)
            {
                if (currentBullets <= 0)
                {
                    weaponAnimation.Play("Reload");
                }
                else if (currentBullets > 0 && currentBullets < bulletsPerMagazine)
                {
                    weaponAnimation.Play("Reload With Ammo");
                }
            }
        }

        private void UpdateAim()
        {
            bool conditionsToAim =
            PlayerKeys.Press("Aim") &&
            (data.weaponMode == WeaponMode.Combat) &&
            !PlayerController.isRunning && PlayerCamera.cursorLocked &&
            !isReloading && !isDrawing && !isHiding && !Retract.isRetracting;

            bool conditionsToReset =
            (transform.localPosition != initialAimPos || transform.localRotation != initialAimRot) &&
            !conditionsToAim;

            if (conditionsToAim)
            {
                isAiming = true;

                PlayerCamera.sensitivityScale = data.aimSensitivityScale;
                Sway.swayScale = data.aimSwayScale;
                Sway.swayHorizontalScale = 0f;

                transform.localPosition = Vector3.Lerp(transform.localPosition, data.aimPosition, data.aimSpeed * Time.deltaTime);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, data.aimRotation, data.aimSpeed * Time.deltaTime);
            }
            else
            {
                PlayerCamera.sensitivityScale = 1;
                Sway.swayHorizontalScale = Sway.defaultSwayHorizontalScale;
                isAiming = false;
            }

            if (conditionsToReset)
            {
                Sway.swayScale = 1;
                transform.localPosition = Vector3.Lerp(transform.localPosition, initialAimPos, data.aimSpeed * Time.deltaTime);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, initialAimRot, data.aimSpeed * Time.deltaTime);
            }
        }

        private void UpdateMode()
        {
            bool conditionsToChangeMode =
            PlayerKeys.Click("Safety") &&
            !isReloading && !isAiming && !isFiring && !isDrawing && !isHiding;

            if (conditionsToChangeMode)
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

        private void CalculateFire()
        {
            if (currentBullets >= data.bulletsPerFire && firerateTimer <= 0)
            {
                // Tracer
                List<Vector3> tracerPositions = new List<Vector3>();
                LineRenderer tracer = GameObject.Instantiate(WeaponManager.Instance.tracerPrefab, fireRoot.position, Quaternion.identity).GetComponent<LineRenderer>();
                BulletTracer tracerScript = tracer.gameObject.GetComponent<BulletTracer>();

                Vector3 point1 = fireRoot.position;
                Vector3 predictedBulletVelocity = fireRoot.forward * data.maxBulletDistance;
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
                            float distance = (fireRoot.position - hit.point).sqrMagnitude;
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
                bulletDrop.Drop();

                // Delegate
                onFiring?.Invoke();
            }
        }

        private IEnumerator CalculateDelay(float time, RaycastHit hit)
        {
            yield return new WaitForSeconds(time);

            // Force
            Rigidbody HitBody = hit.transform.GetComponent<Rigidbody>();
            if (HitBody)
            {
                HitBody.AddForceAtPosition(fireRoot.forward * data.bulletHitForce, hit.point);
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
            muzzleObject.SetActive(true);
            yield return new WaitForSeconds(muzzleTime);
            muzzleObject.SetActive(false);
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
            {
                if (fireRoot != null)
                {
                    Gizmos.color = Color.green;
                    Vector3 point1 = fireRoot.position;
                    Vector3 predictedBulletVelocity = fireRoot.forward * data.maxBulletDistance;
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