using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Code.Interfaces;
using Code.Player;
using Code.Systems.InputSystem;
using Code.Weapons.Utils;

namespace Code.Weapons
{
    public class Weapon : MonoBehaviour
    {
        public enum Type
        {
            Revolver,
            Pistol,
            Shotgun,
            Rifle,
            AssaultRifle,
            SubMachine,
            Machine
        }

        public enum FireMode
        {
            Semi,
            Auto,
            Burst
        }

        [System.Serializable]
        public class Magazine
        {
            public int bullets;
            public bool haveBullets { get { return bullets < 1; } }

            public void ClampBullets(int bulletsPerMagazine)
            {
                bullets = Mathf.Clamp(bullets, 0, bulletsPerMagazine);
            }
        }

        public WeaponData data;

        [Header("Settings")]
        public Transform fireRoot;

        [Header("Magazine")]
        public Magazine currentMagazine;
        public List<Magazine> extraMagazines;

        public delegate void onFiring();
        public delegate void startReload();
        public delegate void endReload();

        public onFiring OnFiring;
        public startReload StartReload;
        public endReload EndReload;

        private float firingCountdown;
        private Vector3 initialAimPos;
        private Quaternion initialAimRot;
        private Vector3 defaultAimPos;
        private Quaternion defaultAimRot;

        public bool isFiring { get; private set; }
        public bool isReloading { get; private set; }
        public bool isAiming { get; private set; }



        private void Start()
        {
            InitVariables();
            GetComponents();
        }

        private void InitVariables()
        {
            if (data.initialMagazines > 0)
            {
                extraMagazines = WeaponUtils.CreateMagazinePackage(data.initialMagazines, data.bulletsPerMagazine);
            }

            initialAimPos = transform.localPosition;
            initialAimRot = transform.localRotation;
        }

        private void GetComponents()
        {

        }

        private void Update()
        {
            // Fire
            switch (data.fireMode)
            {
                case FireMode.Semi:

                    if (InputSystem.OnClick("Fire"))
                    {
                        StartCoroutine(Fire(false));
                    }

                    break;

                case FireMode.Auto:

                    if (InputSystem.OnPressing("Fire"))
                    {
                        StartCoroutine(Fire(false));
                    }

                    break;

                case FireMode.Burst:

                    if (InputSystem.OnClick("Fire"))
                    {
                        StartCoroutine(Fire(true));
                    }

                    break;
            }

            // Reload
            bool reload = InputSystem.OnClick("Reload");

            if (reload)
            {
                CalculateReload();
            }

            // Aim
            isAiming =
                    InputSystem.OnPressing("Aim") && !PlayerController.isRunning &&
                    PlayerController.playerCamera.isCursorLocked && !isReloading;

            bool resetAim = (transform.localPosition != initialAimPos || transform.localRotation != initialAimRot) && !isAiming;

            if (isAiming)
            {
                PlayerController.playerCamera.sensitivityScale = data.aimSensitivityScale;
                //WeaponManager.Sway.swayScale = data.aimSwayScale;
                //WeaponManager.Sway.swayHorScale = 0f;

                transform.localPosition = Vector3.Lerp(transform.localPosition, data.aimPosition, data.aimSpeed * Time.deltaTime);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, data.aimRotation, data.aimSpeed * Time.deltaTime);
            }
            else
            {
                PlayerController.playerCamera.sensitivityScale = 1;
                //WeaponManager.Sway.swayHorScale = WeaponManager.Sway.defaultSwayHorScale;
            }

            if (resetAim)
            {
                //WeaponManager.Sway.swayScale = 1;
                transform.localPosition = Vector3.Lerp(transform.localPosition, initialAimPos, data.aimSpeed * Time.deltaTime);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, initialAimRot, data.aimSpeed * Time.deltaTime);
            }
        }

        private IEnumerator Fire(bool isBurstFire)
        {
            if (!isBurstFire)
            {
                CalculateFire();
                yield break;
            }
            else
            {
                for (int i = 0; i < data.burstPerFire; i++)
                {
                    yield return new WaitForSeconds(data.burstRate);
                    CalculateFire();
                }
            }
        }

        private IEnumerator Firing()
        {
            isFiring = true;
            OnFiring?.Invoke();
            yield return new WaitForSeconds(data.firerate);
            isFiring = false;
        }

        private void CalculateFire()
        {
            if (currentMagazine.bullets >= data.bulletsPerFire)
            {
                StartCoroutine(Firing());

                Vector3 p1 = fireRoot.position;
                Vector3 predictedVelocity = fireRoot.forward * data.maxBulletDistance;
                float stepSize = 0.01f;

                for (float step = 0f; step < 1; step += stepSize)
                {
                    // Apply gravity in bullet
                    if (step > (data.effectiveBulletDistance / data.maxBulletDistance))
                    {
                        predictedVelocity += (Physics.gravity * stepSize) * data.bulletGravityScale;
                    }

                    Vector3 p2 = p1 + predictedVelocity * stepSize;
                    Ray ray = new Ray(p1, p2 - p1);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, (p2 - p1).magnitude, WeaponManager.HittableMask))
                    {
                        if (hit.transform)
                        {
                            // Calculate delay time
                            float distance = (fireRoot.position - hit.point).sqrMagnitude;
                            float time = distance / (data.bulletVelocity * 1000f);

                            StartCoroutine(CalculateFireDelay(time, hit));
                            break;
                        }
                    }
                    p1 = p2;
                }

                currentMagazine.bullets -= data.bulletsPerFire;
            }
        }

        private IEnumerator CalculateFireDelay(float time, RaycastHit hit)
        {
            yield return new WaitForSeconds(time);

            // Apply force in physics bodys
            Rigidbody hittedBody = hit.transform.GetComponent<Rigidbody>();

            if (hittedBody != null)
            {
                hittedBody.AddForceAtPosition(fireRoot.forward * data.bulletHitForce, hit.point);
            }

            // Apply damage to damageable objects
            IDamageable<float> damageableObject = hit.collider.GetComponent<IDamageable<float>>();

            if (damageableObject != null)
            {
                damageableObject.TakeDamage(Random.Range(data.minDamage, data.maxDamage));
            }
        }

        public void CalculateReload()
        {
            if (currentMagazine.bullets < data.bulletsPerMagazine)
            {
                Magazine oldMag = currentMagazine;
                Magazine selectedMag = currentMagazine;

                for (int i = 0; i < extraMagazines.Count; i++)
                {
                    // Select the magazine with the highest number of bullets
                    if (extraMagazines[i].bullets > selectedMag.bullets)
                    {
                        selectedMag = extraMagazines[i];
                    }
                }

                if (selectedMag != currentMagazine && selectedMag.bullets > currentMagazine.bullets)
                {
                    extraMagazines.Add(oldMag);
                    currentMagazine = selectedMag;
                    extraMagazines.Remove(selectedMag);
                }
            }

            UpdateMagazines();
        }

        private void UpdateMagazines()
        {
            currentMagazine.ClampBullets(data.bulletsPerMagazine);

            for (int i = 0; i < extraMagazines.Count; i++)
            {
                extraMagazines[i].ClampBullets(data.bulletsPerMagazine);

                // Cleaning empty magazines
                if (extraMagazines[i].bullets == 0)
                {
                    extraMagazines.Remove(extraMagazines[i]);
                }
            }
        }
    }
}